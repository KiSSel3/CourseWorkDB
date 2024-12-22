using System.Security.Claims;
using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.BLL.ViewModels.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentPlace.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Authorization()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View("Authorization");
        }
        
        try
        {
            var response = await _userService.LoginAsync(model, cancellationToken);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(response));

            return Redirect("/");
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("AuthorizationError", ex.Message);

            return View("Authorization");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View("Authorization");
        }
        
        try
        {
            var response = await _userService.RegisterAsync(model, cancellationToken);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(response));

            return Redirect("/");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("AuthorizationError", ex.Message);

            return View("Authorization");
        }
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}