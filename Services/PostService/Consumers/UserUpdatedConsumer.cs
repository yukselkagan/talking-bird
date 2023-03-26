using MassTransit;
using PostService.Data.Repository;
using TalkingBirdContracts;

namespace PostService.Consumers
{
    public class UserUpdatedConsumer : IConsumer<UserUpdated>
    {
        private readonly IUserRepository _userRepository;
        public UserUpdatedConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<UserUpdated> context)
        {
            var message = context.Message;

            var targetUser = await _userRepository.GetById(message.UserId);
            targetUser.DisplayName = message.DisplayName;
            targetUser.UserName = message.UserName;

            await _userRepository.Update(targetUser);
            await _userRepository.Save();

            Console.WriteLine("Received message user updated");
        }
    }
}
