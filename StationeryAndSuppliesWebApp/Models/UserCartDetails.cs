namespace StationeryAndSuppliesWebApp.Models; 

public class UserCartDetails
{
    public List<UserCartIItem> Items { get; set; } = new List<UserCartIItem>();

    public int TotalItems => Items.Count;

    public decimal Subtotal => Items.Sum(i => i.TotalPriceWithQunatity);

    public decimal ShippingCost;

    public decimal TotalCost => Subtotal + ShippingCost; 
    
}
