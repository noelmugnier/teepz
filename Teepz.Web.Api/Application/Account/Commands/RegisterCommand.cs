using MediatR;
using Microsoft.AspNetCore.Identity;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Domain.Exceptions;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Account;

public record RegisterCommand
    (string Username, string Email, string Password, string Confirm, string Fullname) : BaseRequest<AccessTokenDto>(new CurrentUser());

public class RegisterHandler : IRequestHandler<RegisterCommand, AccessTokenDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AccessTokenDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user != null)
            throw new AccountException("account.invalid.username");

        if (request.Password != request.Confirm)
            throw new AccountException("account.password.mismatch");

        var result = await _userManager.CreateAsync(
            new ApplicationUser(request.Username, request.Fullname) {Email = request.Email}, request.Password);
        if (!result.Succeeded)
            throw new AccountException("account.creation.failed");

        user = await _userManager.FindByNameAsync(request.Username);
        return await _jwtTokenGenerator.GenerateUserJwtToken(user);
    }
}