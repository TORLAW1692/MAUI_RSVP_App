using SQLite;
using RSVPApp.Models;

namespace RSVPApp.DataAccess
{
    public class AppDatabase
    {
        private SQLiteAsyncConnection? database;

        private async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(
                DatabaseConstants.DatabasePath,
                DatabaseConstants.Flags);

            await database.CreateTableAsync<User>();
            await database.CreateTableAsync<Event>();
            await database.CreateTableAsync<RSVP>();
        }

        public async Task<int> AddUserAsync(User user)
        {
            await Init();
            return await database!.InsertAsync(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            await Init();

            return await database!
                .Table<User>()
                .Where(u => u.EmailAddress == email)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            await Init();

            return await database!
                .Table<User>()
                .Where(u => u.EmailAddress == email && u.Password == password)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddEventAsync(Event eventItem)
        {
            await Init();
            return await database!.InsertAsync(eventItem);
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            await Init();
            return await database!.Table<Event>().ToListAsync();
        }

        public async Task<List<Event>> GetHostedEventsAsync(int userID)
        {
            await Init();

            return await database!
                .Table<Event>()
                .Where(e => e.HostUserID == userID)
                .ToListAsync();
        }

        public async Task<int> AddRSVPAsync(RSVP rsvp)
        {
            await Init();
            return await database!.InsertAsync(rsvp);
        }

        public async Task<List<RSVP>> GetRSVPsForUserAsync(int userID)
        {
            await Init();

            return await database!
                .Table<RSVP>()
                .Where(r => r.UserID == userID)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userID)
        {
            await Init();

            return await database!
                .Table<User>()
                .Where(u => u.UserID == userID)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RSVP>> GetRSVPsForEventAsync(int eventID)
        {
            await Init();

            return await database!
                .Table<RSVP>()
                .Where(r => r.EventID == eventID)
                .ToListAsync();
        }

        public async Task<bool> HasUserRSVPedAsync(int userID, int eventID)
        {
            await Init();

            RSVP? existingRSVP = await database!
                .Table<RSVP>()
                .Where(r => r.UserID == userID && r.EventID == eventID)
                .FirstOrDefaultAsync();

            return existingRSVP is not null;
        }
    }
}
