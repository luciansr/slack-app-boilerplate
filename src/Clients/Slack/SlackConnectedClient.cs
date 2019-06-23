using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Config;
using SlackAPI;

namespace Clients.Slack
{
    public class SlackConnectedClient
    {
        private readonly SlackConfig _slackConfig;
        
        private readonly ConcurrentDictionary<string, SlackTaskClient> _clientMap = new ConcurrentDictionary<string, SlackTaskClient>();

        public SlackConnectedClient(SlackConfig slackConfig)
        {
            _slackConfig = slackConfig;
        }

        public async Task<SlackTaskClient> GetClient(string team, CancellationToken cancellationToken)
        {

            if (!_clientMap.ContainsKey(team))
            {
                _clientMap.TryAdd(team, await ConnectClient(team, cancellationToken));
            }

            return _clientMap[team];
        }

        private async Task<SlackTaskClient> ConnectClient(string team, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var token = _slackConfig.TokenMap[team];
            var client = new SlackTaskClient(token);
            var loginResponse = await client.ConnectAsync();
            return loginResponse.ok ? client : null;
        }
    }
}