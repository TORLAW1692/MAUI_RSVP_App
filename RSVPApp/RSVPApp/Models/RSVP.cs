using SQLite;

namespace RSVPApp.Models
{
    public class RSVP
    {
        [PrimaryKey, AutoIncrement]
        public int RSVPId { get; set; }

        public int EventID { get; set; }

        public int UserID { get; set; }
    }
}