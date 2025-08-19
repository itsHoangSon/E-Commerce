using BuildingBlock.Models;
using E_Commerce.Enums;
using E_Commerce.Features.Product.Models;

namespace ECommerceService.WebApi.Controllers.Product.Dtos
{
    public class Product_CreateRequestDto : DataRequestDto
    {
        public Product_CreateRequestDto() { }
        public required string Code { get; set; } = string.Empty;
        public required string Name { get; set; }
        public required long RelayBrandId { get; set; }
        public string? Description { get; set; }
        public StatusEnum Status { get; set; }
    }

    public class Product_CreateResponseDto : AuditableResponseDto
    {
        public Product_CreateResponseDto(ProductModel model) : base(model)
        {
            Id = model.Id;
        }
        public long Id { get; set; }
    }

}
