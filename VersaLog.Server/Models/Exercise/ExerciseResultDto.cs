namespace VersaLog.Models;

public class ExerciseResultDto
{
    public int ExerciseResultId { get; set; }
    public Exercise Exercise { get; set; }
    public string Result { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
}