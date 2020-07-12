using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Events;

namespace Services.Storage
{
    public interface IEventProcessorStorage
    {
        Task<Dictionary<string, TeamProcessingConfiguration>> GetEventProcessingConfigurationAsync(CancellationToken cancellationToken);
    }
}