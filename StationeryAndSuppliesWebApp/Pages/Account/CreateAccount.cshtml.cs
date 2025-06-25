using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding; 
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace StationeryAndSuppliesWebApp.Pages.Account;

public class CreateAccountModel : PageModel
{
    private ILogger<CreateAccountModel> logger; 

    private IAccountService accountService;

    public CreateAccountModel(ILogger<CreateAccountModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }

    // data for the view 
    public string? ValidationMessage { get; private set; }

    [BindProperty]
    public InputModel Input {  get; set; } = new InputModel();

    public IActionResult OnGet()
    {
        logger.LogInformation("Create Account page requested"); 
        return Page(); 
    }

    public async Task<IActionResult> OnPostAsync()
    {
        bool validateInput = await ValidateUserDetailsAsync(); 

        if (!validateInput)
        {
            return Page();
        }

        bool accountCreated = await accountService.CreateNewAccountAsync(
            Input.Name, Input.Email, Input.Password, Input.Phone);

        if (!accountCreated)
        {
            logger.LogWarning("Account service failed to create new account");
            ValidationMessage = "Failed to create Account, Please try again"; 
            return Page();
        }
        else
        {
            logger.LogInformation("Successfuly created new account with email {Email}",
                Input.Email); 

            int? userID = await accountService.GetUserIDByEmailAsync(Input.Email);

            if (userID is null)
            {
                ValidationMessage = "Problem occured while automatic login, Please go to log in page"; 
                return Page();
            }

            ClaimsPrincipal claimsPrincipal = await accountService.CreateClaimsPrincipalAsync((int)userID, Input.Name);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);

            logger.LogInformation("User with eamil {Email} logged in at {Time}",
                Input.Email, DateTime.UtcNow);

            return RedirectToPage("/Index");
        }
    }



    private async Task<bool> ValidateUserDetailsAsync()
    {
        if (!ModelState.IsValid)
        {
            if (ModelState.GetValidationState("Input.Phone") == ModelValidationState.Invalid)
            {
                logger.LogWarning("User with email {Email} provided invalid phone number", Input.Email);
                ValidationMessage = "Phone number is Invalid";
                return false;
            }

            logger.LogWarning("Model state is invalid for Create Account page");
            ValidationMessage = "Please provide all non-optional details";
            return false; 
        }

        if (Input.Password != Input.Repeat_Password)
        {
            logger.LogWarning("User with email {Email} failed to repeat password", Input.Email);
            ValidationMessage = "Repeated password does not match, Please Try again";
            return false;
        }

        bool? anotherEmailExists = await accountService.EmailExistsAsync(Input.Email);

        if (anotherEmailExists is null)
        {
            logger.LogWarning("User provided null or empty email");
            ValidationMessage = "Please provide your email";
            return false;
        }

        if ((bool)anotherEmailExists)
        {
            logger.LogWarning("User provided email {Email} which already exists", Input.Email);
            ValidationMessage = "An account already exists with this email";
            return false;
        }

        return true;
    }





    public class InputModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(100)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(64)] 
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string Repeat_Password { get; set; } = string.Empty;
    }

}
