using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.Common.Mappers
{
    public class ProductItemMapper
    {
        public static ProductItem ToDomain(ProductItemRequest request,Product product)
        {
           throw new NotImplementedException();
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
