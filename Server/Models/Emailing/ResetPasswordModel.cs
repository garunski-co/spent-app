namespace Spent.Server.Models.Emailing;

public class ResetPasswordModel
{
    public string? DisplayName { get; init; }

    public Uri? ResetPasswordLink { get; init; }
}
