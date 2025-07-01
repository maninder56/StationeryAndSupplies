using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;

namespace StationeryAndSuppliesWebApp.Pages.Account;

[Authorize]
public class CheckoutModel : PageModel
{
    private ILogger<CheckoutModel> logger;

    private IAccountService accountService;

    private IUserOrdersDetailsService userOrdersDetailsService;


    public CheckoutModel(ILogger<CheckoutModel> logger, 
        IAccountService accountService, 
        IUserOrdersDetailsService userOrdersDetailsService)
    {
        this.logger = logger;
        this.accountService = accountService;
        this.userOrdersDetailsService = userOrdersDetailsService;
    }

    [BindProperty]
    public InputModel? Input { get; set; }

    // Data for view 
    public UserCartDetails? OrderSummary { get; private set; }
    public UserDetails? CurrnetUserDetails { get; private set; }
    public string? ErrorMessage { get; private set; }
    

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Checkout page requested");

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from HttpContext service");
            return Page();
        }

        CurrnetUserDetails = await accountService.GetUserDetailsByIDAsync((int)userID); 

        if (CurrnetUserDetails is null)
        {
            logger.LogWarning("Failed to load user detials for user with ID {UserID}", userID);
            return Page();
        }
        else
        {
            logger.LogInformation("Successfully loaded user detials for user with ID {UserID}", userID);
        }

        OrderSummary = await userOrdersDetailsService.GetUserCartDetailsByUserIDAsync((int)userID);

        if (OrderSummary is null)
        {
            logger.LogWarning("Failed to get order summary for checkout from service, for user with ID {UserID}", 
                userID);
            return Page();
        }
        else
        {
            logger.LogInformation("Successfully loaded {NumberOfItems} items in order summary for user with ID {UserID}",
                OrderSummary.Items.Count, userID);
            return Page();
        }
    }



    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid Model State, while attempting to place order");
            ErrorMessage = "Please Fill in all the Details"; 
            return await OnGetAsync(); 
        }

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from HttpContext service");
            ErrorMessage = "There was an error getting user details, please try again";
            return await OnGetAsync();
        }

        if (Input is null)
        {
            logger.LogWarning("Input Model is null, so can not get shipping address for user with ID {UserID}",
                userID);
            ErrorMessage = "Error Occured, please try again"; 
            return await OnGetAsync();
        }

        if (string.IsNullOrEmpty(Input.ShippingAddress))
        {
            logger.LogWarning("Empty shipping address recieved for user with ID {UserID}", userID);
            ErrorMessage = "Shipping Address is missing"; 
            return await OnGetAsync();
        }

        bool orderPlaced = await userOrdersDetailsService.PlaceAnOrderForUserByID((int)userID,Input.ShippingAddress); 

        if (orderPlaced)
        {
            return RedirectToPage("/Account/Orders"); 
        }
        else
        {
            ErrorMessage = "Error while processing your order, please try again";
            return await OnGetAsync();
        }
    }




    public class InputModel
    {
        [Required]
        [StringLength(300)]
        public string ShippingAddress { get; set; } = string.Empty;
    }

}
