using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Handlers
{
    public interface ISlackEventHandlerRouter
    {
        Task RouteSlackEventAsync(
            SlackEventType eventType,
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken);
    }

    public class SlackEventHandlerRouter : ISlackEventHandlerRouter
    {
        private readonly ChannelJoinEventHandler _channelJoinEventHandler;
        private readonly ChannelMessageEventHandler _channelMessageEventHandler;
        private readonly ThreadMessageEventHandler _threadMessageEventHandler;
        private readonly AppMentionEventHandler _appMentionEventHandler;
        private readonly ReactionAddedEventHandler _reactionAddedEventHandler;

        public SlackEventHandlerRouter(
            ChannelJoinEventHandler channelJoinEventHandler,
            ChannelMessageEventHandler channelMessageEventHandler,
            ThreadMessageEventHandler threadMessageEventHandler,
            AppMentionEventHandler appMentionEventHandler,
            ReactionAddedEventHandler reactionAddedEventHandler)
        {
            _channelJoinEventHandler = channelJoinEventHandler;
            _channelMessageEventHandler = channelMessageEventHandler;
            _threadMessageEventHandler = threadMessageEventHandler;
            _appMentionEventHandler = appMentionEventHandler;
            _reactionAddedEventHandler = reactionAddedEventHandler;
        }

        public Task RouteSlackEventAsync(
            SlackEventType eventType,
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken)
        {
            return eventType switch
            {
                SlackEventType.ChannelJoin => _channelJoinEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                SlackEventType.Message => _channelMessageEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                SlackEventType.ThreadMessage => _threadMessageEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                SlackEventType.AppMention => _appMentionEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                SlackEventType.ReactionAdded => _reactionAddedEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                _ => UnknownEvent(slackEventBody, cancellationToken)
            };
        }

        private Task UnknownEvent(SlackEventBody eventBody, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}