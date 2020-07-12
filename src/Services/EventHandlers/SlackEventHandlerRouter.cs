using System.Threading;
using System.Threading.Tasks;
using Models.Api;

namespace Services.EventHandlers
{
    public class SlackEventHandler : ISlackEventHandler
    {
        private readonly UserJoinedEventHandler _userJoinedEventHandler;
        private readonly ChannelMessageEventHandler _channelMessageEventHandler;
        private readonly ThreadMessageEventHandler _threadMessageEventHandler;
        private readonly AppMentionEventHandler _appMentionEventHandler;
        private readonly ReactionAddedEventHandler _reactionAddedEventHandler;

        public SlackEventHandler(
            UserJoinedEventHandler userJoinedEventHandler,
            ChannelMessageEventHandler channelMessageEventHandler,
            ThreadMessageEventHandler threadMessageEventHandler,
            AppMentionEventHandler appMentionEventHandler,
            ReactionAddedEventHandler reactionAddedEventHandler)
        {
            _userJoinedEventHandler = userJoinedEventHandler;
            _channelMessageEventHandler = channelMessageEventHandler;
            _threadMessageEventHandler = threadMessageEventHandler;
            _appMentionEventHandler = appMentionEventHandler;
            _reactionAddedEventHandler = reactionAddedEventHandler;
        }

        public Task HandleSlackEventAsync(
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken)
        {
            return slackEventBody switch
            {
                { Event: {Type: "message", Subtype: "channel_join"} } 
                    => _userJoinedEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                { Event: {Type: "message", ParentUserId: null, ThreadParentMessageIdentifier: null} } 
                    => _channelMessageEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                { Event: {Type: "message"} } 
                    when slackEventBody.Event.ThreadParentMessageIdentifier.HasValue 
                         && !string.IsNullOrEmpty(slackEventBody.Event.ParentUserId) 
                    => _threadMessageEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                { Event: {Type: "app_mention"} } 
                    => _appMentionEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                { Event: {Type: "reaction_added"} } 
                    => _reactionAddedEventHandler.HandleSlackEventAsync(slackEventBody, cancellationToken),
                _ => UnknownEvent(slackEventBody, cancellationToken)
            };
        }

        private Task UnknownEvent(SlackEventBody eventBody, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}