
using DataBaseContextLibrary;

namespace StationeryAndSuppliesWebApp.Services;

public class AccountService : IAccountService
{
    private ILogger<AccountService> logger;

    private StationeryAndSuppliesDatabaseContext database; 

    public AccountService(ILogger<AccountService> logger, StationeryAndSuppliesDatabaseContext database)
    {
        this.logger = logger;
        this.database = database;
    }


    // mock user for testing
    public async Task<bool> AuthenticateUserAsync(string email, string password)
    {
        logger.LogInformation("Requested to Authenticate user with email {Email}", email);

        return await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password))
            {
                logger.LogWarning("User Email or passwrd is null"); 
                return false;
            }

            if (email == "user@gmail.com" &&  password == "pass")
            {
                return true;
            }

            logger.LogWarning("Authentication failed "); 
            return false;
        }); 
    }
}
