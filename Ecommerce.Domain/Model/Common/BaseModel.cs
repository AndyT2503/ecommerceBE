using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain.Model.Common
{
    public class BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
