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
            // add logging 

            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Index"); 
            }

            Product = await productInformationService.GetProductDetailsByIDAsync(productid); 

            return Page();
        }
    }
}
