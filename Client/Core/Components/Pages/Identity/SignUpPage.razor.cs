using Spent.Commons.Dtos.Identity;
using Spent.Commons.Extensions;

namespace Spent.Client.Core.Components.Pages.Identity;

public partial class SignUpPage
{
    private readonly SignUpRequestDto _signUpModel = new();

    private bool _isLoading;

    private bool _isSignedUp;

    private string? _signUpMessage;

    private BitMessageBarType _signUpMessageType;

    protected override async Task OnAfterFirstRenderAsync()
    {
        await base.OnAfterFirstRenderAsync();

        if ((await AuthenticationStateTask).User.IsAuthenticated())
        {
            NavigationManager.NavigateTo("/");
        }
    }

    private async Task DoSignUp()
    {
        if (_isLoading)
        {
            return;
        }

        _isLoading = true;
        _signUpMessage = null;

        try
        {
            await HttpClient.PostAsJsonAsync("Identity/SignUp", _signUpModel, AppJsonContext.Default.SignUpRequestDto,
                CurrentCancellationToken);

            _isSignedUp = true;
        }
        catch (ResourceValidationException exception)
        {
            _signUpMessageType = BitMessageBarType.Error;
            _signUpMessage = string.Join(Environment.NewLine,
                exception.Payload.Details.SelectMany(errorResourceCollection => errorResourceCollection.Errors)
                    .Select(errorResource => errorResource.Message));
        }
        catch (KnownException e)
        {
            _signUpMessage = e.Message;
            _signUpMessageType = BitMessageBarType.Error;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task DoResendLink()
    {
        if (_isLoading)
        {
            return;
        }

        _isLoading = true;
        _signUpMessage = null;

        try
        {
            await HttpClient.PostAsJsonAsync("Identity/SendConfirmationEmail", new() { Email = _signUpModel.Email },
                AppJsonContext.Default.SendConfirmationEmailRequestDto, CurrentCancellationToken);

            _signUpMessageType = BitMessageBarType.Success;
            _signUpMessage = Localizer[nameof(AppStrings.ResendConfirmationLinkMessage)];
        }
        catch (KnownException e)
        {
            _signUpMessage = e.Message;
            _signUpMessageType = BitMessageBarType.Error;
        }
        finally
        {
            _isLoading = false;
        }
    }
}
