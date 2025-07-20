namespace BTM.Account.Domain.Abstractions
{
    public record Error(string Code, string Name)
    {
        public static Error None = new(string.Empty, string.Empty);

        public static Error NullValue = new("Error.NullValue", "Null value was provided");
        public static Error UserNotFound = new("Error.UserNotFound", "User not found");
        public static Error UserAlreadyExists = new("Error.UserAlreadyExists", "User already exists");
    }
}
