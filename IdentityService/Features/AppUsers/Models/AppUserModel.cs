using BuildingBlock.Models;
using BuildingBlock.Paginated;
using IdentityService.Data;
using IdentityService.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Models
{
    public class AppUserModel : DataModel
    {
        private const int MIN_PASSWORD = 8;
        private const int MAX_PASSWORD = 20;
        public AppUserModel() { }
        public long Id { get; set; }
        public AppUserModel(string username, string password)
        {
            Username = username;
            Password = password;
        }



        public string? DisplayName { get; set; }
        public string? Description { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
        public StatusEnum Status { get; set; }


        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? DeviceName { get; set; }

        public async Task CheckExistedUsername(IdentityDbContext context)
        {
            var isExistUsername = await context.AppUser.AnyAsync(x => x.Id != this.Id && x.Username == this.Username);
            if (isExistUsername) this.AddError(nameof(Username), $"Mã người dùng {this.Username} đã tồn tại");
        }

        public void CheckValidPassword()
        {
            var password = this.Password;
            if (string.IsNullOrEmpty(password))
            {
                this.AddError(nameof(Password), "Chưa nhập Password");
                return;
            }

            // Password must be between 8 and 20 characters
            if (password.Length < MIN_PASSWORD || password.Length > MAX_PASSWORD)
            {
                this.AddError(nameof(Password), "Password must be between 8 and 20 characters");
                return;
            }

            // Password must contain at least one number, one lowercase letter, one uppercase letter, and one special character
            var hasNumber = new Regex(@"[0-9]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSpecialChar = new Regex(@"[\W]+");

            bool validPassword = hasNumber.IsMatch(password) && hasLowerChar.IsMatch(password)
                && hasUpperChar.IsMatch(password) && hasSpecialChar.IsMatch(password);
            if (!validPassword)
            {
                this.AddError(nameof(Password), $"Password is invalid (Tối thiểu {MIN_PASSWORD} ký tự, tối đa {MAX_PASSWORD} ký tự. Phải bao gồm số, chữ thường, chữ in hoa và ký tự đặc biệt.)");
                return;
            }
        }

    }

    public class AppUserFilterModel : PaginatedQuery
    {
        public string? Search { get; set; }
        public string? Username { get; set; }
        public string? DisplayName { get; set; }
        public bool? IsAdmin { get; set; }
        public StatusEnum? Status { get; set; }

    }
}
