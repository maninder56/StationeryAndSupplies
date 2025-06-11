using DataBaseContextLibrary;
using StationeryAndSuppliesWebApp.Models; 

namespace StationeryAndSuppliesWebApp.Services; 

public interface IProductInformationService
{
    // Category methods 

    // Get Category Information
    public Task<List<Category>> GetAllCategoriesAsync(); 

    // Get only child categories 
    public Task<List<ChildCategory>> GetAllChildCategoriesAsync();

    // Get only parent categories
    public Task<List<Models.ParentCategory>> GetParentCategoryAsync(int numberOfCategories); 

    // Product methods 

    // Get first eight products 
    public Task<List<Models.Product>> GetEightProducts();

    // Get product list by category name 
    public Task<List<Models.Product>> GetProductListByCategoryName(
        string categoryName, OrderByOptions? orderBy, int? pageNumber);

    // Get product list by parent category 
    public Task<List<Models.Product>> GetProductListByParentCategoryName(
        string parentCategoryName, OrderByOptions? orderBy, int? pageNumber);
}
