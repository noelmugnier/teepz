using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Teeps.Web.Api.Application.Settings;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Account;

public interface IJwtTokenGenerator
{
    Task<AccessTokenDto> GenerateUserJwtToken(ApplicationUser user);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AccessTokenDto> GenerateUserJwtToken(ApplicationUser user)
    {
        var claims = (await _userManager.GetClaimsAsync(user)).ToList();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        
        if(!string.IsNullOrWhiteSpace(user.Company))
            claims.Add(new Claim("http://schemas.teepz.com/ws/2008/06/identity/claims/company", user.Company));
        if(!string.IsNullOrWhiteSpace(user.Position))
            claims.Add(new Claim("http://schemas.teepz.com/ws/2008/06/identity/claims/position", user.Position));
        if(!string.IsNullOrWhiteSpace(user.Fullname))
            claims.Add(new Claim("http://schemas.teepz.com/ws/2008/06/identity/claims/fullname", user.Fullname));

        if (user.Tags?.Any() == true)
            claims.AddRange(user.Tags.Select(t =>
                new Claim("http://schemas.teepz.com/ws/2008/06/identity/claims/tag", t.Value)));

        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, DateTime.UtcNow.AddMinutes(-1),
            DateTime.UtcNow.AddYears(1), credentials);
        
        return new AccessTokenDto(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo, JwtBearerDefaults.AuthenticationScheme);
    }
}