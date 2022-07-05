using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record GetPostQuery(long Id, CurrentUser CurrentUser) : BaseRequest<PostDetailsDto>(CurrentUser);

public class GetPostHandler : IRequestHandler<GetPostQuery, PostDetailsDto>
{
    private readonly ApplicationDbContext _context;

    public GetPostHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<PostDetailsDto> Handle(GetPostQuery request, CancellationToken token)
    {
        return _context.Posts
            .Where(u => u.Id == request.Id)
            .Select(u => new PostDetailsDto(
                u.Id, 
                u.Content, 
                u.Votes.Sum(v => v.Value), 
                u.Likes.Count, 
                u.Votes.Count,
                u.Comments.Count, 
                u.Tags.Select(t => t.Value), 
                u.CreatedOn, 
                u.UpdatedOn,
                new AuthorDto(u.User.Fullname, u.User.Company, u.User.Position)))
            .SingleAsync(token);
    }
}