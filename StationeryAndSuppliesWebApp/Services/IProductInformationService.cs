using DataBaseContextLibrary;

namespace StationeryAndSuppliesWebApp.Services; 

public interface IProductInformationService
{
    // Get Category Information
    public Task<List<Category>> GetAllCategoriesAsync(); 
}
