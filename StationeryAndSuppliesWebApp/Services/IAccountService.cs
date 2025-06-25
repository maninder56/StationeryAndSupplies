using System.Security.Claims; 
using StationeryAndSuppliesWebApp.Models;

namespace StationeryAndSuppliesWebApp.Services; 

public interface IAccountService
{
    //public Task<bool> UserExistsByEmailAsync(string email); 

    public Task<LoggedInUser?> AuthenticateUserAsync(string email, string password);

    public Task<UserDetails?> GetUserDetailsByIDAsync(int id);

    public Task<int?> GetUserIDFromHttpContextAsync();

    public Task<bool> CreateNewAccountAsync(string userName, string email, string password, string? phone);

    public Task<bool?> EmailExistsAsync(string email);

    public Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(int id, string userName); 

    public Task<int?> GetUserIDByEmailAsync(string email);

    public Task<bool> UpdateUserPasswordByEmail(string email, string newPassword);

}
