using BuildingBlock.Entities;
using BuildingBlock.Extensions;
using IdentityService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Entities
{
    public class AppUser : AuditableEntity
    {
        public AppUser() { }
        public AppUser(string password, string username)
        {
            IsDeleted = false;
            Username = username;
            var salt = Guid.NewGuid().ToString();
            Salt = salt;
            Password = $"{salt}{password}".ToSHA256Hash();
        }

        public bool CheckPasswordValid(string password)
        {
            var hashPw = $"{Salt}{password}".ToSHA256Hash();
            return hashPw == Password;
        }

        public void GenNewPassword(string password)
        {
            var salt = Guid.NewGuid().ToString();
            Salt = salt;
            Password = $"{Salt}{password}".ToSHA256Hash();
        }

        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
