using Microsoft.AspNetCore.Components;

namespace Spent.Server.Components;

[StreamRendering]
public partial class App
{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;
}
