using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StationeryAndSuppliesWebApp.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        private ILogger<AccessDeniedModel> logger; 

        public AccessDeniedModel(ILogger<AccessDeniedModel> logger)
        {
            this.logger = logger;
        }


        public IActionResult OnGet()
        {
            logger.LogInformation("Access Denied Page requested");
            return Page(); 
        }
    }
}
