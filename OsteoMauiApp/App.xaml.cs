using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Services.Implementations;
using OsteoMAUIApp.Services.Interfaces;
using OsteoMAUIApp.Views.Authentication;
using OsteoMAUIApp.Views.Home;

namespace OsteoMAUIApp
{
    public partial class App : Application
    {
        private bool _isAlertShown = false;
        private IAppSettingsService _appSettingsService;
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1fdnRSRmlYV0x+V0Y=");// TapGrap SF Key V28.X.X
            InitializeComponent();

            // Set Login page as the startup page
            MainPage = new NavigationPage(new AppShell());

            //Register the database service
            DependencyService.Register<IDatabaseService, DatabaseService>();
            var databaseService = DependencyService.Get<IDatabaseService>();

            //Initialize the database 
            System.Threading.Tasks.Task.Run(async () =>
            {
                await databaseService.InitializeAsync();
            });
            _appSettingsService = new AppSettingsService(new RequestProvider(), databaseService);

            //Initialize the app
            InitializeAppAsync();
        }

        private async void InitializeAppAsync()
        {
            // Check if the user is logged in
            if (await Utility.IsSessionValid())
            {
                // User is logged in, navigate to AppShell (with tabs)
                MainPage = new AppShell();
            }
            else
            {
                // User is not logged in, navigate to the LoginPage
                MainPage = new NavigationPage(new AppShell());
            }
        }

        protected async override void OnStart()
        {
            await _appSettingsService.UpdateAppRefreshFlags(true);
            CheckIfAppUpdateRequired();
            RequestNotificationPermission();
        }

        private async void CheckIfAppUpdateRequired()
        {
            //IsAlertShown flag prevents opening multiple alerts if the alert is already opened. 
            if (_isAlertShown) return;
            IRemoteConfigService _remoteConfigService;
            try
            {
                //Get the remote config service
                _remoteConfigService = DependencyService.Get<IRemoteConfigService>();
                var (updateRequired, forceUpdate) = await _remoteConfigService.CheckAppUpdateRequiredAsync();
                //Whether update is required
                if (updateRequired)
                {
                    //Is it a force update 
                    if (forceUpdate)
                    {
                        _isAlertShown = true;
                        await Application.Current.MainPage.DisplayAlert("Update Required", GlobalSettings.ForceUpdateMessage, "Update");

                        OpenAppStore();
                        _isAlertShown = false;
                    }
                    else
                    {
                        _isAlertShown = true;
                        var answer = await Application.Current.MainPage.DisplayAlert("Update Available", GlobalSettings.OptionalUpdateMessage, "Update", "Later");
                        if (answer)
                        {
                            OpenAppStore();
                        }
                        _isAlertShown = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OpenAppStore()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                Launcher.OpenAsync(new Uri(GlobalSettings.AndroidPlayStoreURL));
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                Launcher.OpenAsync(new Uri(GlobalSettings.IOSStoreURL));
            }
        }

       
        protected async override void OnSleep()
        {
            await _appSettingsService.UpdateAppRefreshFlags(true);
        }

        protected override void OnResume()
        {
            CheckIfAppUpdateRequired();
            RequestNotificationPermission();
        }

        private async void RequestNotificationPermission()
        {
            if (!await Utility.NotificationPermissionsAllowed())
            {
                bool shouldOpenSettings = await (Application.Current as App).MainPage.DisplayAlert("Why Allow Notification Access?", GlobalSettings.WhyAllowNotificationAccessMessage, "Go to Settings", "Cancel");
                if (shouldOpenSettings)
                {
                    Utility.OpenAppSettings();
                }
                return;
            }
        }
    }
}
