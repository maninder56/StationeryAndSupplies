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
    public bool? ItemRemovedFromCart { get; private set; }

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
            logger.LogInformation("Successfully loaded {NumberOfItems} items in userbag",
                userCartDetails.Items.Count); 
            return Page();
        }
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid Input model, when attempted to remove item from cart");
            return await OnGetAsync(); 
        }

        if (Input is null)
        {
            logger.LogWarning("Input is null, item id can not be obtained to remove item from cart"); 
            return await OnGetAsync();
        }

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from HttpContext service");
            return await OnGetAsync();
        }

        bool removed = await userOrdersDetailsService.RemoveCartItemFromCartByID((int)userID, Input.CartItemId); 

        if (removed)
        {
            logger.LogInformation("Cart Item with ID {CartItemID} has been removed from user's cart with user ID {UserID}",
                Input.CartItemId, userID); 
            ItemRemovedFromCart = true; 
        }
        else
        {
            logger.LogWarning("Failed to remove cart item from user's cart, CartItemID {CartItemID} UserID {UserID}",
                Input.CartItemId, userID); 
            ItemRemovedFromCart = false;
        }

        return await OnGetAsync();
    }


    public class InputModel
    {
        [Required]
        [Range(1, int.MaxValue)]    
        public int CartItemId { get; set; }
    }
}
