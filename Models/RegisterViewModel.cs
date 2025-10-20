using System.ComponentModel.DataAnnotations;

namespace BookShop.Models;

public class RegisterViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}