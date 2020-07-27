using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTPolicyBasedAuthorization.Models
{
    /// <summary>
    /// Base Model
    /// </summary>
    public abstract class BaseModel : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? CreatedBy { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        
        public int? ModifiedBy { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

    }

    public abstract class BaseModelWithDelete : BaseModel
    {
        public int? DeletedBy { get; set; }
        
        public DateTime? DeletedDate { get; set; }
    }
}