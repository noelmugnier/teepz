using MediatR;
using Microsoft.AspNetCore.Identity;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Domain.Exceptions;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Account;

public record LoginCommand(string Username, string Password) : BaseRequest<AccessTokenDto>(new CurrentUser())
{
}

public class LoginHandler : IRequestHandler<LoginCommand, AccessTokenDto>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
    }
    public async Task<AccessTokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
            throw new NotFoundException("account.invalid.username.or.password");
        
        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if(!result)
            throw new NotFoundException("account.invalid.username.or.password");

        return await _jwtTokenGenerator.GenerateUserJwtToken(user);
    }
}