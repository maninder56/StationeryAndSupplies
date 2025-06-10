namespace StationeryAndSuppliesWebApp.Models; 

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }

    public Product() { }

    public Product(int id, string name, decimal? price, string? imageUrl)
    {
        Id = id;
        Name = name;
        Price = price;
        ImageUrl = imageUrl;
    }
}
