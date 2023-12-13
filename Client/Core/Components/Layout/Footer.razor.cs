using Spent.Client.Core.Extensions;

namespace Spent.Client.Core.Components.Layout;

public partial class Footer
{
    [AutoInject]
    private BitThemeManager _bitThemeManager = default!;

    private BitDropdownItem<string>[] _cultures = default!;

    private string? _selectedCulture;

    [AutoInject]
    private IBitDeviceCoordinator BitDeviceCoordinator { get; set; } = default!;

    protected override Task OnInitAsync()
    {
        _cultures = CultureInfoManager.SupportedCultures
            .Select(sc => new BitDropdownItem<string> { Value = sc.code, Text = sc.name })
            .ToArray();

        _selectedCulture = CultureInfoManager.GetCurrentCulture();

        return base.OnInitAsync();
    }

    private async Task OnCultureChanged()
    {
        await JsRuntime.SetCookie(".AspNetCore.Culture", $"c={_selectedCulture}|uic={_selectedCulture}",
            30 * 24 * 3600, true);

        await StorageService.SetItem("Culture", _selectedCulture);

        // Relevant in the context of Blazor Hybrid, where the reloading of the web view doesn't result in the resetting of all static in memory data on the client side
        CultureInfoManager.SetCurrentCulture(_selectedCulture);

        NavigationManager.Refresh(true);
    }

    private async Task ToggleTheme()
    {
        await BitDeviceCoordinator.ApplyTheme(await _bitThemeManager.ToggleDarkLightAsync() == "dark");
    }
}
