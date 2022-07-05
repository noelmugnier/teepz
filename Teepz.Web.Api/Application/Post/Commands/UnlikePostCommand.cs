using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record UnlikePostCommand(long Id, CurrentUser CurrentUser) : BaseRequest(CurrentUser);

public class UnlikePostHandler : IRequestHandler<UnlikePostCommand>
{
    private readonly ApplicationDbContext _context;

    public UnlikePostHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(UnlikePostCommand request, CancellationToken token)
    {
        var postLike = await _context.Posts.Where(p => p.Id == request.Id)
            .Select(p => new { Post = p, Like = p.Likes.FirstOrDefault(l => l.UserId == request.CurrentUser.Id)})
            .FirstOrDefaultAsync(token);

        if(postLike?.Like == null)
            return Unit.Value;

        try
        {
            _context.Remove(postLike.Like);
            await _context.SaveChangesAsync(token);
            return Unit.Value;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("post.unlike", e);
        }
    }
}