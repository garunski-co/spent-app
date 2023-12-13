namespace Spent.Commons.Infra;

public class CultureInfoManager
{
    public static (string name, string code) DefaultCulture { get; } = ("English", "en-US");

    public static IEnumerable<(string name, string code)> SupportedCultures { get; } =
    [
        ("English US", "en-US"),
        ("English UK", "en-GB"),
        ("Française", "fr-FR")
    ];

    public static CultureInfo CreateCultureInfo(string cultureInfoId)
    {
        var cultureInfo = OperatingSystem.IsBrowser()
            ? CultureInfo.CreateSpecificCulture(cultureInfoId)
            : new CultureInfo(cultureInfoId);
        
        return cultureInfo;
    }

    public static void SetCurrentCulture(string? cultureInfoCookie)
    {
        var currentCulture = GetCurrentCulture(cultureInfoCookie);

        var cultureInfo = CreateCultureInfo(currentCulture);

        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.DefaultThreadCurrentCulture =
            CultureInfo.DefaultThreadCurrentUICulture = Thread.CurrentThread.CurrentCulture =
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }

    public static string GetCurrentCulture(string? preferredCulture = null)
    {
        var culture = preferredCulture ?? CultureInfo.CurrentUICulture.Name;
        if (SupportedCultures.Any(sc => sc.code == culture) is false)
        {
            culture = DefaultCulture.code;
        }

        return culture;
    }
}
