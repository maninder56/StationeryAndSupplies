using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages
{
    public class ProductsListPageModel : PageModel
    {
        private ILogger<ProductsListPageModel> logger;

        private IProductInformationService productInformationService; 

        public ProductsListPageModel(ILogger<ProductsListPageModel> logger, IProductInformationService productInformationService)
        {
            this.logger = logger;
            this.productInformationService = productInformationService;
        }


        // Properties shared with view 
        public List<ProductDetails> ProductList { get; private set; } = new List<ProductDetails>();



        public async Task<IActionResult> OnGetAsync(
            [FromRoute] string parentCategory, [FromRoute] string? childCategory, 
            [FromQuery] OrderByOptions? orderBy, [FromQuery] int? page)
        {

            logger.LogInformation("Requested Products list page with parent category {ParentCategoryName}", parentCategory); 

            // Add more logging

            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Index"); 
            }

            if (childCategory is not null)
            {
                ProductList = await productInformationService
                    .GetProductListByCategoryName(childCategory, orderBy, page); 
                return Page();
            }
            else
            {
                ProductList = await productInformationService
                    .GetProductListByParentCategoryName(parentCategory, orderBy, page);
                return Page();
            }
        }
    }
}
