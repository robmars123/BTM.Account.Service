using System.ComponentModel.DataAnnotations;

namespace BTM.Account.MVC.Client.ViewModels
{
    public class RegisterViewModel
    {
        [EmailAddress]
        [Display(Name = "Email Address")]
        public required string Email { get; set; }

        [Display(Name = "Username")]
        public required string Username { get; set; }

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public required string ConfirmPassword { get; set; }

        // Reset method
        public static RegisterViewModel Reset()
        {
            return new RegisterViewModel
            {
                // Initialize required properties with default values (empty strings in this case)
                Email = string.Empty,
                Username = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };
        }
    }

}
