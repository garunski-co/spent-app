namespace Spent.Server.Services;

public partial class AppIdentityErrorDescriber : IdentityErrorDescriber
{
    [AutoInject]
    private readonly IStringLocalizer<IdentityStrings> _localizer = default!;

    private IdentityError CreateIdentityError(string code, params object[] args)
    {
        return new()
        {
            Code = code,
            Description = _localizer.GetString(code, args)
        };
    }

    public override IdentityError ConcurrencyFailure()
    {
        return CreateIdentityError(nameof(IdentityStrings.ConcurrencyFailure));
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return CreateIdentityError(nameof(IdentityStrings.DuplicateEmail), email);
    }

    public override IdentityError DuplicateRoleName(string role)
    {
        return CreateIdentityError(nameof(IdentityStrings.DuplicateRoleName), role);
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return CreateIdentityError(nameof(IdentityStrings.DuplicateUserName), userName);
    }

    public override IdentityError InvalidEmail(string? email)
    {
        return CreateIdentityError(nameof(IdentityStrings.InvalidEmail), email ?? string.Empty);
    }

    public override IdentityError InvalidRoleName(string? role)
    {
        return CreateIdentityError(nameof(IdentityStrings.InvalidRoleName), role ?? string.Empty);
    }

    public override IdentityError InvalidToken()
    {
        return CreateIdentityError(nameof(IdentityStrings.InvalidToken));
    }

    public override IdentityError InvalidUserName(string? userName)
    {
        return CreateIdentityError(nameof(IdentityStrings.InvalidUserName), userName ?? string.Empty);
    }

    public override IdentityError LoginAlreadyAssociated()
    {
        return CreateIdentityError(nameof(IdentityStrings.LoginAlreadyAssociated));
    }

    public override IdentityError PasswordMismatch()
    {
        return CreateIdentityError(nameof(IdentityStrings.PasswordMismatch));
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return CreateIdentityError(nameof(IdentityStrings.PasswordRequiresDigit));
    }

    public override IdentityError PasswordRequiresLower()
    {
        return CreateIdentityError(nameof(IdentityStrings.PasswordRequiresLower));
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return CreateIdentityError(nameof(IdentityStrings.PasswordRequiresNonAlphanumeric));
    }

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
    {
        return CreateIdentityError(nameof(IdentityStrings.PasswordRequiresUniqueChars), uniqueChars);
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return CreateIdentityError(nameof(IdentityStrings.PasswordRequiresUpper));
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return CreateIdentityError(nameof(IdentityStrings.PasswordTooShort));
    }

    public override IdentityError RecoveryCodeRedemptionFailed()
    {
        return CreateIdentityError(nameof(IdentityStrings.RecoveryCodeRedemptionFailed));
    }

    public override IdentityError UserAlreadyHasPassword()
    {
        return CreateIdentityError(nameof(IdentityStrings.UserAlreadyHasPassword));
    }

    public override IdentityError UserAlreadyInRole(string role)
    {
        return CreateIdentityError(nameof(IdentityStrings.UserAlreadyInRole), role);
    }

    public override IdentityError UserLockoutNotEnabled()
    {
        return CreateIdentityError(nameof(IdentityStrings.UserLockoutNotEnabled));
    }

    public override IdentityError UserNotInRole(string role)
    {
        return CreateIdentityError(nameof(IdentityStrings.UserNotInRole), role);
    }

    public override IdentityError DefaultError()
    {
        return CreateIdentityError(nameof(IdentityStrings.DefaultError));
    }
}
