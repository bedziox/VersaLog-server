using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VersaLog.Models;
using VersaLog.Utils;

namespace VersaLog.Controllers;

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
    public ActionResult<List<Training>> GetAll()
    {
        try
        {
            return _context.Trainings
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .ToList();
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
            var training = _context.Trainings
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .FirstOrDefault(o => o.TrainingId == id);
            if (training != null)
            {
                var trainingDto = new TrainingDto(training.TrainingId,
                                                  training.DateAssigned,
                                                  training.Status,
                                                  training.ExerciseResults.Select(er => new ExerciseResultDto
                                                  {
                                                      ExerciseResultId = er.ExerciseResultId,
                                                      Exercise = er.Exercise,
                                                      Result = er.Result,
                                                      Sets = er.Sets,
                                                      Reps = er.Reps
                                                  }).ToList(),
                                                  training.Note,
                                                  training.UserId);
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
    public ActionResult<List<TrainingDto>> GetByUser(int userId)
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
                var trainingDtos = trainings
                                    .Select(training => new TrainingDto(
                                            training.TrainingId,
                                            training.DateAssigned,
                                            training.Status,
                                            training.ExerciseResults.Select(er => new ExerciseResultDto
                                            {
                                                ExerciseResultId = er.ExerciseResultId,
                                                Exercise = er.Exercise,
                                                Result = er.Result,
                                                Sets = er.Sets,
                                                Reps = er.Reps
                                            }).ToList(),
                                            training.Note,
                                            training.UserId))
                                    .ToList();
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
    public ActionResult<List<TrainingDto>> GetByUserTypeAndDate(int userId, ExerciseType type, DateTime? dateStart, DateTime? dateEnd)
    {
        try
        {
            dateStart ??= DateTime.UtcNow.Date;
            dateEnd ??= DateTime.UtcNow.Date.AddDays(1);

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
                var trainingDtos = trainings
                                    .Select(training => new TrainingDto(
                                            training.TrainingId,
                                            training.DateAssigned,
                                            training.Status,
                                            training.ExerciseResults.Select(er => new ExerciseResultDto
                                            {
                                                ExerciseResultId = er.ExerciseResultId,
                                                Exercise = er.Exercise,
                                                Result = er.Result,
                                                Sets = er.Sets,
                                                Reps = er.Reps
                                            }).ToList(),
                                            training.Note,
                                            training.UserId))
                                    .ToList();
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
    [Route("user/status")]
    public ActionResult<List<TrainingDto>> GetByUserAndStatus(int userId, Status status)
    {
        try
        {
            var trainings = _context.Trainings
                .Where(db => db.UserId == userId && db.Status == status)
                .Include(db => db.ExerciseResults)
                .ThenInclude(er => er.Exercise)
                .ToList();
            if (trainings != null)
            {
                var trainingDtos = trainings
                                    .Select(training => new TrainingDto(
                                            training.TrainingId,
                                            training.DateAssigned,
                                            training.Status,
                                            training.ExerciseResults.Select(er => new ExerciseResultDto
                                            {
                                                ExerciseResultId = er.ExerciseResultId,
                                                Exercise = er.Exercise,
                                                Result = er.Result,
                                                Sets = er.Sets,
                                                Reps = er.Reps
                                            }).ToList(),
                                            training.Note,
                                            training.UserId))
                                    .ToList();
                return Ok(trainingDtos);
            }
            return NotFound($"Training with this user id : {userId} and status : {status} does not exist");
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
            var training = _context.Trainings.FirstOrDefault(db => db.TrainingId == trainingId);
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
                DateAssigned = request.DateAssigned,
                Status = request.Status,
                User = user,
                UserId = request.UserId,
                Note = "TBA"
            };
            var existingExercises = new List<ExerciseResult>();
            if (request.ExerciseResults.Count != 0)
            {
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
            else
            {
                return BadRequest("Training cannot have no exercises");
            }
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> EditTraining(int id, [FromBody] TrainingDto trainingDto)
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

            training.DateAssigned = trainingDto.DateAssigned;
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
            }

            _context.Trainings.Update(training);
            await _context.SaveChangesAsync();

            return Ok(training);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

