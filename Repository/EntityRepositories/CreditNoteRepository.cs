using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class CreditNoteRepository : Repository<CreditNote>, ICreditNoteRepository
    {
        public CreditNoteRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<CreditNote> logger) : base(connectionString, logger)
        {
        }

        public CreditNote Add(CreditNote entity)
        {
            throw new NotImplementedException();
        }

        public CreditNote? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CreditNote> GetAll()
        {
            throw new NotImplementedException();
        }

        public CreditNote? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public CreditNote Update(CreditNote entity)
        {
            throw new NotImplementedException();
        }
    }
}
