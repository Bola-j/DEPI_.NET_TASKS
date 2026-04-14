using E_COMMERCE_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace E_COMMERCE_Web_API.Data.Interceptors
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

                switch(entry.Entity) // admin has id 1
                {
                    case Order o:
                        o.ModifiedBy = o.CustomerId;
                        break;
                    case Product p:
                        p.ModifiedBy = 1;
                        break;
                    case Customer c:
                        c.ModifiedBy = c.Id;
                        break;
                    case Category cat:
                        cat.ModifiedBy = 1;
                        break;
                    case OrderDetail od:
                        var order = od.Order ?? eventData.Context.Set<Order>().Find(od.OrderId);
                        if (order != null)
                        {
                            od.ModifiedBy = order.CustomerId;
                        }
                        else
                        {
                            od.ModifiedBy = 1; // fallback to admin if order not found
                        }
                        break;
                }
                

            }
        }
    }
}