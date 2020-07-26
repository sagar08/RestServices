using System.ComponentModel.DataAnnotations;

namespace JWTPolicyBasedAuthorization.Dtos
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Specify the password between 6 to 20 characters")]
        public string Password { get; set; }
    }
}