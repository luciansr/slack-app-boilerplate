using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Events;

namespace Services.Storage
{
    public interface IProcessingConfigurationStorage
    {
        Task<SlackProcessingConfiguration> GetEventProcessingConfigurationAsync(CancellationToken cancellationToken);
    }
}