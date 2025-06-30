using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services; 

public interface IUserOrdersDetailsService
{
    // Read Operations
    public Task<List<UserOrderDetails>?> GetUserOrdersDetailsByUserIDAsync(int userID);

    public Task<UserCartDetails?> GetUserCartDetailsByUserIDAsync(int userID); 

    // Update Operations
    public Task<bool> AddProductByIDInCartByUserID(int userID, int productID, int quantity); 


    // Delete Operations 
    public Task<bool> RemoveCartItemFromCartByID(int userID, int cartItemID);    

}
