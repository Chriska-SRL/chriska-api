using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsStockMovement;

namespace BusinessLogic.Común.Mappers
{
    public static class StockMovementMapper
    {
        public static StockMovement ToDomain(AddStockMovementRequest request)
        {
            var movement = new StockMovement(
                date: request.Date,
                quantity: request.Quantity,
                type: request.Type,
                reason: request.Reason,
                shelve: new Shelve(request.ShelveId),
                user: new User(request.UserId),
                product: new Product(request.ProductId)
            );

            movement.AuditInfo.SetCreated(request.getUserId(), request.Location);
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
                Shelve = ShelveMapper.ToResponse(movement.Shelve),
                User = UserMapper.ToResponse(movement.User),
                Product = ProductMapper.ToResponse(movement.Product),
                AuditInfo = AuditMapper.ToResponse(movement.AuditInfo)
            };
        }
    }
}
