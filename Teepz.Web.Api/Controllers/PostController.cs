using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teeps.Web.Api.Application.Post;
using Teeps.Web.Api.Application.Security;
using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;

    public PostController(
        ICurrentUserService currentUserService,
        IMediator mediator)
    {
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDetailsDto>> Get(long id)
    {
        var result = await _mediator.Send(new GetPostQuery(id, _currentUserService.GetCurrentUser()));
        return Ok(result);
    }

    [HttpGet("{id}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetPostComments(long id, int page = 0, int take = 10)
    {
        var result = await _mediator.Send(new ListPostCommentsQuery(id, new PageInfo(page, take), _currentUserService.GetCurrentUser()));
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<PostDto>>> Get(string? search = null, OrderKind kind = OrderKind.Preferences, int page = 0, int take = 20)
    {
        var result = await _mediator.Send(new ListPostsQuery(search, new PageInfo(page, take), kind, _currentUserService.GetCurrentUser()));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreatePost([FromBody] CreatePostRequest data)
    {
        var result = await _mediator.Send(new CreatePostCommand(data.Content, data.Tags, _currentUserService.GetCurrentUser()));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditPost(long id, [FromBody] EditPostRequest data)
    {
        await _mediator.Send(new EditPostCommand(id, data.Content, data.Tags, _currentUserService.GetCurrentUser()));
        return Ok();
    }

    [HttpPut("{id}/upvote")]
    public async Task<IActionResult> UpvotePost(long id)
    {
        await _mediator.Send(new UpvotePostCommand(id, _currentUserService.GetCurrentUser()));
        return Ok();
    }

    [HttpPut("{id}/downvote")]
    public async Task<IActionResult> DownvotePost(long id)
    {
        await _mediator.Send(new DownvotePostCommand(id, _currentUserService.GetCurrentUser()));
        return Ok();
    }

    [HttpPut("{id}/like")]
    public async Task<IActionResult> LikePost(long id)
    {
        await _mediator.Send(new LikePostCommand(id, _currentUserService.GetCurrentUser()));
        return Ok();
    }

    [HttpPut("{id}/unlike")]
    public async Task<IActionResult> UnlikePost(long id)
    {
        await _mediator.Send(new UnlikePostCommand(id, _currentUserService.GetCurrentUser()));
        return Ok();
    }

    [HttpPost("{id}/comment")]
    public async Task<ActionResult<long>> CommentPost(long id, [FromBody] CommentPostRequest data)
    {
        var result = await _mediator.Send(new CommentPostCommand(id, data.Comment, _currentUserService.GetCurrentUser()));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(long id)
    {
        await _mediator.Send(new DeletePostCommand(id, _currentUserService.GetCurrentUser()));
        return Ok();
    }
}

public record CreatePostRequest(string Content, IEnumerable<string> Tags);
public record EditPostRequest(string Content, IEnumerable<string> Tags);
public record CommentPostRequest(string Comment);