﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ReservationSystem.Web.ViewModels.User;

public class LoginFormModel
{

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
