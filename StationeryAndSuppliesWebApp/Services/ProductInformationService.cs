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


    public async Task<List<ChildCategory>> GetAllChildCategoriesAsync()
    {
        logger.LogInformation("Requested all child categories from service");

        List<ChildCategory>? list = await database.Categories.AsNoTracking()
            .Where(c => c.ParentId != null)
            .OrderBy(c => c.CategoryId)
            .Select(c => new ChildCategory(c.CategoryId, c.Name, c.ImageUrl))
            .ToListAsync();

        if (list is null || list.Count == 0)
        {
            logger.LogError("No Category exists in category table");
        }

        return list ?? new List<ChildCategory>();
    }


    // Products 

    public async Task<List<Models.Product>> GetEightProducts()
    {
        logger.LogInformation("Requested to get fist eight products"); 

        List<Models.Product>? list = await database.Products.AsNoTracking()
            .OrderBy(p => p.ProductId)
            .Take(8)
            .Select(c => new Models.Product(c.CategoryId, c.Name, c.Price, c.ImageUrl))
            .ToListAsync();

        if (list is null || list.Count == 0)
        {
            logger.LogError("No product exists in products table");
        }

        return list ?? new List<Models.Product>();

    }


    public async Task<List<Models.ProductDetails>> GetProductListByCategoryName(
        string categoryName, OrderByOptions orderBy, int pageNumber)
    {
        if (pageNumber < 1)
        {
            logger.LogWarning("Requested Page number {PageNumber} which is less than 1", pageNumber);
            
            pageNumber = 1;
            logger.LogInformation("New page number is set to 1"); 
        }

        int pageSize = 5; 

        logger.LogInformation("Requested to get product list by category name {CategoryName}, orderBy {OrderBy}, and page number {PageNumber}",
            categoryName, orderBy.ToString(), pageNumber);

        List<Models.ProductDetails>? list = await database.Products.AsNoTracking()
            .Where(p => p.Category.Name == categoryName && p.Status == "active")
            .OrderProductBy(orderBy)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(p =>
                new ProductDetails
                (
                    p.ProductId,
                    p.Name,
                    p.Descripttion,
                    p.Price,
                    p.Stock > 0, // Compute if stock is available 
                    p.ImageUrl
                )
             ).ToListAsync();    

        if (list == null || list.Count == 0)
        {
            logger.LogWarning("No Product exists for Category {CategoryName}", categoryName); 
        }

        return list ?? new List<ProductDetails>();
    }

    public async Task<List<ProductDetails>> GetProductListByParentCategoryName(string parentCategoryName, OrderByOptions orderBy, int pageNumber)
    {
        if (pageNumber < 1)
        {
            logger.LogWarning("Requested Page number {PageNumber} which is less than 1", pageNumber);

            pageNumber = 1;
            logger.LogInformation("New page number is set to 1");
        }

        int pageSize = 5;

        logger.LogInformation("Requested to get product list by parent category name {ParentCategoryName}, orderBy {OrderBy}, and page number {PageNumber}",
            parentCategoryName, orderBy.ToString(), pageNumber);

        List<Models.ProductDetails>? list = await database.Categories.AsNoTracking()
            .Join(database.Categories.AsNoTracking(), c1 => c1.ParentId, c2 => c2.CategoryId,
            (c1, c2) => new { parentCategory = c2, childCategory = c1 })
            .Where(c => c.parentCategory.Name == parentCategoryName)
            .Join(database.Products.AsNoTracking(), c => c.childCategory.CategoryId, p => p.CategoryId,
            (c, p) => p)
            .Where(p => p.Status == "active")
            .OrderProductBy(orderBy)
            .Skip(pageSize * (pageNumber -1))
            .Take(pageSize)
            .Select(p => 
                new ProductDetails
                (
                    p.ProductId, 
                    p.Name, 
                    p.Descripttion, 
                    p.Price, 
                    p.Stock > 0,  // Compute if stock is available 
                    p.ImageUrl
                )
            ).ToListAsync();


        if (list == null || list.Count == 0)
        {
            logger.LogWarning("No Product exists for Parent Category {ParentCategoryName}", parentCategoryName);
        }

        return list ?? new List<ProductDetails>();
    }
}
