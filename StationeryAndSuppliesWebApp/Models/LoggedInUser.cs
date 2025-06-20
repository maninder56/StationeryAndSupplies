namespace StationeryAndSuppliesWebApp.Models; 

public class LoggedInUser
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;   

    public LoggedInUser() { }   

    public LoggedInUser(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }
}
