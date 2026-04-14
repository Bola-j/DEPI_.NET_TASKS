using E_COMMERCE_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace E_COMMERCE_Web_API.Data.Interceptors
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


                switch (entry.Entity) // admin has id 1
                {
                    case Order o:
                        o.CreatedBy = o.CustomerId;
                        break;
                    case Product p:
                        p.CreatedBy = 1;
                        break;
                    case Customer c:
                        c.CreatedBy = c.Id;
                        // createdby is set before the entity is saved,
                        // so Id is not generated yet.
                        // I set it in the controller with double save , but it triggers the modifiedby interceptor
                        
                        break;
                    case Category cat:
                        cat.CreatedBy = 1;
                        break;
                    case OrderDetail od:
                        var order = od.Order ?? eventData.Context.Set<Order>().Find(od.OrderId);
                        if (order != null)
                        {
                            od.CreatedBy = order.CustomerId;
                        }
                        else
                        {
                            od.CreatedBy = 1; // fallback to admin if order not found
                        }
                        break;
                }
            }
        }
    }
}