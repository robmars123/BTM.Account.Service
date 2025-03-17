namespace BTM.Account.Application.Abstractions
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string enteredPassword);
    }
}