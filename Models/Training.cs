using VersaLog_server.Utils;

namespace VersaLog_server.Models;

public class Training()
{
    public int TrainingId { get; set; }
    public DateTime DateAssigned { get; set; }
    public Status Status { get; set; }
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}