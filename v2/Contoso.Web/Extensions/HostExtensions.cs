﻿using System;
using SaveOnCloud.Infrastructure.Data;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace SaveOnCloud.Web.Extensions
{
    public static class HostExtensions
    {
        public static IHost SeedDatabase(this IHost host)
        {
            var test = use(() => host.Services.CreateScope(), Seed);
            return host;
        }

        private static Unit Seed(IServiceScope scope) => 
            Try(() => scope.ServiceProvider)
                .Bind(services => Try(GetDbContext(services)))
                .Bind(ctx => Try(Migrate(ctx)))
                .Bind(ctx => Try(InitializeDb(ctx)))
                .IfFail(ex => LogException(ex, scope.ServiceProvider));

        private static SaveOnCloudDbContext GetDbContext(IServiceProvider provider) =>
            provider.GetRequiredService<SaveOnCloudDbContext>();

        private static SaveOnCloudDbContext Migrate(SaveOnCloudDbContext context)
        {
            context.Database.Migrate();
            return context;
        }

        private static Unit InitializeDb(SaveOnCloudDbContext context) =>
            DbInitializer.Initialize(context);

        private static Unit LogException(Exception ex, IServiceProvider provider)
        {
            provider.GetRequiredService<ILogger<Program>>()
                .LogError(ex, "Error occurred while seeding database");
            return Unit.Default;
        }
    }
}
