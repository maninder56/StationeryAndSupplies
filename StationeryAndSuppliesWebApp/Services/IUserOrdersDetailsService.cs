using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services; 

public interface IUserOrdersDetailsService
{
    // Read operations 
    public Task<List<UserOrderDetails>?> GetUserOrdersDetailsByUserIDAsync(int userID);

}
