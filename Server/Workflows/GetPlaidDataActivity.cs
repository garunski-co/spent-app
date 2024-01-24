using Elsa.Workflows;
using Going.Plaid;

namespace Spent.Server.Workflows;

public partial class GetPlaidDataActivity : CodeActivity<string>
{
    [AutoInject]
    private readonly PlaidClient _client = default!;
    
    [AutoInject]
    private ILogger<GetPlaidDataActivity> _logger = default!;
    
    protected override void Execute(ActivityExecutionContext context)
    {
        _logger.LogInformation("HERE WE Go!");
    }
}
