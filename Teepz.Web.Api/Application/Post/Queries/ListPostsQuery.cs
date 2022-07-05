using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record ListPostsQuery(string? Terms, PageInfo PageInfo, OrderKind Kind, CurrentUser CurrentUser) : BaseRequest<IEnumerable<PostDto>>(CurrentUser);

public class ListPostsHandler : IRequestHandler<ListPostsQuery, IEnumerable<PostDto>>
{
    private readonly ApplicationDbContext _context;

    public ListPostsHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PostDto>> Handle(ListPostsQuery request, CancellationToken token)
    {
        var query = _context.Posts.AsQueryable();
        if (request.Kind == OrderKind.Fresh)
            query = query.OrderBy(p => p.CreatedOn);
        else if (request.Kind == OrderKind.Rating)
            query = query.OrderBy(p => p.Votes.Sum(v => v.Value));
        else if (request.Kind == OrderKind.Trending)
            query = query.OrderBy(p => p.Likes.Sum(l => l.Value));
        else
        {
            if (request.CurrentUser.Tags?.Any() == true)
                query = query
                    .Where(p => p.Tags.Any(t => request.CurrentUser.Tags.Contains(t.Value)));
            
            query = query.OrderBy(p => p.CreatedOn);
        }

        if (!string.IsNullOrWhiteSpace(request.Terms))
            query = query.Where(p => p.Content.Contains(request.Terms));

        return await query
            .Select(u => new PostDto(
                u.Id, 
                u.Content.Substring(0, u.Content.Length > 500 ? 500 : u.Content.Length), 
                u.Votes.Sum(v => v.Value), 
                u.Likes.Count, 
                u.Votes.Count,
                u.Comments.Count,
                u.Content.Length > 500, 
                u.Tags.Select(t => t.Value), 
                u.CreatedOn, 
                u.UpdatedOn,
                new AuthorDto(u.User.Fullname, u.User.Company, u.User.Position)))
            .Skip(request.PageInfo.Skip)
            .Take(request.PageInfo.Take)
            .ToListAsync(token);
    }
}