using BuildingBlock.Entities;
using E_Commerce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities
{
    public class AppUser : AuditableEntity
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string Username { get; set; } = String.Empty;
        public bool IsAdmin { get; set; }
        public StatusEnum Status { get; set; }
    }
}
