using DataBaseContextLibrary;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation; 
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Models;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using System.Security.Cryptography;

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

    private async Task<(string passwordHash, string saltHash)>GeneratePasswordAndSaltHash(string password)
    {
        byte[] saltBytes = new byte[16];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        byte[] hashedPasswordBytes = await CreateHashFromPassword(password, saltBytes);

        string saltInBase64String = Convert.ToBase64String(saltBytes);
        string passwordInBase64String = Convert.ToBase64String(hashedPasswordBytes);

        return (passwordHash: passwordInBase64String, saltHash: saltInBase64String);
    }



    // Account service methods
    
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

            User? user = await database.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

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
        logger.LogInformation("Requested detailes of user with ID {UserID}", id);

        if (id < 1)
        {
            logger.LogWarning("User ID is less than 1");
            return null; 
        }

        User? user = await database.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == id);   

        if (user is not null)
        {
            return new UserDetails(id, user.Name, user.Email, user.Phone); 
        }

        logger.LogWarning("User with ID {userID} was not Found", id); 
        return null;    
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


    public async Task<bool> CreateNewAccountAsync(string userName, string email, string password, string? phone)
    {
        logger.LogInformation("Requested to create new accout with User name {UserName} and Email {Email}",
            userName, email); 

        if (string.IsNullOrEmpty(userName) || 
            string.IsNullOrEmpty(email) || 
            string.IsNullOrEmpty(password))
        {
            logger.LogWarning("User name, email or password is null"); 
            return false;
        }

        bool anotherEmailExists = await database.Users.AsNoTracking().AnyAsync(u => u.Email == email); 

        if (anotherEmailExists)
        {
            logger.LogWarning("Another Account already exists with eamil {Email}", email); 
            return false;
        }

        // Generate password hash and salt hash to store
        (string passwordToStore, string saltToStore) = await GeneratePasswordAndSaltHash(password); 

        database.Users.Add(new User()
        {
            Name = userName,
            Email = email,
            Phone = phone,
            PasswordHash = passwordToStore,
            PasswordSalt = saltToStore
        }); 

        int userSaved = await database.SaveChangesAsync();

        if (userSaved == 0)
        {
            logger.LogWarning("Failed to Add new user with email {Email}", email);
            return false;
        }

        return true;

    }


    public async Task<bool?> EmailExistsAsync(string email)
    {
        logger.LogInformation("Requested to check Email {Email} Exists", email);

        if (string.IsNullOrEmpty(email))
        {
            logger.LogWarning("Provided Email is empty"); 
            return null;
        }

        bool anotherEmail = await database.Users.AsNoTracking().AnyAsync(u => u.Email == email);

        return anotherEmail;
    }

    public async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(int id, string userName)
    {
        return await Task.Run(() =>
        {
            logger.LogInformation("Requested to create claims principal for user ID {UserID} and user name {UserName}", 
                id, userName);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserID", id.ToString()),
                new Claim("FullName", userName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            return new ClaimsPrincipal(claimsIdentity);
        });
    }


    public async Task<int?> GetUserIDByEmailAsync(string email)
    {
        logger.LogInformation("Requested to get user ID by email {Email}", email); 

        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        int userID =  await database.Users.AsNoTracking()
            .Where(u => u.Email == email)
            .Select(u => u.UserId)
            .FirstOrDefaultAsync();

        return userID == 0 ? null : userID;
    }

    public async Task<bool> UpdateUserPasswordByEmail(string email, string newPassword)
    {
        logger.LogInformation("Requested to update user password by email {Email}", email); 

        if (string.IsNullOrEmpty(email))
        {
            logger.LogWarning("Provided email is empty"); 
            return false; 
        }

        User? user = await database.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            logger.LogWarning("User with {Email} does not exists", email); 
            return false;
        }

        // Generate password hash and salt hash to store for new password
        (string passwordToStore, string saltToStore) = await GeneratePasswordAndSaltHash(newPassword);

        user.PasswordHash = passwordToStore;
        user.PasswordSalt = saltToStore;

        int updated = await database.SaveChangesAsync();

        if (updated == 0)
        {
            logger.LogWarning("Failed to save new password by eamil {Email}", email); 
            return false;
        }
        else
        {
            logger.LogInformation("Successfully updated user password by email {Email}", email); 
            return true;
        }
    }
}
