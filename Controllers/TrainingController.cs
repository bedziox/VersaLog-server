using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersaLog_server.Models;

namespace VersaLog_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrainingController : Controller
{
    private readonly VersaDbContext _context;

    public TrainingController(VersaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Training>>> GetAll()
    {
        try
        {
            return await _context.Trainings.ToListAsync();
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Trainings");
        }
    }

    [HttpGet]
    [Route("id")]
    public ActionResult<Training?> GetById(int id)
    {

        try
        {
            var training = _context.Trainings.FirstOrDefault(o => o.TrainingId == id);
            if (training != null)
            {
                return Ok(training);
            }

            return NotFound("Training with this ID does not exist");
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Training with id");
        }
    }

    [HttpGet]
    [Route("user")]
    public ActionResult<ICollection<Training>> GetAllByUser(int userId)
    {
        try
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                var trainings = user.Trainings;
                return Ok(trainings);
            }
            return NotFound("User with this id does not exist");
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Trainings");
        }
    }
    [HttpGet]
    [Route("user/id")]
    public ActionResult<Training> GetByUserAndId(int userId, int trainingId)
    {
        try
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                var training = user.Trainings.Where(o=>o.TrainingId == trainingId);
                return Ok(training);
            }
            return NotFound("User with this id does not exist");
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Trainings");
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTraining(int trainingId)
    {
        try
        {
            var training = _context.Trainings.Find(trainingId);
            if (training != null)
            {
                _context.Trainings.Remove(training);
                await _context.SaveChangesAsync();
                return (Ok($"Exercise with id:{trainingId} removed"));
            }
            return NotFound($"Training with id: {trainingId} not found");
        }
        catch (Exception ex)
        {
            return BadRequest("Something went wrong");
        }
    }
    
}

