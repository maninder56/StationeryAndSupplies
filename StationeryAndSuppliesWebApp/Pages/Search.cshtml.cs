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



        // Handlers 

        public async Task<IActionResult> OnGetAsync([FromQuery] string? name)
        {
            logger.LogInformation("Search result page requested with {ProductName} from query", name);
            
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid model state, redirected to home page");
                return RedirectToPage("/Index"); 
            }

            if (name is null)
            {
                logger.LogWarning("Search string from query is null");
                SearchStringValidationResult = false;
                return Page();
            }

            // Remove all whitespace characters from search string,
            // if its just whitespace characters string will be empty
            name = name.Trim(); 

            if (name.Length > 20)
            {
                name = name.Substring(0, 20);
            }

            if(!await IsSearchStringValid(name))
            {
                logger.LogWarning("Search string unvalid");
                SearchStringValidationResult = false; 
                return Page(); 
            }

            SearchStringValidationResult = true;
            SearchedStringForView = name;

            SearchResultList = await productInformationService.SearchProductWithNameAsync(SearchedStringForView);

            if (SearchResultList.Count == 0)
            {
                logger.LogWarning("No Item recieved from serivce"); 
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync([FromForm] string? SearchString)
        {
            logger.LogInformation("Search result page requested by Search form"); 

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid model state, redirected to home page");
                return RedirectToPage("/Index");
            }

            if (SearchString is null)
            {
                logger.LogWarning("Search string from form is null");
                SearchStringValidationResult = false;
                return Page(); 
            }

            return await OnGetAsync(SearchString);
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
