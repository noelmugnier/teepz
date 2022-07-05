using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record EditCommentCommand(long Id, string Comment, CurrentUser CurrentUser) : BaseRequest(CurrentUser);

public class EditCommentHandler : IRequestHandler<EditCommentCommand>
{
    private readonly ApplicationDbContext _context;

    public EditCommentHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(EditCommentCommand request, CancellationToken token)
    {
        var entity = await _context.Comments.SingleAsync(p => p.Id == request.Id, token);
        entity.Value = request.Comment;
        await _context.SaveChangesAsync(token);
        return Unit.Value;
    }
}