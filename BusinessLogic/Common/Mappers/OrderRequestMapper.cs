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
            orderRequest.AuditInfo?.SetCreated(request.getUserId(), request.Location);
            return orderRequest;
        }

        public static OrderRequest.UpdatableData ToUpdatableData(OrderRequestUpdateRequest request)
        {
            throw new NotImplementedException("ToUpdatableData method is not implemented yet.");
        }

        public static OrderRequestResponse? ToResponse(OrderRequest? orderRequest)
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
