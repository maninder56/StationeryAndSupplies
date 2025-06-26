using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages.Account;

[Authorize]
public class OrdersModel : PageModel
{
    private ILogger<OrdersModel> logger;

    private IUserOrdersDetailsService orderService; 

    public OrdersModel(ILogger<OrdersModel> logger, IUserOrdersDetailsService orderService)
    {
        this.logger = logger;
        this.orderService = orderService;
    }



    // Data for view 
    public List<UserOrderDetails>? OrderList { get; private set; } = new List<UserOrderDetails>();
    public string? ErrorMessage { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Orders page requested"); 

        int userID;

        string userIDString = HttpContext?.User.FindFirst("UserID")?.Value ?? string.Empty; 

        bool userIDParsed = int.TryParse(userIDString, out userID);

        if(!userIDParsed)
        {
            logger.LogWarning("Falied to parse userID from {UserIDInString}", userIDString); 
            ErrorMessage = "Error while loading user data, Please try again.";
            return Page(); 
        }

        OrderList = await orderService.GetUserOrdersDetailsByUserIDAsync(userID);

        if (OrderList is null)
        {
            logger.LogWarning("Failed to load user orders with user ID {UserID}", userID);
            ErrorMessage = "Error while loading orders, Pleae try again.";
            return Page(); 
        }

        return Page();

    }
}
