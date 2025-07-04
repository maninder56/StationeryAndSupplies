using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StationeryAndSuppliesWebApp.Services;
using System.ComponentModel.DataAnnotations;

namespace StationeryAndSuppliesWebApp.Pages;

public class ProductModel : PageModel
{
    private ILogger<ProductModel> logger;

    private IAccountService accountService;

    private IProductInformationService productInformationService;

    private IUserOrdersDetailsService userOrdersDetailsService; 

    public ProductModel(ILogger<ProductModel> logger, 
        IProductInformationService productInformationService,
        IAccountService accountService, 
        IUserOrdersDetailsService userOrdersDetailsService)
    {
        this.logger = logger;
        this.productInformationService = productInformationService;
        this.accountService = accountService;
        this.userOrdersDetailsService = userOrdersDetailsService;
    }

    [TempData]
    public string? UserMustLoginMessage { get; set; }

    [BindProperty]
    public InputModel Input {  get; set; } = new InputModel();

    // Properties shared with view 
    public Models.ProductDetails? Product { get; private set; }
    public Models.UserReviewsList? UserReviewsList { get; private set; }
    public bool? ProductAddedToCart { get; private set; }

    // Quantity drop down options 
    public IEnumerable<SelectListItem> QuantityOptions { get; private set; } = Enumerable.Range(1, 10)
        .Select(number => new SelectListItem() { Value = number.ToString(), Text = number.ToString() }); 


    public async Task<IActionResult> OnGetAsync(int productid)
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


    public async Task<IActionResult> OnPostAsync()
    {
        logger.LogInformation("Requested to add product in user's cart");

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Unvalid Model State, when attempted to add product in user's cart");
            return RedirectToPage("/Index"); 
        }

        // Redirect user is not logged in 

        bool userLoggedIn = HttpContext.User.Identity?.IsAuthenticated ?? false; 

        if (!userLoggedIn)
        {
            UserMustLoginMessage = "Please login to save products in bag"; 
            logger.LogWarning("Anonymous user tried to add product to bag, redirected to login page");
            return RedirectToPage("/Account/Login", new { returnUrl = $"/Product/{Input.ProductID}"}); 
        }

        int? userID = await accountService.GetUserIDFromHttpContextAsync();

        if (userID is null)
        {
            logger.LogWarning("Failed to get user id from HttpContext service");
            ProductAddedToCart = false;
            return await OnGetAsync(Input.ProductID);
        }

        ProductAddedToCart = await userOrdersDetailsService.AddProductByIDInCartByUserID(
            (int)userID, Input.ProductID, Input.Quantity);

        return await OnGetAsync(Input.ProductID);
    }




    public class InputModel
    {
        [Required]
        [Range(1,int.MaxValue)]
        public int ProductID {  get; set; }

        [Required]
        [Range(1,10)]
        public int Quantity { get; set; }
    }

    
}
