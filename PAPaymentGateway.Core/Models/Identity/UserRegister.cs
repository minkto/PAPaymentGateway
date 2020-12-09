using System.ComponentModel.DataAnnotations;

namespace PAPaymentGateway.Core.Models
{
    public class UserRegister
    {
        public string Username { get; set; }

        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
