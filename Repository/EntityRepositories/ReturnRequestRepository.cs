using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class ReturnRequestRepository : Repository<ReturnRequest>, IReturnRequestRepository
    {
        public ReturnRequestRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<ReturnRequest> logger) : base(connectionString, logger)
        {
        }

        public ReturnRequest Add(ReturnRequest entity)
        {
            throw new NotImplementedException();
        }

        public ReturnRequest? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<ReturnRequest> GetAll()
        {
            throw new NotImplementedException();
        }

        public ReturnRequest? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ReturnRequest Update(ReturnRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
