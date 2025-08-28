using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsDiscount;
using BusinessLogic.DTOs.DTOsProduct;

namespace BusinessLogic.Common.Mappers
{
    public static class DiscountMapper
    {
        public static Discount ToDomain(
            DiscountAddRequest request,
            Brand? brand,
            SubCategory? subCategory,
            Zone? zone,
            List<Client> clients,
            List<Product> products)
        {
            var discount = new Discount(
                description: request.Description,
                percentage: request.Percentage,
                productQuantity: request.ProductQuantity,
                expirationDate: request.ExpirationDate,
                status: request.Status,
                subCategory: subCategory,
                brand: brand,
                zone: zone,
                products: products,
                clients: clients
            );

            discount.AuditInfo?.SetCreated(request.getUserId(), request.AuditLocation);
            return discount;
        }

        public static Discount.UpdatableData ToUpdatableData(
            DiscountUpdateRequest request,
            Brand? brand,
            SubCategory? subCategory,
            Zone? zone,
            List<Client> clients,
            List<Product> products)
        {
            return new Discount.UpdatableData
            {
                Description = request.Description,
                Percentage = request.Percentage,
                ProductQuantity = request.ProductQuantity,
                ExpirationDate = request.ExpirationDate,
                Status = request.Status,
                SubCategory = subCategory,
                Brand = brand,
                Zone = zone,
                Clients = clients,
                Products = products,

                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static DiscountResponse? ToResponse(Discount? discount)
        {
            if( discount == null) return null;
            return new DiscountResponse
            {
                Id = discount.Id,
                Description = discount.Description,
                Percentage = discount.Percentage,
                ProductQuantity = discount.ProductQuantity,
                ExpirationDate = discount.ExpirationDate,
                Status = discount.Status,
                SubCategory = SubCategoryMapper.ToResponse(discount.SubCategory),
                Brand = BrandMapper.ToResponse(discount.Brand),
                Zone = ZoneMapper.ToResponse(discount.Zone),
                Clients = discount.Clients.Select(ClientMapper.ToResponse).OfType<ClientResponse>().ToList(),
                Products = discount.Products?.Select(ProductMapper.ToResponse).OfType<ProductResponse>().ToList(),
                AuditInfo = AuditMapper.ToResponse(discount.AuditInfo)
            };
        }
    }
}
