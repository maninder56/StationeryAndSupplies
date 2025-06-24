using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding; 

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
    public InputMode Input {  get; set; } = new InputMode();

    public IActionResult OnGet()
    {
        logger.LogInformation("Create Account page requested"); 
        return Page(); 
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            if (ModelState.GetValidationState("Input.Phone") == ModelValidationState.Invalid)
            {
                logger.LogWarning("User with email {Email} provided invalid phone number", Input.Email);
                ValidationMessage = "Phone number is Invalid"; 
                return Page();
            }

            logger.LogWarning("Model state is invalid for Create Account page");
            ValidationMessage = "Please provide all non-optional details"; 
            return Page();
        }

        if (Input.Password != Input.Repeat_Password)
        {
            logger.LogWarning("User with email {Email} failed to repeat password", Input.Email);
            ValidationMessage = "Repeated password is incorrect, Please Try again"; 
            return Page();
        }

        bool? anotherEmailExists = await accountService.CheckAnotherEmailExistsAsync(Input.Email); 

        if (anotherEmailExists is null)
        {
            logger.LogWarning("User provided null or empty email");
            ValidationMessage = "Please provide your email"; 
            return Page();
        }

        if ((bool)anotherEmailExists)
        {
            logger.LogWarning("User provided email {Email} which already exists", Input.Email);
            ValidationMessage = "An account already exists with this email"; 
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
            logger.LogInformation("Successfuly created new account with email {Email}, redirected user to log in page",
                Input.Email); 
            return RedirectToPage("/Account/Login"); 
        }
    }





    public class InputMode
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

        [StringLength(64)] 
        public string Password { get; set; } = string.Empty;

        [StringLength(64)]
        public string Repeat_Password { get; set; } = string.Empty;
    }

}
