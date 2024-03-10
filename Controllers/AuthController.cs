using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VersaLog_server.Models;
namespace VersaLog_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly VersaDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration, VersaDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto request)
    {
        if (_context.Users.Any(db => db.Username == request.Username || db.Email == request.Email))
            return BadRequest("User already exists");
        User user = new User();
        try
        {
            user = UserBuilder.BuildUser(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok(user);
    }
    [HttpPost("login")]
    public ActionResult<User> Login(UserDto request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == request.Username || u.Email == request.Email);
        if (user != null && !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return BadRequest("User not found or wrong password");
        string token = CreateToken(user);
        return Ok(token);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Username)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:token").Value!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}