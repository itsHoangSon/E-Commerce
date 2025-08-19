using BuildingBlock.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities
{
    public class ProductImage : AuditableEntity
    {
        public string ImageUrl { get; set; } = String.Empty;
        public bool IsMain { get; set; } = false;
        public string ProductId { get; set; } = String.Empty;
        public Product? Product { get; set; }
    }
}
