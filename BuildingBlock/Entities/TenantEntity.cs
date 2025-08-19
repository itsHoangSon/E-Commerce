using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Entities
{
    public abstract class TenantEntity : AuditableEntity, ITenantEntity
    {
        public long TenantId { get; set; }
    }

    public interface ITenantEntity
    {
        long TenantId { get; set; }
    }
}
