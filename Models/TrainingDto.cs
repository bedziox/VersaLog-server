using System;
using System.Collections.Generic;
using VersaLog_server.Utils;

namespace VersaLog_server.Models;

public class TrainingDto
{
    public int TrainingId { get; set; }
    public DateTime DateAssigned { get; set; }
    public Status Status { get; set; }
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    public ICollection<string> Results { get; set; } = new List<string>();
    public int UserId { get; set; }
}