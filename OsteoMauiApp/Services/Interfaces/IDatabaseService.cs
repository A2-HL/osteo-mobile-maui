using OsteoMAUIApp.Models.Common;
using OsteoMAUIApp.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task InitializeAsync();

        //Users Functions
        Task UpsertUserAsync(UserModel user);
        Task InsertUserAsync(UserModel user);
        Task UpdateUserAsync(UserModel user);
        Task<UserModel> GetUserAsync();

        //App Settings Functions 
        Task<AppSettingsModel> GetAppSettingsAsync();
        Task UpsertAppSettingsAsync(AppSettingsModel settings);
        Task InsertAppSettingsAsync(AppSettingsModel settings);
        Task UpdateAppSettingsAsync(AppSettingsModel settings);
    }
}
