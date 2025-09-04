using BusinessLogic.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface ISupplierBalanceItemRepository : IRepository<BalanceItem>
    {
        Task<List<BalanceItem>> GetBySupplierIdAsync(int supplierId, DateTime? from = null, DateTime? to = null);
    }
}