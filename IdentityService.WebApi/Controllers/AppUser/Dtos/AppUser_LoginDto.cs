using BuildingBlock.Models;
using IdentityService.Features.AppUsers.Models;

namespace IdentityService.WebApi.Controllers.AppUser.Dtos
{
    public class AppUser_LoginRequestDto : DataRequestDto
    {
        public AppUser_LoginRequestDto() { }
        public AppUser_LoginRequestDto(AppUserModel model)
        {
            Username = model.Username;
            Password = model.Password;
            DeviceName = model.DeviceName;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? DeviceName { get; set; }
    }

    public class AppUser_LoginResponseDto : AuditableResponseDto
    {
        public AppUser_LoginResponseDto(AppUserModel model) : base(model)
        {
            Token = model.Token;
            RefreshToken = model.RefreshToken;
        }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

    }
}
