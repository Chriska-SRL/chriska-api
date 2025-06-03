using NUnit.Framework;
using Repository.EntityRepositories;
using Microsoft.Extensions.Logging.Abstractions;
using BusinessLogic.Dominio;
using Chriska.Tests.Repository.Tests;
using BusinessLogic.Repository;

namespace Repository.Tests
{
    public class UserRepositoryTests: RepositoryTestsBase<User,User.UpdatableData>
    {
        private UserRepository _repo;
        public override User CreateSampleEntity()
        {
            var roleRepo = new RoleRepository(connectionString, NullLogger<RoleRepository>.Instance);
            var role = roleRepo.Add(new Role(
                id: 0,
                name: $"TestRole{getANumber()}",
                permissions: new List<Permission> { Permission.VIEW_USERS }
            ));

            return new User(
                id: 0,
                name: $"newUser{getANumber()}",
                username: $"newUser{getANumber()}",
                password: "password123",
                isEnabled: true,
                role: role,
                requests: new List<Request>()
            );
        }


        public override bool CompareEntities(User a, User b)
        {
            return a.Id == b.Id
                && a.Name == b.Name
                && a.Username == b.Username
                && a.Password == b.Password
                && a.isEnabled == b.isEnabled
                && a.Role.Id == b.Role.Id
                && a.Role.Name == b.Role.Name
                && a.Role.Permissions.SequenceEqual(b.Role.Permissions);
        }


        public override User ModifyEntity(User entity)
        {
            entity.Name += "Modified";
            entity.Password = "newPassword456";
            entity.isEnabled = false;
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
        [Test]
        public void GetByUsername_ShouldReturnCorrectUser()
        {
            var user = CreateSampleEntity();
            var added = _estandarRepo.Add(user);

            var repo = (UserRepository)_estandarRepo;
            var result = repo.GetByUsername(added.Username);

            Assert.IsNotNull(result);
            Assert.IsTrue(CompareEntities(result!, added));
        }
        [Test]
        public void ExistsByUsername_ShouldReturnTrueIfUserExists()
        {
            var user = CreateSampleEntity();
            var added = _estandarRepo.Add(user);

            var repo = (UserRepository)_estandarRepo;
            var exists = repo.ExistsByUsername(added.Username);

            Assert.IsTrue(exists);
        }

        [Test]
        public void ExistsByUsername_ShouldReturnFalseIfUserDoesNotExist()
        {
            var repo = (UserRepository)_estandarRepo;
            var exists = repo.ExistsByUsername("nonexistent_user");

            Assert.IsFalse(exists);
        }


    }
}
