using System;
using System.Collections.Generic;
using VersaLog_server.Utils;

namespace VersaLog_server.Models;

public class TrainingDto
{
    public int TrainingId { get; set; }
    public DateTime DateAssigned { get; set; }
    public Status Status { get; set; }
    public List<ExerciseResultDto> ExerciseResults { get; set; }
    public string Note { get; set; }
    public int UserId { get; set; }
}