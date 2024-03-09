using System.ComponentModel.DataAnnotations;
using VersaLog_server.Utils;

namespace VersaLog_server.Models;

public class Exercise
{
    [Key]
    public int ExerciseId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public ExerciseType Type { get; set; }
    public string Result { get; set; } = string.Empty;
}