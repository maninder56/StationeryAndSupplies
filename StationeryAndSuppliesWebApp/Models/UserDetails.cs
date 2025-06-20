namespace StationeryAndSuppliesWebApp.Models; 

public class UserDetails
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserDetails() { }    

    public UserDetails(string username, string email)
    {
        UserName = username; Email = email;
    }
}
