using BTM.Account.Application.Abstractions;
using BTM.Account.Shared.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BTM.Account.Infrastructure.Services
{
    public class HttpRequestService : IHttpRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpRequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> SendPostRequestAsync(string endpoint, object? requestData, string? accessToken = null)
        {
            var httpClient = _httpClientFactory.CreateClient(GlobalConstants.ApiConstants.AccountAPI); //TODO: Make generic

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = jsonContent
            };
            return await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        }

        public async Task<HttpResponseMessage> GetRequestAsync(string endpoint, object requestData, string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient(GlobalConstants.ApiConstants.AccountAPI);

            // Add the token to the request headers if provided
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            // If requestData is not null and needs to be sent, convert it to query parameters
            if (requestData != null)
            {
                var queryString = await ToQueryStringAsync(requestData);
                endpoint = $"{endpoint}?{queryString}";
            }

            // Create a GET request
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, endpoint);

            // Send the request
            var result = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
            return result;
        }

        private async Task<string> ToQueryStringAsync(object requestData)
        {
            var queryString = new List<string>();

            foreach (var property in requestData.GetType().GetProperties())
            {
                var value = property.GetValue(requestData);
                if (value != null)
                {
                    queryString.Add($"{Uri.EscapeDataString(property.Name)}={Uri.EscapeDataString(value.ToString())}");
                }
            }

            return string.Join("&", queryString);
        }
    }
}
