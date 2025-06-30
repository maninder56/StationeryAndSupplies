using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages.Account;

[Authorize]
public class UserDetailsModel : PageModel
{
    private ILogger<UserDetailsModel> logger;

    private IAccountService accountService;

    public UserDetailsModel(ILogger<UserDetailsModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }


    // Data for the view 
    public UserDetails? UserDetails { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("User Details Page Requested"); 

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from HttpContext service"); 
            return Page(); 
        }

        UserDetails = await accountService.GetUserDetailsByIDAsync((int)userID);
        
        if (UserDetails is null)
        {
            logger.LogWarning("Failed to get user details of User with ID {UserID}", userID);
        }

        return Page();
    }
}
