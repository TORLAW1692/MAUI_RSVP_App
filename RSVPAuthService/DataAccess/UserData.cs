using SQLite;
using RSVPAuthService.Models;

namespace RSVPAuthService.DataAccess;

public class UserData
{
    private SQLiteConnection? database;

    private void Init()
    {
        if (database is not null)
            return;

        database = new SQLiteConnection(
            DatabaseConstants.DatabasePath,
            DatabaseConstants.Flags);

        database.CreateTable<User>();
    }

    public User? GetUser(string email)
    {
        Init();

        return database!
            .Table<User>()
            .FirstOrDefault(u => u.EmailAddress == email);
    }

    public bool ValidateUser(string email, string password)
    {
        Init();

        return database!
            .Table<User>()
            .Any(u =>
                u.EmailAddress == email &&
                u.Password == password);
    }

    public bool AddUser(User user)
    {
        Init();

        User? existing = GetUser(user.EmailAddress);

        if (existing is not null)
            return false;

        return database!.Insert(user) > 0;
    }
}