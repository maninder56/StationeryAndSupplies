using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;
using System.Collections.Generic;

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
    public List<ChildCategory> ChildCategoriesListInFirstSection { get; private set; } = new List<ChildCategory>();
    public List<ChildCategory> ChildCategoriesListInSecondSection { get; private set; } = new List<ChildCategory>();
    public List<Models.Product> ProductsInThirdSection { get; private set; } = new List<Product>(); 


    // Page Handlers
    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Index page requested");

        // Get category list 

        List<ChildCategory> categoryList = await productInformationService.GetAllChildCategoriesAsync();

        logger.LogInformation("Total {NumberOfChildCategories} child categories loaded in category list", categoryList.Count);

        if (categoryList.Count == 0)
        {
            logger.LogWarning("Failed to get any category from service"); 
        }

        if (categoryList.Count > 8)
        {
            ChildCategoriesListInFirstSection = categoryList.Slice(0, 4);
            ChildCategoriesListInSecondSection = categoryList.Slice(4, 4);

            logger.LogInformation("Loaded {CategoryNumber} categories in ChildCategoriesListInFirstSection list", ChildCategoriesListInFirstSection.Count);
            logger.LogInformation("Loaded {CategoryNumber} categories in ChildCategoriesListInSecondSection list", ChildCategoriesListInSecondSection.Count);
        }

        if (ChildCategoriesListInFirstSection.Count == 0)
        {
            logger.LogWarning("Failed to load categoreis in Firstsection list"); 
        }

        if (ChildCategoriesListInSecondSection.Count == 0)
        {
            logger.LogWarning("Failed to load categoreis in Secondsection list");
        }


        // Get product list

        List<Models.Product> productList = await productInformationService.GetEightProductsAsync();

        logger.LogInformation("Total {NumberOfProducts} products loaded in product list", productList.Count);

        if (productList.Count == 0)
        {
            logger.LogWarning("Failed to get any products from service");
        }

        ProductsInThirdSection = productList; 

        return Page();
    }
}
