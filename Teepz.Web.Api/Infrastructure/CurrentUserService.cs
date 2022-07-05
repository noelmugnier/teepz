using System.Security.Claims;
using Teeps.Web.Api.Application.Security;
using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user is not {Identity: {IsAuthenticated: true}})
            return new CurrentUser();

        var id = long.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        var username = user.Identity.Name;
        var email = user.FindFirstValue(ClaimTypes.Email);
        var company = user.FindFirstValue("http://schemas.teepz.com/ws/2008/06/identity/claims/company");
        var position = user.FindFirstValue("http://schemas.teepz.com/ws/2008/06/identity/claims/position");
        var fullname = user.FindFirstValue("http://schemas.teepz.com/ws/2008/06/identity/claims/fullname");
        var tags = user.Claims.Where(c => c.Type == "http://schemas.teepz.com/ws/2008/06/identity/claims/tag")
            .Select(t => t.Value);

        return new CurrentUser(true, id, username, email, fullname, company, position, tags);
    }
}