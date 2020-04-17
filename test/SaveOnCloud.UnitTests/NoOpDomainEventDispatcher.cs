using System.Threading.Tasks;
using SaveOnCloud.SharedKernel.Interfaces;
using SaveOnCloud.SharedKernel;

namespace SaveOnCloud.UnitTests
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task Dispatch(BaseDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
