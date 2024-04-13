using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WT.Trigger.Application.Common.Interfaces;

namespace WT.Trigger.Infrastructure.InternalHTTP
{
    public class InternalHttpService : IInternalHttpService
    {

        private readonly HttpClient _httpClient;

        public InternalHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationtoken, Dictionary<string, string> values = null)
        {
            if (values != null)
            {
                url = QueryHelpers.AddQueryString(url, values);
            }
            var jsonString = await _httpClient.GetStringAsync(url, cancellationtoken);
            return JsonConvert.DeserializeObject<TResponse>(jsonString);
        }

        public async Task<TResponse> PostJosnAsync<TResponse>(string url, object request, CancellationToken cancellationtoken)
        {
            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationtoken);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(jsonString);
        }
    }
}
