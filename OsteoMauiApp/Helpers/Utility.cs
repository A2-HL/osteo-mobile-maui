using Newtonsoft.Json;
using OsteoMAUIApp.Models.Authentication;
using OsteoMAUIApp.Models.User;
using OsteoMAUIApp.Services.Implementations;
using OsteoMAUIApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Helpers
{
    public class Utility
    {
        public static bool IsLoggedIn = false;
        private static IDatabaseService _databaseService;
        private static IAuthenticationService _authenticationService;

        public Utility()
        {
            _databaseService = DependencyService.Get<IDatabaseService>();
            _authenticationService = new AuthenticationService(new RequestProvider(), _databaseService);
        }

        public static async Task<bool> IsSessionValid()
        {
            //if (!string.IsNullOrEmpty(GlobalSettings.Instance.access_token) &&
            //    IsTokenValid(GlobalSettings.Instance.access_token))
            //    return true;
            try
            {

                var accessToken = await SecureStorage.GetAsync(GlobalSettings.AccessTokenKey);
                var refreshToken = await SecureStorage.GetAsync(GlobalSettings.RefreshTokenKey);
                var userJson = await SecureStorage.GetAsync(GlobalSettings.UserKey);

                if (!string.IsNullOrEmpty(accessToken) &&
                    IsTokenValid(accessToken))
                    return true;

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userJson))
                {
                    return false;
                }

                var userData = JsonConvert.DeserializeObject<UserModel>(userJson);

                if (IsTokenValid(accessToken))
                {
                    ActivateSession(accessToken, refreshToken, userData);
                    return true;
                }
                else if (IsTokenValid(refreshToken))
                {
                    var res = await _authenticationService.RefreshAccessToken(refreshToken, userData.id);
                    if (res != null && res.user != null)
                    {
                        RenewSession(res);
                        ActivateSession(res.user.oauthToken.access.token, res.user.oauthToken.refresh.token, res.user);
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return false;
        }

        private static bool IsTokenValid(string Token)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(Token) as JwtSecurityToken;

                if (jsonToken.ValidTo > DateTime.UtcNow)
                    return true;
            }
            return false;
        }

        public static void LogoutUser()
        {
            SecureStorage.RemoveAll();
            GlobalSettings.Instance.access_token = null;
            GlobalSettings.Instance.refresh_token = null;
            GlobalSettings.Instance.user = null;
        }
        private static async void RenewSession(LoginResponseModel res)
        {
            await SecureStorage.SetAsync(GlobalSettings.AccessTokenKey, res.user.oauthToken.access.token);
            await SecureStorage.SetAsync(GlobalSettings.RefreshTokenKey, res.user.oauthToken.refresh.token);
            await SecureStorage.SetAsync(GlobalSettings.UserKey, JsonConvert.SerializeObject(res.user));
        }

        private static void ActivateSession(string accessToken, string refreshToken, UserModel user)
        {
            GlobalSettings.Instance.access_token = accessToken;
            GlobalSettings.Instance.refresh_token = refreshToken;
            GlobalSettings.Instance.user = user;

        }


        private static bool isPerformingTapGesture = false;
        public static void ShowToastMessage(string message, ToastDuration duration)
        {
            var toastService = DependencyService.Get<IToastService>();
            if (toastService != null)
            {
                switch (duration)
                {
                    case ToastDuration.Short:
                        toastService.ShowToastShort(message);
                        break;
                    case ToastDuration.Long:
                        toastService.ShowToastLong(message);
                        break;
                }
            }

        }
        public enum ToastDuration
        {
            Short,
            Long
        }
        public enum DeviceType
        {
            Web,
            iOS,
            Android
        }
        public static async Task<bool> NotificationPermissionsAllowed()
        {
            var notificationStatus = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (notificationStatus == PermissionStatus.Granted)
            {
                return true;
            }
            if (notificationStatus == PermissionStatus.Denied || notificationStatus == PermissionStatus.Restricted)
            {
                notificationStatus = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            return notificationStatus == PermissionStatus.Granted;
        }

        public static void OpenAppSettings()
        {
            try
            {
#if ANDROID
                        var context = Android.App.Application.Context;
                        var intent = new Android.Content.Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
                        intent.AddFlags(Android.Content.ActivityFlags.NewTask);
                        intent.SetData(Android.Net.Uri.Parse("package:" + context.PackageName));
                        context.StartActivity(intent);
#elif IOS
                var url = new Uri("app-settings:");
                Launcher.OpenAsync(url);
#else
                        Console.WriteLine("App settings are not supported on this platform.");
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening settings: {ex.Message}");
            }
        }
        public static void DismissKeyboard()
        {
#if ANDROID
                var inputMethodManager = (Android.Views.InputMethods.InputMethodManager)
                    Android.App.Application.Context.GetSystemService(Android.Content.Context.InputMethodService);

                var currentActivity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
                var currentFocus = currentActivity?.CurrentFocus;

                if (currentFocus != null)
                {
                    inputMethodManager.HideSoftInputFromWindow(currentFocus.WindowToken, Android.Views.InputMethods.HideSoftInputFlags.None);
                }
#elif IOS
                        UIKit.UIApplication.SharedApplication.KeyWindow.EndEditing(true);
#endif
        }

    }
}
