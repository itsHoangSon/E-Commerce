using BuildingBlock.Logging;
using BuildingBlock.Models;
using BuildingBlock.Paginated;
using BuildingBlock.Web;
using E_Commerce.Features.Category.Commands;
using E_Commerce.Features.Product.Commands;
using E_Commerce.Features.Product.Models;
using E_Commerce.Features.Product.Queries;
using ECommerceService.WebApi.Controllers.Product.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceService.WebApi.Controllers.Product
{
    public partial class ProductController : BaseController
    {
        private readonly IAppLogger<ProductController> _logger;
        private readonly ICurrentUserProvider _currentUserProvider;
        public ProductController(ISender sender, IAppLogger<ProductController> logger, ICurrentUserProvider currentUserProvider) : base(sender)
        {
            _logger = logger;
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<Product_GetResponseDto>>> List([FromQuery] Product_FilterDto request)
        {
            var FirmwareFilter = request.Adapt<ProductFilterModel>();
            var model = await _sender.Send(new GetPaginatedProductQuery(FirmwareFilter));
            PaginatedList<Product_GetResponseDto> result = new PaginatedList<Product_GetResponseDto>(model.Items.Select(x => new Product_GetResponseDto(x)).ToList(),
                model.TotalItems, model.PageIndex, model.PageSize);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product_GetResponseDto>> Get(long id)
        {
            var result = await _sender.Send(new GetProductQuery(id));
            Product_GetResponseDto response = new Product_GetResponseDto(result);
            if (result.IsValidated)
            {
                return response;
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuditableResponseDto>> Create([FromBody] Product_CreateRequestDto request)
        {
            ProductModel model = request.Adapt<ProductModel>();
            var result = await _sender.Send(new CreateProductCommand(model));
            var response = new AuditableResponseDto(result);
            if (result.IsValidated)
            {
                return response;
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<AuditableResponseDto>> Update([FromBody] Product_UpdateRequestDto request)
        {
            ProductModel model = request.Adapt<ProductModel>();
            var result = await _sender.Send(new UpdateProductCommand(model));
            var response = new AuditableResponseDto(result);
            if (result.IsValidated)
            {
                return response;
            }
            else
            {
                return BadRequest(response);
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(long id)
        //{
        //    await _sender.Send(new DeleteFirmwareCommand(id));
        //    return Ok();
        //}
    }
}
