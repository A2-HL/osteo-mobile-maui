using Newtonsoft.Json;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models.Authentication;
using OsteoMAUIApp.Models.User;
using OsteoMAUIApp.Services.Implementations;
using OsteoMAUIApp.Services.Interfaces;
using OsteoMAUIApp.Views.Authentication;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Task = System.Threading.Tasks.Task;


namespace OsteoMAUIApp.ViewModels.Authentication
{
   public class SignUpVM : BaseViewModel
    {
        private readonly IAuthenticationService _authService;

        public SignUpVM(INavigation navigation)
        {
            _databaseService = DependencyService.Get<IDatabaseService>();
            _navigation = navigation;
            _authService = new AuthenticationService(new RequestProvider(), _databaseService);
            _loginVM = new LoginVM(navigation);
            _signupDetails = new SignUpModel();
            LoginCommand = new Command(LoginClicked);
            SignupSubmitCommand = new Command(SignupSubmitClicked);
            InitializeCommand = new Command(Initialize);
            InitializeCommand.Execute(null);
        }

        #region |Private|
        bool isSessionChecking = true;
        INavigation _navigation;
        IDatabaseService _databaseService;
        LoginVM _loginVM;
        SignUpModel _signupDetails;
        #endregion

        #region |Public|
        public bool IsSessionChecking
        {
            get { return isSessionChecking; }
            set { SetProperty(ref isSessionChecking, value); }
        }

        public SignUpModel SignupDetails
        {
            get
            {
                return _signupDetails;
            }

            set
            {
                if (_signupDetails == value)
                {
                    return;
                }

                SetProperty(ref _signupDetails, value);
            }
        }

        #endregion

        #region |Commands|        
        public Command LoginCommand;
        public Command SignupSubmitCommand;
        public Command InitializeCommand;
        #endregion

        #region |Methods|
        private async void Initialize(object obj)
        {


        }
        private async void SignupSubmitClicked(object obj)
        {
            if (IsBusy) return;

            if (await SignupDetails.Validate())
            {
                StartBusyIndicator();
                await Task.Delay(500);
                try
                {
                    var fcm = await SecureStorage.GetAsync(GlobalSettings.FcmTokenKey);
                    if (fcm == null) { fcm = ""; }
                    var res = await _authService.SignupAsync(SignupDetails);
                    if (res != null)
                    {
                        if (res.statusCode == 200 || res.statusCode == 201)
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Success", res.statusMessage, "OK");
                            await _navigation.PopModalAsync();
                        }
                        else if (res.statusCode == 401 || res.statusCode == 402 || res.statusCode == 404)
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", res.statusMessage, "OK");
                        }
                        else if (res.statusCode == 503)
                        {
                            Utility.ShowToastMessage(res.statusMessage, Utility.ToastDuration.Long);
                        }
                        else
                        {
                            await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
                        }
                    }
                    else
                    {
                        await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoSignupMessage, "OK");
                    }
                }
                catch (System.Exception ex)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
                }
            }
            StopBusyIndicator();
        }

        private async void LoginClicked(object obj)
        {
            try
            {
                if (IsBusy) return;
                // Instantiate the signup page
                StartBusyIndicator();
                var loginPage = new Login();

                // Handle the OnAppearing event to stop the busy indicator when the page is fully loaded
                loginPage.Appearing += (s, e) =>
                {
                    StopBusyIndicator();
                };

                // Navigate to signup page
                await _navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or display it
                Console.WriteLine($"Error navigating to SignupPage: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                StopBusyIndicator();
            }
        }

        #endregion              
    }
}
