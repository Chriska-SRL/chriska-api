using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(string connectionString, ILogger<Payment> logger) : base(connectionString, logger)
        {
        }

        public Payment Add(Payment entity)
        {
            throw new NotImplementedException();
        }

        public Payment? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Payment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Payment? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Payment Update(Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}
