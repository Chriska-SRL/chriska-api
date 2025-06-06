using NUnit.Framework;
using Repository.EntityRepositories;
using Microsoft.Extensions.Logging.Abstractions;
using BusinessLogic.Dominio;
using Chriska.Tests.Repository.Tests;
using BusinessLogic.Repository;

namespace Repository.Tests
{
    public class ProductRepositoryTests : RepositoryTestsBase<Product>
    {
        private ProductRepository _repo;

        public override Product CreateSampleEntity()
        {
            var subCategory = new SubCategory(
                id: 1,
                name: "TestSubCat",
                category: new Category(1, "TestCat")
            );

            return new Product(
                id: 0,
                internalCode: "X0001",
                barcode: "1234567890123",
                name: $"TestProduct{getANumber()}",
                price: 100,
                image: "img.jpg",
                stock: 10,
                description: "desc",
                unitType: "K",
                temperatureCondition: "Frio",
                observation: "obs",
                subCategory: subCategory,
                suppliers: new()
            );
        }

        public override bool CompareEntities(Product a, Product b)
        {
            return a.Id == b.Id
                && a.Name == b.Name
                && a.InternalCode == b.InternalCode
                && a.Barcode == b.Barcode
                && a.Price == b.Price
                && a.Image == b.Image
                && a.Stock == b.Stock
                && a.Description == b.Description
                && a.UnitType == b.UnitType
                && a.TemperatureCondition == b.TemperatureCondition
                && a.Observation == b.Observation
                && a.SubCategory.Id == b.SubCategory.Id;
        }

        public override Product ModifyEntity(Product entity)
        {
            entity.Name += "Mod";
            entity.Price += 50;
            entity.Stock += 5;
            return entity;
        }

        public override IRepository<Product> CreateRepositoryInstance()
        {
            return new ProductRepository(connectionString, NullLogger<ProductRepository>.Instance);
        }

        [SetUp]
        public void Setup()
        {
            DBControl.ClearDatabase();
            _repo = (ProductRepository)CreateRepositoryInstance();
            _estandarRepo = _repo;
        }
    }
}
