using BuildingBlock.Exceptions;
using E_Commerce.Data;
using E_Commerce.Features.Category.Models;
using E_Commerce.Entities;
using CategoryEntity = E_Commerce.Entities.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.Category.Commands
{
    public class CreateCategoryCommand : IRequest<CategoryModel>
    {
        public CategoryModel Model { get; set; }
        public CreateCategoryCommand(CategoryModel model)
        {
            Model = model;
        }
    }
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryModel>
    {
        private readonly ECommerceDbContext _context;
        private readonly IMediator _mediator;
        public CreateCategoryCommandHandler(ECommerceDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<CategoryModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;

                if (!await Validate(model, cancellationToken))
                {
                    return model;
                }

                var category = new CategoryEntity();
                category.Name = model.Name;
                category.Description = model.Description;

                await _context.Category.AddAsync(category);
                await _context.SaveChangesAsync(cancellationToken);

                return model;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }

        private async Task<bool> Validate(CategoryModel model, CancellationToken cancellationToken)
        {
            return model.IsValidated;
        }
    }
}
