using MassTransit;
using TalkingBirdContracts;

namespace TrendService.Consumers
{
    public class PostCreatedConsumer : IConsumer<PostCreated>
    {
        public async Task Consume(ConsumeContext<PostCreated> context)
        {
            var message = context.Message;

            //read hashtag add to trend
        }
    }
}
