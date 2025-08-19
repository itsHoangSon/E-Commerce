using BuildingBlock.Models;
using BuildingBlock.Paginated;
using IdentityService.Enums;
using IdentityService.Features.AppUsers.Models;

namespace IdentityService.WebApi.Controllers.AppUser.Dtos
{
    public class AppUser_GetResponseDto : AuditableResponseDto
    {
        public AppUser_GetResponseDto(AppUserModel model) : base(model)
        {
            Id = model.Id;
            Username = model.Username;
            DisplayName = model.DisplayName;
            Description = model.Description;
            Status = model.Status;

            CreatedAt = model.CreatedAt;
            LastModifiedAt = model.LastModifiedAt;
        }

        public long Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }

        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime? LastModifiedAt { get; set; } = DateTime.MinValue;
    }
    public class AppUser_FilterDto : PaginatedQuery
    {
        public string? Search { get; set; }
        public string? Username { get; set; }
        public string? DisplayName { get; set; }
        public bool? IsAdmin { get; set; }
        public StatusEnum? Status { get; set; }

    }
}
