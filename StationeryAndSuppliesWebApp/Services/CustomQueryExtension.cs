using StationeryAndSuppliesWebApp.Models;
using Context = DataBaseContextLibrary; 

namespace StationeryAndSuppliesWebApp.Services; 

public static class CustomQueryExtension
{
    public static IQueryable<Context.Product> OrderProductBy(
        this IQueryable<Context.Product> query, OrderByOptions orderBy)
    {
        switch (orderBy)
        {
            case OrderByOptions.PriceLowToHigh: 
                return query.OrderBy(p => p.Price); 

            case OrderByOptions.PriceHighToLow:
                return query.OrderByDescending(p => p.Price);

            case OrderByOptions.NameAToZ: 
                return query.OrderBy(p => p.Name);

            case OrderByOptions.NameZToA: 
                return query.OrderByDescending(p => p.Name);

            case OrderByOptions.Default:
            default: 
                return query.OrderBy(p => p.ProductId);
        }
    }
}
