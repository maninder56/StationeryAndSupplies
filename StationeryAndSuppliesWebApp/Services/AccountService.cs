
using DataBaseContextLibrary;
using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services;

public class AccountService : IAccountService
{
    private ILogger<AccountService> logger;

    private StationeryAndSuppliesDatabaseContext database;

    private IHttpContextAccessor httpContextAccessor; 

    private string mockUserkEmail = "user@gmail.com";
    private string mockUserName = "Dazai Ken";
    private int mockUserID = 7; 

    public AccountService(
        ILogger<AccountService> logger, 
        StationeryAndSuppliesDatabaseContext database, 
        IHttpContextAccessor httpContextAccessor)
    {
        this.logger = logger;
        this.database = database;
        this.httpContextAccessor = httpContextAccessor;
    }


    // mock user for testing
    public async Task<LoggedInUser?> AuthenticateUserAsync(string email, string password)
    {
        return await Task.Run(() =>
        {
            logger.LogInformation("Requested to Authenticate user with email {Email}", email);

            if (string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password))
            {
                logger.LogWarning("User Email or passwrd is null or empty"); 
                return null;
            }

            if (email == mockUserkEmail &&  password == "pass")
            {
                logger.LogInformation("User with email {Email} has provided correct credentials", email); 
                return new LoggedInUser(mockUserID, mockUserName, mockUserkEmail);
            }

            logger.LogWarning("Authentication failed for the user with email {Eamil}", email); 
            return null;
        }); 
    }

    public async Task<UserDetails?> GetUserDetailsByIDAsync(int id)
    {
        return await Task.Run(() =>
        {
            logger.LogInformation("Requested detailes of user with ID {UserID}", id);

            if (id < 1)
            {
                logger.LogWarning("User ID is less than 1");
                return null; 
            }

            if (id == mockUserID)
            {
                return new UserDetails(id, mockUserName, mockUserkEmail, phone: "939384940"); 
            }

            logger.LogWarning("User with ID {userID} was not Found", id); 
            return null; 
        }); 
    }

    public async Task<int?> GetUserIDFromHttpContextAsync()
    {
        return await Task.Run(GetUserID);

        int? GetUserID()
        {
            logger.LogInformation("Requested user id from HttpContext");

            string? userIDClaimValue = httpContextAccessor.HttpContext?.User.FindFirst("UserID")?.Value;

            if (string.IsNullOrEmpty(userIDClaimValue))
            {
                logger.LogWarning("No UserID Claim exists in claims principal");
                return null;
            }

            if (int.TryParse(userIDClaimValue, out int userID))
            {
                logger.LogInformation("Found User with ID {UserID} in Claims principal", userID);
                return userID;
            }


            logger.LogInformation("User with ID {userIDClaimValue} failed to Parse", userIDClaimValue);
            return null;
        }

    }
}
