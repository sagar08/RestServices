using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JWTPolicyBasedAuthorization.Models
{
    public class ConfigKey : IEntity
    {
        public ConfigKey()
        {
            ConfigValues = new HashSet<ConfigValue>();
        }
        
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Config Key length should not exceed 20 chars")]
        public string Key { get; set; }

        [StringLength(500, ErrorMessage = "Config Key length should not exceed 500 chars")]
        public string Description { get; set; }

        public ICollection<ConfigValue> ConfigValues { get; set; }
    }
}