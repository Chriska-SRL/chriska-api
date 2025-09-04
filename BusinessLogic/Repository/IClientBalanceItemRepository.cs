using BusinessLogic.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IClientBalanceItemRepository : IRepository<BalanceItem>
    {
        Task<List<BalanceItem>> GetByClientIdAsync(int clientId, DateTime? from = null, DateTime? to = null);
    }
}