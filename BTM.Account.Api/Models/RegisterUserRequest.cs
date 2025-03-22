namespace BTM.Account.Api.Models
{
    public sealed record RegisterUserRequest(
        string Email,
        string Username,
        string Password
    );
}
