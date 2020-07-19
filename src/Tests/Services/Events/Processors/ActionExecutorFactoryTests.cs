using System;
using Models.Events;
using Services.Events.Actions;
using Services.Events.Processors;
using Xunit;

namespace Tests.Services.Events.Processors
{
    public class ActionExecutorFactoryTests
    {
        [Theory]
        [InlineData(ActionType.AnswerToMessage, typeof(AnswerMessageActionExecutor))]
        public void ProviderCreatesCorrectMatchers(ActionType actionType, Type eventMatcherType)
        {
            var actionExecutorFactory = new ActionExecutorFactory();

            var actionExecutor = actionExecutorFactory.GetActionExecutor(
                new ActionConfiguration
                {
                    Type = actionType
                });
            
            Assert.Equal(eventMatcherType, actionExecutor.GetType());
        }
    }
}