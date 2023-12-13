using Spent.Client.Core.Extensions;
using Spent.Commons.Dtos.Identity;

namespace Spent.Client.Core.Components.Pages.Identity;

[Authorize]
[UsedImplicitly]
public partial class EditProfilePage
{
    private readonly EditUserDto _userToEdit = new();

    private string? _editProfileMessage;

    private BitMessageBarType _editProfileMessageType;

    private bool _isDeleteAccountConfirmModalOpen;

    private bool _isLoading;

    private bool _isRemoving;

    private bool _isSaving;

    private string? _profileImageError;

    private string? _profileImageRemoveUrl;

    private string? _profileImageUploadUrl;

    private string? _profileImageUrl;

    private UserDto _user = new();

    protected override async Task OnInitAsync()
    {
        _isLoading = true;

        try
        {
            await LoadEditProfileData();

            var accessToken = await PrerenderStateService.GetValue($"{nameof(EditProfilePage)}-access_token",
                AuthTokenProvider.GetAccessTokenAsync);

            _profileImageUploadUrl =
                $"{Configuration.GetApiServerAddress()}Attachment/UploadProfileImage?access_token={accessToken}";
            _profileImageUrl =
                $"{Configuration.GetApiServerAddress()}Attachment/GetProfileImage?access_token={accessToken}";
            _profileImageRemoveUrl = $"Attachment/RemoveProfileImage?access_token={accessToken}";
        }
        finally
        {
            _isLoading = false;
        }

        await base.OnInitAsync();
    }

    private async Task LoadEditProfileData()
    {
        _user = await GetCurrentUser() ?? new();

        UpdateEditProfileData();
    }

    private async Task RefreshProfileData()
    {
        await LoadEditProfileData();

        PubSubService.Publish(PubSubMessages.ProfileUpdated, _user);
    }

    private void UpdateEditProfileData()
    {
        _userToEdit.Gender = _user.Gender;
        _userToEdit.FullName = _user.FullName;
        _userToEdit.BirthDate = _user.BirthDate;
    }

    private Task<UserDto?> GetCurrentUser()
    {
        return PrerenderStateService.GetValue(
            $"{nameof(EditProfilePage)}-{nameof(_user)}",
            () => HttpClient.GetFromJsonAsync("User/GetCurrentUser", AppJsonContext.Default.UserDto));
    }

    private async Task DoSave()
    {
        if (_isSaving)
        {
            return;
        }

        _isSaving = true;
        _editProfileMessage = null;

        try
        {
            _user.FullName = _userToEdit.FullName;
            _user.BirthDate = _userToEdit.BirthDate;
            _user.Gender = _userToEdit.Gender;

            (await (await HttpClient.PutAsJsonAsync("User/Update", _userToEdit, AppJsonContext.Default.EditUserDto,
                    CurrentCancellationToken))
                .Content.ReadFromJsonAsync(AppJsonContext.Default.UserDto, CurrentCancellationToken))!.Patch(_user);

            PubSubService.Publish(PubSubMessages.ProfileUpdated, _user);

            _editProfileMessageType = BitMessageBarType.Success;
            _editProfileMessage = Localizer[nameof(AppStrings.ProfileUpdatedSuccessfullyMessage)];
        }
        catch (KnownException e)
        {
            _editProfileMessageType = BitMessageBarType.Error;

            _editProfileMessage = e.Message;
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task RemoveProfileImage()
    {
        if (_isRemoving)
        {
            return;
        }

        _isRemoving = true;

        try
        {
            await HttpClient.DeleteAsync(_profileImageRemoveUrl, CurrentCancellationToken);

            await RefreshProfileData();
        }
        catch (KnownException e)
        {
            _editProfileMessage = e.Message;
            _editProfileMessageType = BitMessageBarType.Error;
        }
        finally
        {
            _isRemoving = false;
        }
    }
}
