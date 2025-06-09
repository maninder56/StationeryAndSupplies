using DataBaseContextLibrary;
using StationeryAndSuppliesWebApp.Models; 

namespace StationeryAndSuppliesWebApp.Services; 

public interface IProductInformationService
{
    // Get Category Information
    public Task<List<Category>> GetAllCategoriesAsync(); 

    // Get only child categories 
    public Task<List<ChildCategory>> GetAllChildCategoriesAsync();

    // Get first eight products 
    public Task<List<Models.Product>> GetEightProducts(); 
}
