namespace VersaLog.Models;

public record UserDto(int Id, string? FirstName, string? LastName, string? Username, string? Password, string? Email, string Token);