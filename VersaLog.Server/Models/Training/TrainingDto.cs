using VersaLog.Utils;

namespace VersaLog.Models;
public record TrainingDto(int TrainingId, DateTime DateAssigned, Status Status, List<ExerciseResultDto> ExerciseResults, string Note, int UserId);