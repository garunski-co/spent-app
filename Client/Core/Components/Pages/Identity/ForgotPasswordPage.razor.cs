using Spent.Commons.Dtos.Identity;

namespace Spent.Client.Core.Components.Pages.Identity;

public partial class ForgotPasswordPage
{
    private readonly SendResetPasswordEmailRequestDto _forgotPasswordModel = new();

    private string? _forgotPasswordMessage;

    private BitMessageBarType _forgotPasswordMessageType;

    private bool _isLoading;

    private async Task DoSubmit()
    {
        if (_isLoading)
        {
            return;
        }

        _isLoading = true;
        _forgotPasswordMessage = null;

        try
        {
            await HttpClient.PostAsJsonAsync("Identity/SendResetPasswordEmail", _forgotPasswordModel,
                AppJsonContext.Default.SendResetPasswordEmailRequestDto, CurrentCancellationToken);

            _forgotPasswordMessageType = BitMessageBarType.Success;

            _forgotPasswordMessage = Localizer[nameof(AppStrings.ResetPasswordLinkSentMessage)];
        }
        catch (KnownException e)
        {
            _forgotPasswordMessageType = BitMessageBarType.Error;

            _forgotPasswordMessage = e.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }
}
