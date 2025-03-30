using System.ComponentModel.DataAnnotations;

namespace BTM.Account.MVC.UI.Models
{
    public class UserDTO
    {
        [Display(Name = "Email Address")]
        public string Email { get; init; }

        [Display(Name = "Username")]
        public string Username { get; init; }

        [Display(Name = "Password")]
        public string Password { get; init; }
    }
}
