using Library_System_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Library_System_Web_API.Data.Interceptors
{
    public class CreatedByInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyCreatedBy(eventData);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplyCreatedBy(eventData);
            return ValueTask.FromResult(result);
        }

        private static void ApplyCreatedBy(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is null || entry.State != EntityState.Added || entry.Entity is not BaseEntity entity)
                    continue;

                entity.CreatedBy = 1;
            }
        }
    }
}
