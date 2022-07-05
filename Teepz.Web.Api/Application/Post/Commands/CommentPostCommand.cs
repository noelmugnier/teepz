using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record CommentPostCommand(long PostId, string Comment, CurrentUser CurrentUser) : BaseRequest<long>(CurrentUser);

public class CommentPostHandler : IRequestHandler<CommentPostCommand, long>
{
    private readonly ApplicationDbContext _context;

    public CommentPostHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<long> Handle(CommentPostCommand request, CancellationToken token)
    {
        var post = await _context.Posts.SingleAsync(p => p.Id == request.PostId, token);
        var entity = await _context.AddAsync(new PostComment(post, request.Comment, request.CurrentUser), token);
        await _context.SaveChangesAsync(token);
        return entity.Entity.Id;
    }
}