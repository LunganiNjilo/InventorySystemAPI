using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class DatabaseMigrationHelper
    {
        public static void ApplyMigrations<TContext>(
            IServiceProvider services,
            int retryCount = 10,
            int delayMilliseconds = 3000)
            where TContext : DbContext
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            while (retryCount-- > 0)
            {
                try
                {
                    dbContext.Database.Migrate();
                    return;
                }
                catch
                {
                    if (retryCount <= 0)
                        throw;

                    Thread.Sleep(delayMilliseconds);
                }
            }
        }
    }
}
