namespace Teeps.Web.Api.Domain.Common;

public record CurrentUser(bool IsAuthenticated, long Id = 0, string? Username = null, string? Email = null, string? Fullname = null, string? Company = null, string? Position = null, IEnumerable<string>? Tags = null)
{
    public CurrentUser() : this(false)
    {
    }
};