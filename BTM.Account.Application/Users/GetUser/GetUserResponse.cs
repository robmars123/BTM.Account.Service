namespace BTM.Account.Application.Users.GetUser
{
    public sealed class GetUserResponse
    {
        public Guid Id { get; init; }

        public string Email { get; init; }

        public string Username { get; init; }
    }
}
