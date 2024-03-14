using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
    private readonly string _keyToken = string.Empty;

    public AuthController(IConfiguration configuration, VersaDbContext context)
    {
        _configuration = configuration;
        _context = context;
        _keyToken = _configuration.GetSection("AppSettings:token").Value!;
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
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username || u.Email == request.Email);
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception();
            }
            string token = CreateToken(user);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest("Wrong user or password");
        }
    }

    [HttpPost("valid")]
    public ActionResult<string> ValidateToken(string? token)
    {
        string? jwt;
        if (token == null)
            return BadRequest("Token cannot be empty");
        try
        {
            JwtSecurityToken newToken = new JwtSecurityToken(token);
            if (newToken.ValidTo > DateTime.UtcNow)
                return Ok(token);
            return Unauthorized("Token not valid");
        }
        catch (Exception ex)
        {
            return BadRequest("Exception occurred during token processing");
        }
    }
    
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Username),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _keyToken));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: cred);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}