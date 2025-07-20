namespace BTM.Account.Application.Abstractions
{
    public interface IHttpRequestService
    {
        Task<HttpResponseMessage> SendPostRequestAsync(string endpoint, object? requestData, string? accessToken);
        Task<HttpResponseMessage> GetRequestAsync(string endpoint, object requestData, string accessToken);
    }
}
