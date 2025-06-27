using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages.Account;

[Authorize]
public class DeleteAccountModel : PageModel
{
    private ILogger<DeleteAccountModel> logger;

    private IAccountService accountService;

    public DeleteAccountModel(ILogger<DeleteAccountModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }


    // Data for view 
    public string? ValidationMessage { get; private set; }

    public IActionResult OnGet()
    {
        logger.LogInformation("Delete account page requested");
        return Page(); 
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Model state is invalid for delete account page");
            ValidationMessage = "Error while processing your request";
            return Page();
        }

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from service");
            ValidationMessage = "Failed to load user account, Please try again";
            return Page();
        }

        bool accountDeleted = await accountService.DeleteUserAccountByIDAsync((int)userID); 

        if (accountDeleted)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            logger.LogInformation("Successfully deleted and logged out user account by user ID {UserID} at {Time}", 
                userID, DateTime.UtcNow);
            return RedirectToPage("/Index");
        }
        else
        {
            logger.LogWarning("Account service failed to delete account by user ID {UserID}", userID);
            ValidationMessage = "Error while processing your request, Please try again";
            return Page();
        }
    }

}
