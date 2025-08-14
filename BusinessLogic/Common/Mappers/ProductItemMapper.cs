using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.Common.Mappers
{
    public class ProductItemMapper
    {
        public static ProductItem ToDomain(ProductItemRequest request,Product product)
        {
            ProductItem productItem = new ProductItem(
                quantity: request.Quantity,
                weight: request.Weight,
                unitPrice: request.UnitPrice,
                discount: request.Discount,
                product:product
            );

            productItem.AuditInfo?.SetCreated(request.getUserId(), request.Location);
            return productItem;
        }

        public static ProductItemResponse ToResponse(ProductItem request)
        {
            return new ProductItemResponse
            {
                Quantity = request.Quantity,
                Weight = request.Weight??0,
                UnitPrice = request.UnitPrice,
                Discount = request.Discount,
                Product = ProductMapper.ToResponse(request.Product)
            };
        }
    }
}
