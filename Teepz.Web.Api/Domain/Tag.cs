namespace Teeps.Web.Api.Domain;

public record Tag
{
    private Tag(){}
    public Tag(string value)
    {
        Value = value
            .ToLowerInvariant()
            .Replace("-", " ")
            .Replace("_", " ")
            .Trim();
    }

    public string Value { get; private set; }
}