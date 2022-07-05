using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record LikePostCommand(long Id, CurrentUser CurrentUser) : BaseRequest(CurrentUser);

public class LikePostHandler : IRequestHandler<LikePostCommand>
{
    private readonly ApplicationDbContext _context;

    public LikePostHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(LikePostCommand request, CancellationToken token)
    {
        var postLike = await _context.Posts.Where(p => p.Id == request.Id)
            .Select(p => new { Post = p, Like = p.Likes.FirstOrDefault(l => l.UserId == request.CurrentUser.Id)})
            .FirstOrDefaultAsync(token);

        if(postLike == null || postLike.Like != null)
            return Unit.Value;
        
        try
        {
            await _context.AddAsync(new PostLike(postLike.Post, request.CurrentUser), token);
            await _context.SaveChangesAsync(token);
            return Unit.Value;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("post.like", e);
        }
    }
}