using Microsoft.AspNetCore.Components.Web;
using OS = System.OperatingSystem;

namespace Spent.Client.Core.Services;

public static class AppRenderMode
{
    private const bool PrerenderEnabled =
#if PrerenderEnabled
        true;
#else
        false;
#endif

    public const bool PwaEnabled =
#if PwaEnabled
        true;
#else
        false;
#endif

    private static IComponentRenderMode Auto { get; } = new InteractiveAutoRenderMode(PrerenderEnabled);

    // private static IComponentRenderMode BlazorWebAssembly { get; } =
    //     new InteractiveWebAssemblyRenderMode(PrerenderEnabled);

    private static IComponentRenderMode BlazorServer { get; } = new InteractiveServerRenderMode(PrerenderEnabled);

    public static IComponentRenderMode NoPrerenderBlazorWebAssembly => new InteractiveWebAssemblyRenderMode(false);

    public static IComponentRenderMode Current =>
        BuildConfiguration.IsDebug() ? BlazorServer /*For better development experience*/ : Auto;

    public static bool IsHybrid()
    {
        return OS.IsAndroid() || OS.IsIOS() || OS.IsMacCatalyst() || OS.IsMacOS() || OS.IsWindows();
    }
}
