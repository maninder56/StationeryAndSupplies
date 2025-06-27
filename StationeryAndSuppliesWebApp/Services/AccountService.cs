using DataBaseContextLibrary;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Tls;
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

    private async Task<byte[]> CreateHashFromPasswordAsync(string password, byte[] saltBytes)
    {
        return await Task.Run(() =>
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                // Three properties below can never change
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 210_000,
                numBytesRequested: 64);
        }); 
    }

    private async Task<bool> VerifyPasswordAsync(string password, string storedPassword, string storedSalt)
    {
        byte[] storedsaltBytes = Convert.FromBase64String(storedSalt);
        byte[] storedPasswordBytes = Convert.FromBase64String(storedPassword);

        byte[] providedPassword = await CreateHashFromPasswordAsync(password, storedsaltBytes);

        return CryptographicOperations.FixedTimeEquals(storedPasswordBytes, providedPassword);
    }

    private async Task<(string passwordHash, string saltHash)>GeneratePasswordAndSaltHash(string password)
    {
        byte[] saltBytes = new byte[16];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        byte[] hashedPasswordBytes = await CreateHashFromPasswordAsync(password, saltBytes);

        string saltInBase64String = Convert.ToBase64String(saltBytes);
        string passwordInBase64String = Convert.ToBase64String(hashedPasswordBytes);

        return (passwordHash: passwordInBase64String, saltHash: saltInBase64String);
    }



    // Account service methods


    // Create Operations

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




    // Read Operations

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

           bool passwordIsCorrect = await VerifyPasswordAsync(password, user.PasswordHash, user.PasswordSalt);

            if (passwordIsCorrect)
            {
                logger.LogInformation("User with email {Email} has provided correct credentials", email);
                return new LoggedInUser(user.UserId, user.Name, email); 
            }

            logger.LogWarning("Authentication failed for the user with email {Eamil}, Invalid password", email); 
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




    // Update Operations

    public async Task<bool> UpdateUserPasswordByEmailAsync(string email, string newPassword)
    {
        logger.LogInformation("Requested to update user password by email {Email}", email); 

        if (string.IsNullOrEmpty(email))
        {
            logger.LogWarning("Provided email is empty"); 
            return false; 
        }

        // only allow users who have user id bigger than 10 to avoid mock users
        User? user = await database.Users
            .Where(u => u.Email == email && u.UserId > 10)
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


    public async Task<bool> UpdateUserDetailsByIDAsync(int id, string newName, string? newPhone)
    {
        logger.LogInformation("Requested to update user details by user ID {UserID}", id); 

        if (id < 1)
        {
            logger.LogWarning("Given user id {UserID} is less than 1", id); 
            return false;
        }

        // only allow users who have user id bigger than 10 to avoid mock users
        User? user = await database.Users
            .Where(u => u.UserId == id && u.UserId > 10)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            logger.LogWarning("User with ID {UserID} does not exists", id); 
            return false;
        }

        bool isNameNew = false, isPhoneNew = false; 

        // Only make changes if new data is differenct than existing one
        if ((!string.IsNullOrEmpty(newName)) && user.Name != newName)
        {
            user.Name = newName;
            isNameNew = true;
        }

        if ((!string.IsNullOrEmpty(newPhone)) && user.Phone != newPhone)
        {
            user.Phone = newPhone;
            isPhoneNew = true;
        }

        if (!(isNameNew || isPhoneNew))
        {
            logger.LogInformation("No new details provided for update, no changes were made to profile"); 
            return true; 
        }

        int updated = await database.SaveChangesAsync();

        if (updated == 0)
        {
            logger.LogWarning("Failed to save new user details by user ID {UserID}", id); 
            return false;
        }
        else
        {
            logger.LogInformation("Successfully updated user details by user ID {UserID}", id); 
            return true; 
        }

    }



    // Delete Operations 

    public async Task<bool> DeleteUserAccountByIDAsync(int id)
    {
        logger.LogInformation("Requested to delete user account by user ID {userID}", id);

        if (id < 1)
        {
            logger.LogWarning("Given user id {UserID} is less than 1", id);
            return false;
        }

        // only allow users who have user id bigger than 10 to avoid mock users
        User? user = await database.Users
            .Where(u => u.UserId == id && u.UserId > 10)
            .Include(u => u.Cart)
                .ThenInclude(c => c!.CartItems)
            .Include(u => u.Reviews)
            .Include(u => u.Orders)
                .ThenInclude(o => o.OrderItems)
            .Include(u => u.Orders)
                .ThenInclude(o => o.Payment)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            logger.LogWarning("User with ID {UserID} does not exists", id);
            return false;
        } 

        database.Users.Remove(user);

        int deleted = await database.SaveChangesAsync();

        if (deleted == 0)
        {
            logger.LogWarning("Failed to delete user account by user ID {UserID}", id);
            return false;
        }
        else
        {
            logger.LogInformation("Successfully deleted user account by user ID {UserID} from database", id);
            return true;
        }
    }
}
