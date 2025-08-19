using BuildingBlock.Authorization;
using BuildingBlock.Exceptions;
using IdentityService.Data;
using IdentityService.Features.AppUsers.Models;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Commands
{
    public class GenTokenCommand : IRequest<string>
    {
        public AppUserModel Model { get; set; }
        public GenTokenCommand(AppUserModel model)
        {
            Model = model;
        }
    }

    public class GenTokenCommandHandler : IRequestHandler<GenTokenCommand, string>
    {
        private readonly IdentityDbContext _context;
        private readonly ISender _sender;
        private readonly StaticParams _staticParams;
        public GenTokenCommandHandler(IdentityDbContext context, ISender sender, IOptionsMonitor<StaticParams> staticParams)
        {
            _context = context;
            _sender = sender;
            _staticParams = staticParams.CurrentValue;
        }

        public async Task<string> Handle(GenTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var systemUser = request.Model;
                long userId = systemUser.Id;
                string username = systemUser.Username;
                bool isAdmin = systemUser.IsAdmin;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);

                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

                if (isAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var expiredDays = TimeSpan.FromDays(_staticParams.ExpiredTime);
                var now = DateTime.Now;
                var expires = now.Add(expiredDays);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims.ToArray()),
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }

    }
}
