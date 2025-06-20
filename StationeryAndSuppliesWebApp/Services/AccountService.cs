
using DataBaseContextLibrary;
using StationeryAndSuppliesWebApp.Models;

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
    public async Task<LoggedInUser?> AuthenticateUserAsync(string email, string password)
    {
        logger.LogInformation("Requested to Authenticate user with email {Email}", email);

        return await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password))
            {
                logger.LogWarning("User Email or passwrd is null"); 
                return null;
            }

            if (email == "user@gmail.com" &&  password == "pass")
            {
                logger.LogInformation("User with email {Email} has provided correct credentials", email); 
                return new LoggedInUser("Dazai Ken", email);
            }

            logger.LogWarning("Authentication failed for the user with email {Eamil}", email); 
            return null;
        }); 
    }

   
}
