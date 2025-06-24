using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation; 
using DataBaseContextLibrary;
using StationeryAndSuppliesWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace StationeryAndSuppliesWebApp.Services;

public class AccountService : IAccountService
{
    private ILogger<AccountService> logger;

    private StationeryAndSuppliesDatabaseContext database;

    private IHttpContextAccessor httpContextAccessor; 

    public AccountService(
        ILogger<AccountService> logger, 
        StationeryAndSuppliesDatabaseContext database, 
        IHttpContextAccessor httpContextAccessor)
    {
        this.logger = logger;
        this.database = database;
        this.httpContextAccessor = httpContextAccessor;
    }


    // Helper methods 

    private async Task<byte[]> CreateHashFromPassword(string password, byte[] saltBytes)
    {
        return await Task.Run(() =>
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                // Three properties below can never change
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10_000,
                numBytesRequested: 64);
        }); 
    }

    private async Task<bool> VerifyPassword(string password, string storedPassword, string storedSalt)
    {
        byte[] storedsaltBytes = Convert.FromBase64String(storedSalt);
        byte[] storedPasswordBytes = Convert.FromBase64String(storedPassword);

        byte[] providedPassword = await CreateHashFromPassword(password, storedsaltBytes);

        return CryptographicOperations.FixedTimeEquals(storedPasswordBytes, providedPassword);
    }



    // mock user for testing
    public async Task<LoggedInUser?> AuthenticateUserAsync(string email, string password)
    {
        return await Task.Run(async () =>
        {
            logger.LogInformation("Requested to Authenticate user with email {Email}", email);

            if (string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password))
            {
                logger.LogWarning("User Email or passwrd is null or empty"); 
                return null;
            }

            User? user = database.Users.AsNoTracking()
                .FirstOrDefault(u => u.Email == email);

            if (user is null)
            {
                logger.LogWarning("User with Email {Email} does not exists", email); 
                return null;
            }

           bool passwordIsCorrect = await VerifyPassword(password, user.PasswordHash, user.PasswordSalt);

            if (passwordIsCorrect)
            {
                logger.LogInformation("User with email {Email} has provided correct credentials", email);
                return new LoggedInUser(user.UserId, user.Name, email); 
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

            User? user = database.Users.AsNoTracking()
                .FirstOrDefault(u => u.UserId == id);   

            if (user is not null)
            {
                return new UserDetails(id, user.Name, user.Email, user.Phone); 
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
