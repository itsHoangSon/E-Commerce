using IdentityService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Messages
{
    public record AppUserChangedMessage(long Id, string Username, string? DisplayName, StatusEnum Status, bool IsAdmin, bool IsDeleted);
    public record ListAppUserChangedMessage(List<AppUserChangedMessage> Items);
}
