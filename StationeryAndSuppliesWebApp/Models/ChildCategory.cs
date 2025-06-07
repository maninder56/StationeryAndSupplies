namespace StationeryAndSuppliesWebApp.Models; 

public class ChildCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public ChildCategory() { }

    public ChildCategory(int id, string name, string? imageUrl)
    {
        Id = id;
        Name = name;
        ImageUrl = imageUrl;
    }
}
