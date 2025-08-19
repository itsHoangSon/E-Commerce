using BuildingBlock.Models;
using IdentityService.Enums;
using IdentityService.Features.AppUsers.Models;

namespace IdentityService.WebApi.Controllers.AppUser.Dtos
{
    public class AppUser_CreateRequestDto : DataRequestDto
    {
        public AppUser_CreateRequestDto() { }
        public string Username { get; set; } = string.Empty;
        public required string Password { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool IsAdmin { get; set; }
        public StatusEnum Status { get; set; }

    }

    public class AppUser_CreateResponseDto : AuditableResponseDto
    {
        public AppUser_CreateResponseDto(AppUserModel model) : base(model)
        {
            Id = model.Id;
        }
        public long Id { get; set; }
    }
}
