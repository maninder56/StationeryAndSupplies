using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StationeryAndSuppliesWebApp.Pages;

public class ContactUsModel : PageModel
{
    private ILogger<ContactUsModel> logger;

    public ContactUsModel(ILogger<ContactUsModel> logger)
    {
        this.logger = logger;
    }

    public IActionResult OnGet()
    {
        logger.LogInformation("Requested to get Contact us page"); 
        return Page();
    }
}
