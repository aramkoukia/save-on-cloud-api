using System.Threading.Tasks;
using Ardalis.GuardClauses;
using SaveOnCloud.Core.Events;
using SaveOnCloud.SharedKernel.Interfaces;

namespace SaveOnCloud.Core.Services
{
    public class ItemCompletedEmailNotificationHandler : IHandle<ToDoItemCompletedEvent>
    {
        public Task Handle(ToDoItemCompletedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            // Do Nothing
            return Task.CompletedTask;
        }
    }
}
