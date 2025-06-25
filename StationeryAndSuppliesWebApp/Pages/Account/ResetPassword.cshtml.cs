using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;

namespace StationeryAndSuppliesWebApp.Pages.Account;

public class ResetPasswordModel : PageModel
{
    private ILogger<ResetPasswordModel> logger;

    private IAccountService accountService; 

    public ResetPasswordModel(ILogger<ResetPasswordModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }

    // data for the view 
    public string? ValidationMessage { get; private set; }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel(); 

    public IActionResult OnGet()
    {
        logger.LogInformation("Reset password page requested"); 
        return Page(); 
    }

    public async Task<IActionResult> OnPostAsync()
    {
        logger.LogInformation("Requested to reset password by email {Email}", Input.Email);

        bool validateInput = await ValidateUserDetailsAsync();

        if (!validateInput)
        {
            return Page();
        }

        bool passwordUpdated = await accountService.UpdateUserPasswordByEmail(Input.Email, Input.Password);

        if (!passwordUpdated)
        {
            logger.LogInformation("Failed to update user password by email {Email}", Input.Email); 
            ValidationMessage = "Failed to update password, please try again"; 
            return Page();
        }
        else
        {
            logger.LogInformation("Successfully update user password by email {Email}", Input.Email); 
            return RedirectToPage("/Account/Login");
        }
    }



    private async Task<bool> ValidateUserDetailsAsync()
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Model state is invalid for Create Account page");
            ValidationMessage = "Please provide all details";
            return false;
        }

        if (Input.Password != Input.Repeat_Password)
        {
            logger.LogWarning("User with email {Email} failed to repeat password", Input.Email);
            ValidationMessage = "Repeated password does not match, Please Try again";
            return false;
        }

        bool? emailExists = await accountService.EmailExistsAsync(Input.Email);

        if (emailExists is null)
        {
            logger.LogWarning("User provided null or empty email");
            ValidationMessage = "Please provide your email";
            return false;
        }

        if (!(bool)emailExists)
        {
            logger.LogWarning("User provided email {Email} which Does not exists", Input.Email);
            ValidationMessage = "Invalid Email";
            return false;
        }

        return true;
    }


    public class InputModel
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string Repeat_Password { get; set; } = string.Empty;
    }

}
