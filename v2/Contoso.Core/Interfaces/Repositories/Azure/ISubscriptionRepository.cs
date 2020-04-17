using System.Threading.Tasks;
using LanguageExt;
using SaveOnCloud.Core.Domain.Azure;
using System;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<Option<Subscription>> Get(Guid id);
        Task<Guid> Add(Subscription subscription);
        Task<Unit> Update(Subscription subscription);
        Task<Unit> Delete(Guid id);
    }
}
