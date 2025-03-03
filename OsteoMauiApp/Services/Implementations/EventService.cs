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

        #region Event
        public async Task<ResponseStatusModel> CreateAsync(EventModel model)
        {
            ResponseStatusModel Response = null;
            try
            {
                var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Event/CreateEvent";
                var payload = model.SerializeCreateEventFields();
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                using var httpClient = request.CreateHttpClient();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJJZCI6IjcyMCIsIlVzZXJuYW1lIjoiQW1lbGlhIE1hcmN1cyIsIlVzZXJUeXBlSWQiOiIxIiwiUGhvbmVOdW1iZXIiOiI3OTc5ODg2OTk5Iiwic3ViIjoiYW1lbGlhQG1haWxpbmF0b3IuY29tIiwiZW1haWwiOiJhbWVsaWFAbWFpbGluYXRvci5jb20iLCJqdGkiOiI4OGI4OTQxOC0zOTc0LTRmZTUtYjUwZC1mMmUwZGU3YjVjZmUiLCJuYmYiOjE3NDA5MDkyNTYsImV4cCI6MTc0MDkwOTg1NiwiaWF0IjoxNzQwOTA5MjU2LCJpc3MiOiJhbnkiLCJhdWQiOiJ0ZXN0In0.Rk7q9nbgCgRsYdG2un_PiN-auxD5-SAC6Qepj4hbbdI"; 
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                    return Response = null;
                }
                var serialized = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<ResponseStatusModel>(serialized, settings);
            }
            catch (Exception)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                return Response = null;
                
            }
        }
        public async Task<ResponseStatusModel> RescheduleEventAsync(RescheduleModel model)
        {
            ResponseStatusModel Response = null;
            try
            {
                var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Event/RescheduleEvent";
                var payload = model.SerializeRescheduleEventFields();
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                using var httpClient = request.CreateHttpClient();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJJZCI6IjcyMCIsIlVzZXJuYW1lIjoiQW1lbGlhIE1hcmN1cyIsIlVzZXJUeXBlSWQiOiIxIiwiUGhvbmVOdW1iZXIiOiI3OTc5ODg2OTk5Iiwic3ViIjoiYW1lbGlhQG1haWxpbmF0b3IuY29tIiwiZW1haWwiOiJhbWVsaWFAbWFpbGluYXRvci5jb20iLCJqdGkiOiI4OGI4OTQxOC0zOTc0LTRmZTUtYjUwZC1mMmUwZGU3YjVjZmUiLCJuYmYiOjE3NDA5MDkyNTYsImV4cCI6MTc0MDkwOTg1NiwiaWF0IjoxNzQwOTA5MjU2LCJpc3MiOiJhbnkiLCJhdWQiOiJ0ZXN0In0.Rk7q9nbgCgRsYdG2un_PiN-auxD5-SAC6Qepj4hbbdI";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                    return Response = null;
                }
                var serialized = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<ResponseStatusModel>(serialized, settings);
            }
            catch (Exception)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                return Response = null;
            }
        }
        public async Task<ResponseStatusModel> EventInviteAsync(EventInviteModel model)
        {
            ResponseStatusModel Response = null;
            var uri = "https://8wjtdmtj-44363.inc1.devtunnels.ms/api/" + "Event/InviteUser";//GlobalSettings.Instance.APIsBaseUrl + "Event/InviteUser";
            try
            {
                var _httpClient = new HttpClient();
                var n = model.SerializeInviteFields();
                var content = new StringContent(n);

                HttpClient httpClient;
                httpClient = request.CreateHttpClient();
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
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

        public async Task<EventListResponseModel> LoadMyEvents(EventRequestModel model)
        {
            var tempList = new EventListResponseModel();
            try
            {
                var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Event/AllEvents";
                var payload = model.SerializeEventRequestFilterFields();
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                using var httpClient = request.CreateHttpClient();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJJZCI6IjcyMCIsIlVzZXJuYW1lIjoiQW1lbGlhIE1hcmN1cyIsIlVzZXJUeXBlSWQiOiIxIiwiUGhvbmVOdW1iZXIiOiI3OTc5ODg2OTk5Iiwic3ViIjoiYW1lbGlhQG1haWxpbmF0b3IuY29tIiwiZW1haWwiOiJhbWVsaWFAbWFpbGluYXRvci5jb20iLCJqdGkiOiI4OGI4OTQxOC0zOTc0LTRmZTUtYjUwZC1mMmUwZGU3YjVjZmUiLCJuYmYiOjE3NDEwMDM4MTMsImV4cCI6MTc0MTAwNDQxMywiaWF0IjoxNzQxMDAzODEzLCJpc3MiOiJhbnkiLCJhdWQiOiJ0ZXN0In0.O7mS2dWSvxSWrMQj-qQ8gBO1AQfl6JfgnsBDk0V3lU4";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                    return tempList;
                }
                var serialized = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<EventListResponseModel>(serialized, settings);
            }
            catch (Exception)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                return tempList;
            }
        }
        public async Task<EventListResponseModel> LoadUpcomingNearbyEvents(EventRequestModel model)
        {
            var tempList = new EventListResponseModel();
            try
            {
                var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Event/UpcomingNearbyEvents";
                var payload = model.SerializeEventRequestFilterFields();
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                using var httpClient = request.CreateHttpClient();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJJZCI6IjcyMCIsIlVzZXJuYW1lIjoiQW1lbGlhIE1hcmN1cyIsIlVzZXJUeXBlSWQiOiIxIiwiUGhvbmVOdW1iZXIiOiI3OTc5ODg2OTk5Iiwic3ViIjoiYW1lbGlhQG1haWxpbmF0b3IuY29tIiwiZW1haWwiOiJhbWVsaWFAbWFpbGluYXRvci5jb20iLCJqdGkiOiI4OGI4OTQxOC0zOTc0LTRmZTUtYjUwZC1mMmUwZGU3YjVjZmUiLCJuYmYiOjE3NDA5ODUxMTcsImV4cCI6MTc0MDk4NTcxNywiaWF0IjoxNzQwOTg1MTE3LCJpc3MiOiJhbnkiLCJhdWQiOiJ0ZXN0In0.myi7mXrJdllsVSjQ3Iv-Wf9d3OYbvLK0dhBCY0PhqUw";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                    return tempList;
                }
                var serialized = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<EventListResponseModel>(serialized, settings);
            }
            catch (Exception)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                return tempList;
            }
        }
        public async Task<EventResponseModel> EventDetail(string guid)
        {
            var resp = new EventResponseModel();
            try
            {
                var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Event/Detail?id={guid}";
                using var httpClient = request.CreateHttpClient();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJJZCI6IjcyMCIsIlVzZXJuYW1lIjoiQW1lbGlhIE1hcmN1cyIsIlVzZXJUeXBlSWQiOiIxIiwiUGhvbmVOdW1iZXIiOiI3OTc5ODg2OTk5Iiwic3ViIjoiYW1lbGlhQG1haWxpbmF0b3IuY29tIiwiZW1haWwiOiJhbWVsaWFAbWFpbGluYXRvci5jb20iLCJqdGkiOiI4OGI4OTQxOC0zOTc0LTRmZTUtYjUwZC1mMmUwZGU3YjVjZmUiLCJuYmYiOjE3NDEwMTg3NzEsImV4cCI6MTc0MTAxOTM3MSwiaWF0IjoxNzQxMDE4NzcxLCJpc3MiOiJhbnkiLCJhdWQiOiJ0ZXN0In0.389Zh2HOVjqlAIv02BVZ5Nq_XVIXMTw8BKSF-CPSqY0";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                    return resp;
                }
                var serialized = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<EventResponseModel>(serialized, settings);
            }
            catch (Exception)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                return resp;
            }
        }

        public async Task<ParticepantResponseModel> LoadParticepants(EventSlotRequestModel model)
        {
            var tempList = new ParticepantResponseModel();
            try
            {
                var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Event/Eventparticipants";
                var payload = model.SerializeRequestFields();
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                using var httpClient = request.CreateHttpClient();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJJZCI6IjcyMCIsIlVzZXJuYW1lIjoiQW1lbGlhIE1hcmN1cyIsIlVzZXJUeXBlSWQiOiIxIiwiUGhvbmVOdW1iZXIiOiI3OTc5ODg2OTk5Iiwic3ViIjoiYW1lbGlhQG1haWxpbmF0b3IuY29tIiwiZW1haWwiOiJhbWVsaWFAbWFpbGluYXRvci5jb20iLCJqdGkiOiI4OGI4OTQxOC0zOTc0LTRmZTUtYjUwZC1mMmUwZGU3YjVjZmUiLCJuYmYiOjE3NDEwMTg3NzEsImV4cCI6MTc0MTAxOTM3MSwiaWF0IjoxNzQxMDE4NzcxLCJpc3MiOiJhbnkiLCJhdWQiOiJ0ZXN0In0.389Zh2HOVjqlAIv02BVZ5Nq_XVIXMTw8BKSF-CPSqY0";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                    return tempList;
                }
                var serialized = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<ParticepantResponseModel>(serialized, settings);
            }
            catch (Exception)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                return tempList;
            }
        }

        #endregion

        #region Appointments
        public async Task<EventSlotResponseModel> LoadUpcommingEventSlots(EventSlotRequestModel model)
        {
            var tempList = new EventSlotResponseModel();
            try
            {
                var uri = $"{GlobalSettings.Instance.APIsBaseUrl}Event/LoadUpcommingEventSlots";
                var payload = model.SerializeRequestFields();
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                using var httpClient = request.CreateHttpClient();
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJJZCI6IjcyMCIsIlVzZXJuYW1lIjoiQW1lbGlhIE1hcmN1cyIsIlVzZXJUeXBlSWQiOiIxIiwiUGhvbmVOdW1iZXIiOiI3OTc5ODg2OTk5Iiwic3ViIjoiYW1lbGlhQG1haWxpbmF0b3IuY29tIiwiZW1haWwiOiJhbWVsaWFAbWFpbGluYXRvci5jb20iLCJqdGkiOiI4OGI4OTQxOC0zOTc0LTRmZTUtYjUwZC1mMmUwZGU3YjVjZmUiLCJuYmYiOjE3NDEwMTgzMTcsImV4cCI6MTc0MTAxODkxNywiaWF0IjoxNzQxMDE4MzE3LCJpc3MiOiJhbnkiLCJhdWQiOiJ0ZXN0In0.uX6UGeWwAy79DrMD4rRN3TltMcnQ09k0mbmBR1Ot1Nk";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                    return tempList;
                }
                var serialized = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                };
                return JsonConvert.DeserializeObject<EventSlotResponseModel>(serialized, settings);
            }
            catch (Exception ex)
            {
                await (Application.Current as App).MainPage.DisplayAlert("Error", "Something went wrong, please try again later", "OK");
                return tempList;
            }
        }
        #endregion

    }
}
