using BuildingBlock.Exceptions;
using E_Commerce.Data;
using E_Commerce.Features.Product.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.Product.Queries
{
    public class GetProductQuery : IRequest<ProductModel>
    {
        public GetProductQuery()
        {

        }
        public GetProductQuery(long id)
        {
            ProductId = id;
        }
        public long ProductId { get; set; }
    }
    public class GetFirmwareQueryHandler : IRequestHandler<GetProductQuery, ProductModel>
    {

        private readonly ECommerceDbContext _context;
        private readonly IMediator _mediator;
        private readonly StaticParams _staticParams;
        public GetFirmwareQueryHandler(ECommerceDbContext context, IMediator mediator, IOptionsMonitor<StaticParams> staticParams)
        {
            _context = context;
            _mediator = mediator;
            _staticParams = staticParams.CurrentValue;
        }

        public async Task<ProductModel> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Product.Where(m => m.Id == request.ProductId).AsNoTracking()
                    .ProjectToType<ProductModel>().FirstOrDefaultAsync();
                if (entity == null)
                {
                    entity = new ProductModel();
                    entity.AddError(nameof(entity.Id), $"ProductId {request.ProductId} không tồn tại");
                }

                return entity;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }
    }
}
