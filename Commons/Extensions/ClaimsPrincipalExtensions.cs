﻿namespace Spent.Commons.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse((claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) ?? claimsPrincipal.FindFirst("nameid"))!
            .Value);
    }

    public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
    {
        return (claimsPrincipal.FindFirst(ClaimTypes.Name) ?? claimsPrincipal.FindFirst("unique_name"))!.Value;
    }

    public static bool IsAuthenticated(this ClaimsPrincipal? claimsPrincipal)
    {
        return claimsPrincipal?.Identity?.IsAuthenticated is true;
    }
}
