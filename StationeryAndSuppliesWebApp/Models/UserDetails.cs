namespace StationeryAndSuppliesWebApp.Models; 

public class UserDetails
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty; 

    public UserDetails() { }    

    public UserDetails(string username, string email, string phone)
    {
        UserName = username; Email = email; Phone = phone;
    }
}
