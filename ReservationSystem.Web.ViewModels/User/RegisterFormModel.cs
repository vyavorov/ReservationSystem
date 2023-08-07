﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static ReservationSystem.Common.EntityValidationConstants.User;

namespace ReservationSystem.Web.ViewModels.User;

public class RegisterFormModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [Display(Name = "First Name")]
    [StringLength(FirstNameMaxLength), MinLength(FirstNameMinLength)]
    public string FirstName { get; set; } = null!;
    [Required]
    [Display(Name = "LastName")]
    [StringLength(LastNameMaxLength), MinLength(LastNameMinLength)]
    public string LastName { get; set; } = null!;
}
