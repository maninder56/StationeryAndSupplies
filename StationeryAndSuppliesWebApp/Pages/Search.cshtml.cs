using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;

namespace StationeryAndSuppliesWebApp.Pages
{
    public class SearchModel : PageModel
    {
        private ILogger<SearchModel> logger;    

        private IProductInformationService productInformationService;

        public SearchModel(ILogger<SearchModel> logger, IProductInformationService productInformationService)
        {
            this.logger = logger;
            this.productInformationService = productInformationService;
        }


        // Properties shared with view 

        public List<Models.Product> SearchResult { get; private set; } = new List<Models.Product>();

        public async Task<IActionResult> OnGetAsync(string name)
        {
            SearchResult = await productInformationService.SearchProductWithNameAsync(name);

            return Page();
        }
    }
}
