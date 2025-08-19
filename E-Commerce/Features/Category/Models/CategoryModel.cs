using BuildingBlock.Models;
using BuildingBlock.Paginated;
using E_Commerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.Category.Models
{
    public class CategoryModel : DataModel
    {
        public CategoryModel() { }
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public async Task CheckExistedCategory(ECommerceDbContext context)
        {
            var isExistCategory = await context.Category.AnyAsync(x => x.Id == this.CategoryId);
            if (!isExistCategory) this.AddError(nameof(CategoryId), $"Loại sản phẩm {this.CategoryId} không tồn tại");
        }
    }

    public class CategoryFilterModel : PaginatedQuery
    {
        public long? CategoryId { get; set; }
        public string? Name { get; set; }
    }
}
