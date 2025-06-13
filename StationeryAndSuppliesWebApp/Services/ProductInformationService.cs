using DataBaseContextLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using StationeryAndSuppliesWebApp.Models;
using System.Runtime.CompilerServices;

namespace StationeryAndSuppliesWebApp.Services;

public class ProductInformationService : IProductInformationService
{
    private StationeryAndSuppliesDatabaseContext database; 

    private readonly ILogger<ProductInformationService> logger; 

    private int pageSize = 10;

    public ProductInformationService(ILogger<ProductInformationService> logger, StationeryAndSuppliesDatabaseContext database)
    {
        this.logger = logger;
        this.database = database;
    }

    // Categories 
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        logger.LogInformation("Requested All categories from service");

        List<Category>? list = await database.Categories.AsNoTracking()
            .OrderBy(x => x.CategoryId)
            .ToListAsync(); 

        if (list is null || list.Count == 0)
        {
            logger.LogError("No Category exists in category table"); 
        }

        return list ?? new List<Category>(); 
    }

    public async Task<List<Models.ParentCategory>> GetParentCategoryAsync(int numberOfCategories)
    {
        logger.LogInformation("Requested to get {NumberOfCategories} parent categories from service", 
            numberOfCategories);

        if (numberOfCategories < 1)
        {
            logger.LogWarning("Requested to get {NumberOfCategories} which is less than 1, new number of categories is set to 1",
                numberOfCategories);
            numberOfCategories = 1;
        }

        List<Models.ParentCategory>? list = await database.Categories.AsNoTracking()
            .Where(c => c.ParentId == null)
            .OrderBy(c => c.CategoryId)
            .Take(numberOfCategories)
            .Select(c => new ParentCategory(c.CategoryId, c.Name))
            .ToListAsync();

        if (list is null || list.Count == 0)
        {
            logger.LogError("No Parent Category exists in category table");
        }

        return list ?? new List<Models.ParentCategory>();

    }


    public async Task<List<ChildCategory>> GetAllChildCategoriesAsync()
    {
        logger.LogInformation("Requested all child categories from service");

        List<ChildCategory>? list = await database.Categories.AsNoTracking()
            .Join(database.Categories.AsNoTracking(), c1 => c1.ParentId, c2 => c2.CategoryId,
            (c1, c2) => new { parentCategory = c2, childCategory = c1 })
            .OrderBy(c => c.childCategory.CategoryId)
            .Select(c => 
                new ChildCategory
                (
                    c.childCategory.CategoryId, 
                    c.childCategory.Name, 
                    c.parentCategory.Name, 
                    c.childCategory.ImageUrl
                )
            ).ToListAsync();

        if (list is null || list.Count == 0)
        {
            logger.LogError("No Category exists in category table");
        }

        return list ?? new List<ChildCategory>();
    }


    // Products 

    public async Task<List<Models.Product>> GetEightProductsAsync()
    {
        logger.LogInformation("Requested to get fist eight products"); 

        List<Models.Product>? list = await database.Products.AsNoTracking()
            .OrderBy(p => p.ProductId)
            .Take(8)
            .Select(c => new Models.Product
                (c.CategoryId, c.Name, c.Price, c.ImageUrl, c.Stock > 0))
            .ToListAsync();

        if (list is null || list.Count == 0)
        {
            logger.LogError("No product exists in products table");
        }

        return list ?? new List<Models.Product>();

    }


    public async Task<List<Models.Product>> GetProductListByCategoryNameAsync(
        string categoryName, OrderByOptions orderBy, int pageNumber)
    {

        logger.LogInformation("Requested to get product list by category name {CategoryName}, orderBy {OrderBy}, and page number {PageNumber}",
            categoryName, orderBy.ToString(), pageNumber);

        List<Models.Product>? list = await database.Products.AsNoTracking()
            .Where(p => p.Category.Name == categoryName && p.Status != "archived")
            .OrderProductBy(orderBy)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(p =>
                new Models.Product
                (
                    p.ProductId,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.Stock > 0  // Compute if stock is available 
                )
             ).ToListAsync();    

        if (list == null || list.Count == 0)
        {
            logger.LogWarning("No Product exists for Category {CategoryName}", categoryName); 
        }

        return list ?? new List<Models.Product>();
    }




    public async Task<List<Models.Product>> GetProductListByParentCategoryNameAsync(
        string parentCategoryName, OrderByOptions orderBy, int pageNumber)
    {

        logger.LogInformation("Requested to get product list by parent category name {ParentCategoryName}, orderBy {OrderBy}, and page number {PageNumber}",
            parentCategoryName, orderBy.ToString(), pageNumber);

        List<Models.Product>? list = await database.Categories.AsNoTracking()
            .Join(database.Categories.AsNoTracking(), c1 => c1.ParentId, c2 => c2.CategoryId,
            (c1, c2) => new { parentCategory = c2, childCategory = c1 })
            .Where(c => c.parentCategory.Name == parentCategoryName)
            .Join(database.Products.AsNoTracking(), c => c.childCategory.CategoryId, p => p.CategoryId,
            (c, p) => p)
            .Where(p => p.Status != "archived")
            .OrderProductBy(orderBy)
            .Skip(pageSize * (pageNumber -1))
            .Take(pageSize)
            .Select(p => 
                new Models.Product
                (
                    p.ProductId, 
                    p.Name, 
                    p.Price,
                    p.ImageUrl,
                    p.Stock > 0  // Compute if stock is available 
                )
            ).ToListAsync();


        if (list == null || list.Count == 0)
        {
            logger.LogWarning("No Product exists for Parent Category {ParentCategoryName}", parentCategoryName);
        }

        return list ?? new List<Models.Product>();
    }




    public async Task<int> GetMaximumPageSizeAvailableByCategoryAsync(string categoryName)
    {
        logger.LogInformation("Requested to get maximum page size for category {CategoryName}", 
            categoryName);

        int totalProducts = await database.Products.AsNoTracking()
            .Where(p => p.Category.Name == categoryName && p.Status != "archived")
            .CountAsync(); 


        int maxPageSize = (totalProducts / pageSize) + 1;

        logger.LogInformation("{CategoryName} category has maximum page size of {MaxPageSize}", 
            categoryName, maxPageSize);

        return maxPageSize;
    }



    public async Task<int> GetMaximumPageSizeAvailableByParentCategoryAsync(string parentCategoryName)
    {
        logger.LogInformation("Requested to get maximum page size for Parent category {ParentCategoryName}",
            parentCategoryName);

        int totalProducts = await database.Categories.AsNoTracking()
            .Join(database.Categories.AsNoTracking(), c1 => c1.ParentId, c2 => c2.CategoryId,
            (c1, c2) => new { parentCategory = c2, childCategory = c1 })
            .Where(c => c.parentCategory.Name == parentCategoryName)
            .Join(database.Products.AsNoTracking(), c => c.childCategory.CategoryId, p => p.CategoryId,
            (c, p) => p)
            .Where(p => p.Status != "archived")
            .CountAsync(); 

        int maxPageSize = (totalProducts / pageSize) + 1;

        logger.LogInformation("{ParentCategoryName} category has maximum page size of {MaxPageSize}",
            parentCategoryName, maxPageSize);

        return maxPageSize;
    }



    public async Task<ProductDetails?> GetProductDetailsByIDAsync(int productId)
    {
       logger.LogInformation("Requested to get Product detials of product with ID {ProductID}", productId);

        ProductDetails? productDetails = await database.Products.AsNoTracking()
            .Where(p => p.ProductId == productId && p.Status != "archived")
            .Select(p => 
                new ProductDetails
                (
                    p.ProductId, 
                    p.Name, 
                    p.Descripttion, 
                    p.Price, 
                    p.Stock > 0, 
                    p.ImageUrl
                ))
            .FirstOrDefaultAsync();

        if (productDetails is null)
        {
            logger.LogWarning("No product with ID {ProductID} Exists", productId); 
        }

        return productDetails; 
    }
}
