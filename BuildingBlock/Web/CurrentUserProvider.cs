using BuildingBlock.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Web
{
    public interface ICurrentUserProvider
    {
        long UserId { get; }
        string UserName { get; }
        long TenantId { get; }
        string Token { get; }
        bool IsAdmin { get; }
    }

    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token
        {
            get
            {
                string token = _httpContextAccessor.HttpContext?.Request?.Headers[HeaderNames.Authorization] ?? "";
                return token.Substring(token.IndexOf(" ") + 1);
            }
        }
        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "";
            }
        }
        public long UserId
        {
            get
            {
                var nameIdentifier = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                long.TryParse(nameIdentifier, out var userId);
                return userId;

            }
        }


        public long TenantId
        {
            get
            {
                var tenant = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(AuthorizationConstants.CLAIM_TENANT_ID);
                long.TryParse(tenant, out var tenantId);
                return tenantId;
            }
        }

        public bool IsAdmin
        {
            get
            {
                var role = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
                if (role != null && role.Equals("Admin"))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
