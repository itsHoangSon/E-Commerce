using BuildingBlock.Exceptions;
using E_Commerce.Data;
using E_Commerce.Entities;
using E_Commerce.Features.Product.Models;
using ProductEntity = E_Commerce.Entities.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.Product.Commands
{
    public class CreateProductCommand : IRequest<ProductModel>
    {
        public ProductModel Model { get; set; }
        public CreateProductCommand(ProductModel model)
        {
            Model = model;
        }

    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductModel>
    {
        private readonly ECommerceDbContext _context;
        private readonly IMediator _mediator;
        public CreateProductCommandHandler(ECommerceDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ProductModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;

                if (!await Validate(model, cancellationToken))
                {
                    return model;
                }

                ProductEntity Firmware = new ProductEntity();
                Firmware.Code = model.Code;
                Firmware.Name = model.Name;
                Firmware.Description = model.Description;

                await _context.Product.AddAsync(Firmware);
                await _context.SaveChangesAsync(cancellationToken);
                model.Id = Firmware.Id;
                return model;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }

        private async Task<bool> Validate(ProductModel model, CancellationToken cancellationToken)
        {
            await model.CheckExistedCode(_context);

            return model.IsValidated;
        }
    }
}
