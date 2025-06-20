
using DataBaseContextLibrary;
using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services;

public class AccountService : IAccountService
{
    private ILogger<AccountService> logger;

    private StationeryAndSuppliesDatabaseContext database;

    private string mockUserkEmail = "user@gmail.com";
    private string mockUserName = "Dazai Ken"; 

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
                logger.LogWarning("User Email or passwrd is null or empty"); 
                return null;
            }

            if (email == mockUserkEmail &&  password == "pass")
            {
                logger.LogInformation("User with email {Email} has provided correct credentials", email); 
                return new LoggedInUser(mockUserName, email);
            }

            logger.LogWarning("Authentication failed for the user with email {Eamil}", email); 
            return null;
        }); 
    }

    public async Task<UserDetails?> GetUserDetailsByEmailAsync(string email)
    {
        logger.LogInformation("Requested detailes of user with eamil {Email}", email);

        return await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(email))
            {
                logger.LogWarning("User Email is null or empty");
                return null; 
            }

            if (email == mockUserkEmail)
            {
                return new UserDetails(mockUserName, mockUserkEmail); 
            }

            logger.LogWarning("User with email {Email} was not Found", email); 
            return null; 
        }); 

    }
}
