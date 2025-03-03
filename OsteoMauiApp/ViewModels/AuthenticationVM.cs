using Newtonsoft.Json;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models.Authentication;
using OsteoMAUIApp.Models.User;
using OsteoMAUIApp.Services.Implementations;
using OsteoMAUIApp.Services.Interfaces;
using OsteoMAUIApp.Views.Authentication;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Task = System.Threading.Tasks.Task;
using OsteoMAUIApp.Models;
#if ANDROID
using Android.Gms.Tasks;
using Firebase.Messaging;
#endif

namespace OsteoMAUIApp.ViewModels
{
    public class AuthenticationVM : BaseViewModel
    {
        private readonly IAuthenticationService _authService;
        private readonly IDatabaseService _databaseService;
        private readonly IRequestProvider _requestProvider;
        private readonly IAppSettingsService _appSettingsService;
        private readonly INavigation _navigation;

        public int SelectedUserType { get; set; } = 2;

        public AuthenticationVM(INavigation navigation)
        {
            Helpers.Utility.IsLoggedIn = false;
            _databaseService = DependencyService.Get<IDatabaseService>();
            _navigation = navigation;
            _requestProvider = new RequestProvider();
            _authService = new AuthenticationService(_requestProvider, _databaseService);
            _appSettingsService = new AppSettingsService(_requestProvider, _databaseService);

            InitializeCommands();
        }

        #region Properties
        private bool _isSessionChecking = true;
        private LoginModel _loginDetails = new LoginModel();
        private SignUpModel _signupDetails = new SignUpModel();
        private string _verifyAccountMessage;

        public bool IsSessionChecking
        {
            get => _isSessionChecking;
            set => SetProperty(ref _isSessionChecking, value);
        }

        public LoginModel LoginDetails
        {
            get => _loginDetails;
            set => SetProperty(ref _loginDetails, value);
        }

        public SignUpModel SignupDetails
        {
            get => _signupDetails;
            set => SetProperty(ref _signupDetails, value);
        }

        public string VerifyAccountMessage
        {
            get => _verifyAccountMessage;
            set => SetProperty(ref _verifyAccountMessage, value);
        }
        #endregion

        #region Commands
        public Command LoginCommand { get; private set; }
        public Command LogoutCommand { get; private set; }
        public Command SignupCommand { get; private set; }
        public Command ForgotPasswordCommand { get; private set; }
        public Command ForgotPasswordSubmitCommand { get; private set; }
        public Command InitializeCommand { get; private set; }
        public Command SignupSubmitCommand { get; private set; }
        #endregion

        #region Initialization
        private void InitializeCommands()
        {
            LoginCommand = new Command(OnLoginClicked);
            LogoutCommand = new Command(OnLogoutClicked);
            SignupCommand = new Command(OnSignupClicked);
            ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
            ForgotPasswordSubmitCommand = new Command(OnForgotPasswordSubmitClicked);
            InitializeCommand = new Command(OnInitialize);
            SignupSubmitCommand = new Command(OnSignupSubmitClicked);
        }

        private async void OnInitialize(object obj)
        {
            IsSessionChecking = true;
            try
            {
#if ANDROID
                LoginDetails.fcmToken = await GetDeviceTokenAsync();
                Console.WriteLine($"FCM Token: {LoginDetails.fcmToken}");
#endif

                if (!string.IsNullOrEmpty(LoginDetails.fcmToken))
                {
                    await SecureStorage.SetAsync(GlobalSettings.FcmTokenKey, LoginDetails.fcmToken);
                }

                if (await Utility.IsSessionValid())
                {
                    Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//Dashboard");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Something went wrong!");
            }
            finally
            {
                IsSessionChecking = false;
            }
        }
        #endregion

        #region Authentication Methods
        private async void OnLoginClicked(object obj)
        {
            if (IsBusy) return;
            LoginDetails.UserTypeId = SelectedUserType;
            if (await LoginDetails.ValidateModelForLogin())
            {
                await ExecuteAsync(async () =>
                {
                    Utility.DismissKeyboard();
                    

                    var response = await _authService.LoginAsync(LoginDetails);
                    if (response != null)
                    {
                        if (response.statusCode == 201 || response.statusCode == 200)
                        {
                            if (await InitializeUser(response.user))
                            {
                                Application.Current.MainPage = new AppShell();
                                await Shell.Current.GoToAsync("//Dashboard");
                            }
                        }
                        else if (response.statusCode == 401)
                        {
                            //await _navigation.PushModalAsync(new VerifyAccount(Response.statusMessage, LoginDetails.emailAddress));
                            //LoginDetails.emailAddress = string.Empty;
                            //LoginDetails.currentPassword = string.Empty;
                            //LoginDetails.emailAddressError = string.Empty;
                            //LoginDetails.currentPasswordError = string.Empty;
                        }
                        else if (response.status == "error" || response.statusCode == 402 || response.statusCode == 404)
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", response.Message, "OK");
                        }
                        else if (response.statusCode == 503)
                        {
                            Utility.ShowToastMessage(response.statusMessage, Utility.ToastDuration.Long);
                        }
                        else
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
                        }
                        
                    }
                });
            }
        }

        private async void OnSignupClicked(object obj)
        {
            await NavigateToPage(new SignUp());
        }

        private async void OnSignupSubmitClicked(object obj)
        {
            if (IsBusy) return;

            if (await SignupDetails.Validate())
            {
                await ExecuteAsync(async () =>
                {
                    var response = await _authService.SignupAsync(SignupDetails);
                    if (response != null)
                    {
                        if (response.statusCode == 200 || response.statusCode == 201)
                        {
                            await DisplayAlert("Success", response.statusMessage);
                            await _navigation.PopModalAsync();
                        }
                        else
                        {
                            await HandleResponseErrors(response);
                        }
                    }
                });
            }
        }

        private async void OnForgotPasswordClicked(object obj)
        {
            await NavigateToPage(new ForgotPassword());
        }

        private async void OnForgotPasswordSubmitClicked(object obj)
        {
            if (IsBusy) return;

            LoginDetails.UserTypeId = SelectedUserType;
            if (await LoginDetails.ValidateModelForForgotPassword())
            {
                await ExecuteAsync(async () =>
                {
                    var response = await _authService.ForgotPasswordAsync(LoginDetails);
                    if (response != null)
                    {
                        if (response.statusCode == 200 || response.statusCode == 201)
                        {
                            await DisplayAlert("Success", response.statusMessage);
                            await _navigation.PopModalAsync();
                        }
                        else
                        {
                            await HandleResponseErrors(response);
                        }
                    }
                });
            }
        }

        private async void OnLogoutClicked(object obj)
        {
            await ExecuteAsync(async () =>
            {
                // Implement logout logic here
            });
        }
        #endregion

        #region Helper Methods
        private async Task<bool> InitializeUser(UserModel user)
        {
            try
            {
                GlobalSettings.Instance.access_token = user.oauthToken.access.token;
                GlobalSettings.Instance.refresh_token = user.oauthToken.refresh.token;
                GlobalSettings.Instance.user = user;

                await SecureStorage.SetAsync(GlobalSettings.LoginDateTimeKey, DateTime.Now.ToString());
                await SecureStorage.SetAsync(GlobalSettings.AccessTokenKey, user.oauthToken.access.token);
                await SecureStorage.SetAsync(GlobalSettings.RefreshTokenKey, user.oauthToken.refresh.token);
                await SecureStorage.SetAsync(GlobalSettings.UserKey, JsonConvert.SerializeObject(user));

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

        private async Task HandleResponseErrors(ResponseStatusModel response)
        {
            if (response.statusCode == 401 || response.statusCode == 402 || response.statusCode == 404)
            {
                await DisplayAlert("Error", response.statusMessage);
            }
            else if (response.statusCode == 503)
            {
                Utility.ShowToastMessage(response.statusMessage, Utility.ToastDuration.Long);
            }
            else
            {
                await DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage);
            }
        }

        private async Task ExecuteAsync(Func<Task> action)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToPage(Page page)
        {
            if (IsBusy) return;

            StartBusyIndicator();
            try
            {
                page.Appearing += (s, e) => StopBusyIndicator();
                await _navigation.PushModalAsync(page);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message);
            }
            finally
            {
                StopBusyIndicator();
            }
        }

        private async Task DisplayAlert(string title, string message)
        {
            await (Application.Current as App).MainPage.DisplayAlert(title, message, "OK");
        }
        #endregion

#if ANDROID
        public Task<string> GetDeviceTokenAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            FirebaseMessaging.Instance.GetToken()
                .AddOnSuccessListener(new OnSuccessListener(result => taskCompletionSource.SetResult(result)))
                .AddOnFailureListener(new OnFailureListener(exception => taskCompletionSource.SetException(exception)));

            return taskCompletionSource.Task;
        }

        private class OnSuccessListener : Java.Lang.Object, IOnSuccessListener
        {
            private readonly Action<string> _onSuccess;

            public OnSuccessListener(Action<string> onSuccess) => _onSuccess = onSuccess;

            public void OnSuccess(Java.Lang.Object result) => _onSuccess?.Invoke(result?.ToString());
        }

        private class OnFailureListener : Java.Lang.Object, IOnFailureListener
        {
            private readonly Action<Java.Lang.Exception> _onFailure;

            public OnFailureListener(Action<Java.Lang.Exception> onFailure) => _onFailure = onFailure;

            public void OnFailure(Java.Lang.Exception exception) => _onFailure?.Invoke(exception);
        }
#endif
    }
}