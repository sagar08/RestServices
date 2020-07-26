using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTPolicyBasedAuthorization.Models
{
    public class User : BaseModel
    {
        public User()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "User name must be between 4 to 20 characters")]
        public string UserName { get; set; }
        
        [Required]
        public byte[] PasswordHash { get; set; }
        
        [Required]
        public byte[] PasswordSalt { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }

    }
}