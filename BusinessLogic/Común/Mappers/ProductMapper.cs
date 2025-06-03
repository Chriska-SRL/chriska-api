using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class ProductMapper
    {
        public static Product toDomain(AddProductRequest addProductRequest)
        {
            return new Product(
                id:0,
                internalCode: addProductRequest.InternalCode,
                barcode: addProductRequest.Barcode,
                name: addProductRequest.Name,
                price: addProductRequest.Price,
                image: addProductRequest.Image,
                stock: addProductRequest.Stock,
                description: addProductRequest.Description,
                unitType: addProductRequest.UnitType,
                temperatureCondition: addProductRequest.TemperatureCondition,
                observation: addProductRequest.Observation,
                subCategory: new SubCategory (
                    id: addProductRequest.SubCategoryId,
                    name: "",
                    category:null
                ),
                suppliers: new List<Supplier>()
            );
        }
        public static Product.UpdatableData toDomain(UpdateProductRequest updateProductRequest)
        {
            return new Product.UpdatableData
            {
                Name = updateProductRequest.Name,
                Price = updateProductRequest.Price,
                Image = updateProductRequest.Image,
                Stock = updateProductRequest.Stock,
                Description = updateProductRequest.Description,
                UnitType = updateProductRequest.UnitType,
                TemperatureCondition = updateProductRequest.TemperatureCondition,
                Observation = updateProductRequest.Observation,
                SubCategory = new SubCategory(
                    id: updateProductRequest.SubCategoryId,
                    name: "",
                    category: null
                )
            };
        }
        public static ProductResponse toResponse(Product product)
        {
            return new ProductResponse
            {

                Id = product.Id,
                InternalCode = product.InternalCode,
                Barcode = product.Barcode,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Stock = product.Stock,
                UnitType = product.UnitType,
                Description = product.Description,
                TemperatureCondition = product.TemperatureCondition,
                Observation = product.Observation,
                SubCategory = new SubCategoryResponse
                {
                    Id = product.SubCategory.Id,
                    Name = product.SubCategory.Name,
                    Category = new CategoryResponse
                    {
                        Id = product.SubCategory.Category.Id,
                        Name = product.SubCategory.Category.Name
                    }

                },
                Suppliers = product.Suppliers.Select(s => new SupplierResponse
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            };

        }
    }
}
