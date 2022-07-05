using MediatR;
using Teeps.Web.Api.Application.Mediatr;
using Teeps.Web.Api.Domain;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Application.Post;

public record CreatePostCommand(string Content, IEnumerable<string> Tags, CurrentUser CurrentUser) : BaseRequest<long>(CurrentUser);

public class CreatePostHandler : IRequestHandler<CreatePostCommand, long>
{
    private readonly ApplicationDbContext _context;

    public CreatePostHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<long> Handle(CreatePostCommand request, CancellationToken token)
    {
        var entity = await _context.AddAsync(new Domain.Post(request.Content, request.Tags?.Select(t => new Tag(t)) ?? new List<Tag>(), request.CurrentUser), token);
        await _context.SaveChangesAsync(token);
        return entity.Entity.Id;
    }
}