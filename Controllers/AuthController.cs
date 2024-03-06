using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VersaLog_server.Models;

namespace VersaLog_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public static User user = new User();

    [HttpPost("register")]
    public IActionResult Register(UserDto request)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.Username = request.Username;
        user.PasswordHash = passwordHash;
        user.Email = request.Email;
        // User.FirstName = User.FirstName;
        // User.LastName = User.FirstName;
        return Ok(user);
    }
}