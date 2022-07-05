using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teeps.Web.Api.Application.Post;
using Teeps.Web.Api.Application.Security;
using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;

    public CommentController(
        ICurrentUserService currentUserService,
        IMediator mediator)
    {
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<long>> ReplyToComment(long id, [FromBody] ReplyToCommentRequest data)
    {
        await _mediator.Send(new ReplyToCommentCommand(id, data.Comment, _currentUserService.GetCurrentUser()));
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetReplies(long id, int page = 0, int take = 20)
    {
        var result = await _mediator.Send(new ListCommentRepliesQuery(id, new PageInfo(page, take), _currentUserService.GetCurrentUser()));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(long id, [FromBody] EditCommentRequest data)
    {
        await _mediator.Send(new EditCommentCommand(id, data.Comment, _currentUserService.GetCurrentUser()));
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteCommentCommand(id, _currentUserService.GetCurrentUser()));
        return Ok();
    }
}

public record ReplyToCommentRequest(string Comment);
public record EditCommentRequest(string Comment);