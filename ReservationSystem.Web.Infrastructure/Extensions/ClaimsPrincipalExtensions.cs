using System.Security.Claims;
using static ReservationSystem.Common.GeneralApplicationConstants;

namespace ReservationSystem.Web.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole(AdminRoleName);
    }
}
