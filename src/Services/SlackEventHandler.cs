using System.Threading;
using System.Threading.Tasks;
using Models;
using Models.Api;

namespace Services
{
    public class SlackEventHandler
    {
        public async Task HandleSlackEventAsync(
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken)
        {
            
        }
    }
}