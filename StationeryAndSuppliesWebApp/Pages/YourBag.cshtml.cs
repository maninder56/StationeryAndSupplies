using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;

namespace StationeryAndSuppliesWebApp.Pages;

public class YourBagModel : PageModel
{
    private ILogger<YourBagModel> logger; 

    private IUserOrdersDetailsService userOrdersDetailsService;

    private IAccountService accountService;

    public YourBagModel(
        ILogger<YourBagModel> logger, 
        IUserOrdersDetailsService userOrdersDetailsService,
        IAccountService accountService)
    {
        this.logger = logger;
        this.userOrdersDetailsService = userOrdersDetailsService;
        this.accountService = accountService;
    }


    [BindProperty]
    public InputModel? Input { get; set; }

    // Data for view 
    public UserCartDetails? userCartDetails { get; private set; } 

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Your Bag Page requested");

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from HttpContext service");
            return Page();
        }

        userCartDetails = await userOrdersDetailsService.GetUserCartDetailsByUserIDAsync((int)userID);

        if (userCartDetails is null)
        {
            logger.LogWarning("Failed to get user cart detials from service"); 
            return Page();
        }
        else
        {
            logger.LogInformation("Successfully loaded items in userbag"); 
            return Page();
        }
    }


    public class InputModel
    {
        [Required]
        [Range(0, int.MaxValue)]    
        public int CartItemId { get; set; }
    }
}
