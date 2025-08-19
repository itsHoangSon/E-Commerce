using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Entities
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTime? CreatedAt { get; set; }
        public long? CreatorId { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public long? LastModifierId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public interface IAuditableEntity
    {
        DateTime? CreatedAt { get; set; }
        long? CreatorId { get; set; }
        DateTime? LastModifiedAt { get; set; }
        long? LastModifierId { get; set; }
        bool IsDeleted { get; set; }
    }

}
