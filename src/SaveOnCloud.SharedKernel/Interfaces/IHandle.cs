using System.Threading.Tasks;
using SaveOnCloud.SharedKernel;

namespace SaveOnCloud.SharedKernel.Interfaces
{
    public interface IHandle<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}