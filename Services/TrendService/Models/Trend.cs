namespace TrendService.Models
{
    public class Trend
    {
        public Trend()
        {
            CreatedAt = DateTime.Now;
        }

        public int TrendId { get; set; }
        public string Content { get; set; } = "";
        public int PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
