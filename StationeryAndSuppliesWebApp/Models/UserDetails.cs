namespace StationeryAndSuppliesWebApp.Models; 

public class UserDetails
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty; 

    public UserDetails() { }    

    public UserDetails(int id, string username, string email, string phone)
    {
        Id = id;  UserName = username; Email = email; Phone = phone;
    }
}
