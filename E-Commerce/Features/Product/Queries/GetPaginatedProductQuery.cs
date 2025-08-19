using BuildingBlock.Exceptions;
using BuildingBlock.Paginated;
using E_Commerce.Data;
using E_Commerce.Features.Product.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.Product.Queries
{
    public class GetPaginatedProductQuery : IRequest<PaginatedList<ProductModel>>
    {
        public ProductFilterModel ProductFilterModel;
        public GetPaginatedProductQuery(ProductFilterModel filter)
        {
            ProductFilterModel = filter;
        }
    }

    public class GetPaginatedFirmwaresHandler : IRequestHandler<GetPaginatedProductQuery, PaginatedList<ProductModel>>
    {

        private readonly ECommerceDbContext _context;
        public GetPaginatedFirmwaresHandler(ECommerceDbContext context)
        {
            _context = context;
        }


        public async Task<PaginatedList<ProductModel>> Handle(GetPaginatedProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = request.ProductFilterModel;
                var query = _context.Product.AsNoTracking();
                if (filter.Search != null)
                {
                    query = query.Where(x => x.Code.Contains(filter.Search)
                    || (x.Name != null && x.Name.Contains(filter.Search)));
                }
                if (filter.Code != null)
                {
                    query = query.Where(x => x.Code.Contains(filter.Code));
                }
                if (filter.Name != null)
                {
                    query = query.Where(x => x.Name != null && x.Name.Contains(filter.Name));
                }
                if (filter.Status != null)
                {
                    query = query.Where(x => x.Status == filter.Status);
                }

                var total = await query.CountAsync();
                var items = await query.OrderByDescending(x => x.CreatedAt).ProjectToType<ProductModel>().ToPagedAsync(filter, cancellationToken);
                return new PaginatedList<ProductModel>(items, total, filter.PageIndex, filter.PageSize);

            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }
    }
}
