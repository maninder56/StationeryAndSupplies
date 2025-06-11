namespace StationeryAndSuppliesWebApp.Models; 

public class ChildCategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string ParentCategoryName { get; set; } = string.Empty; 

    public string? ImageUrl { get; set; }

    public ChildCategory() { }

    public ChildCategory(int id, string categoryName, string parentCategoryName , string? imageUrl)
    {
        Id = id;
        CategoryName = categoryName;
        ParentCategoryName = parentCategoryName;
        ImageUrl = imageUrl;
    }
}
