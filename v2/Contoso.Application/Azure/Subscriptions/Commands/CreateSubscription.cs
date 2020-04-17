using SaveOnCloud.Core;
using LanguageExt;
using MediatR;
using System;

namespace SaveOnCloud.Application.Azure.Subscriptions.Commands
{
    public class CreateSubscription : Record<CreateSubscription>, IRequest<Either<Error, int>>
    {
        public CreateSubscription(Guid subscriptionId, string tenantId, Guid clientId, string clientPassword, int userId)
        {
            SubscriptionId = subscriptionId;
            TenantId = tenantId;
            ClientId = clientId;
            ClientPassword = clientPassword;
            UserId = userId;
        }

        public Guid SubscriptionId { get; }
        public string TenantId { get; }
        public Guid ClientId { get; }
        public string ClientPassword { get; }
        public int UserId { get; }
    }
}
