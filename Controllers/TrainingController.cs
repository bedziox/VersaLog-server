using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersaLog_server.Models;
using VersaLog_server.Utils;

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
            return await _context.Trainings.Include(db=> db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Trainings");
        }
    }

    [HttpGet]
    [Route("id")]
    public ActionResult<TrainingDto> GetById(int id)
    {

        try
        {
            var training = _context.Trainings.
                Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .FirstOrDefault(o => o.TrainingId == id);
            if (training != null)
            {
                var trainingDto = new TrainingDto
                {
                    TrainingId = training.TrainingId,
                    DateAssigned = training.DateAssigned,
                    Note = training.Note,
                    ExerciseResults = training.ExerciseResults.Select(er => new ExerciseResultDto
                    {
                        ExerciseResultId = er.ExerciseResultId,
                        Exercise = er.Exercise,
                        Result = er.Result,
                        Sets = er.Sets,
                        Reps = er.Reps
                    }).ToList(),
                    Status = training.Status
                };
                return Ok(trainingDto);
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
    public ActionResult<List<TrainingDto>> GetAllByUser(int userId)
    {
        try
        {
            var trainings = _context.Trainings
                .Where(db => db.UserId == userId)
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .ToList();
            if (trainings != null)
            {
                var trainingDtos = trainings.Select(training => new TrainingDto
                {
                    TrainingId = training.TrainingId,
                    DateAssigned = training.DateAssigned,
                    Status = training.Status,
                    ExerciseResults = training.ExerciseResults.Select(er => new ExerciseResultDto
                    {
                        ExerciseResultId = er.ExerciseResultId,
                        Exercise = er.Exercise,
                        Result = er.Result,
                        Sets = er.Sets,
                        Reps = er.Reps
                    }).ToList(),
                    Note = training.Note,
                    UserId = training.UserId
                }).ToList();
                return Ok(trainingDtos);
            }
            return NotFound("User with this id does not exist");
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Trainings");
        }
    }
    [HttpGet]
    [Route("user/date")]
    public ActionResult<List<TrainingDto>> GetAllByUserAndDatePeriod(int userId, DateTime? dateStart, DateTime? dateEnd)
    {
        try
        {
            dateStart ??= DateTime.UtcNow.Date;
            dateEnd ??= DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
            var trainings = _context.Trainings
                .Where(db => db.UserId == userId)
                .Where(tr => tr.DateAssigned >= dateStart && tr.DateAssigned <= dateEnd)
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .ToList();
            if (trainings != null)
            {
                var trainingDtos = trainings.Select(training => new TrainingDto
                {
                    TrainingId = training.TrainingId,
                    DateAssigned = training.DateAssigned,
                    Status = training.Status,
                    ExerciseResults = training.ExerciseResults.Select(er => new ExerciseResultDto
                    {
                        ExerciseResultId = er.ExerciseResultId,
                        Exercise = er.Exercise,
                        Result = er.Result,
                        Sets = er.Sets,
                        Reps = er.Reps
                    }).ToList(),
                    Note = training.Note,
                    UserId = training.UserId
                }).ToList();
                return Ok(trainingDtos);
            }
            return NotFound("User with this id does not exist");
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Trainings");
        }
    }
    [HttpGet]
    [Route("user/type")]
    public ActionResult<List<TrainingDto>> GetAllByUserAndType(int userId, ExerciseType type)
    {
        try
        {
            var trainings = _context.Trainings
                .Where(db => db.UserId == userId)
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .ToList();
            
            trainings = trainings
                .Where(training => training.ExerciseResults.Any(er => er.Exercise.Type == type))
                .ToList();
            if (trainings != null)
            {
                var trainingDtos = trainings.Select(training => new TrainingDto
                {
                    TrainingId = training.TrainingId,
                    DateAssigned = training.DateAssigned,
                    Status = training.Status,
                    ExerciseResults = training.ExerciseResults.Select(er => new ExerciseResultDto
                    {
                        ExerciseResultId = er.ExerciseResultId,
                        Exercise = er.Exercise,
                        Result = er.Result,
                        Sets = er.Sets,
                        Reps = er.Reps
                    }).ToList(),
                    Note = training.Note,
                    UserId = training.UserId
                }).ToList();
                return Ok(trainingDtos);
            }
            return NotFound("User with this id does not exist");
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred during sending Trainings");
        }
    }
    
    [HttpGet]
    [Route("user/type/date")]
    public ActionResult<List<TrainingDto>> GetAllByUserTypeAndDate(int userId, ExerciseType type, DateTime? dateStart, DateTime? dateEnd)
    {
        try
        {
            dateStart ??= DateTime.UtcNow.Date;
            dateEnd ??= DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
            
            var trainings = _context.Trainings
                .Where(db => db.UserId == userId)
                .Where(tr => tr.DateAssigned >= dateStart && tr.DateAssigned <= dateEnd)
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .ToList();
            
            trainings = trainings
                .Where(training => training.ExerciseResults.Exists(er => er.Exercise.Type == type))
                .ToList();
            if (trainings != null)
            {
                var trainingDtos = trainings.Select(training => new TrainingDto
                {
                    TrainingId = training.TrainingId,
                    DateAssigned = training.DateAssigned,
                    Status = training.Status,
                    ExerciseResults = training.ExerciseResults.Select(er => new ExerciseResultDto
                    {
                        ExerciseResultId = er.ExerciseResultId,
                        Exercise = er.Exercise,
                        Result = er.Result,
                        Sets = er.Sets,
                        Reps = er.Reps
                    }).ToList(),
                    Note = training.Note,
                    UserId = training.UserId
                }).ToList();
                return Ok(trainingDtos);
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
    public ActionResult<TrainingDto> GetByUserAndId(int userId, int trainingId)
    {
        try
        {
            var training = _context.Trainings
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .FirstOrDefault(db => db.UserId == userId && db.TrainingId == trainingId);
            if (training != null)
            {
                return Ok(training);
            }
            return NotFound($"Training with this user id : {userId} and training id: {trainingId} does not exist");
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
                return (Ok($"Training with id:{trainingId} removed"));
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
            var user = await _context.Users.FirstOrDefaultAsync(db => db.UserId == request.UserId);
            if (user == null)
                return NotFound($"User with id: {request.UserId} not found");
            var training = new Training
            {
                DateAssigned = request.DateAssigned.ToUniversalTime(),
                Status = request.Status,
                User = user,
                UserId = request.UserId,
                Note = "TBA"
            };
            var existingExercises = new List<ExerciseResult>();
            foreach (var item in request.ExerciseResults)
            {
                var exercise = await _context.Exercises.FirstOrDefaultAsync(db => db.ExerciseId == item.Exercise.ExerciseId);
                if (exercise != null)
                {
                    var newExeResult = new ExerciseResult
                    {
                        Training = training,
                        TrainingId = training.TrainingId,
                        Exercise = exercise,
                        Result = "TBA",
                        Reps = item.Reps,
                        Sets = item.Sets
                    };
                    _context.ExerciseResults.Add(newExeResult);
                    existingExercises.Add(newExeResult);
                }
                else
                {
                    return NotFound($"Exercise with id: {item.Exercise.ExerciseId} not found");
                }
            }
            training.ExerciseResults = existingExercises;
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
                 .Include(t => t.ExerciseResults)
                 .ThenInclude(er => er.Exercise)
                 .FirstOrDefaultAsync(t => t.TrainingId == id);
             if (training == null)
             {
                 return NotFound("Training not found");
             }
             
             training.DateAssigned = trainingDto.DateAssigned.ToUniversalTime();
             training.Status = trainingDto.Status;
             training.Note = trainingDto.Note;
             
             var exerciseResultIdsInRequest = trainingDto.ExerciseResults.Select(erDto => erDto.ExerciseResultId);
             var exerciseResultsToRemove = training.ExerciseResults.Where(er => !exerciseResultIdsInRequest.Contains(er.ExerciseResultId)).ToList();
             foreach (var erToRemove in exerciseResultsToRemove)
             {
                 _context.ExerciseResults.Remove(erToRemove);
             }

             foreach (var erDto in trainingDto.ExerciseResults)
             {
                 var existingExerciseResult = training.ExerciseResults.FirstOrDefault(er => er.ExerciseResultId == erDto.ExerciseResultId);
                 if (existingExerciseResult != null)
                 {
                     existingExerciseResult.Result = erDto.Result;
                     existingExerciseResult.Sets = erDto.Sets;
                     existingExerciseResult.Reps = erDto.Reps;
                 }
                 else
                 {
                     var exercise = await _context.Exercises.FindAsync(erDto.Exercise.ExerciseId);
                     if (exercise == null)
                     {
                         return NotFound($"Exercise with ID {erDto.Exercise.ExerciseId} not found");
                     }

                     var newExerciseResult = new ExerciseResult
                     {
                         Exercise = exercise,
                         Result = erDto.Result,
                         Sets = erDto.Sets,
                         Reps = erDto.Reps
                     };
                     
                     training.ExerciseResults.Add(newExerciseResult);
                 }
             }
             
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

