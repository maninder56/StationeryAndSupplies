namespace StationeryAndSuppliesWebApp.Models; 

public class UserCartIItem
{
    public int cartItemID { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal? Price { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPriceWithQunatity =>
        (Price ?? 0) * Quantity;

    public string? ImageUrl { get; set; }
}
