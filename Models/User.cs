using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace VersaLog_server.Models;


public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public ICollection<Training> Trainings { get; set; } = new List<Training>();
}