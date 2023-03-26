using MassTransit;
using PostService.Data.Repository;
using PostService.Model;
using TalkingBirdContracts;

namespace PostService.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreated>
    {
        private readonly IUserRepository _userRepository;
        public UserCreatedConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var message = context.Message;

            var newUser = new User()
            {
                UserId = message.UserId,
                UserName = message.UserName,
                DisplayName = message.DisplayName
            };

            await _userRepository.Insert(newUser);
            await _userRepository.Save();

            Console.WriteLine("Received message from rabbitMQ");
        }
    }
}
