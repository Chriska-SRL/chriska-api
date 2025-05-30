using NUnit.Framework;
using Repository.EntityRepositories;
using Microsoft.Extensions.Logging.Abstractions;
using BusinessLogic.Dominio;
using Chriska.Tests.Repository.Tests;
using BusinessLogic.Repository;

namespace Repository.Tests
{
    public class UserRepositoryTests: RepositoryTestsBase<User>
    {
        private UserRepository _repo;
        public override User CreateSampleEntity()
        {
            return new User(
            id: 0,
            name: $"newUser{getANumber()}",
            username: $"newUser{getANumber()}",
            password: "password123",
            isEnabled: true,
            role: new Role(0, "UserRole", new List<Permission> { Permission.VIEW_USERS })
            );
        }

        public override bool CompareEntities(User a, User b)
        {
            return a.Id == b.Id && a.Name == b.Name;
        }

        public override User ModifyEntity(User entity)
        {
            entity.Name += "Modified";
            return entity;
        }

        public override IRepository<User> CreateRepositoryInstance()
        {
            return new UserRepository(connectionString, NullLogger<UserRepository>.Instance);
        }

        [SetUp]
        public void Setup()
        {
            DBControl.ClearDatabase();
            _repo = (UserRepository)CreateRepositoryInstance();
            _estandarRepo = _repo;
        }
    }
}
