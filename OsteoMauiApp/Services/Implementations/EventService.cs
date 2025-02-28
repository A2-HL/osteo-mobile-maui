using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using OsteoMAUIApp.Helpers;
using OsteoMAUIApp.Models;
using OsteoMAUIApp.Models.Event;
using OsteoMAUIApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Implementations
{
   public class EventService: IEventService
    {
        private readonly IRequestProvider request;
        public EventService(IRequestProvider requestProvider)
        {
            this.request = requestProvider;
        }
        public async Task<ResponseStatusModel> CreateAsync(EventModel model)
        {
            ResponseStatusModel Response = null;
            var uri = GlobalSettings.Instance.APIsBaseUrl + "Event/CreateEvent";
            try
            {
                var n = model.SerializeCreateEventFields();
                var content = new StringContent(n);

                HttpClient httpClient;
                httpClient = request.CreateHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
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
            catch (Exception)
            {
                Response = null;
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
            }
            return Response;
        }
        public async Task<ResponseStatusModel> RescheduleEventAsync(RescheduleModel model)
        {
            ResponseStatusModel Response = null;
            var uri = GlobalSettings.Instance.APIsBaseUrl + "Event/RescheduleEvent";
            try
            {
                var n = model.SerializeRescheduleEventFields();
                var content = new StringContent(n);

                HttpClient httpClient;
                httpClient = request.CreateHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
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
            catch (Exception)
            {
                Response = null;
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
            }
            return Response;
        }
    }
}
