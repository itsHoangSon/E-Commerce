using BuildingBlock.Exceptions;
using E_Commerce.Data;
using E_Commerce.Features.Product.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.Category.Commands
{
    public class UpdateProductCommand : IRequest<ProductModel>
    {
        public ProductModel Model { get; set; }
        public UpdateProductCommand(ProductModel model)
        {
            Model = model;
        }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductModel>
    {
        private readonly ECommerceDbContext _context;
        public UpdateProductCommandHandler(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<ProductModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;
                await Validate(model, cancellationToken);
                if (!model.IsValidated)
                {
                    return model;
                }
                var dbEntity = await _context.Product.FirstOrDefaultAsync(x => x.Id == model.Id);

                dbEntity.Code = model.Code;
                dbEntity.Name = model.Name;
                dbEntity.Description = model.Description;
                dbEntity.Price = model.Price;
                dbEntity.StockQuantity = model.StockQuantity;
                dbEntity.CategoryId = model.CategoryId;
                dbEntity.Status = model.Status;
                await _context.SaveChangesAsync(cancellationToken);
                
                return model;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }

        private async Task Validate(ProductModel model, CancellationToken cancellationToken)
        {
            var dbEntity = await _context.Product.AnyAsync(m => m.Id == model.Id, cancellationToken);
            if (!dbEntity)
            {
                model.AddError(nameof(model.Id), $"ProductId {model.Id} không tồn tại");
                return;
            }

            await model.CheckExistedCode(_context);
        }
    }
}
