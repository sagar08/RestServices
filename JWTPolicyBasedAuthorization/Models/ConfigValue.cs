using System.ComponentModel.DataAnnotations;

namespace JWTPolicyBasedAuthorization.Models
{
    public class ConfigValue : IEntity
    {
        public int Id { get; set; }

        [Required]
        public int KeyId { get; set; }

        [Required]
        [StringLength(120, ErrorMessage = "Config Value length should not exceed 500 chars")]
        public string Value { get; set; }

        [StringLength(500, ErrorMessage = "Config Key length should not exceed 500 chars")]
        public string Description { get; set; }

        public int Rank { get; set; }

        public virtual ConfigKey ConfigKey { get; set; }
    }
}