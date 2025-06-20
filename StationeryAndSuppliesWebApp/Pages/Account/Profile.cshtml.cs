using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages.Account;

[Authorize]
public class ProfileModel : PageModel
{
    private ILogger<ProfileModel> logger;  

    private IAccountService accountService;

    public ProfileModel(ILogger<ProfileModel> logger, IAccountService accountService)
    {
        this.logger = logger;
        this.accountService = accountService;
    }


    // Data shared with view
    public UserDetails? UserDetails { get; private set; }


    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Profile Page requested");




        return Page(); 
    }
}
