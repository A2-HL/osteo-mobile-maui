using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models.Authentication;
using OsteoMAUIApp.Services.Interfaces;
using OsteoMAUIApp.Models;
using OsteoMAUIApp.Views.Authentication;

namespace OsteoMAUIApp.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IDatabaseService _databaseService;

        public AuthenticationService(IRequestProvider requestProvider, IDatabaseService databaseService)
        {
            _requestProvider = requestProvider;
            _databaseService = databaseService;
        }

        public async Task<LoginResponseModel> LoginAsync(LoginModel credentials)
        {
            var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Account/Signin";
            var content = new StringContent(credentials.SerializeLoginFields(), Encoding.UTF8, "application/json");

            if (!Helpers.Utility.IsLoggedIn || await Utility.IsSessionValid())
            {
                return await ExecutePostAsync<LoginResponseModel>(uri, content);
            }
            else
            {
                (Application.Current as App).MainPage = new NavigationPage(new Login());
                return null;
            }
        }

        public async Task<ResponseStatusModel> SignupAsync(SignUpModel credentials)
        {
            var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Account/SignUp";
            var content = new StringContent(credentials.SerializeSignupFields(), Encoding.UTF8, "application/json");
            return await ExecutePostAsync<ResponseStatusModel>(uri, content);
        }

        public async Task<ResponseStatusModel> ForgotPasswordAsync(LoginModel credentials)
        {
            var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Account/ForgotPassword";
            var content = new StringContent(credentials.SerializeForgotPasswordFields(), Encoding.UTF8, "application/json");
            return await ExecutePostAsync<ResponseStatusModel>(uri, content);
        }

        public async Task<LoginResponseModel> RefreshAccessToken(string refreshToken, string userId)
        {
            var uri = $"{GlobalSettings.Instance.APIsBaseUrl}drivers/refresh-tokens";
            var body = new { driverId = userId, refreshToken = refreshToken };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            return await ExecutePostAsync<LoginResponseModel>(uri, content);
        }

        public async Task<ResponseStatusModel> ResendVerificationEmail(string emailAddress)
        {
            var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Account/ResendVerificationEmail?email={emailAddress}";
            return await ExecuteGetAsync<ResponseStatusModel>(uri);
        }

        #region Helper Methods
        private async Task<T> ExecutePostAsync<T>(string uri, HttpContent content)
        {
            try
            {
                var httpClient = _requestProvider.CreateHttpClient(GlobalSettings.Instance.access_token);
                var response = await httpClient.PostAsync(uri, content);
                var serialized = await response.Content.ReadAsStringAsync();
                return DeserializeResponse<T>(serialized);
            }
            catch (Exception ex)
            {
                await ShowErrorAlert("Something went wrong, please try again later.");
                return default;
            }
        }

        private async Task<T> ExecuteGetAsync<T>(string uri)
        {
            try
            {
                var httpClient = _requestProvider.CreateHttpClient(GlobalSettings.Instance.access_token);
                var response = await httpClient.GetAsync(uri);
                var serialized = await response.Content.ReadAsStringAsync();
                return DeserializeResponse<T>(serialized);
            }
            catch (Exception ex)
            {
                await ShowErrorAlert("Something went wrong, please try again later.");
                return default;
            }
        }

        private T DeserializeResponse<T>(string serialized)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
            return JsonConvert.DeserializeObject<T>(serialized, serializerSettings);
        }

        private async Task ShowErrorAlert(string message)
        {
            await (Application.Current as App).MainPage.DisplayAlert("Error", message, "OK");
        }
        #endregion
    }
}