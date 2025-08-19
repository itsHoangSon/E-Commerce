using BuildingBlock.Models;
using BuildingBlock.Paginated;
using E_Commerce.Data;
using E_Commerce.Entities;
using E_Commerce.Enums;
using E_Commerce.Features.Category.Models;
using E_Commerce.Features.ProductImage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.Product.Models
{
    public class ProductModel : DataModel
    {
        public ProductModel() { }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; } = 0;
        public long? CategoryId { get; set; } 
        public CategoryModel? Category { get; set; } 
        public StatusEnum Status { get; set; } = StatusEnum.Active;
        public async Task CheckExistedCode(ECommerceDbContext context)
        {
            var isExistUsername = await context.Product.AnyAsync(x => x.Id != this.Id && x.Code == this.Code);
            if (isExistUsername) this.AddError(nameof(Code), $"Mã sản phẩm {this.Code} đã tồn tại");
        }
    }

    public class ProductFilterModel : PaginatedQuery
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public StatusEnum? Status { get; set; }
        public long? CategỏyId { get; set; }
    }
}
