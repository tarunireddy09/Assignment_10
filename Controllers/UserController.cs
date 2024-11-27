using CarRentalSystem.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _userService.RegisterUser(user);
        if (!result)
            return BadRequest("User registration failed.");
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid request data.");
        }

        var token = await _userService.AuthenticateUser(request.Email, request.Password);
        if (token == null)
            return Unauthorized("Invalid credentials.");


        return Ok(new { Token = token });
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}