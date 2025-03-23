namespace BTM.Account.Application.Factories.HttpRequest
{
    public interface IRequestFactory
    {
        /// <summary>
        /// Sends a POST request to the specified endpoint with the provided data
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> SendPostRequestAsync(string endpoint, object requestData, string accessToken);
    }
}
