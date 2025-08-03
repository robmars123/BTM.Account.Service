namespace BTM.Account.Application.Abstractions
{
    public interface IHttpRequestService
    {
        Task<HttpResponseMessage> SendPostRequestAsync(string context, string endpoint, object? requestData, string? accessToken);
        Task<HttpResponseMessage> GetRequestAsync(string context, string endpoint, object requestData, string accessToken);
    }
}
