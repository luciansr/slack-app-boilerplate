using System.Threading;
using System.Threading.Tasks;
using SlackAPI;

namespace Clients.Slack
{
    public class SlackScopedClient
    {
        private readonly SlackConnectedClient _slackConnectedClient;
        private SlackTaskClient _client;

        public SlackScopedClient(SlackConnectedClient slackConnectedClient)
        {
            _slackConnectedClient = slackConnectedClient;
        }

        public async Task SetTeam(string team, CancellationToken cancellationToken)
        {
            _client = await _slackConnectedClient.GetClient(team, cancellationToken);
        }

        public SlackTaskClient GetClient()
        {
            return _client;
        }
    }
}