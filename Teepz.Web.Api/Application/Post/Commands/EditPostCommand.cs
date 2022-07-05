using MediatR;
using Microsoft.EntityFrameworkCore;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record EditPostCommand(long Id, string Content, IEnumerable<string> Tags, CurrentUser CurrentUser) : BaseRequest(CurrentUser);

public class EditPostHandler : IRequestHandler<EditPostCommand>
{
    private readonly ApplicationDbContext _context;

    public EditPostHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(EditPostCommand request, CancellationToken token)
    {
        var entity = await _context.Posts.SingleAsync(p => p.Id == request.Id, token);
        entity.Content = request.Content;
        entity.Tags = request.Tags?.Select(t => new Tag(t)) ?? new List<Tag>();
        
        await _context.SaveChangesAsync(token);
        return Unit.Value;
    }
}