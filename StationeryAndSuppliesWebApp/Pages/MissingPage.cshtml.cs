using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StationeryAndSuppliesWebApp.Pages
{
    public class MissingPageModel : PageModel
    {
        private ILogger<MissingPageModel> logger;   


        public MissingPageModel(ILogger<MissingPageModel> logger)
        {
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            int code = HttpContext.Response.StatusCode;

            logger.LogInformation("Missing Page requested, Status Code: {StatusCode}", code);
            return Page(); 
        }
    }
}
