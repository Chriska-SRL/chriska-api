using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Delivery> logger) : base(connectionString, logger)
        {
        }

        public Delivery Add(Delivery entity)
        {
            throw new NotImplementedException();
        }

        public Delivery? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Delivery> GetAll()
        {
            throw new NotImplementedException();
        }

        public Delivery? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Delivery Update(Delivery entity)
        {
            throw new NotImplementedException();
        }
    }
}
