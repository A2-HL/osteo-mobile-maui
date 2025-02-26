using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Nodes;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models.Authentication;
using OsteoMAUIApp.Services.Interfaces;
using OsteoMAUIApp.Views.Authentication;
using OsteoMAUIApp.Models;

namespace OsteoMAUIApp.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestProvider request;
        private readonly IDatabaseService databaseService;
        readonly Uri baseUri = new Uri(GlobalSettings.Instance.APIsBaseUrl);

        public AuthenticationService(IRequestProvider requestProvider, IDatabaseService databaseService)
        {
            this.request = requestProvider;
            this.databaseService = databaseService;
        }
        public async Task<LoginResponseModel> LoginAsync(LoginModel credentials)
        {
            LoginResponseModel Response = null;
            var uri = GlobalSettings.Instance.APIsBaseUrl + "Account/Signin";

            try
            {
                var n = credentials.SerializeLoginFields();
                var content = new StringContent(n);

                if (!Helpers.Utility.IsLoggedIn || await Utility.IsSessionValid())
                {
                    HttpClient httpClient;
                    httpClient = request.CreateHttpClient(GlobalSettings.Instance.access_token);


                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                    string serialized = await response.Content.ReadAsStringAsync();
                    var _serializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    _serializerSettings.Converters.Add(new StringEnumConverter());
                    Response = await Task.Run(() =>
                        JsonConvert.DeserializeObject<LoginResponseModel>(serialized, _serializerSettings));
                }
                else
                {
                    (Application.Current as App).MainPage = new NavigationPage(new Login());
                }
                return Response;
            }
            catch (Exception ex)
            {
                Response = null;
                await (Application.Current as App).MainPage.DisplayAlert("Error", GlobalSettings.FailedtoProcessMessage, "OK");
            }
            return Response;
        }
        public async Task<ResponseStatusModel> SignupAsync(SignUpModel credentials)
        {
            ResponseStatusModel Response = null;
            var uri = GlobalSettings.Instance.APIsBaseUrl + "Account/SignUp";
            try
            {
                var n = credentials.SerializeSignupFields();
                var content = new StringContent(n);

                HttpClient httpClient;
                httpClient = request.CreateHttpClient();


                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                string serialized = await response.Content.ReadAsStringAsync();
                var _serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore
                };
                _serializerSettings.Converters.Add(new StringEnumConverter());
                var result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<ResponseStatusModel>(serialized, _serializerSettings));
                
                Response = result;
            }
            catch (Exception ex)
            {
                Response = null;
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
            }
            return Response;
        }
        public Task<bool> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponseModel> RefreshAccessToken(string refreshToken, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
