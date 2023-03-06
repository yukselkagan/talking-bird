namespace TalkingBird.Contracts
{
    public class PostCreated
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int LikeCount { get; set; }
    }
}
