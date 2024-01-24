
using Going.Plaid;
using Going.Plaid.Accounts;

namespace Spent.Server.Controllers.Plaid;

[Route("api/plaid/[controller]/[action]")]
[ApiController]
public partial class AccountsController : AppControllerBase<LinkController>
{
    [AutoInject]
    private readonly PlaidClient _client = default!;

    [HttpGet]
    public async Task<string> Get(CancellationToken cancellationToken)
    {
        var response = await _client.AccountsBalanceGetAsync(new AccountsBalanceGetRequest()
        {
            ShowRawJson = true,
            AccessToken = "test"
        });

        Logger.LogInformation("Success for public token: {0}", response.RawJson);
        
        return response.RawJson;
    }
}
