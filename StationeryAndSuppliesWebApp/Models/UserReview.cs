namespace StationeryAndSuppliesWebApp.Models; 

public class UserReview
{
    public sbyte Rating { get; set; }

    public string? Comment { get; set; }

    public string UserName { get; set; } = string.Empty;
}
