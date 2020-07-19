using System.Threading;
using System.Threading.Tasks;
using Models.Api;
using Models.Events;

namespace Services.Events.Handlers
{
    public interface ISlackEventIdentifier
    {
        Task IdentifySlackEventAsync(
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken);
    }

    public class SlackEventIdentifier : ISlackEventIdentifier
    {
        private readonly ISlackEventHandler _slackEventHandler;

        public SlackEventIdentifier(ISlackEventHandler slackEventHandler)
        {
            _slackEventHandler = slackEventHandler;
        }

        public Task IdentifySlackEventAsync(
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken)
        {
            return slackEventBody switch
            {
                { Event: {Type: "message", Subtype: "channel_join"} }
                    => _slackEventHandler.HandleSlackEventAsync(SlackEventType.ChannelJoin, slackEventBody, cancellationToken),
                { Event: {Type: "message", ParentUserId: null, ThreadParentMessageIdentifier: null} }
                    => _slackEventHandler.HandleSlackEventAsync(SlackEventType.Message, slackEventBody, cancellationToken),
                { Event: {Type: "message"} }
                when slackEventBody.Event.ThreadParentMessageIdentifier.HasValue
                     && !string.IsNullOrEmpty(slackEventBody.Event.ParentUserId)
                    => _slackEventHandler.HandleSlackEventAsync(SlackEventType.ThreadMessage, slackEventBody, cancellationToken),
                { Event: {Type: "app_mention"} }
                    => _slackEventHandler.HandleSlackEventAsync(SlackEventType.AppMention, slackEventBody, cancellationToken),
                { Event: {Type: "reaction_added"} }
                    => _slackEventHandler.HandleSlackEventAsync(SlackEventType.ReactionAdded, slackEventBody, cancellationToken),
                _ => _slackEventHandler.HandleSlackEventAsync(SlackEventType.Unknown, slackEventBody, cancellationToken),
            };
        }
    }
}