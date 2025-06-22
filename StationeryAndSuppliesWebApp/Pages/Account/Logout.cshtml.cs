using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies; 

namespace StationeryAndSuppliesWebApp.Pages.Account;

[Authorize]
public class LogoutModel : PageModel
{
    private ILogger<LogoutModel> logger; 

    private IAccountService accountService;

    public LogoutModel(ILogger<LogoutModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }

    public IActionResult OnGet()
    {
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        logger.LogInformation("User requested to be logged out");

        int? userID = await accountService.GetUserIDFromHttpContextAsync();
        UserDetails? userDetails = null; 

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from service");
        }
        else
        {
            userDetails = await accountService.GetUserDetailsByIDAsync((int)userID);
        }

        if (userDetails is null)
        {
            logger.LogWarning("Failed to get user details of User with ID {UserID}", userID);
        }

        // Log user out even when failed to get user details
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 

        logger.LogInformation("User with ID {UserID}, Email {Email} logged out at {Time}.",
            userID, userDetails?.Email, DateTime.UtcNow); 
        return RedirectToPage("/Index"); 

    }
}
