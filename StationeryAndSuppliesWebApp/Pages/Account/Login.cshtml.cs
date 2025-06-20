using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StationeryAndSuppliesWebApp.Pages.Account; 

public class LoginModel : PageModel
{
    private ILogger<LoginModel> logger;

    private IAccountService accountService; 

    public LoginModel(ILogger<LoginModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }

    // data for view 
    public string? ValidationMessage { get; private set; }


    [BindProperty]
    public string? ReturnUrl { get; set; }

    [BindProperty]
    public InputMode Input { get; set; } = new InputMode(); 


    // Login page handlers 
    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        logger.LogInformation("Login Page requested"); 

        if (!string.IsNullOrEmpty(returnUrl))
        {
            ReturnUrl = returnUrl;
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Model state is invalid for Login page"); 
            ValidationMessage = "Please fill all the fields"; 
            return Page();
        }

        if (!await accountService.AuthenticateUserAsync(Input.Email, Input.Password))
        {
            logger.LogWarning("User {Email} had Invalid login attempt", Input.Email); 
            ValidationMessage = "Invalid Email or Password";
            return Page();
        }

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, Input.Email)
        }; 

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, 
            CookieAuthenticationDefaults.AuthenticationScheme);

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal);

        logger.LogInformation("User with eamil {Email} logged in at {Time}",
            Input.Email, DateTime.UtcNow); 

        return LocalRedirect(ReturnUrl ?? "/");

    }


    public class InputMode
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
