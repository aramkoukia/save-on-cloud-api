using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SaveOnCloud.Core;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;
using static SaveOnCloud.Validators;
using SaveOnCloud.Application.Departments.Queries;
using SaveOnCloud.Core.Domain.Azure;

namespace SaveOnCloud.Application.Azure.Subscriptions.Commands
{
    public class CreateSubscriptionHandler : IRequestHandler<CreateSubscription, Either<Error, Guid>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMediator _mediator;

        public CreateSubscriptionHandler(ISubscriptionRepository subscriptionRepository,
                                         IMediator mediator)
        {
            _subscriptionRepository = subscriptionRepository;
            _mediator = mediator;
        }

        public Task<Either<Error, Guid>> Handle(CreateSubscription request, CancellationToken cancellationToken) =>
            Validate(request)
                .MapT(PersistSubscription)
                .Bind(v => v.ToEitherAsync());

        private Task<Guid> PersistSubscription(Subscription s) => _subscriptionRepository.Add(s);

        private async Task<Validation<Error, Subscription>> Validate(CreateSubscription create) => 
            (await GetOrganizationId(create), ValidatePassword(create))
                .Apply((organizationId) => new Subscription { 
                    SubscriptionId = create.SubscriptionId,
                    ClientId = create.ClientId,
                    ClientPassword = create.ClientPassword,
                    TenantId = create.TenantId,
                    OrganizationId = organizationId
                });

        private Validation<Error, string> ValidatePassword(CreateSubscription course) =>
            NotEmpty(course.ClientPassword)
                .Bind(NotLongerThan(250));

        private async Task<Validation<Error, Guid>> GetOrganizationId(CreateSubscription create) => 
            (await _mediator.Send(new GetOrganizationByUserId(create.UserId)))
                .ToValidation<Error>($"No Organization found for User Id {create.UserId}.")
                .Map(v => v.OrganizationId);
    }
}
