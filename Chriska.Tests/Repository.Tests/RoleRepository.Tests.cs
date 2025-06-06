using NUnit.Framework;
using Repository.EntityRepositories;
using Microsoft.Extensions.Logging.Abstractions;
using BusinessLogic.Dominio;
using Chriska.Tests.Repository.Tests;
using BusinessLogic.Repository;

namespace Repository.Tests
{
    public class RoleRepositoryTests: RepositoryTestsBase<Role, Role.UpdatableData>
    {
        private RoleRepository _repo;
        public override Role CreateSampleEntity()
        {
            return new Role(
            id: 0,
            name: $"newRole{getANumber()}",
            permissions: new List<Permission>() { Permission.CREATE_ROLES, Permission.DELETE_ROLES }
            );
        }

        public override bool CompareEntities(Role a, Role b)
        {
            return a.Id == b.Id && a.Name == b.Name && a.Permissions.SequenceEqual(b.Permissions);
        }

        public override Role ModifyEntity(Role entity)
        {
            entity.Name += "Modified";
            entity.Permissions.Add(Permission.EDIT_ROLES);
            return entity;
        }

        public override IRepository<Role> CreateRepositoryInstance()
        {
            return new RoleRepository(connectionString, NullLogger<RoleRepository>.Instance);
        }

        [SetUp]
        public void Setup()
        {
            DBControl.ClearDatabase();
            _repo = (RoleRepository)CreateRepositoryInstance();
            _estandarRepo = _repo;
        }
    }
}
