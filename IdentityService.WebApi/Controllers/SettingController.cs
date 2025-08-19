using BuildingBlock.Logging;
using BuildingBlock.Web;
using IdentityService.Features.AppUsers.Commands;
using IdentityService.WebApi.Controllers.AppUser;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebApi.Controllers
{
    public class SettingController : BaseController
    {
        private readonly IAppLogger<AppUserController> _logger;
        private readonly ICurrentUserProvider _currentUserProvider;
        public SettingController(ISender sender, IAppLogger<AppUserController> logger, ICurrentUserProvider currentUserProvider) : base(sender)
        {
            _logger = logger;
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet]
        public async Task<int> PublishAppUser()
        {
            return await _sender.Send(new PublishAppUserCommand());
        }
    }
}
