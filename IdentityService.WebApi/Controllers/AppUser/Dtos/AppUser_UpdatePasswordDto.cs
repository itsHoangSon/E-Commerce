using BuildingBlock.Models;

namespace IdentityService.WebApi.Controllers.AppUser.Dtos
{
    public class AppUser_UpdatePasswordRequestDto : DataRequestDto
    {
        public AppUser_UpdatePasswordRequestDto() { }
        public AppUser_UpdatePasswordRequestDto(long id, string password)
        {
            Id = id;
            Password = password;
        }

        public required long Id { get; set; }
        public string Password { get; set; }
    }
}
