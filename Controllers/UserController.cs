using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.Entities;
using WebApplicationDemo.Interface;
using WebApplicationDemo.Model;

namespace WebApplicationDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    /// <summary>
    /// The constructor with config
    /// </summary>
    /// <param name="config">The app-setting config</param>
    /// <param name="userService">The user service</param>
    public UserController(IConfiguration config, IUserService userService)
    {
        _config = config;
        _userService = userService;
    }

    #region Public Methods APIs

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto userDto)
    {
        User? registerUser = await _userService.Register(userDto);

        if (registerUser == null)
            return Conflict(new { message = $"User Already Exits with {userDto.UserName}" });

        return Ok(registerUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto userDto)
    {
        TokenResponseDto? token = await _userService.Login(userDto);

        if (token == null)
            return Unauthorized(new { message = "Un-Authorize User/In-Valid Password" });

        return Ok(token);
    }

    [HttpGet("auth-check")]
    [Authorize(Roles = "Admin")]
    public IActionResult CheckJwtAuth()
    {
        return Ok();
    }

    [HttpPost("refresh-token")]
    [Authorize]
    public async Task<ActionResult<TokenResponseDto?>> UpdateToken(RefreshTokenRequestDto refreshTokenRequest)
    {
        TokenResponseDto? updateResponseDto = await _userService.UpdateToken(refreshTokenRequest);

        if (updateResponseDto == null)
            return ValidationProblem("In-Valid Request Info");

        return updateResponseDto;
    }

    #endregion

        #region Private Fields

        /// <summary>
        /// The app-settings config.
        /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// The user service.
    /// </summary>
    private readonly IUserService _userService;

    #endregion
}