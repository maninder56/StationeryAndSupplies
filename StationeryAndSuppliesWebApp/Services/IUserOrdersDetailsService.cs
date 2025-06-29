using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services; 

public interface IUserOrdersDetailsService
{
    // Read Operations
    public Task<List<UserOrderDetails>?> GetUserOrdersDetailsByUserIDAsync(int userID);

    // Update Operations
    public Task<bool> AddProductByIDInBaketByUserID(int userID, int productID, int quantity); 

}
