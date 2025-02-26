using VersaLog.Utils;

namespace VersaLog.Models;

public class Exercise
{
    public int ExerciseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
}
