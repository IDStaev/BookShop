using BookShop.Common;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookShop.Controllers;

[Route("[controller]/[action]")]
public class AuthController : Controller
{
    private readonly UserRepository _userRepository;
    private readonly Hasher _hasher;

    public AuthController(UserRepository userRepository, Hasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        if (CurrentUser.CurrentUserId != null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var model = new RegisterViewModel();
        
        return View(model);
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (_userRepository.GetByEmail(model.Email) != null)
        {
            ModelState.AddModelError(nameof(model.Email), "User with that email already exists.");
            return View(model);
        }

        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PasswordHash = _hasher.GetHash(model.Password)
        };

        _userRepository.Create(user);

        return RedirectToAction("LogIn");
    }

    [HttpGet]
    public IActionResult LogIn()
    {
        if (CurrentUser.CurrentUserId != null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var model = new LogInViewModel();

        return View(model);
    }

    [HttpPost]
    public IActionResult LogIn(LogInViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = _userRepository.GetByEmail(model.Email);

        if (user == null)
        {
            ModelState.AddModelError(nameof(model.Password), "Invalid credentials.");
            return View(model);
        }

        if (user.PasswordHash != _hasher.GetHash(model.Password))
        {
            ModelState.AddModelError(nameof(model.Password), "Invalid credentials.");
            return View(model);
        }

        CurrentUser.CurrentUserId = user.Id;

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult LogOut()
    {
        if (CurrentUser.CurrentUserId == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        CurrentUser.CurrentUserId = null;

        return RedirectToAction("Index", "Home");
    }
}