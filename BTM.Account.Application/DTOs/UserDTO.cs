using System.ComponentModel.DataAnnotations;

namespace BTM.Account.Application.DTOs
{
    public class UserDTO
    {
        [Display(Name = "Email Address")]
        public string Email { get; init; }

        [Display(Name = "Username")]
        public string Username { get; init; }
    }
}
