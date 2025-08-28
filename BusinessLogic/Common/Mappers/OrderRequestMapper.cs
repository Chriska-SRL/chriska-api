using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.Common.Mappers
{
    public static class OrderRequestMapper
    {
        public static OrderRequest ToDomain(OrderRequestAddRequest request, Client client, List<ProductItem> productItems, User user)
        {
            OrderRequest orderRequest = new OrderRequest
            (
                client,
                request.Observations,
                user,
                productItems
            );
            orderRequest.AuditInfo?.SetCreated(request.getUserId(), request.AuditLocation);
            return orderRequest;
        }

        public static OrderRequest.UpdatableData ToUpdatableData(OrderRequestUpdateRequest request, User user, List<ProductItem> productItems)
        {
            return new OrderRequest.UpdatableData
            {
                Observations = request.Observations,
                User = user,
                ProductItems = productItems,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static OrderRequestResponse? ToResponse(OrderRequest? orderRequest, Boolean notBucle = false)
        {
            if (orderRequest == null)
            {
                return null;
            }
            OrderRequestResponse? response = new OrderRequestResponse
            {
                Id = orderRequest.Id ,
                Date = orderRequest.Date,
                ConfirmedDate = orderRequest.ConfirmedDate,
                Observations = orderRequest.Observations ?? "",
                Status = orderRequest.Status,
                Client = ClientMapper.ToResponse(orderRequest.Client),
                Order = notBucle ? null : (orderRequest.Order != null ? OrderMapper.ToResponse(orderRequest.Order, true) : null),
                User = UserMapper.ToResponse(orderRequest.User),
                ProductItems = orderRequest.ProductItems.Select(pi => new ProductItemResponse
                {
                    Product =  ProductMapper.ToResponse(pi.Product),
                    Quantity = pi.Quantity,
                    Weight = pi.Weight,
                    UnitPrice = pi.UnitPrice,
                    Discount = pi.Discount
                }).ToList(),
                AuditInfo = AuditMapper.ToResponse(orderRequest.AuditInfo)
            };
            return response;
        }
    }
}