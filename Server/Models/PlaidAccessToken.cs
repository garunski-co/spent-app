namespace Spent.Server.Models;

[Owned]
public class PlaidAccessToken
{
    [MaxLength(100)]
    public string? Value { get; set; }
    
    // [MaxLength(100)]
    // public string? ItemId { get; set; }
    //
    // [MaxLength(100)]
    // public string RequestId { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }
}
