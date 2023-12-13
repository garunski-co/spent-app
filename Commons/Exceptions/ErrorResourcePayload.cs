namespace Spent.Commons.Exceptions;

public class ErrorResourcePayload
{
    public string? ResourceTypeName { get; set; } = "*";

    public List<PropertyErrorResourceCollection> Details { get; set; } = [];
}
