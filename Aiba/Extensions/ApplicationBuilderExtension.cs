using Aiba.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aiba.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static void InitDatabase(this IApplicationBuilder builder)
        {
            if (builder.ApplicationServices == null)
            {
                throw new InvalidOperationException("Application services is null");
            }

            IDbContextFactory<AppDBContext> contextFactory =
                builder.ApplicationServices.GetRequiredService<IDbContextFactory<AppDBContext>>();
            AppDBContext context = contextFactory.CreateDbContext();
            IEnumerable<string> pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
            }

            context.Database.EnsureCreated();
        }
    }
}