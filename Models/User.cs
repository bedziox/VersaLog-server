using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace VersaLog_server.Models;


public class User
{
    [Key]
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

public class UserBuilder
{
    public static User BuildUser(UserDto request)
    {
        User user = new User();
        user.Username = request.Username;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        return user;
    }
}