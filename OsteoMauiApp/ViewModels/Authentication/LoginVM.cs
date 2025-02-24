using Newtonsoft.Json;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models.Authentication;
using OsteoMAUIApp.Models.User;
using OsteoMAUIApp.Services.Implementations;
using OsteoMAUIApp.Services.Interfaces;
using Task = System.Threading.Tasks.Task;
//#if ANDROID
//using Android.Gms.Tasks;
//using Firebase.Messaging;
//#endif
namespace OsteoMAUIApp.ViewModels.Authentication
{
    public class LoginVM : BaseViewModel
    {
        private readonly IAuthenticationService _authService;

        public LoginVM(INavigation navigation)
        {
            Helpers.Utility.IsLoggedIn = false;
            _databaseService = DependencyService.Get<IDatabaseService>();
            _navigation = navigation;
            LoginDetails = new LoginModel();
            _requestProvier = new RequestProvider();
            _authService = new AuthenticationService(_requestProvier, _databaseService);
            _appSettingsService = new AppSettingsService(_requestProvier, _databaseService);
            LoginCommand = new Command(LoginClicked);
            LogoutCommand = new Command(LogoutClicked);
            //ResendVerificationEmailCommand = new Command<string>(ResendVerificationEmailClicked);
            InitializeCommand = new Command(Initialize);
        }

        #region |Private|
        bool isSessionChecking = true;
        private string _email;
        private string _password;
        private string _emailError;
        private string _passwordError;
        INavigation _navigation;
        LoginModel _loginDetails;
        private IDatabaseService _databaseService;
        IRequestProvider _requestProvier;
        IAppSettingsService _appSettingsService;
        private string _verifyAccountMessage;
        #endregion

        #region |Public|
        public bool IsSessionChecking
        {
            get { return isSessionChecking; }
            set { SetProperty(ref isSessionChecking, value); }
        }

        public LoginModel LoginDetails
        {
            get { return this._loginDetails; }
            set
            {
                if (this._loginDetails == value)
                {
                    return;
                }

                SetProperty(ref _loginDetails, value);
            }
        }

        public string verifyAccountMessage
        {
            get { return this._verifyAccountMessage; }
            set
            {
                if (this._verifyAccountMessage == value)
                {
                    return;
                }

                SetProperty(ref _verifyAccountMessage, value);
            }
        }

        #endregion

        #region |Commands|
        public Command LoginCommand;
        public Command GoogleLoginCommand;
        public Command LogoutCommand;
        public Command SignupCommand;
        public Command ForgotPasswordCommand;
        public Command ForgotPasswordSubmitCommand;
        public Command InitializeCommand;
        public Command ResendVerificationEmailCommand;
        #endregion

        #region |Methods|
        private async void Initialize(object obj)
        {
            IsSessionChecking = true;
            try
            {
                try
                {
//#if ANDROID
//                    LoginDetails.fcmToken = await GetDeviceTokenAsync();
//                    Console.WriteLine($"FCM Token App.xaml.cs: {LoginDetails.fcmToken}");                    
//#endif
                    //fcm = await CrossFirebasePushNotification.Current.GetTokenAsync();
                }
                catch (Exception e)
                {

                }
                if (!string.IsNullOrEmpty(LoginDetails.fcmToken))
                {
                    await SecureStorage.SetAsync(GlobalSettings.FcmTokenKey, LoginDetails.fcmToken);
                    //GlobalSetting.FcmTokenKey = fcm;
                }

                if (await Utility.IsSessionValid())
                {
                    Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//CardsPage");
                }
                IsSessionChecking = false;
            }
            catch (System.Exception ex)
            {
                (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong!", "OK");
                IsSessionChecking = false;
            }

        }
        private async void LoginClicked(object obj)
        {
            if (IsBusy) return;
            //StartBusyIndicator();            
            if (await LoginDetails.ValidateModelForLogin())
            {
                IsBusy = true;
                await Task.Delay(500);
                try
                {
                   // Utility.DismissKeyboard();
                    var Response = await _authService.LoginAsync(LoginDetails);
                    if (Response != null)
                    {
                        if (Response.status == "success" || Response.statusCode == 201)
                        {
                            //if (await InitializeUser(Response.user))
                            //{
                                Application.Current.MainPage = new AppShell();
                                await Shell.Current.GoToAsync("//Dashboard");
                            //}
                        }
                        else if (Response.statusCode == 401)
                        {
                            //await _navigation.PushModalAsync(new VerifyAccount(Response.statusMessage, LoginDetails.emailAddress));
                            //LoginDetails.emailAddress = string.Empty;
                            //LoginDetails.currentPassword = string.Empty;
                            //LoginDetails.emailAddressError = string.Empty;
                            //LoginDetails.currentPasswordError = string.Empty;
                        }
                        else if (Response.statusCode == 402 || Response.statusCode == 404)
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", Response.statusMessage, "OK");
                        }
                        else if (Response.statusCode == 503)
                        {
                            Utility.ShowToastMessage(Response.statusMessage, Utility.ToastDuration.Long);
                        }
                        else
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
                        }
                    }
                    Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//Dashboard");
                }
                catch (System.Exception ex)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
                }
            }
            //StopBusyIndicator();
            IsBusy = false;
        }
     
        public async Task<bool> InitializeUser(UserModel user)
        {
            try
            {
                GlobalSettings.Instance.access_token = user.oauthToken.access.token;
                GlobalSettings.Instance.refresh_token = user.oauthToken.refresh.token;
                GlobalSettings.Instance.user = user;
                //GlobalSetting.FcmTokenKey = fcm;

                await SecureStorage.SetAsync(GlobalSettings.LoginDateTimeKey, DateTime.Now.ToString());
                await SecureStorage.SetAsync(GlobalSettings.AccessTokenKey, user.oauthToken.access.token);
                await SecureStorage.SetAsync(GlobalSettings.RefreshTokenKey, user.oauthToken.refresh.token);
                await SecureStorage.SetAsync(GlobalSettings.UserKey, JsonConvert.SerializeObject(user));

                //Initialize the user in local db
                //_userVM.InitializeCommand.Execute(null);

                await _databaseService.UpsertUserAsync(user);
                await _appSettingsService.InitializeAppSettings();

                Helpers.Utility.IsLoggedIn = true;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       
       
        private async void LogoutClicked(object obj)
        {
            if (IsBusy) return;
            await Task.Delay(500);
            try
            {
                var res = await _authService.LogoutAsync();
            }
            catch (Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
        }
        //private async void ResendVerificationEmailClicked(string emailAddress)
        //{
        //    if (IsBusy) return;
        //    StartBusyIndicator();
        //    await Task.Delay(500);
        //    try
        //    {
        //        var Response = await _authService.ResendVerificationEmail(emailAddress);
        //        if (Response != null)
        //        {
        //            if (Response.statusCode == 200 || Response.statusCode == 201)
        //            {
        //                await _navigation.PopModalAsync();
        //                await (Application.Current as App).MainPage.DisplayAlert("Success", Response.statusMessage, "OK");
        //            }
        //            else if (Response.statusCode == 401 || Response.statusCode == 402 || Response.statusCode == 404)
        //            {
        //                await (Application.Current as App).MainPage.DisplayAlert("Error", Response.statusMessage, "OK");
        //            }
        //            else if (Response.statusCode == 503)
        //            {
        //                Utility.ShowToastMessage(Response.statusMessage, Utility.ToastDuration.Long);
        //            }
        //            else
        //            {
        //                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
        //            }
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
        //    }
        //    StopBusyIndicator();
        //}

        #endregion

//#if ANDROID
        
//        public Task<string> GetDeviceTokenAsync()
//        {
//            var taskCompletionSource = new TaskCompletionSource<string>();

//            FirebaseMessaging.Instance.GetToken()
//        .AddOnSuccessListener(new OnSuccessListener(result =>
//        {
//            taskCompletionSource.SetResult(result);
//        }))
//        .AddOnFailureListener(new OnFailureListener(exception =>
//        {
//            taskCompletionSource.SetException(exception);
//        }));

//            return taskCompletionSource.Task;
//        }

//        // Listener for success
//        public class OnSuccessListener : Java.Lang.Object, IOnSuccessListener
//        {
//            private readonly Action<string> _onSuccess;

//            public OnSuccessListener(Action<string> onSuccess)
//            {
//                _onSuccess = onSuccess;
//            }

//            public void OnSuccess(Java.Lang.Object result)
//            {
//                _onSuccess?.Invoke(result?.ToString());
//            }
//        }

//        // Listener for failure
//        public class OnFailureListener : Java.Lang.Object, IOnFailureListener
//        {
//            private readonly Action<Java.Lang.Exception> _onFailure;

//            public OnFailureListener(Action<Java.Lang.Exception> onFailure)
//            {
//                _onFailure = onFailure;
//            }

//            public void OnFailure(Java.Lang.Exception exception)
//            {
//                _onFailure?.Invoke(exception);
//            }
//        }
//#endif
    }
}
