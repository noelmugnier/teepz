using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teeps.Web.Api.Application.Account;
using Teeps.Web.Api.Application.Security;

namespace Teeps.Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;

    public AccountController(
        ICurrentUserService currentUserService,
        IMediator mediator)
    {
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ProfileDto>> GetProfile()
    {
        var result = await _mediator.Send(new GetProfileQuery(_currentUserService.GetCurrentUser()));
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AccessTokenDto>> Login([FromBody] LoginRequest data)
    {
        var result = await _mediator.Send(new LoginCommand(data.Username, data.Password));
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Update([FromBody] RegisterRequest data)
    {
        var result = await _mediator.Send(new RegisterCommand(data.Username, data.Email, data.Password, data.Confirm, data.Fullname));
        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest data)
    {
        var result = await _mediator.Send(new UpdateProfileCommand(data.Email, data.Fullname, data.Company, data.Position, data.Tags, _currentUserService.GetCurrentUser()));
        return Ok(result);
    }
}

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Email, string Password, string Confirm, string Fullname);
public record UpdateProfileRequest(string Email, string Fullname, string Company, string Position, IEnumerable<string> Tags);