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
        private readonly ISlackEventHandlerRouter _slackEventHandlerRouter;

        public SlackEventIdentifier(ISlackEventHandlerRouter slackEventHandlerRouter)
        {
            _slackEventHandlerRouter = slackEventHandlerRouter;
        }

        public Task IdentifySlackEventAsync(
            SlackEventBody slackEventBody,
            CancellationToken cancellationToken)
        {
            return slackEventBody switch
            {
                { Event: {Type: "message", Subtype: "channel_join"} }
                    => _slackEventHandlerRouter.RouteSlackEventAsync(SlackEventType.ChannelJoin, slackEventBody, cancellationToken),
                { Event: {Type: "message", ParentUserId: null, ThreadParentMessageIdentifier: null} }
                    => _slackEventHandlerRouter.RouteSlackEventAsync(SlackEventType.Message, slackEventBody, cancellationToken),
                { Event: {Type: "message"} }
                when slackEventBody.Event.ThreadParentMessageIdentifier.HasValue
                     && !string.IsNullOrEmpty(slackEventBody.Event.ParentUserId)
                    => _slackEventHandlerRouter.RouteSlackEventAsync(SlackEventType.ThreadMessage, slackEventBody, cancellationToken),
                { Event: {Type: "app_mention"} }
                    => _slackEventHandlerRouter.RouteSlackEventAsync(SlackEventType.AppMention, slackEventBody, cancellationToken),
                { Event: {Type: "reaction_added"} }
                    => _slackEventHandlerRouter.RouteSlackEventAsync(SlackEventType.ReactionAdded, slackEventBody, cancellationToken),
                _ => _slackEventHandlerRouter.RouteSlackEventAsync(SlackEventType.Unknown, slackEventBody, cancellationToken),
            };
        }
    }
}