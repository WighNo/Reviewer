﻿using FeedbackDService.Data.Models;

namespace FeedbackDService.Data.Context.Entities;

public class User : EntityBase
{
    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public Role Role { get; set; }
}