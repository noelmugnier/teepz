using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record ReplyToCommentCommand(long CommentId, string Comment, CurrentUser CurrentUser) : BaseRequest<long>(CurrentUser);

public class ReplyToCommentHandler : IRequestHandler<ReplyToCommentCommand, long>
{
    private readonly ApplicationDbContext _context;

    public ReplyToCommentHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<long> Handle(ReplyToCommentCommand request, CancellationToken token)
    {
        var postComment = await _context.Comments.SingleAsync(p => p.Id == request.CommentId, token);
        var entity = await _context.AddAsync(new CommentReply(postComment, request.Comment, request.CurrentUser), token);
        await _context.SaveChangesAsync(token);
        return entity.Entity.Id;
    }
}