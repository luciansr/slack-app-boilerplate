using System.Linq;
using System.Threading.Tasks;

namespace Clients.Slack
{
    public class SlackCustomClient
    {
        private readonly SlackScopedClient _slackScopedClient;

        public SlackCustomClient(SlackScopedClient slackScopedClient)
        {
            _slackScopedClient = slackScopedClient;
        }

        public async Task PostOnChannelAsync(string channelId, string text)
        {
            var client = _slackScopedClient.GetClient();
            var channelList = await client.GetChannelListAsync();
            if (channelList.ok)
            {
                var channel = client.Channels.FirstOrDefault(x => x.id == channelId);

                if (channel != null)
                {
                    await client.PostMessageAsync(channel.id, text);
                }
            }
        }

        public async Task SendMessageToUserAsync(string userId, string text)
        {
            var client = _slackScopedClient.GetClient();
            var user = client.Users.FirstOrDefault(x => x.id == userId);

            if (user != null)
            {
                var directMessageChannel = client.DirectMessages.FirstOrDefault(x => x.user == user.id);

                if (directMessageChannel != null)
                {
                    await client.PostMessageAsync(directMessageChannel.id, text);
                }
            }
        }
    }
}