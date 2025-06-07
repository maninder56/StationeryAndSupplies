using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services; 

namespace StationeryAndSuppliesWebApp.Pages; 

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;

    private IProductInformationService productInformationService;

    public IndexModel(ILogger<IndexModel> logger, IProductInformationService productInformationService)
    {
        this.logger = logger;
        this.productInformationService = productInformationService;
    }

    // Properties shared with view 
    public List<ChildCategory> ChildCategoriesList { get; private set; } = new List<ChildCategory>();


    public async Task<IActionResult> OnGet()
    {
        logger.LogInformation("Index page requested");

        ChildCategoriesList = await productInformationService.GetAllChildCategoriesAsync();

        if (ChildCategoriesList.Count == 0)
        {
            logger.LogWarning("Failed to get any category from service"); 
        }

        logger.LogInformation("Loaded {NumberOfChildCategories} child categories", ChildCategoriesList.Count);
        return Page();
    }
}
