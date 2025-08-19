using BuildingBlock.Logging;
using BuildingBlock.Models;
using BuildingBlock.Paginated;
using BuildingBlock.Web;
using IdentityService.Features.AppUsers.Commands;
using IdentityService.Features.AppUsers.Models;
using IdentityService.Features.AppUsers.Queries;
using IdentityService.WebApi.Controllers.AppUser.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.WebApi.Controllers.AppUser
{
    public partial class AppUserController : BaseController
    {
        private readonly IAppLogger<AppUserController> _logger;
        private readonly ICurrentUserProvider _currentUserProvider;
        public AppUserController(ISender sender, IAppLogger<AppUserController> logger, ICurrentUserProvider currentUserProvider) : base(sender)
        {
            _logger = logger;
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<AppUser_GetResponseDto>>> List([FromQuery] AppUser_FilterDto request)
        {
            var appUserFilter = request.Adapt<AppUserFilterModel>();
            var model = await _sender.Send(new GetPaginatedAppUsersQuery(appUserFilter));
            PaginatedList<AppUser_GetResponseDto> result = new PaginatedList<AppUser_GetResponseDto>(model.Items.Select(x => new AppUser_GetResponseDto(x)).ToList(),
                model.TotalItems, model.PageIndex, model.PageSize);

            return result;
        }

        [AllowAnonymous]
        [HttpGet("{AppUserId}")]
        public async Task<ActionResult<AppUser_GetResponseDto>> Get(long AppUserId)
        {
            var result = await _sender.Send(new GetAppUserQuery(AppUserId));
            AppUser_GetResponseDto response = new AppUser_GetResponseDto(result);
            if (result.IsValidated)
            {
                return response;
            }
            else
            {
                return BadRequest(response);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AuditableResponseDto>> Create([FromBody] AppUser_CreateRequestDto request)
        {
            AppUserModel model = request.Adapt<AppUserModel>();
            var result = await _sender.Send(new CreateAppUserCommand(model));
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
        public async Task<ActionResult<AuditableResponseDto>> Update([FromBody] AppUser_UpdateRequestDto request)
        {
            AppUserModel model = request.Adapt<AppUserModel>();
            var result = await _sender.Send(new UpdateAppUserCommand(model));
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _sender.Send(new DeleteAppUserCommand(id));
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<AuditableResponseDto>> UpdatePassword([FromBody] AppUser_UpdatePasswordRequestDto AppUserDto)
        {
            AppUserModel model = AppUserDto.Adapt<AppUserModel>();
            var result = await _sender.Send(new UpdatePasswordAppUserCommand(model));
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

        [HttpPut("{id}")]
        public async Task<ActionResult<AuditableResponseDto>> Lock(long id)
        {
            var result = await _sender.Send(new LockAppUserCommand(id));
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

        [HttpPut("{id}")]
        public async Task<ActionResult<AuditableResponseDto>> UnLock(long id)
        {
            var result = await _sender.Send(new UnlockAppUserCommand(id));
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
    }
}
