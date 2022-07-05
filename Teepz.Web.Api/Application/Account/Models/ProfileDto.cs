namespace Teeps.Web.Api.Application.Account;

public record ProfileDto(long Id, string Name, string Company, string Position, string Email, IEnumerable<string> Tags);
public record AccessTokenDto(string AccessToken, DateTimeOffset ExpiresOn, string TokenType);