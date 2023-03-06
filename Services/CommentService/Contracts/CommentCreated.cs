namespace TalkingBird.Contracts
{
    public class CommentCreated
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
    }
}
