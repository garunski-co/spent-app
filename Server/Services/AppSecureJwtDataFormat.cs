using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Spent.Server.Settings;

namespace Spent.Server.Services;

/// <summary>
///     Stores bearer token in jwt format
/// </summary>
public class AppSecureJwtDataFormat(AppSettings appSettings, TokenValidationParameters validationParameters)
    : ISecureDataFormat<AuthenticationTicket>
{
    public AuthenticationTicket Unprotect(string? protectedText)
    {
        return Unprotect(protectedText, null);
    }

    public AuthenticationTicket Unprotect(string? protectedText, string? purpose)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(protectedText, validationParameters, out var validToken);
            var validJwt = (JwtSecurityToken)validToken;
            var data = new AuthenticationTicket(principal, new AuthenticationProperties
            {
                ExpiresUtc = validJwt.ValidTo
            }, IdentityConstants.BearerScheme);
            return data;
        }
        catch
        {
            return NotSignedIn();
        }
    }

    public string Protect(AuthenticationTicket data)
    {
        return Protect(data, null);
    }

    public string Protect(AuthenticationTicket data, string? purpose)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var securityToken = jwtSecurityTokenHandler
            .CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Issuer = appSettings.IdentitySettings.Issuer,
                Audience = appSettings.IdentitySettings.Audience,
                IssuedAt = DateTimeOffset.UtcNow.DateTime,
                Expires = data.Properties.ExpiresUtc!.Value.UtcDateTime,
                SigningCredentials =
                    new SigningCredentials(validationParameters.IssuerSigningKey, SecurityAlgorithms.RsaSha512),
                Subject = new ClaimsIdentity(data.Principal.Claims)
            });

        var encodedJwt = jwtSecurityTokenHandler.WriteToken(securityToken);

        return encodedJwt;
    }

    private static AuthenticationTicket NotSignedIn()
    {
        return new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity()), string.Empty);
    }
}
