using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StationeryAndSuppliesWebApp.Pages.Account;

[Authorize]
public class EditProfileModel : PageModel
{
    private ILogger<EditProfileModel> logger;

    private IAccountService accountService; 

    public EditProfileModel(ILogger<EditProfileModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }

    // Data for the view 
    public string? ValidationMessage { get; private set; }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Edit profile page requested");

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from service");
            return Page();
        }

        UserDetails? userDetails = await accountService.GetUserDetailsByIDAsync((int)userID);

        if (userDetails is null)
        {
            logger.LogWarning("Failed to get user details of User with ID {UserID}", userID);
            return Page();
        }

        Input.Name = userDetails?.UserName ?? string.Empty; 
        Input.Phone = userDetails?.Phone;

        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            if (ModelState.GetValidationState("Input.Phone") == ModelValidationState.Invalid)
            {
                logger.LogWarning("User provided invalid phone number while attempting to change profile details");
                ValidationMessage = "Phone number is Invalid";
                return Page();
            }

            logger.LogWarning("Model state is invalid for Edit profile page");
            ValidationMessage = "Please provide all non-optional details";
            return Page();
        }

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from service");
            ValidationMessage = "Error while saving, Please try again"; 
            return Page();
        }

        bool profileUpdated = await accountService.UpdateUserDetailsByIDAsync(
            (int)userID, Input.Name, Input.Phone);

        if (profileUpdated)
        {
            ClaimsPrincipal claimsPrincipal = await accountService.CreateClaimsPrincipalAsync((int)userID, Input.Name);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);

            logger.LogInformation("User profile updated by user ID {UserID}, and recreated cookie for that user", 
                userID); 
            return RedirectToPage("/Account/UserDetails");
        }
        else
        {
            logger.LogWarning("Account service failed to update profile by user ID {UserID}", userID);
            ValidationMessage = "Failed to update profile, Please try again";
            return Page();
        }
    }


    public class InputModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Phone]
        [StringLength(100)]
        public string? Phone { get; set; }
    }
}
