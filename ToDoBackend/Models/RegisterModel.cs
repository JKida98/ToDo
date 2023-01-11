﻿using System.ComponentModel.DataAnnotations;

namespace ToDo.Models;

public class RegisterModel
{
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }
    [Required] public string? Username { get; set; }

    [EmailAddress] [Required] public string? Email { get; set; }

    [Required] public string? Password { get; set; }
}