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

    public async Task<List<UserOrderDetails>> GetUserOrdersDetailsByUserIDAsync(int userID)
    {
        logger.LogInformation("Requested to get User order details with Use ID {UserID}", userID);

        if (userID < 1)
        {
            logger.LogWarning("User ID {UserID} provided which is less than 1", userID);
            return new List<UserOrderDetails>(); 
        }

        bool userHasOrders = await database.Orders.AsNoTracking()
            .AnyAsync(o => o.UserId == userID);

        if (!userHasOrders)
        {
            logger.LogWarning("User with ID {UserID} does not have orders", userID); 
            return new List<UserOrderDetails>();
        }

        // Get top 5 recent orders of the user
        List<UserOrderDetails>? userOrderDetails = await database.Orders.AsNoTracking()
            .Where(o => o.UserId == userID)
            .Take(5)
            .OrderByDescending(o => o.OrderDate)
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

        if (userOrderDetails is null)
        {
            logger.LogWarning("Failed to get user order details with User ID {UserID}", userID); 
            return new List<UserOrderDetails>();
        }

        return userOrderDetails;
    }
}
