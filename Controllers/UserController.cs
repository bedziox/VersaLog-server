using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersaLog_server.Models;

namespace VersaLog_server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly VersaDbContext _context;

    public UserController(VersaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<User>> GetAll()
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }   
        catch (Exception ex)
        {
            return BadRequest("Something went wrong");
        }
    }
    
    [HttpGet]
    [Route("id")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(us => us.UserId == id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound("User with id does not exist");
            }
        }   
        catch (Exception ex)
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with id: {id} not found");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return (Ok($"User with id:{id} removed"));
        }
        catch (Exception ex)
        {
            return BadRequest("Something went wrong with request, try again");
        }
    }
    
}