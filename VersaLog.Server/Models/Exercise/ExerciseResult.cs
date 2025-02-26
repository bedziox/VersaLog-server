namespace VersaLog.Models;
public class ExerciseResult
{
    public int ExerciseResultId { get; set; }
    public Exercise Exercise { get; set; }
    public int ExerciseId { get; set; }
    public string Result { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public virtual Training Training { get; set; }
    public int TrainingId { get; set; }
}