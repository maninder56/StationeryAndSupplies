namespace StationeryAndSuppliesWebApp.Models; 

public class UserOrderDetails
{
    public List<OrderItemDetail> OrderItems { get; set; } = new List<OrderItemDetail>();

    public DateTime OrderDate { get; set; }

    public string? Status { get; set; }

    public decimal? TotalAmount { get; set; }
}
