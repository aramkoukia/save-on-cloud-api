using System.Threading.Tasks;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using SaveOnCloud.Core.Domain.Azure;
using System;

namespace SaveOnCloud.Infrastructure.Data.Repositories.Azure
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SaveOnCloudDbContext SaveOnCloudDbContext;

        public SubscriptionRepository(SaveOnCloudDbContext SaveOnCloudDbContext) => 
            this.SaveOnCloudDbContext = SaveOnCloudDbContext;

        public async Task<Guid> Add(Subscription subscription)
        {
            await SaveOnCloudDbContext.Subscriptions.AddAsync(subscription);
            await SaveOnCloudDbContext.SaveChangesAsync();
            return subscription.SubscriptionId;
        }

        public async Task<Unit> Delete(Guid id)
        {
            var subscription = await SaveOnCloudDbContext.Subscriptions.FindAsync(id);
            SaveOnCloudDbContext.Subscriptions.Remove(subscription);
            return await SaveOnCloudDbContext
                .SaveChangesAsync()
                .Map(_ => Unit.Default);
        }

        public async Task<Option<Subscription>> Get(Guid id) => 
            await SaveOnCloudDbContext.Subscriptions.SingleOrDefaultAsync(c => c.SubscriptionId == id);

        public Task<Unit> Update(Subscription subscription)
        {
            SaveOnCloudDbContext.Subscriptions.Update(subscription);
            return SaveOnCloudDbContext.SaveChangesAsync()
                .Map(_ => Unit.Default);
        }
    }
}
