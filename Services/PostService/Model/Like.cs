namespace PostService.Model
{
    public class Like
    {
        public Like()
        {
            CreatedAt = DateTime.Now;
        }

        public int LikeId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
