using DataBaseContextLibrary;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Models;
using System.Security.AccessControl;

namespace StationeryAndSuppliesWebApp.Services;

public class UserOrdersDetailsService : IUserOrdersDetailsService
{
    private ILogger<UserOrdersDetailsService> logger;

    private StationeryAndSuppliesDatabaseContext database;

    private readonly decimal shippingCost = 2.99M; 

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


    public async Task<UserCartDetails?> GetUserCartDetailsByUserIDAsync(int userID)
    {
        logger.LogInformation("Requested to get user cart details by user ID {UserID}", userID); 

        if (userID < 1)
        {
            logger.LogWarning("User ID {UserID} is less than 1", 
                userID);
            return null;
        }

        User? user = await database.Users.AsNoTracking()
            .Where(u => u.UserId == userID)
            .Include(u => u.Cart)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            logger.LogWarning("User with ID {UserID} does not exists", userID);
            return null;
        }

        if (user.Cart is null)
        {
            logger.LogWarning("User with ID {UserID} have Empty cart", userID); 
            return null;
        }

        UserCartDetails userCartDetails = new UserCartDetails()
        {
            Items = await database.CartItems.AsNoTracking()
                .Where(ci => ci.CartId == user.Cart.CartId)
                .Select(ci => new UserCartIItem
                {
                    cartItemID = ci.CartItemId,
                    Name = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    ImageUrl = ci.Product.ImageUrl, 
                }).ToListAsync(),

            ShippingCost = shippingCost
        };   

        if (userCartDetails.Items.Count == 0)
        {
            logger.LogWarning("Failed to get cart detail of user with ID {UserID}", userID); 
            return null; 
        }
        else
        {
            logger.LogInformation("Loaded total {NumberOfItems} items in user cart detials",
                userCartDetails.Items.Count); 
            return userCartDetails;
        }
    }



    // Update Operations

    public async Task<bool> AddProductByIDInCartByUserID(int userID, int productID, int quantity)
    {
        logger.LogInformation("Requested to add Product in user's cart by product ID {ProductID} with Quanitity {Quanitity} for user with ID {UserID}", 
            productID, quantity, userID);

        if (productID < 1 ||  quantity < 1)
        {
            logger.LogWarning("Product id or quantity is less than 1"); 
            return false;
        }

        User? user = await database.Users
            .Where(u => u.UserId == userID)
            .Include(u => u.Cart)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            logger.LogWarning("User with ID {UserID} does not exists", userID); 
            return false; 
        }

        if (user.Cart is not null)
        {
            logger.LogInformation("User with ID {UserID} has cart with ID {CartID}", userID, user.Cart.CartId); 

            user.Cart.CartItems.Add(new CartItem
            {
                ProductId = productID,
                Quantity = quantity,
            }); 
        }
        else
        {
            logger.LogInformation("User with ID {UserID} does not yet have cart, creating new cart", userID); 

            user.Cart = new Cart()
            {
                CartItems = new List<CartItem>()
                {
                    new CartItem()
                    {
                        ProductId = productID, 
                        Quantity= quantity, 
                    }
                }
            }; 
        }

        int itemsAddedInCart = await database.SaveChangesAsync(); 

        if (itemsAddedInCart > 0)
        {
            logger.LogInformation("Product with ID {ProductID} has been added to user's cart with user ID {UserID}",
                productID, userID); 
            return true;
        }
        else
        {
            logger.LogWarning("Failed to add product in user's cart, user ID {UserID}", userID); 
            return false;
        }
    }



    // Delete Operations 
    public async Task<bool> RemoveCartItemFromCartByID(int userID, int cartItemID)
    {
        logger.LogInformation("Requested to remove cart item from cart by cart item ID {cartItemID} for user with ID {UserID}",
            cartItemID, userID);

        if (userID < 1 || cartItemID < 1)
        {
            logger.LogInformation("User id {UserID} or cartItemID {CartItemID} is less than 1",
                userID, cartItemID);
            return false;
        }


        User? user = await database.Users.AsNoTracking()
            .Where(u => u.UserId == userID)
            .Include(u => u.Cart)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            logger.LogWarning("User with ID {UserID} does not exists", userID);
            return false;
        }

        if (user.Cart is null)
        {
            logger.LogWarning("User with ID {UserID} have Empty cart", userID);
            return false;
        }

        CartItem? cartItem = await database.CartItems
            .Where(c => c.CartItemId == cartItemID && c.CartId == user.Cart.CartId)
            .FirstOrDefaultAsync();

        if (cartItem is null)
        {
            logger.LogWarning("User with ID {UserID} does not have cart item with ID {cartItemID} in cart with ID {cartID}",
                userID, cartItemID, user.Cart.CartId);
            return false;
        }

        database.CartItems.Remove(cartItem);

        int deleted = await database.SaveChangesAsync();

        if (deleted > 0)
        {
            logger.LogInformation("Successfully removed cart item from user's cart, UserID {UserID}, CartID {CartID}, CartItemID {CartItemID}", 
                userID, user.Cart.CartId, cartItemID);
            return true;
        }
        else
        {
            logger.LogWarning("Failed to remove cart item from user's cart, UserID {UserID}, CartID {CartID}, CartItemID {CartItemID}",
                userID, user.Cart.CartId, cartItemID);
            return false;
        }
    }






}
