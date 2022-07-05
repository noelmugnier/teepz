using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Account;

public record GetProfileQuery(CurrentUser CurrentUser) : BaseRequest<ProfileDto>(CurrentUser);

public class GetProfileHandler : IRequestHandler<GetProfileQuery, ProfileDto>
{
    private readonly ApplicationDbContext _context;

    public GetProfileHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken token)
    {
        return _context.Users
            .Where(u => u.Id == request.CurrentUser.Id)
            .Select(u => new ProfileDto(u.Id, u.Fullname, u.Company, u.Position, u.Email, u.Tags.Select(t => t.Value)))
            .SingleAsync(token);
    }
}