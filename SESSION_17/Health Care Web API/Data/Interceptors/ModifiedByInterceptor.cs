using Health_Care_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace Health_Care_Web_API.Data.Interceptors
{
    public class ModifiedByInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyModifiedBy(eventData);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplyModifiedBy(eventData);
            return ValueTask.FromResult(result);
        }

        private static void ApplyModifiedBy(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is null || entry.State != EntityState.Modified || entry.Entity is not BaseEntity entity)
                    continue;

                entity.ModifiedBy = 1; // admin (is a doctor) has id 1, in real app this should come from the authenticated user context



            }
        }
    }
}