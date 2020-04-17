using System.Threading.Tasks;
using SaveOnCloud.SharedKernel;

namespace SaveOnCloud.SharedKernel.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}