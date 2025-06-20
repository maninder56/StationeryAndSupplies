namespace StationeryAndSuppliesWebApp.Services; 

public interface IAccountService
{
    //public Task<bool> UserExistsByEmailAsync(string email); 

    public Task<bool> AuthenticateUserAsync(string email, string password); 
}
