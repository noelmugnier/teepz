using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Application.Settings;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Domain.Exceptions;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Account;

public record UpdateProfileCommand
    (string Email, string Fullname, string Company, string Position, IEnumerable<string> Tags, CurrentUser CurrentUser) : BaseRequest<AccessTokenDto>(CurrentUser);

public class UpdateSubscribedTagsHandler : IRequestHandler<UpdateProfileCommand, AccessTokenDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UpdateSubscribedTagsHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AccessTokenDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.CurrentUser.Id.ToString());
        if (user == null)
            throw new NotFoundException("profile.notfound");

        user.Company = request.Company;
        user.Fullname = request.Fullname;
        user.Position = request.Position;
        user.Tags = request.Tags?.Select(t => new Tag(t)) ?? new List<Tag>();
        user.Email = request.Email;

        await _userManager.UpdateAsync(user);

        return await _jwtTokenGenerator.GenerateUserJwtToken(user);
    }
}