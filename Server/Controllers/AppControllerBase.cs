using Spent.Server.Settings;

namespace Spent.Server.Controllers;

public partial class AppControllerBase<T> : ControllerBase
{
    [AutoInject]
    protected AppSettings AppSettings = default!;

    [AutoInject]
    protected AppDbContext DbContext = default!;

    [AutoInject]
    protected IStringLocalizer<AppStrings> Localizer = default!;

    [AutoInject]
    protected ILogger<T> Logger = default!;
}
