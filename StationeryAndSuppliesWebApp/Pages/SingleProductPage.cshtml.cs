using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages
{
    public class SingleProductPageModel : PageModel
    {
        private ILogger<SingleProductPageModel> logger;

        private IProductInformationService productInformationService; 

        public SingleProductPageModel(ILogger<SingleProductPageModel> logger, 
            IProductInformationService productInformationService)
        {
            this.logger = logger;
            this.productInformationService = productInformationService;
        }

        // Properties shared with view 
        public Models.ProductDetails? Product { get; private set; }

        public async Task<IActionResult> OnGetAsync([FromRoute] int productid)
        {
            logger.LogInformation("Requested to get Product page with product ID {ProductID}", productid);

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Unvalid Model State, redirecting from SingleProductPageModel to home page"); 
                return RedirectToPage("/Index"); 
            }

            Product = await productInformationService.GetProductDetailsByIDAsync(productid); 

            if(Product is null)
            {
                logger.LogWarning("No Product recieved from service"); 
            }

            return Page();
        }
    }
}
