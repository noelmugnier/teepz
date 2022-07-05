using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Application.Security;

public interface ICurrentUserService
{
    CurrentUser GetCurrentUser();
}