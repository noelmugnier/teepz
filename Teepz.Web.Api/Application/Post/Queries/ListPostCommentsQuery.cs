using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record ListPostCommentsQuery(long PostId, PageInfo PageInfo, CurrentUser CurrentUser) : BaseRequest<IEnumerable<CommentDto>>(CurrentUser);

public class ListPostCommentsHandler : IRequestHandler<ListPostCommentsQuery, IEnumerable<CommentDto>>
{
    private readonly ApplicationDbContext _context;

    public ListPostCommentsHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommentDto>> Handle(ListPostCommentsQuery request, CancellationToken token)
    {
        return await _context.Comments
            .OfType<PostComment>()
            .OrderBy(c => c.CreatedOn)
            .Where(c => c.PostId == request.PostId)
            .Select(c => new CommentDto(
                c.Value,
                c.CreatedOn, 
                c.UpdatedOn,
                new AuthorDto(c.User.Fullname, c.User.Company, c.User.Position)))
            .Skip(request.PageInfo.Skip)
            .Take(request.PageInfo.Take)
            .ToListAsync(token);
    }
}