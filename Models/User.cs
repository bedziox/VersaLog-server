using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VersaLog_server.Models;


public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Training> Trainings { get; set; } = new();
}

public class UserBuilder
{
    public static User BuildUser(UserDto request)
    {
        if(!UserValidation.ValidateEmail(request.Email))
            throw new Exception("Incorrect email");
        if (!UserValidation.ValidatePassword(request.Password))
            throw new Exception("Password too weak, password should contain at least 8 characters, capital letter and special sign");
        User user = new User
        {
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };
        return user;
    }
}

public class UserValidation
{
    public static bool ValidateEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }

    public static bool ValidatePassword(string password)
    {
        string regex = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
        return Regex.IsMatch(password, regex);
    }
}