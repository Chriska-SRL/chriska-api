using NUnit.Framework;
using Repository.EntityRepositories;
using Microsoft.Extensions.Logging.Abstractions;
using BusinessLogic.Dominio;
using Chriska.Tests.Repository.Tests;
using BusinessLogic.Repository;

namespace Repository.Tests
{
    public class SubCategoryRepositoryTests : RepositoryTestsBase<SubCategory, SubCategory.UpdatableData>
    {
        private SubCategoryRepository _repo;

        public override SubCategory CreateSampleEntity()
        {
            var category = new Category(1, "CategoriaDummy");
            return new SubCategory(
                id: 0,
                name: $"SubCategoria{getANumber()}",
                category: category
            );
        }

        public override bool CompareEntities(SubCategory a, SubCategory b)
        {
            return a.Id == b.Id && a.Name == b.Name && a.Category.Id == b.Category.Id;
        }

        public override SubCategory ModifyEntity(SubCategory entity)
        {
            entity.Name += "Modificado";
            return entity;
        }

        public override IRepository<SubCategory> CreateRepositoryInstance()
        {
            return new SubCategoryRepository(connectionString, NullLogger<SubCategoryRepository>.Instance);
        }

        [SetUp]
        public void Setup()
        {
            DBControl.ClearDatabase();
            _repo = (SubCategoryRepository)CreateRepositoryInstance();
            _estandarRepo = _repo;
        }
    }
}
