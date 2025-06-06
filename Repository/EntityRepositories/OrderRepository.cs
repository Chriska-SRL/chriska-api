using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Order> logger) : base(connectionString, logger)
        {
        }

        public Order Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public Order? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
