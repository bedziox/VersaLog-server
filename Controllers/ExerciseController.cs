using System.Collections;
using System.Diagnostics;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersaLog_server.Models;

namespace VersaLog_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly VersaDbContext _context;

    public ExerciseController(IConfiguration configuration, VersaDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<ICollection<Exercise>>> GetAll()
    {
        try
        {
            return await _context.Exercises.ToListAsync();
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending exercises");
        }
    }

    [HttpGet]
    [Route("id")]
    public ActionResult<Exercise?> GetById(int id)
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
        if (_context.Exercises.Any(db => db.Name == request.Name))
            return BadRequest("Exercise already exists");
        var exercise = new Exercise();
        try
        {

            exercise.Name = request.Name;
            exercise.Description = request.Description;
            exercise.Type = request.Type;
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
    public async Task<ActionResult> DeleteExercise(int id)
    {
        try
        {
            _context.Exercises.Remove(_context.Exercises.First(db => db.ExerciseId == id));
            await _context.SaveChangesAsync();
            return (Ok($"Exercise with id:{id} removed"));
        }
        catch (Exception ex)
        {
            return BadRequest("Something went wrong with request, try again");
        }
    }
}