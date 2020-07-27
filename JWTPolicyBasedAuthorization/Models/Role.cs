using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JWTPolicyBasedAuthorization.Data;

namespace JWTPolicyBasedAuthorization.Models
{
    public class Role : BaseModelWithDelete
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool CanCreate { get; set; } = false;

        [Required]
        [DefaultValue(false)]
        public bool CanDelete { get; set; } = false;

        [Required]
        [DefaultValue(false)]
        public bool CanApprove { get; set; } = false;

        [Required]
        [DefaultValue(true)]
        public bool CanView { get; set; } = true;

        public IList<UserRoles> UserRoles { get; set; }

    }
}