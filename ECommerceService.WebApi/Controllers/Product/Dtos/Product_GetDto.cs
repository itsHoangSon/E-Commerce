using BuildingBlock.Models;
using BuildingBlock.Paginated;
using E_Commerce.Entities;
using E_Commerce.Enums;
using E_Commerce.Features.Category.Models;
using E_Commerce.Features.Product.Models;
using E_Commerce.Features.ProductImage.Models;

namespace ECommerceService.WebApi.Controllers.Product.Dtos
{
    public class Product_GetResponseDto : AuditableResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; } = 0;
        public CategoryModel Category { get; set; }
        public List<ProductImageModel> ProductImage { get; set; } = new List<ProductImageModel>();
        public Product_GetResponseDto(ProductModel model) : base(model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            Price = model.Price;
            StockQuantity = model.StockQuantity;
            Category = model.Category ?? new CategoryModel();
        }
    }
    public class Product_FilterDto : PaginatedQuery
    {
        public long? CategoryId { get; set; }
        public StatusEnum? Status { get; set; }
    }
}
