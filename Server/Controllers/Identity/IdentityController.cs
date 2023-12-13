using System.Web;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Spent.Commons.Dtos.Identity;
using Spent.Server.Components;
using Spent.Server.Extensions;
using Spent.Server.Models.Emailing;
using Spent.Server.Models.Identity;
using Spent.Server.Resources;

namespace Spent.Server.Controllers.Identity;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public partial class IdentityController : AppControllerBase
{
    [AutoInject]
    private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions = default!;

    [AutoInject]
    private readonly IStringLocalizer<EmailStrings> _emailLocalizer = default!;

    [AutoInject]
    private readonly IFluentEmail _fluentEmail = default!;

    [AutoInject]
    private readonly HtmlRenderer _htmlRenderer = default!;

    [AutoInject]
    private readonly SignInManager<User> _signInManager = default!;

    [AutoInject]
    private readonly UserManager<User> _userManager = default!;

    /// <summary>
    ///     By leveraging summary tags in your controller's actions and DTO properties you can make your codes much easier to
    ///     maintain.
    ///     These comments will also be used in swagger docs and ui.
    /// </summary>
    [HttpPost]
    public async Task SignUp(SignUpRequestDto signUpRequest, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByNameAsync(signUpRequest.Email!);

        var userToAdd = signUpRequest.Map();

        if (existingUser is not null)
        {
            if (await _userManager.IsEmailConfirmedAsync(existingUser))
            {
                throw new BadRequestException(Localizer.GetString(nameof(AppStrings.DuplicateEmail),
                    existingUser.Email!));
            }

            var deleteResult = await _userManager.DeleteAsync(existingUser);
            if (!deleteResult.Succeeded)
            {
                throw new ResourceValidationException(deleteResult.Errors
                    .Select(err => new LocalizedString(err.Code, err.Description)).ToArray());
            }

            userToAdd.ConfirmationEmailRequestedOn = existingUser.ConfirmationEmailRequestedOn;
        }

        userToAdd.LockoutEnabled = true;

        var result = await _userManager.CreateAsync(userToAdd, signUpRequest.Password!);

        if (result.Succeeded is false)
        {
            throw new ResourceValidationException(result.Errors.Select(e => new LocalizedString(e.Code, e.Description))
                .ToArray());
        }

        await SendConfirmationEmail(userToAdd, cancellationToken);
    }

    [HttpPost]
    public async Task SendConfirmationEmail(
        SendConfirmationEmailRequestDto sendConfirmationEmailRequest,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(sendConfirmationEmailRequest.Email!);

        if (user is null)
        {
            throw new BadRequestException(Localizer.GetString(nameof(AppStrings.UserNameNotFound),
                sendConfirmationEmailRequest.Email!));
        }

        if (await _userManager.IsEmailConfirmedAsync(user))
        {
            throw new BadRequestException(Localizer[nameof(AppStrings.EmailAlreadyConfirmed)]);
        }

        await SendConfirmationEmail(user, cancellationToken);
    }

    private async Task SendConfirmationEmail(
        User user,
        CancellationToken cancellationToken)
    {
        var resendDelay = DateTimeOffset.Now - user.ConfirmationEmailRequestedOn -
                          AppSettings.IdentitySettings.ConfirmationEmailResendDelay;

        if (resendDelay < TimeSpan.Zero)
        {
            throw new TooManyRequestsExceptions(Localizer.GetString(
                nameof(AppStrings.WaitForConfirmationEmailResendDelay), resendDelay.Value.ToString("mm\\:ss")));
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var controller = RouteData.Values["controller"]!.ToString();

        var confirmationLink = Url.Action(nameof(ConfirmEmail), controller,
            new { user.Email, token },
            HttpContext.Request.Scheme);

        var body = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var renderedComponent = await _htmlRenderer.RenderComponentAsync<EmailConfirmationTemplate>(
                ParameterView.FromDictionary(new Dictionary<string, object>
                {
                    {
                        nameof(EmailConfirmationTemplate.Model),
                        new EmailConfirmationModel(confirmationLink)
                    },
                    { nameof(HttpContext), HttpContext }
                }!));

            return renderedComponent.ToHtmlString();
        });

        var result = await _fluentEmail
            .To(user.Email, user.DisplayName)
            .Subject(_emailLocalizer[EmailStrings.ConfirmationEmailSubject])
            .Body(body, true)
            .SendAsync(cancellationToken);

        user.ConfirmationEmailRequestedOn = DateTimeOffset.Now;

        await _userManager.UpdateAsync(user);

        if (!result.Successful)
        {
            throw new ResourceValidationException(result.ErrorMessages.Select(err => Localizer[err]).ToArray());
        }
    }

    [HttpGet]
    public async Task<ActionResult> ConfirmEmail(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            throw new BadRequestException(Localizer.GetString(nameof(AppStrings.UserNameNotFound), email));
        }

        var emailConfirmed = user.EmailConfirmed;
        var errors = string.Empty;

        if (emailConfirmed is false)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            }

            emailConfirmed = result.Succeeded;
        }

        var url =
            $"/email-confirmation?email={email}&email-confirmed={emailConfirmed}{(string.IsNullOrEmpty(errors) ? "" : $"&error={errors}")}";

        return Redirect(url);
    }

    [HttpPost]
    public async Task SignIn(SignInRequestDto signInRequest)
    {
        _signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        var result = await _signInManager.PasswordSignInAsync(signInRequest.UserName!, signInRequest.Password!,
            false, true);

        if (result.IsLockedOut)
        {
            var user = await _userManager.FindByNameAsync(signInRequest.UserName!);
            throw new BadRequestException(Localizer.GetString(nameof(AppStrings.UserLockedOut),
                (DateTimeOffset.UtcNow - user!.LockoutEnd!).Value.ToString("mm\\:ss")));
        }

        /* if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(signInRequest.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(signInRequest.TwoFactorCode, rememberClient: true);
            }
            else if (!string.IsNullOrEmpty(signInRequest.TwoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(signInRequest.TwoFactorRecoveryCode);
            }
        } */

        if (result.Succeeded is false)
        {
            throw new UnauthorizedException(Localizer.GetString(nameof(AppStrings.InvalidUsernameOrPassword)));
        }
    }

    [HttpPost]
    public async Task<ActionResult<TokenResponseDto>> Refresh(RefreshRequestDto refreshRequest)
    {
        var refreshTokenProtector = _bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

        if (refreshTicket?.Properties.ExpiresUtc is not { } expiresUtc || DateTimeOffset.UtcNow >= expiresUtc ||
            await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            return Challenge();
        }

        var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

        return SignIn(newPrincipal, IdentityConstants.BearerScheme);
    }

    [HttpPost]
    public async Task SendResetPasswordEmail(
        SendResetPasswordEmailRequestDto sendResetPasswordEmailRequest,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(sendResetPasswordEmailRequest.Email!);

        if (user is null)
        {
            throw new BadRequestException(Localizer.GetString(nameof(AppStrings.UserNameNotFound),
                sendResetPasswordEmailRequest.Email!));
        }

        var resendDelay = DateTimeOffset.Now - user.ResetPasswordEmailRequestedOn -
                          AppSettings.IdentitySettings.ResetPasswordEmailResendDelay;

        if (resendDelay < TimeSpan.Zero)
        {
            throw new TooManyRequestsExceptions(Localizer.GetString(
                nameof(AppStrings.WaitForResetPasswordEmailResendDelay), resendDelay.Value.ToString("mm\\:ss")));
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetPasswordLink = new Uri(HttpContext.Request.GetBaseUrl(),
            $"reset-password?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}");

        var body = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var renderedComponent = await _htmlRenderer.RenderComponentAsync<ResetPasswordTemplate>(
                ParameterView.FromDictionary(new Dictionary<string, object?>
                {
                    {
                        nameof(ResetPasswordTemplate.Model),
                        new ResetPasswordModel
                        {
                            DisplayName = user.DisplayName,
                            ResetPasswordLink = resetPasswordLink
                        }
                    },
                    { nameof(HttpContext), HttpContext }
                }));

            return renderedComponent.ToHtmlString();
        });

        var result = await _fluentEmail
            .To(user.Email, user.DisplayName)
            .Subject(_emailLocalizer[EmailStrings.ResetPasswordEmailSubject])
            .Body(body, true)
            .SendAsync(cancellationToken);

        user.ResetPasswordEmailRequestedOn = DateTimeOffset.Now;

        await _userManager.UpdateAsync(user);

        if (!result.Successful)
        {
            throw new ResourceValidationException(result.ErrorMessages.Select(err => Localizer[err]).ToArray());
        }
    }

    [HttpPost]
    public async Task ResetPassword(ResetPasswordRequestDto resetPasswordRequest)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email!);

        if (user is null)
        {
            throw new BadRequestException(Localizer.GetString(nameof(AppStrings.UserNameNotFound),
                resetPasswordRequest.Email!));
        }

        var result =
            await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token!, resetPasswordRequest.Password!);

        if (!result.Succeeded)
        {
            throw new ResourceValidationException(result.Errors.Select(e => new LocalizedString(e.Code, e.Description))
                .ToArray());
        }
    }
}
