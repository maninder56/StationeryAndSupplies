namespace StationeryAndSuppliesWebApp.Models; 

public class ProductDetails
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public bool InStock { get; set; }

    public string? ImageUrl { get; set; }

    public ProductDetails() { }

    public ProductDetails(int id, string name, string? description, decimal? price, bool inStock, string? imageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        InStock = inStock;
        ImageUrl = imageUrl;
    }
}
