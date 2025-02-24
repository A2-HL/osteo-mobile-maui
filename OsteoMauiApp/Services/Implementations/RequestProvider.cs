using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OsteoMAUIApp.Services.Interfaces;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Views.Authentication;
using OsteoMAUIApp.Models.Authentication;
using OsteoMAUIApp.Models;

namespace OsteoMAUIApp.Services.Implementations
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }
        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            try
            {
                TResult result = default(TResult);
                //Check if internet is available else send no internet response
                if (!IsInternetAvailable())
                    return HandleResponse<TResult>(503, GlobalSettings.NoInternetMessage);

                HttpClient httpClient;
                if (!Helpers.Utility.IsLoggedIn || await Utility.IsSessionValid())
                {
                    httpClient = CreateHttpClient(await SecureStorage.GetAsync(GlobalSettings.AccessTokenKey));

                    HttpResponseMessage response = await httpClient.GetAsync(uri);

                    //Check if request is authorized else send unauthorized response
                    var isUnauthorized = await IsRequestUnauthorized(response);
                    if (isUnauthorized)
                    {
                        return HandleResponse<TResult>(401, GlobalSettings.UnauthorizedMessage);
                    }
                    string serialized = await response.Content.ReadAsStringAsync();
                    result = await Task.Run(() =>
                        JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

                }
                else
                {
                    (Application.Current as App).MainPage = new NavigationPage(new Login());
                }

                return result;
            }
            catch (Exception ex)
            {
                return HandleResponse<TResult>(404, GlobalSettings.FailedtoProcessMessage);
            }
        }
        public async Task<bool> PostBoolAsync(string uri, StringContent stringContent, string header = "")
        {
            //Check if internet is available else send no internet response
            if (!IsInternetAvailable())
                return false;

            if (!Helpers.Utility.IsLoggedIn || await Utility.IsSessionValid())
            {
                HttpClient httpClient;
                httpClient = CreateHttpClient(await SecureStorage.GetAsync(GlobalSettings.AccessTokenKey));

                if (!string.IsNullOrEmpty(header))
                {
                    AddHeaderParameter(httpClient, header);
                }

                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PostAsync(uri, stringContent);
                //Check if request is authorized else send unauthorized response
                var isUnauthorized = await IsRequestUnauthorized(response);
                if (isUnauthorized)
                {
                    return false;
                }
                if (!await IsAccountDeleted(response))
                {
                    if (response.IsSuccessStatusCode)
                        return true;
                }
                else
                {
                    (Application.Current as App).MainPage = new NavigationPage(new Login());
                }
            }
            else
            {
                (Application.Current as App).MainPage = new NavigationPage(new Login());
            }

            return false;
        }
        public async Task<TResult> PostAsync<TResult>(string uri, StringContent stringContent, string token = "", string header = "")
        {
            try
            {
                TResult result = default(TResult);
                //Check if internet is available else send no internet response
                if (!IsInternetAvailable())
                    return HandleResponse<TResult>(503, GlobalSettings.NoInternetMessage);

                if (!Helpers.Utility.IsLoggedIn)// || await Utility.IsSessionValid())
                {
                    HttpClient httpClient;
                    httpClient = CreateHttpClient(await SecureStorage.GetAsync(GlobalSettings.AccessTokenKey));

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }
                    if (stringContent != null)
                    {
                        stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    }
                    HttpResponseMessage response = await httpClient.PostAsync(uri, stringContent);

                    //Check if request is authorized else send unauthorized response
                    var isUnauthorized = await IsRequestUnauthorized(response);
                    if (isUnauthorized)
                    {
                        return HandleResponse<TResult>(401, GlobalSettings.UnauthorizedMessage);
                    }

                    string serialized = await response.Content.ReadAsStringAsync();

                    result = await Task.Run(() =>
                        JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                }
                else
                {
                    (Application.Current as App).MainPage = new NavigationPage(new Login());
                }

                return result;
            }
            catch (Exception ex)
            {
                return HandleResponse<TResult>(404, GlobalSettings.FailedtoProcessMessage);
            }
        }
        public async Task<TResult> PutAsync<TResult>(string uri, StringContent stringContent)
        {
            try
            {
                TResult result = default(TResult);
                //Check if internet is available else send no internet response
                if (!IsInternetAvailable())
                    return HandleResponse<TResult>(503, GlobalSettings.NoInternetMessage);

                if (!Helpers.Utility.IsLoggedIn)// || await Utility.IsSessionValid())
                {
                    HttpClient httpClient;
                    httpClient = CreateHttpClient(await SecureStorage.GetAsync(GlobalSettings.AccessTokenKey));

                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await httpClient.PutAsync(uri, stringContent);

                    //Check if request is authorized else send unauthorized response
                    var isUnauthorized = await IsRequestUnauthorized(response);
                    if (isUnauthorized)
                    {
                        return HandleResponse<TResult>(401, GlobalSettings.UnauthorizedMessage);
                    }
                    string serialized = await response.Content.ReadAsStringAsync();

                    result = await Task.Run(() =>
                        JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                }
                else
                {
                    (Application.Current as App).MainPage = new NavigationPage(new Login());
                }

                return result;
            }
            catch (Exception ex)
            {
                return HandleResponse<TResult>(404, GlobalSettings.FailedtoProcessMessage);
            }
        }
        public async Task<TResult> DeleteAsync<TResult>(string uri)
        {
            try
            {
                TResult result = default(TResult);
                //Check if internet is available else send no internet response
                if (!IsInternetAvailable())
                    return HandleResponse<TResult>(503, GlobalSettings.NoInternetMessage);

                if (!Helpers.Utility.IsLoggedIn)// || await Utility.IsSessionValid())
                {
                    HttpClient httpClient;
                    httpClient = CreateHttpClient(await SecureStorage.GetAsync(GlobalSettings.AccessTokenKey));

                    HttpResponseMessage response = await httpClient.DeleteAsync(uri);

                    //Check if request is authorized else send unauthorized response
                    var isUnauthorized = await IsRequestUnauthorized(response);
                    if (isUnauthorized)
                    {
                        return HandleResponse<TResult>(401, GlobalSettings.UnauthorizedMessage);
                    }
                    string serialized = await response.Content.ReadAsStringAsync();

                    result = await Task.Run(() =>
                        JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                }
                else
                {
                    (Application.Current as App).MainPage = new NavigationPage(new Login());
                }

                return result;
            }
            catch (Exception ex)
            {
                return HandleResponse<TResult>(404, GlobalSettings.FailedtoProcessMessage);
            }
        }
        public async Task<TResult> PostFormAsync<TResult>(string uri, MultipartFormDataContent formData)
        {
            try
            {
                TResult result = default(TResult);
                //Check if internet is available else send no internet response
                if (!IsInternetAvailable())
                    return HandleResponse<TResult>(503, GlobalSettings.NoInternetMessage);

                if (!Helpers.Utility.IsLoggedIn || await Utility.IsSessionValid())
                {
                    HttpClient httpClient;
                    httpClient = CreateHttpClient(await SecureStorage.GetAsync(GlobalSettings.AccessTokenKey));
                    HttpResponseMessage response;

                    response = await httpClient.PostAsync(uri, formData);
                    //Check if request is authorized else send unauthorized response
                    var isUnauthorized = await IsRequestUnauthorized(response);
                    if (isUnauthorized)
                    {
                        return HandleResponse<TResult>(401, GlobalSettings.UnauthorizedMessage);
                    }
                    string serialized = await response.Content.ReadAsStringAsync();
                    result = await Task.Run(() =>
                        JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                }
                else
                {
                    (Application.Current as App).MainPage = new NavigationPage(new Login());
                }

                return result;
            }
            catch (Exception ex)
            {
                return HandleResponse<TResult>(404, GlobalSettings.FailedtoProcessMessage);
            }
        }

        #region |Common Methods|
        public bool IsInternetAvailable()
        {
            var network = Connectivity.NetworkAccess;
            if (network != NetworkAccess.Internet)
            {
                //var backup = (Application.Current as App).MainPage;
                //(Application.Current as App).backupPage = backup;
                //(Application.Current as App).MainPage = new NoInternetPage();
                return false;
            }
            return true;
        }
        public HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }
        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }
        private async Task<bool> IsRequestUnauthorized(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    AuthenticationService _authService = new AuthenticationService(new RequestProvider(), DependencyService.Get<IDatabaseService>());
                    var res = await _authService.LogoutAsync();
                    return true;
                }
            }
            return false;
        }

        private TResult HandleResponse<TResult>(int statusCode, string statusMessage)
        {
            // Depending on the type of TResult, return an appropriate response
            if (typeof(TResult) == typeof(LoginResponseModel))
            {
                return (TResult)(object)new LoginResponseModel
                {
                    statusCode = statusCode,
                    statusMessage = statusMessage,
                    user = null
                };
            }
            else if (typeof(TResult) == typeof(ResponseStatusModel))
            {
                return (TResult)(object)new ResponseStatusModel
                {
                    statusCode = statusCode,
                    statusMessage = statusMessage
                };
            }

            // If TResult doesn't match a known model, return the default value
            return default(TResult);
        }

        private async Task<bool> IsAccountDeleted(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var serializedContent = await response.Content.ReadAsStringAsync();
                var result = await Task.Run(() =>
                    JsonConvert.DeserializeObject<ResponseStatusModel>(serializedContent, _serializerSettings));
                if (result.statusCode == 403)
                    return true;
                else if (!string.IsNullOrEmpty(result.statusMessage))
                {
                    (Application.Current as App).MainPage.DisplayAlert("Error", result.statusMessage, "OK");
                }
            }
            return false;
        }

        #endregion
    }
}
