using OsteoMAUIApp.Models.Common;
using OsteoMAUIApp.Models.User;
using OsteoMAUIApp.Services.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Implementations
{
    public class DatabaseService : IDatabaseService
    {
        private const int CurrentDBVersion = 1;
        private static class MetadataKeys
        {
            public const string DbVersion = "DBVersion";
        }
        private SQLiteAsyncConnection _database;

        public async Task InitializeAsync()
        {
            if (_database != null)
                return;

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TapGrap.db3");
            //if (!File.Exists(databasePath))
            //{
            //    File.Delete(databasePath);
            //}

            _database = new SQLiteAsyncConnection(databasePath);

            //Initialize MetaData
            await InitializeMetadataAsync();

            //Initialize Tables
            await InitializeTables();

            //Apply Migrations if Required
            //await ApplyMigration();

        }

        

        private async Task InitializeMetadataAsync()
        {
            await _database.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Metadata (Key TEXT PRIMARY KEY, Value TEXT)");

            var versionString = await _database.ExecuteScalarAsync<string>($"SELECT Value FROM Metadata WHERE Key = '{MetadataKeys.DbVersion}'");

            if (string.IsNullOrEmpty(versionString))
            {
                await _database.ExecuteAsync(@"INSERT OR REPLACE INTO Metadata (Key, Value) VALUES (?, ?)", MetadataKeys.DbVersion, CurrentDBVersion);
            }
        }

        private async Task InitializeTables()
        {
           
            await _database.CreateTableAsync<UserModel>();
            await _database.CreateTableAsync<AppSettingsModel>();
        }

        
        #region |Profile & Logo Batch together Dumped|
        //        if (images == null || !images.Any())
        //                return;

        //            try
        //            {
        //                // Build the SQL update query using CASE statements for both columns
        //                var updateQuery = new StringBuilder();
        //        updateQuery.Append(@"
        //            UPDATE CardModel 
        //            SET 
        //                profilePhotoBase64 = CASE ");

        //                // Add CASE logic for profilePhotoBase64
        //                foreach (var (CardId, ProfilePhotoBase64, _) in images)
        //                {
        //                    if (!string.IsNullOrEmpty(ProfilePhotoBase64))
        //                        updateQuery.Append($"WHEN id = {CardId} THEN '{ProfilePhotoBase64}' ");
        //                }
        //    updateQuery.Append("ELSE profilePhotoBase64 END,");

        //                // Add CASE logic for companyLogoBase64
        //                updateQuery.Append(@"
        //                companyLogoBase64 = CASE ");
        //                foreach (var (CardId, _, CompanyLogoBase64) in images)
        //                {
        //                    if (!string.IsNullOrEmpty(CompanyLogoBase64))
        //                        updateQuery.Append($"WHEN id = {CardId} THEN '{CompanyLogoBase64}' ");
        //                }
        //updateQuery.Append("ELSE companyLogoBase64 END ");

        //// Add WHERE clause to limit the updates
        //updateQuery.Append("WHERE id IN (");
        //updateQuery.Append(string.Join(", ", images.Select(i => i.CardId)));
        //updateQuery.Append(");");
        //Console.WriteLine("===================== Query " + updateQuery);
        //// Execute the batch query
        //await _database.ExecuteAsync(updateQuery.ToString());
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle exceptions and log errors
        //                Console.WriteLine($"BatchUpdateLocalCardImages Error: {ex.Message}");
        //            }
        #endregion

      
        

        public async Task ClearTablesAsync()
        {
            await _database.ExecuteAsync("DELETE FROM CardModel");
            await _database.ExecuteAsync("DELETE FROM UserModel");
            await _database.ExecuteAsync("DELETE FROM AppSettingsModel");
            await _database.ExecuteAsync("DELETE FROM ScanQRModel");
            await _database.ExecuteAsync("VACUUM");
        }

  

        //User Table Functions
        public async Task UpsertUserAsync(UserModel user)
        {
            var existingUser = await GetUserAsync();
            if (existingUser == null)
            {
                await InsertUserAsync(user);
            }
            else
            {
                await UpdateUserAsync(user);
            }
        }
        public async Task InsertUserAsync(UserModel user)
        {
            await _database.InsertAsync(user);
        }
        public async Task UpdateUserAsync(UserModel user)
        {
            try
            {
                var (mClauses, mParameters) = CreateUserUpdateQuery(user);
                if (mClauses.Count == 0)
                    throw new InvalidOperationException("No fields to update.");

                var setClause = string.Join(", ", mClauses);
                var query = $"UPDATE UserModel SET {setClause} WHERE id = ?";

                mParameters.Add(user.id);

                await _database.ExecuteAsync(query, mParameters.ToArray());
                //await _database.UpdateAsync(card);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<UserModel> GetUserAsync()
        {
            return await _database.Table<UserModel>().FirstOrDefaultAsync();
        }
        private (List<string> setClauses, List<object> cParameters) CreateUserUpdateQuery(UserModel user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var setClauses = new List<string>();
            var parameters = new List<object>();
            if (!string.IsNullOrEmpty(user.email))
            {
                setClauses.Add("email = ?");
                parameters.Add(user.email);
            }
            if (!string.IsNullOrEmpty(user.firstName))
            {
                setClauses.Add("firstName = ?");
                parameters.Add(user.firstName);
            }

            if (!string.IsNullOrEmpty(user.lastName))
            {
                setClauses.Add("lastName = ?");
                parameters.Add(user.lastName);
            }

            if (!string.IsNullOrEmpty(user.stripeCustomerId))
            {
                setClauses.Add("stripeCustomerId = ?");
                parameters.Add(user.stripeCustomerId);
            }

            if (!string.IsNullOrEmpty(user.defaultCardGuid))
            {
                setClauses.Add("defaultCardGuid = ?");
                parameters.Add(user.defaultCardGuid);
            }

            setClauses.Add("isAnyPlanSubscribed = ?");
            parameters.Add(user.isAnyPlanSubscribed);

           

            if (string.IsNullOrEmpty(user.profilePicture))
            {
                setClauses.Add("profilePicture = ?");
                parameters.Add(user.profilePicture);
                setClauses.Add("profilePictureBase64 = ?");
                parameters.Add(user.profilePictureBase64);
            }
            else if (!string.IsNullOrEmpty(user.profilePicture))
            {
                setClauses.Add("profilePicture = ?");
                parameters.Add(user.profilePicture);
            }

            if (!string.IsNullOrEmpty(user.profilePictureBase64))
            {
                setClauses.Add("profilePictureBase64 = ?");
                parameters.Add(user.profilePictureBase64);
            }

            return (setClauses, parameters);
        }

        //App Settings Table Functions
        public async Task<AppSettingsModel> GetAppSettingsAsync()
        {
            return await _database.Table<AppSettingsModel>().FirstOrDefaultAsync();
        }

        public async Task UpsertAppSettingsAsync(AppSettingsModel settings)
        {
            var existingSettings = await GetAppSettingsAsync();
            if (existingSettings == null)
            {
                await InsertAppSettingsAsync(settings);
            }
            else
            {
                await UpdateAppSettingsAsync(settings);
            }
        }

        public async Task InsertAppSettingsAsync(AppSettingsModel settings)
        {
            await _database.InsertAsync(settings);
        }

        public async Task UpdateAppSettingsAsync(AppSettingsModel settings)
        {
            try
            {
                await _database.UpdateAsync(settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
