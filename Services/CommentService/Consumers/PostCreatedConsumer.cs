using CommentService.Data.Repository;
using CommentService.Models;
using MassTransit;
using TalkingBird.Contracts;

namespace CommentService.Consumers
{
    public class PostCreatedConsumer : IConsumer<PostCreated>
    {
        private readonly IPostRepository _postRepository;
        public PostCreatedConsumer(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task Consume(ConsumeContext<PostCreated> context)
        {
            var message = context.Message;

            var newPost = new Post
            {
                ExternalPostId = message.PostId,
                Content = message.Content,
                UserId = message.UserId,
                LikeCount = message.LikeCount
            };
            await _postRepository.Insert(newPost);
            await _postRepository.Save();

            Console.WriteLine("Received message from rabbitMQ");
        }
    }
}
