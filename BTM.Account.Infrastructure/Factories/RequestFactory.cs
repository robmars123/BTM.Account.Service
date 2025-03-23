using BTM.Account.Application.Factories.HttpRequest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace BTM.Account.Infrastructure.Factories
{
    public class RequestFactory : IRequestFactory
    {
        private readonly string ApiName = "AccountAPI";
        private readonly IHttpClientFactory _httpClientFactory;

        public RequestFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// Sends a POST request to the specified endpoint with the provided data
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendPostRequestAsync(string endpoint, object requestData, string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient(ApiName);

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Attach the token to the request header if available
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = jsonContent
            };
            return await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        }
    }
}
