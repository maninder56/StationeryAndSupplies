namespace StationeryAndSuppliesWebApp.Models; 

public class OrderItemDetail
{
    public string ProductName { get; set; } = null!; 

    public decimal? UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal? TotalPriceByMultipyingQuntity 
    { 
        get { return (UnitPrice ?? 0) * Quantity; }
    }
}
