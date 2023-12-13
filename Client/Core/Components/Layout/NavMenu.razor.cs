using Spent.Client.Core.Extensions;
using Spent.Commons.Dtos.Identity;

namespace Spent.Client.Core.Components.Layout;

public partial class NavMenu
{
    private bool _disposed;

    private bool _isSignOutModalOpen;

    private List<BitNavItem> _navItems = [];

    [AutoInject]
    private NavigationManager _navManager = default!;

    private string? _profileImageUrl;

    private string? _profileImageUrlBase;

    private Action _unsubscribe = default!;

    private UserDto _user = new();

    [Parameter]
    public bool IsMenuOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsMenuOpenChanged { get; set; }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitAsync()
    {
        _navItems =
        [
            new BitNavItem
            {
                Text = Localizer[nameof(AppStrings.Home)],
                IconName = BitIconName.Home,
                Url = "/"
            },
            new BitNavItem
            {
                Text = Localizer[nameof(AppStrings.EditProfileTitle)],
                IconName = BitIconName.EditContact,
                Url = "/edit-profile"
            },
            new BitNavItem
            {
                Text = Localizer[nameof(AppStrings.TermsTitle)],
                IconName = BitIconName.EntityExtraction,
                Url = "/terms"
            }
        ];

        _unsubscribe = PubSubService.Subscribe(PubSubMessages.ProfileUpdated, async payload =>
        {
            if (payload is null)
            {
                return;
            }

            _user = (UserDto)payload;

            SetProfileImageUrl();

            StateHasChanged();
        });

        _user = await PrerenderStateService.GetValue($"{nameof(NavMenu)}-{nameof(_user)}", () =>
            HttpClient.GetFromJsonAsync("User/GetCurrentUser", AppJsonContext.Default.UserDto,
                CurrentCancellationToken)) ?? new();

        var accessToken = await PrerenderStateService.GetValue($"{nameof(NavMenu)}-access_token",
            AuthTokenProvider.GetAccessTokenAsync);
        _profileImageUrlBase =
            $"{Configuration.GetApiServerAddress()}Attachment/GetProfileImage?access_token={accessToken}&file=";

        SetProfileImageUrl();
    }

    private void SetProfileImageUrl()
    {
        _profileImageUrl = _user.ProfileImageName is not null ? _profileImageUrlBase + _user.ProfileImageName : null;
    }

    private Task DoSignOut()
    {
        _isSignOutModalOpen = true;

        return CloseMenu();
    }

    private async Task GoToEditProfile()
    {
        await CloseMenu();
        _navManager.NavigateTo("edit-profile");
    }

    private Task CloseMenu()
    {
        IsMenuOpen = false;
        return IsMenuOpenChanged.InvokeAsync(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed || disposing is false)
        {
            return;
        }

        _unsubscribe();

        _disposed = true;
    }
}
