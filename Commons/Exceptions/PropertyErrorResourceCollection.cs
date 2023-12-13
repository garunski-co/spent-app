namespace Spent.Commons.Exceptions;

public class PropertyErrorResourceCollection
{
    public string? Name { get; set; } = "*";

    public List<ErrorResource> Errors { get; set; } = [];
}
