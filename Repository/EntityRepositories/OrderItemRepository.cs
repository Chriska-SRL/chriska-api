using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<OrderItem> logger) : base(connectionString, logger)
        {
        }

        public OrderItem Add(OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public OrderItem? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<OrderItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public OrderItem? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public OrderItem Update(OrderItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
