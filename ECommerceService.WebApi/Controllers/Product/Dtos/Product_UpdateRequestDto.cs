using BuildingBlock.Models;
using E_Commerce.Enums;
using E_Commerce.Features.Product.Models;

namespace ECommerceService.WebApi.Controllers.Product.Dtos
{
    public class Product_UpdateRequestDto : DataRequestDto
    {
        public Product_UpdateRequestDto() { }

        public required long Id { get; set; }
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public long CategoryId { get; set; }
        public StatusEnum Status { get; set; }

    }
    public class Product_UpdateResponseDto : AuditableResponseDto
    {
        public Product_UpdateResponseDto(ProductModel model) : base(model)
        {
            Id = model.Id;
        }
        public long Id { get; set; }
    }
}
