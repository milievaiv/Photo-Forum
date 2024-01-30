namespace PhotoForum.Models
{
    public class Log
    {
        public int Id { get; set; } // Assuming you have an identifier for each log entry
        public string Event { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
