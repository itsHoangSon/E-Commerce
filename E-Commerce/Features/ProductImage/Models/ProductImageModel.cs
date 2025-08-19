using BuildingBlock.Models;
using BuildingBlock.Paginated;
using E_Commerce.Data;
using E_Commerce.Features.Product.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.ProductImage.Models
{
    public class ProductImageModel : DataModel
    {
        public ProductImageModel() { }
        public long ProductImageId { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsMain { get; set; }
        public ProductModel ProductId { get; set; }

        public async Task CheckExistedProductImage(ECommerceDbContext context)
        {
            var isExistProductImage = await context.ProductImage.AnyAsync(x => x.Id == this.ProductImageId);
            if (!isExistProductImage) this.AddError(nameof(ProductImageId), $"Hình ảnh {this.ProductImageId} không tồn tại");
        }
    }

    public class CategoryImageFilterModel : PaginatedQuery
    {
        public long? CategoryId { get; set; }
        public string? Name { get; set; }
    }
}
