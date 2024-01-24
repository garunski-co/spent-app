using Elsa.Workflows;
using Going.Plaid;

namespace Spent.Server.Workflows;

public class GetPlaidDataActivity : CodeActivity<string>
{
    [AutoInject]
    private readonly PlaidClient _client = default!;
    
    [AutoInject]
    protected ILogger<GetPlaidDataActivity> Logger = default!;
    
    protected override void Execute(ActivityExecutionContext context)
    {
        Logger.LogInformation("HERE WE Go!");
    }
}
