using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record DownvotePostCommand(long Id, CurrentUser CurrentUser) : BaseRequest(CurrentUser);

public class DownvotePostHandler : IRequestHandler<DownvotePostCommand>
{
    private readonly ApplicationDbContext _context;

    public DownvotePostHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(DownvotePostCommand request, CancellationToken token)
    {
        var postVote = await _context.Posts.Where(p => p.Id == request.Id)
            .Select(p => new { Post = p, Vote = p.Votes.FirstOrDefault(l => l.UserId == request.CurrentUser.Id)})
            .FirstOrDefaultAsync(token);
        
        if(postVote == null || postVote.Vote is {Value: < 0})
            return Unit.Value;
        
        if (postVote.Vote is {Value: > 0})
            _context.Remove(postVote.Vote);

        try
        {
            await _context.AddAsync(new PostVote(postVote.Post, request.CurrentUser, -1), token);
            await _context.SaveChangesAsync(token);
            return Unit.Value;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("post.upvote", e);
        }
    }
}