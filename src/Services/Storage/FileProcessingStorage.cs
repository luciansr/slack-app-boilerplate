using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Models.Events;
using Newtonsoft.Json;

namespace Services.Storage
{
    public class FileProcessingStorage : IProcessingConfigurationStorage
    {
        public Task<SlackProcessingConfiguration> GetEventProcessingConfigurationAsync(
            CancellationToken cancellationToken)
        {
            var allLines = string.Join(Environment.NewLine, File.ReadAllLines("./ExampleConfig/slack-bot-config.json"));

            return Task.FromResult(JsonConvert.DeserializeObject<SlackProcessingConfiguration>(allLines));
        }
    }
}