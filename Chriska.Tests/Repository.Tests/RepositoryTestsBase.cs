using BusinessLogic.Repository;
using BusinessLogic.Dominio;
using NUnit.Framework;
using Repository.EntityRepositories;
using Microsoft.Extensions.Configuration;

namespace Chriska.Tests.Repository.Tests
{
    public abstract class RepositoryTestsBase<T> where T : IEntity
    {
        protected string connectionString = LoadConnectionString();
        protected IRepository<T> _estandarRepo;
        public abstract T CreateSampleEntity();
        public abstract bool CompareEntities(T a, T b);
        public abstract T ModifyEntity(T entity);

        static int aNumber = 1;
        protected static int getANumber()
        {
            return aNumber++;
        }

        private static string LoadConnectionString()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.Development.json", optional: false)
                .Build();

            return config.GetConnectionString("TestDatabase");
        }

        [SetUp]
        public void Setup()
        {
            DBControl.ClearDatabase();
            //_repo = (EntityRepository)CreateRepositoryInstance();
            //_estandarRepo = _repo;
        }
        public abstract IRepository<T> CreateRepositoryInstance();

        [Test]
        public void Add_ShouldInsertEntity()
        {
            var entity = CreateSampleEntity();
            var entityAdded = _estandarRepo.Add(entity);

            var result = _estandarRepo.GetById(entityAdded.Id);

            Assert.IsNotNull(result);
            Assert.IsTrue(CompareEntities(result!, entityAdded));
        }

        [Test]
        public void GetById_ShouldReturnCorrectEntity()
        {
            var entity = CreateSampleEntity();
            var entityAdded = _estandarRepo.Add(entity);

            var result = _estandarRepo.GetById(entityAdded.Id);

            Assert.IsNotNull(result);
            Assert.IsTrue(CompareEntities(result!, entityAdded));
        }

        [Test]
        public void GetAll_ShouldIncludeInsertedEntity()
        {
            var entity = CreateSampleEntity();
            var entityAdded = _estandarRepo.Add(entity);

            var all = _estandarRepo.GetAll();

            Assert.IsTrue(all.Any(e => CompareEntities(e, entityAdded)));
        }

        [Test]
        public void Update_ShouldModifyEntity()
        {
            var entity = CreateSampleEntity();
            var entityAdded = _estandarRepo.Add(entity);

            var modified = ModifyEntity(entityAdded);
            var entityModify = _estandarRepo.Update(modified);

            var updated = _estandarRepo.GetById(entityAdded.Id);

            Assert.IsNotNull(updated);
            Assert.IsTrue(CompareEntities(updated!, entityModify));
        }

        [Test]
        public void Delete_ShouldRemoveEntity()
        {
            var entity = CreateSampleEntity();
            var entityAdded = _estandarRepo.Add(entity);

            _estandarRepo.Delete(entityAdded.Id);

            var result = _estandarRepo.GetById(entityAdded.Id);
            Assert.IsNull(result);
        }
    }
}