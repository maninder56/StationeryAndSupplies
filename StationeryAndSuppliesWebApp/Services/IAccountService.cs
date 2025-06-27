using System.Security.Claims; 
using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services; 

public interface IAccountService
{
    // Create Operations 
    public Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(int id, string userName);
    public Task<bool> CreateNewAccountAsync(string userName, string email, string password, string? phone);

    // Read operations
    public Task<LoggedInUser?> AuthenticateUserAsync(string email, string password);
    public Task<UserDetails?> GetUserDetailsByIDAsync(int id);
    public Task<int?> GetUserIDFromHttpContextAsync();
    public Task<bool?> EmailExistsAsync(string email);
    public Task<int?> GetUserIDByEmailAsync(string email);

    // Update Operations 
    public Task<bool> UpdateUserPasswordByEmailAsync(string email, string newPassword);
    public Task<bool> UpdateUserDetailsByIDAsync(int id, string newName, string? newPhone);

    // Delete Operations 
    public Task<bool> DeleteUserAccountByIDAsync(int id);
}
