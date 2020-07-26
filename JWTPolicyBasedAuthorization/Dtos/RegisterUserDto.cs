using System.ComponentModel.DataAnnotations;
using JWTPolicyBasedAuthorization.Models;

namespace JWTPolicyBasedAuthorization.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "User name must be between 4 to 20 characters")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Specify the password between 6 to 20 characters")]
        public string Password { get; set; }

        public string RoleName { get; set; }
    }
}