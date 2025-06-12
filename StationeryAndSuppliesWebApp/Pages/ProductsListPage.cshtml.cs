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
        public string CategoryName
        {
            get { return _categoryName; }
            private set { _categoryName = value.Replace('-', ' '); }
        }

        public List<Models.Product> ProductList { get; private set; } = new List<Models.Product>();
        public int CurrentPageNumber { get; private set; }
        public OrderByOptions CurrentOrderByOptions { get; private set; }
        public string ParentCategoryName { get; private set; } = string.Empty;
        public string ChildCategoryName { get; private set; } = string.Empty;
        public int MaxPageSize { get; private set; } = 1; 

        // Page handler
        public async Task<IActionResult> OnGetAsync(
            [FromRoute] string parentCategory, [FromRoute] string? childCategory, 
            [FromQuery] OrderByOptions? orderBy, [FromQuery] int? pageNumber)
        {

            logger.LogInformation("Requested Products list page with parent category {ParentCategoryName}", 
                parentCategory); 

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Unvalid Model State, redirecting to hope page"); 
                return RedirectToPage("/Index"); 
            }

            CurrentPageNumber = CheckPageNumber(pageNumber);
            CurrentOrderByOptions = CheckOrderByOptions(orderBy);

            ParentCategoryName = parentCategory;

            if (childCategory is not null)
            {
                CategoryName = childCategory;
                ChildCategoryName = childCategory; 
                MaxPageSize = await productInformationService.GetMaximumPageSizeAvailableByCategory(childCategory);

                ProductList = await productInformationService
                    .GetProductListByCategoryName(childCategory, CurrentOrderByOptions, CurrentPageNumber);
            }
            else
            {
                CategoryName = parentCategory;
                MaxPageSize = await productInformationService.GetMaximumPageSizeAvailableByParentCategory(parentCategory);

                ProductList = await productInformationService
                    .GetProductListByParentCategoryName(parentCategory, CurrentOrderByOptions, CurrentPageNumber);
            }

            if (ProductList.Count == 0)
            {
                logger.LogWarning("No Product exits in category {CategoryName} with page number {PageNumber}",
                    CategoryName, CurrentPageNumber);
            }

            logger.LogInformation("Loaded {ProductCount} with category {CategoryName}", ProductList.Count,
                CategoryName);

            return Page();
        }


        
        // Helper methods 

        private int CheckPageNumber(int? pageNumber)
        {
            // if page number is null or less than 1, set it to 1 
            if (pageNumber is null || pageNumber < 1)
            {
                logger.LogInformation("Requested Page number {PageNumber} which is less than 1 or is null", pageNumber);

                pageNumber = 1;
                logger.LogInformation("New page number is set to 1");
            }

            return (int)pageNumber; 
        }

        private OrderByOptions CheckOrderByOptions(OrderByOptions? orderBy)
        {
            // if order by is null set it to default
            if (orderBy is null)
            {
                orderBy = OrderByOptions.Default;
                logger.LogInformation("Order by is set to Default due to being null");
            }

            return (OrderByOptions)orderBy; 
        }

    }
}
