using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StationeryAndSuppliesWebApp.Pages
{
    public class AboutUsModel : PageModel
    {
        private ILogger<AboutUsModel> logger;

        public AboutUsModel(ILogger<AboutUsModel> logger)
        {
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            logger.LogInformation("Requested About us page");
            return Page();
        }
    }
}
