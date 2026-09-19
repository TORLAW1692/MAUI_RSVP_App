using SQLite;

namespace RSVPApp.Models
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int EventID { get; set; }

        public int HostUserID { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string EventAddress { get; set; } = string.Empty;

        public int MaximumAllowedAttendees { get; set; }

        public DateTime EventDateTime { get; set; }

        public DateTime RSVPDeadline { get; set; }
    }
}