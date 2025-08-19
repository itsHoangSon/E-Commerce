using BuildingBlock.Exceptions;
using IdentityService.Data;
using IdentityService.Features.AppUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Commands
{
    public class LoginAppUserCommand : IRequest<AppUserModel>
    {
        public AppUserModel Model { get; set; }
        public LoginAppUserCommand(AppUserModel model)
        {
            Model = model;
        }
    }

    public class LoginAppUserCommandHandler : IRequestHandler<LoginAppUserCommand, AppUserModel>
    {
        private readonly IdentityDbContext _context;
        private readonly ISender _sender;
        public LoginAppUserCommandHandler(IdentityDbContext context, ISender sender)
        {
            _context = context;
            _sender = sender;
        }

        public async Task<AppUserModel> Handle(LoginAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appUser = request.Model;
                var dbAppUser = await _context.AppUser.FirstOrDefaultAsync(m => m.Username == appUser.Username);
                if (dbAppUser == null)
                {
                    appUser.AddError(nameof(appUser.Id), "Username không tồn tại");
                    return appUser;
                }

                if (!dbAppUser.CheckPasswordValid(appUser.Password))
                {
                    appUser.AddError(nameof(appUser.Id), "Mật khẩu không đúng");
                    return appUser;
                }
                appUser.Id = dbAppUser.Id;
                appUser.Token = await _sender.Send(new GenTokenCommand(appUser));

                return appUser;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }
    }
}
