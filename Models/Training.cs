using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VersaLog_server.Utils;

namespace VersaLog_server.Models;

public class Training()
{
    public int TrainingId { get; set; }
    public DateTime DateAssigned { get; set; }
    public Status Status { get; set; }
    public List<Exercise> Exercises { get; set; } = new();
    public List<string> Results { get; set; } = new();
    public virtual User User { get; set; }
    public int UserId { get; set; }
}