namespace BTM.Account.Application.Factories.HttpRequest
{
    public interface IRequestFactory
    {
        Task<HttpResponseMessage> SendPostRequestAsync(string endpoint, object requestData, string accessToken);
        Task<HttpResponseMessage> GetRequestAsync(string endpoint, object requestData, string accessToken);
    }
}
