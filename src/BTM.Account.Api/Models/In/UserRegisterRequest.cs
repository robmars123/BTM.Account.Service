namespace BTM.Account.Api.Models.In
{
    public sealed record UserRegisterRequest(
        string Email,
        string Username,
        string Password
    );
}
