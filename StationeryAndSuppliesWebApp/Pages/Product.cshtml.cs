using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages;

public class ProductModel : PageModel
{
    private ILogger<ProductModel> logger;

    private IProductInformationService productInformationService; 

    public ProductModel(ILogger<ProductModel> logger, 
        IProductInformationService productInformationService)
    {
        this.logger = logger;
        this.productInformationService = productInformationService;
    }

    // Properties shared with view 
    public Models.ProductDetails? Product { get; private set; }
    public Models.UserReviewsList? UserReviewsList { get; private set; }

    // Quantity drop down options 
    public IEnumerable<SelectListItem> QuantityOptions { get; private set; } = Enumerable.Range(1, 10)
        .Select(number => new SelectListItem() { Value = number.ToString(), Text = number.ToString() }); 


    public async Task<IActionResult> OnGetAsync([FromRoute] int productid)
    {
        logger.LogInformation("Requested to get Product page with product ID {ProductID}", productid);

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Unvalid Model State, redirecting from ProductModel to home page"); 
            return RedirectToPage("/Index"); 
        }

        Product = await productInformationService.GetProductDetailsByIDAsync(productid); 

        if(Product is null)
        {
            logger.LogWarning("No detailes recieved from service of product with ID {ProductID}", productid); 
        }

        // get 10 most recent reviews of given product
        UserReviewsList = await productInformationService.GetRecentUserReviewsListByProductID(productid, 10); 

        if (UserReviewsList is null)
        {
            logger.LogWarning("No reviews recieved from service of product with ID {ProductID}", productid); 
        }

        return Page();
    }

    
}
