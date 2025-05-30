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
    public class UserRepository:Repository<UserRepository>, IUserRepository
    {
        public UserRepository(string connectionString, ILogger<UserRepository> logger) : base(connectionString, logger)
        {
        }

        public User Add(User entity)
        {
            throw new NotImplementedException();
        }

        public User Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool ExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public User? GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }

    }
}
