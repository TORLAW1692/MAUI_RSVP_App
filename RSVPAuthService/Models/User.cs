using SQLite;

namespace RSVPAuthService.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int UserID { get; set; }

    [Unique]
    public string EmailAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
