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
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(string connectionString, ILogger<Receipt> logger) : base(connectionString, logger)
        {
        }

        public Receipt Add(Receipt entity)
        {
            throw new NotImplementedException();
        }

        public Receipt? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Receipt> GetAll()
        {
            throw new NotImplementedException();
        }

        public Receipt? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Receipt Update(Receipt entity)
        {
            throw new NotImplementedException();
        }
    }
}
