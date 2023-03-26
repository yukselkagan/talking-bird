using MassTransit;
using TalkingBirdContracts;
using TrendService.Data.Repository;
using TrendService.Models;

namespace TrendService.Consumers
{
    public class PostCreatedConsumer : IConsumer<PostCreated>
    {
        private readonly ITrendRepository _trendRepository;
        public PostCreatedConsumer(ITrendRepository trendRepository)
        {
            _trendRepository = trendRepository;
        }

        public async Task Consume(ConsumeContext<PostCreated> context)
        {
            var message = context.Message;

            await ProcessTrend(message.Content);
            Console.WriteLine("Content read");
        }

        private async Task ProcessTrend(string postContent)
        {
            var contentParts = postContent.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tags = new List<string>();

            foreach(var contentPart in contentParts)
            {
                if (contentPart.StartsWith('#'))
                {
                    var tag = contentPart.Substring(1);
                    tags.Add(tag);

                    var existingTrend = (await _trendRepository.Get(filter: x => x.Content == tag)).FirstOrDefault();
                    if(existingTrend == null)
                    {
                        var newTrend = new Trend()
                        {
                            Content = tag,
                            PostCount = 1,
                            UpdatedAt = DateTime.Now
                        };

                        await _trendRepository.Insert(newTrend);
                        await _trendRepository.Save();
                    }
                    else
                    {
                        existingTrend.PostCount += 1;
                        existingTrend.UpdatedAt = DateTime.Now;

                        await _trendRepository.Update(existingTrend);
                        await _trendRepository.Save();
                    }
                }
            }
            
        }





    }
}
