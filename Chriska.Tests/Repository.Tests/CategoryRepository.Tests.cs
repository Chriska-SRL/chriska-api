using NUnit.Framework;
using Repository.EntityRepositories;
using Microsoft.Extensions.Logging.Abstractions;
using BusinessLogic.Dominio;
using Chriska.Tests.Repository.Tests;
using BusinessLogic.Repository;

namespace Repository.Tests
{
    public class CategoryRepositoryTests : RepositoryTestsBase<Category, Category.UpdatableData>
    {
        private CategoryRepository _repo;

        public override Category CreateSampleEntity()
        {
            return new Category(
                id: 0,
                name: $"Category{getANumber()}",
                description: "Descripción inicial"
            );
        }

        public override bool CompareEntities(Category a, Category b)
        {
            return a.Id == b.Id && a.Name == b.Name && a.Description == b.Description;
        }

        public override Category ModifyEntity(Category entity)
        {
            entity.Name += "Mod";
            entity.Description += " modificada";
            return entity;
        }

        public override IRepository<Category> CreateRepositoryInstance()
        {
            return new CategoryRepository(connectionString, NullLogger<CategoryRepository>.Instance);
        }

        [SetUp]
        public void Setup()
        {
            DBControl.ClearDatabase();
            _repo = (CategoryRepository)CreateRepositoryInstance();
            _estandarRepo = _repo;
        }
    }
}
