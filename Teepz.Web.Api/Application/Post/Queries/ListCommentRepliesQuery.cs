using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record ListCommentRepliesQuery(long CommentId, PageInfo PageInfo, CurrentUser CurrentUser) : BaseRequest<IEnumerable<CommentDto>>(CurrentUser);

public class ListCommentRepliesHandler : IRequestHandler<ListCommentRepliesQuery, IEnumerable<CommentDto>>
{
    private readonly ApplicationDbContext _context;

    public ListCommentRepliesHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommentDto>> Handle(ListCommentRepliesQuery request, CancellationToken token)
    {
        return await _context.Comments
            .OfType<CommentReply>()
            .OrderBy(c => c.CreatedOn)
            .Where(c => c.CommentId == request.CommentId)
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