using System.Text.Json;
using Going.Plaid;
using Going.Plaid.Entity;
using Going.Plaid.Item;
using Going.Plaid.Link;
using Spent.Commons.Dtos.Identity;
using Spent.Commons.Extensions;

namespace Spent.Server.Controllers.Plaid;

[Route("api/plaid/[controller]/[action]")]
[ApiController]
public partial class LinkController : AppControllerBase<LinkController>
{
    [AutoInject]
    private readonly PlaidClient _client = default!;

    [HttpGet]
    public async Task<string> CreateLinkToken(CancellationToken cancellationToken)
    {
        var response = await _client.LinkTokenCreateAsync(
            new LinkTokenCreateRequest
            {
                User = new LinkTokenCreateRequestUser { ClientUserId = User.GetUserId().ToString() },
                ClientName = Localizer.GetString("ApplicationName"),
                Products = AppSettings.PlaidSettings.Products,
                Language = Language
                    .English, //TODO: tie in the CultureManager to get the current culture and convert to the language enum
                CountryCodes = AppSettings.PlaidSettings.CountryCodes
            });

        if (response.Error is not null)
            throw new UnknownException($"Error in plaid link: {response.Error}");

        // TODO: this returns expiration date, maybe this link token should be stored.
        return response.LinkToken;
    }

    [HttpPost]
    public async Task<string> ExchangePublicToken(LinkResultDto link, CancellationToken cancellationToken)
    {
        //TODO: store the result dto
        var response = await _client.ItemPublicTokenExchangeAsync(new ItemPublicTokenExchangeRequest()
        {
            PublicToken = link.PublicToken,
            ShowRawJson = true
        });

        if (response.Error is not null)
            throw new UnknownException($"Error in plaid exchange: {response.Error}");

        Logger.LogInformation("Success for public token: {0}", response.RawJson);
        
        //store the public token
        
        return "sweet";
    }
}
