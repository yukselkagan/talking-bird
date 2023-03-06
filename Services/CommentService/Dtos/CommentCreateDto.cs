namespace CommentService.Dtos
{
    public class CommentCreateDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}
