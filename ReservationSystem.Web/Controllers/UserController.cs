﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Data.Models;
using ReservationSystem.Web.ViewModels.User;

namespace ReservationSystem.Web.Controllers;

public class UserController : Controller
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;

    public UserController(SignInManager<ApplicationUser> signInManager,
                            UserManager<ApplicationUser> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ApplicationUser user = new ApplicationUser()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
        };
        await this.userManager.SetUserNameAsync(user, model.Email);
        await this.userManager.SetEmailAsync(user, model.Email);

        IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        await this.signInManager.SignInAsync(user, false);
        return this.RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        LoginFormModel model = new LoginFormModel()
        {
            ReturnUrl = returnUrl
        };
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return this.View(model);
        }

        var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        if (!result.Succeeded)
        {
            TempData["LoginError"] = "There was an error while logging you in! Please try again later or contact administrator.";
            return View(model);
        }
        return this.Redirect(model.ReturnUrl ?? "/Home/Index");
    }

    public async Task<IActionResult> Logout(string returnUrl = null)
    {
        await signInManager.SignOutAsync();

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
