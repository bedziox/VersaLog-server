using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
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
    public async Task<ActionResult<List<Training>>> GetAll()
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
    public ActionResult<List<Training>> GetAllByUser(int userId)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(db=>db.UserId == userId);
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
            var user = _context.Users.FirstOrDefault(db => db.UserId == userId);
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
            var training = _context.Trainings.FirstOrDefault(db=> db.TrainingId == trainingId);
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
    [HttpPost]
    public async Task<ActionResult<Training>> AddTraining([FromBody] TrainingDto request)
    {
        try
        {
            var training = new Training
            {
                DateAssigned = request.DateAssigned,
                Status = request.Status,
                Results = request.Results.ToList(),
            };
            var existingExercises = new List<Exercise>();
            foreach (var item in request.Exercises)
            {
                var exercise = await _context.Exercises.FirstOrDefaultAsync(db => db.ExerciseId == item.ExerciseId);
                if (exercise != null)
                {
                    existingExercises.Add(exercise);
                }
                else
                {
                    return NotFound($"Exercise with id: {item.ExerciseId} not found");
                }
            }
            training.Exercises = existingExercises;
            var user = await _context.Users.FirstOrDefaultAsync(db => db.UserId == request.UserId);
            if (user != null)
            {
                training.User = user;
                training.UserId = request.UserId;
            }
            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();
            return Ok(training);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> EditTraining(int id, [FromBody] TrainingDto trainingDto)
    {
         try
         {
             var training = await _context.Trainings
                 .Include(t => t.Exercises)
                 .FirstOrDefaultAsync(t => t.TrainingId == id);
             if (training == null)
             {
                 return NotFound("Training not found");
             }
             
             training.DateAssigned = trainingDto.DateAssigned;
             training.Status = trainingDto.Status;
             training.Results = trainingDto.Results.ToList();
        
             _context.Trainings.Update(training);
             await _context.SaveChangesAsync();
        
             return Ok(training);
         }
         catch (Exception ex)
         {
             return BadRequest( ex.Message);
         }
    }
    
}

