using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsStockMovement;

namespace BusinessLogic.Common.Mappers
{
    public static class StockMovementMapper
    {
        public static StockMovement ToDomain(AddStockMovementRequest request, User user, Product product)
        {
            var movement = new StockMovement(
                date: request.Date ?? DateTime.Now,
                quantity: request.Quantity,
                type: request.Type,
                reasonType: ReasonType.Adjustment,
                reason: request.Reason,
                user: user,
                product: product
            );

            movement.AuditInfo.SetCreated(request.getUserId(), request.AuditLocation);
            return movement;
        }

        public static StockMovementResponse ToResponse(StockMovement movement)
        {
            return new StockMovementResponse
            {
                Id = movement.Id,
                Date = movement.Date,
                Quantity = movement.Quantity,
                Type = movement.Type,
                Reason = movement.Reason,
                ReasonType = movement.ReasonType,
                User = UserMapper.ToResponse(movement.User),
                Product = ProductMapper.ToResponse(movement.Product),
                AuditInfo = AuditMapper.ToResponse(movement.AuditInfo)
            };
        }
    }
}
