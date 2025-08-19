using BuildingBlock.Models;
using IdentityService.Enums;
using IdentityService.Features.AppUsers.Models;

namespace IdentityService.WebApi.Controllers.AppUser.Dtos
{
    public class AppUser_UpdateRequestDto : DataRequestDto
    {
        public AppUser_UpdateRequestDto() { }

        public required long Id { get; set; }
        public string Username { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool IsAdmin { get; set; }
        public StatusEnum Status { get; set; }

    }
    public class AppUser_UpdateResponseDto : AuditableResponseDto
    {
        public AppUser_UpdateResponseDto(AppUserModel model) : base(model)
        {
            Id = model.Id;
        }
        public long Id { get; set; }
    }
}
