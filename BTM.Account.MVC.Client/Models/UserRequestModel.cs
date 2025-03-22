using System.ComponentModel.DataAnnotations;

namespace BTM.Account.MVC.Client.Models
{
    public class UserRequestModel
    {
        public required string Email { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
