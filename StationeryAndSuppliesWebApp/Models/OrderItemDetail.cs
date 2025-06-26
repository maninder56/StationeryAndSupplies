namespace StationeryAndSuppliesWebApp.Models; 

public class OrderItemDetail
{
    public string ProductName { get; set; } = null!; 

    public decimal? UnitPrice { get; set; }

    public int Quantity { get; set; }
}
