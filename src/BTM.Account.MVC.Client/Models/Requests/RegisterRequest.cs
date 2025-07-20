using System.ComponentModel.DataAnnotations;

namespace BTM.Account.MVC.UI.Models.Requests
{
    public sealed class RegisterRequest
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; init; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; init; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; init; }

        public RegisterRequest()
        {
            
        }
        public RegisterRequest(string email, string username, string password, string confirmPassword)
        {
            // Use string.Empty if parameter is null
            Email = email ?? string.Empty;
            Username = username ?? string.Empty;
            Password = password ?? string.Empty;
            ConfirmPassword = confirmPassword ?? string.Empty;

            // Optional: Check if password and confirm password match
            if (Password != ConfirmPassword)
            {
                throw new ArgumentException("Password and confirmation password do not match.", nameof(confirmPassword));
            }
        }

        // Reset method
        public static RegisterRequest Reset()
        {
            return new RegisterRequest(string.Empty, string.Empty, string.Empty, string.Empty);
        }
    }

}
