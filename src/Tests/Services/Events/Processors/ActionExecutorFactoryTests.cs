using System;
using Models.Events;
using Moq;
using Services.Events.Actions;
using Services.Events.Processors;
using Services.Slack;
using Xunit;

namespace Tests.Services.Events.Processors
{
    public class ActionExecutorFactoryTests
    {
        [Theory]
        [InlineData(ActionType.AnswerToMessage, typeof(AnswerMessageActionExecutor))]
        public void ProviderCreatesCorrectMatchers(ActionType actionType, Type eventMatcherType)
        {
            var slackClientMock = new Mock<ISlackClient>();
            
            var actionExecutorFactory = new ActionExecutorFactory(slackClientMock.Object);

            var actionExecutor = actionExecutorFactory.GetActionExecutor(
                new ActionConfiguration
                {
                    Type = actionType
                });
            
            Assert.Equal(eventMatcherType, actionExecutor.GetType());
        }
    }
}