using BuildingBlock.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
    }
}
