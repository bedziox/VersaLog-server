using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VersaLog.Models;

namespace VersaLog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController : Controller
{
    private readonly VersaDbContext _context;

    public ExerciseController(VersaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<List<Exercise>> GetAll()
    {
        try
        {
            return _context.Exercises.ToList();
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending exercises");
        }
    }

    [HttpGet]
    [Route("id")]
    public ActionResult<Exercise> GetById(int id)
    {

        try
        {
            var exercise = _context.Exercises.FirstOrDefault(o => o.ExerciseId == id);
            if (exercise != null)
            {
                return Ok(exercise);
            }

            return NotFound("Exercise with this ID does not exist");
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending exercise with id");
        }

    }

    [HttpPost]
    public async Task<ActionResult<Exercise>> AddExercise(Exercise request)
    {
        if (request.Name.IsNullOrEmpty() || request.Description.IsNullOrEmpty() || request.Type.Equals(null))
            return BadRequest("All data must be filled");
        if (_context.Exercises.Any(db => db.Name == request.Name))
            return BadRequest("Exercise already exists");
        try
        {
            var exercise = new Exercise
            {
                Description = request.Description,
                Name = request.Name,
                Type = request.Type
            };
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return Ok(exercise);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> EditExercise(Exercise request)
    {
        if (request.Name.IsNullOrEmpty() || request.Description.IsNullOrEmpty() || request.Type.Equals(null))
            return BadRequest("All data must be filled");
        var exercise = _context.Exercises.FirstOrDefault(o => o.ExerciseId == request.ExerciseId);
        try
        {
            if (exercise != null)
            {
                exercise.Name = request.Name;
                exercise.Description = request.Description;
                exercise.Type = request.Type;
                _context.Exercises.Update(exercise);
                await _context.SaveChangesAsync();
                return Ok(exercise);
            }

            return NotFound(exercise);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteExercise(int exerciseId)
    {
        try
        {
            var exercise = await _context.Exercises.FirstOrDefaultAsync(db => db.ExerciseId == exerciseId);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
                return (Ok($"Exercise with id:{exerciseId} removed"));
            }

            return NotFound($"Exercise with id:{exerciseId} not found");

        }
        catch (Exception ex)
        {
            return BadRequest("Something went wrong with request, try again");
        }
    }
}