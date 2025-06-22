using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services; 

public interface IAccountService
{
    //public Task<bool> UserExistsByEmailAsync(string email); 

    public Task<LoggedInUser?> AuthenticateUserAsync(string email, string password);

    public Task<UserDetails?> GetUserDetailsByIDAsync(int id);

    public Task<int?> GetUserIDFromHttpContextAsync(); 

}
