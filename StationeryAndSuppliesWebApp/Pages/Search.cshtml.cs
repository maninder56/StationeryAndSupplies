using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StationeryAndSuppliesWebApp.Services;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

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
        public List<Models.Product> SearchResultList { get; private set; } = new List<Models.Product>();
        public string SearchedStringForView { get; private set; } = string.Empty;
        public bool SearchStringValidationResult { get; private set; } = false;
        public bool SearchStringIsNull {  get; private set; }



        // Handlers 
        public async Task<IActionResult> OnGetAsync(string? productName)
        {
            logger.LogInformation("Search result page requested with {ProductName} from query", productName);
            
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid model state, redirected to home page");
                return RedirectToPage("/Index"); 
            }

            if (productName is null)
            {
                logger.LogWarning("Search string is null");
                SearchStringValidationResult = false;
                SearchStringIsNull = true;
                return Page();
            }

            // Remove all whitespace characters from search string,
            // if its just whitespace characters string will be empty
            productName = productName.Trim(); 

            if (productName.Length > 20)
            {
                productName = productName.Substring(0, 20);
            }

            if(!await IsSearchStringValid(productName))
            {
                logger.LogWarning("Search string unvalid");
                SearchStringValidationResult = false; 
                return Page(); 
            }

            SearchStringValidationResult = true;
            SearchedStringForView = productName;

            SearchResultList = await productInformationService.SearchProductWithNameAsync(SearchedStringForView);

            if (SearchResultList.Count == 0)
            {
                logger.LogWarning("No Item recieved from serivce"); 
            }

            return Page();
        }


        // Helper method for validating search string 

        public Task<bool> IsSearchStringValid(string searchString)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    logger.LogWarning("Search string is Empty"); 
                    return false;
                }

                // Only allow alphanumeric characters
                if (Regex.IsMatch(searchString, @"\W"))
                {
                    logger.LogWarning("Search string containes Non-alphanumeric character"); 
                    return false;
                }

                return true; 
            }); 
        }
    }
}
