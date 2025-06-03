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
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(string connectionString, ILogger<Vehicle> logger) : base(connectionString, logger)
        {
        }

        public Vehicle Add(Vehicle entity)
        {
            throw new NotImplementedException();
        }

        public Vehicle? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> GetAll()
        {
            throw new NotImplementedException();
        }

        public Vehicle? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Vehicle Update(Vehicle entity)
        {
            throw new NotImplementedException();
        }
    }
}
