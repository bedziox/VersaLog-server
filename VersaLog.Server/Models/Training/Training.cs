using VersaLog.Utils;

namespace VersaLog.Models;

public class Training()
{
    public int TrainingId { get; set; }
    public DateTime DateAssigned { get; set; }
    public Status Status { get; set; }
    public List<ExerciseResult> ExerciseResults { get; set; }
    public string Note { get; set; } = string.Empty;
    public virtual User User { get; set; }
    public int UserId { get; set; }
}