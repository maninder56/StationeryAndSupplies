using DataBaseContextLibrary;
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Models;
using System.Security.AccessControl;

namespace StationeryAndSuppliesWebApp.Services;

public class UserOrdersDetailsService : IUserOrdersDetailsService
{
    private ILogger<UserOrdersDetailsService> logger;

    private StationeryAndSuppliesDatabaseContext database; 

    public UserOrdersDetailsService(
        ILogger<UserOrdersDetailsService> logger, 
        StationeryAndSuppliesDatabaseContext database)
    {
        this.logger = logger;
        this.database = database;
    }


    // User Orders details serivce methods 

    // Read operations 

    public async Task<List<UserOrderDetails>?> GetUserOrdersDetailsByUserIDAsync(int userID)
    {
        logger.LogInformation("Requested to get User order details with Use ID {UserID}", userID);

        if (userID < 1)
        {
            logger.LogWarning("User ID {UserID} provided which is less than 1", userID);
            return null; 
        }

        bool userHasOrders = await database.Orders.AsNoTracking()
            .AnyAsync(o => o.UserId == userID);

        if (!userHasOrders)
        {
            logger.LogWarning("User with ID {UserID} does not have orders", userID); 
            return new List<UserOrderDetails>();
        }

        // Get top 5 recent orders of the user
        List<UserOrderDetails> userOrderDetails = await database.Orders.AsNoTracking()
            .Where(o => o.UserId == userID)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .Select(o => new UserOrderDetails
            {
                OrderItems = o.OrderItems.Select(oi => new OrderItemDetail
                {
                    ProductName = oi.Product.Name,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList(),
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount
            }).ToListAsync(); 

        if (userOrderDetails.Count == 0)
        {
            logger.LogWarning("Failed to get user orders details with User ID {UserID}", userID); 
            return null;
        }

        logger.LogInformation("Total {TotalOrders} Found with user ID {UserID}", 
            userOrderDetails.Count, userID);
        return userOrderDetails;
    }



    // Update Operations

    public async Task<bool> AddProductByIDInBaketByUserID(int userID, int productID, int quantity)
    {
        logger.LogInformation("Requested to add Product by ID {ProductID} with Quanitity {Quanitity} for user with ID {UserID}", 
            productID, quantity, userID);

        if (productID < 1 ||  quantity < 1)
        {
            logger.LogWarning("Product id or quantity is less than 1"); 
        }

        User? user = await database.Users.AsNoTracking()
            .Where(u => u.UserId == userID)
            .FirstOrDefaultAsync();



    }


}
