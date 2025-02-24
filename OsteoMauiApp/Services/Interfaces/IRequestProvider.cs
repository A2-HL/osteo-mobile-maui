using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Services.Interfaces
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");
        Task<bool> PostBoolAsync(string uri, StringContent stringContent, string header = "");
        Task<TResult> PostAsync<TResult>(string uri, StringContent stringContent, string token = "", string header = "");
        Task<TResult> PutAsync<TResult>(string uri, StringContent stringContent);
        Task<TResult> DeleteAsync<TResult>(string uri);
        Task<TResult> PostFormAsync<TResult>(string uri, MultipartFormDataContent formData);
        bool IsInternetAvailable();
        HttpClient CreateHttpClient(string token = "");
    }
}
