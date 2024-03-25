using System.ComponentModel.DataAnnotations;
using VersaLog_server.Utils;

namespace VersaLog_server.Models;

public class Exercise
{
    public int ExerciseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
}