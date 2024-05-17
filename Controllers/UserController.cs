using System;
using System.Threading.Tasks;
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
    public <ActionResult<List<User>> GetAll()
    {
        try
        {
            var users =  _context.Users.ToList();
            return Ok(users);
        }   
        catch (Exception ex)
        {
            return BadRequest("Something went wrong");
        }
    }
    
    [HttpGet]
    [Route("id")]
    public  ActionResult<User> GetById(int id)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(us => us.UserId == id);
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
            var user = _context.Users.FirstOrDefault(db => db.UserId == id);
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