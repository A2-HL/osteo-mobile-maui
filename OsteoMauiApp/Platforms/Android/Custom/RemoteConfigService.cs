//using Firebase.RemoteConfig;
//using OsteoMAUIApp.Services.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OsteoMAUIApp.Platforms.Android.Custom
//{
//    public class RemoteConfigService : IRemoteConfigService
//    {
//        private FirebaseRemoteConfig _remoteConfig;

//        public RemoteConfigService()
//        {
//            // Initialize Remote Config
//            _remoteConfig = null;// FirebaseRemoteConfig.Instance;

//            // Set default values for Remote Config parameters
//            var defaults = new Dictionary<string, Java.Lang.Object>
//            {
//                { "latest_version", "1.0.0" }
//            };
//            _remoteConfig.SetDefaultsAsync(defaults);
//        }

//        public async Task<(bool updateRequired, bool forceUpdate)> CheckAppUpdateRequiredAsync()
//        {
//            bool updateRequired = false;
//            bool forceUpdate = false;
//            try
//            {
//                // Fetch remote config values
//                await _remoteConfig.FetchAsync(TimeSpan.FromSeconds(3600).Seconds);
//                _remoteConfig.FetchAndActivate();

//                // Get the latest version from Remote Config
//                string latestVersionString = _remoteConfig.GetString("latest_version");
//                string updateType = _remoteConfig.GetString("update_type");

//                string currentVersionString = AppInfo.VersionString;
//                if (Version.TryParse(currentVersionString, out Version currentVersion) &&
//            Version.TryParse(latestVersionString, out Version latestVersion))
//                {
//                    // Compare versions
//                    updateRequired = currentVersion < latestVersion;
//                    forceUpdate = updateType.Equals("force", StringComparison.OrdinalIgnoreCase);
//                }
//                else
//                {
//                    Console.WriteLine("Error parsing version strings.");
//                }


//                //updateRequired = string.Compare(currentVersion, latestVersion, StringComparison.Ordinal) < 0;
//                //forceUpdate = (updateType == "force");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching remote config: {ex.Message}");
//            }
//            return (updateRequired, forceUpdate);
//        }

//    }
//}
