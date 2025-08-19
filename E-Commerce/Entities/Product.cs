using BuildingBlock.Entities;
using E_Commerce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities
{
    public class Product : AuditableEntity
    {
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public Category? Category { get; set; }
        public long? CategoryId { get; set; }
        public StatusEnum Status { get; set; }
    }
}
