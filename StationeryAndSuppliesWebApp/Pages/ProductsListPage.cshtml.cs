using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryAndSuppliesWebApp.Models;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;

namespace StationeryAndSuppliesWebApp.Pages
{
    public class ProductsListPageModel : PageModel
    {
        private ILogger<ProductsListPageModel> logger;

        private IProductInformationService productInformationService; 

        private string _categoryName = string.Empty; 

        public ProductsListPageModel(ILogger<ProductsListPageModel> logger, 
            IProductInformationService productInformationService)
        {
            this.logger = logger;
            this.productInformationService = productInformationService;
        }


        // Properties shared with view 
        public List<Models.Product> ProductList { get; private set; } = new List<Models.Product>();
        public string CategoryName
        {
            get { return _categoryName; }
            private set { _categoryName = value.Replace('-', ' '); }
        }


        public async Task<IActionResult> OnGetAsync(
            [FromRoute] string parentCategory, [FromRoute] string? childCategory, 
            [FromQuery] OrderByOptions? orderBy, [FromQuery] int? page)
        {

            logger.LogInformation("Requested Products list page with parent category {ParentCategoryName}", 
                parentCategory); 

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Unvalid Model State, redirecting to hope page"); 
                return RedirectToPage("/Index"); 
            }

            if (childCategory is not null)
            {
                CategoryName = childCategory; 

                ProductList = await productInformationService
                    .GetProductListByCategoryName(childCategory, orderBy, page); 

                if (ProductList.Count == 0)
                {
                    logger.LogWarning("No Product exits in category {CategoryName} with page number {PageNumber}", 
                        childCategory, page); 
                }

                logger.LogInformation("Loaded {ProductCount} with category {CategoryName}", ProductList.Count, 
                    childCategory);

                return Page();
            }
            else
            {
                CategoryName = parentCategory; 

                ProductList = await productInformationService
                    .GetProductListByParentCategoryName(parentCategory, orderBy, page);

                if (ProductList.Count == 0)
                {
                    logger.LogWarning("No Product exits in Parent category {ParentCategoryName} with page number {PageNumber}",
                        parentCategory, page);
                }

                logger.LogInformation("Loaded {ProductCount} with Parent category {ParentCategoryName}", ProductList.Count,
                    parentCategory);

                return Page();
            }
        }
    }
}
