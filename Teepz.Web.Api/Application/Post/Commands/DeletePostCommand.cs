using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record DeletePostCommand(long Id, CurrentUser CurrentUser) : BaseRequest(CurrentUser);

public class DeletePostHandler : IRequestHandler<DeletePostCommand>
{
    private readonly ApplicationDbContext _context;

    public DeletePostHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken token)
    {
        var entity = await _context.Posts.SingleAsync(p => p.Id == request.Id, token);
        _context.Remove(entity);
        
        await _context.SaveChangesAsync(token);
        return Unit.Value;
    }
}