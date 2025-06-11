namespace StationeryAndSuppliesWebApp.Models; 

public class ParentCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ParentCategory () { }

    public ParentCategory(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
