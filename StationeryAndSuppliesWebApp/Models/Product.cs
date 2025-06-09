namespace StationeryAndSuppliesWebApp.Models; 

public class Product
{
    public string Name { get; set; } = string.Empty;

    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }

    public Product() { }

    public Product(string name, decimal? price, string? imageUrl)
    {
        Name = name;
        Price = price;
        ImageUrl = imageUrl;
    }
}
