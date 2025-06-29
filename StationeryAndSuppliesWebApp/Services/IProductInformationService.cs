using DataBaseContextLibrary;
using StationeryAndSuppliesWebApp.Models; 

namespace StationeryAndSuppliesWebApp.Services; 

public interface IProductInformationService
{
    // Read Operations 


    // Category methods 

    // Get Category Information
    public Task<List<Category>> GetAllCategoriesAsync(); 

    // Get only child categories 
    public Task<List<ChildCategory>> GetAllChildCategoriesAsync();

    // Get only parent categories
    public Task<List<Models.ParentCategory>> GetParentCategoryAsync(int numberOfCategories); 



    // Product methods 

    // Get first eight products 
    public Task<List<Models.Product>> GetEightProductsAsync();

    // Get product list by category name 
    public Task<List<Models.Product>> GetProductListByCategoryNameAsync(
        string categoryName, OrderByOptions orderBy, int pageNumber);

    // Get product list by parent category 
    public Task<List<Models.Product>> GetProductListByParentCategoryNameAsync(
        string parentCategoryName, OrderByOptions orderBy, int pageNumber);


    // Get maximum page size available given category name
    public Task<int> GetMaximumPageSizeAvailableByCategoryAsync(string categoryName);

    // Get maximum page size available given category name
    public Task<int> GetMaximumPageSizeAvailableByParentCategoryAsync(string ParentcategoryName);


    // Get single product Details 
    public Task<Models.ProductDetails?> GetProductDetailsByIDAsync(int productId);

    // Serach for a product by name, get top 20 products
    public Task<List<Models.Product>> SearchProductWithNameAsync(string productName);
    

    // Product Reviews

    // Get recent reviews for the product by id
    public Task<UserReviewsList?> GetRecentUserReviewsListByProductID(int productId, int limit);

}
