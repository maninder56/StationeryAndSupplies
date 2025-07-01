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




    public class InputModel
    {
        [Required]
        [StringLength(300)]
        public string ShippingAddress { get; set; } = string.Empty;
    }

}
