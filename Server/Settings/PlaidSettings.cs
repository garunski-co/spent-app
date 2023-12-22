using Going.Plaid.Entity;

namespace Spent.Server.Settings;

[UsedImplicitly]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class PlaidSettings 
{
    public string ClientId { get; init; } = default!;
    
    public string Secret { get; init; } = default!;
    
    public string Environment { get; init; } = default!;
    
    public IReadOnlyList<Products> Products { get; init; } = default!;
    
    public IReadOnlyList<CountryCode> CountryCodes { get; init; } = default!;
}
