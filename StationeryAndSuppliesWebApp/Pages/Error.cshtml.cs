using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StationeryAndSuppliesWebApp.Pages
{
    public class ErrorModel : PageModel
    {
        private ILogger<ErrorModel> logger; 

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            logger.LogInformation("Error page requested");
            return Page(); 
        }
    }
}
