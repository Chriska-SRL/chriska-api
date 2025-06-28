using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsStockMovement;

namespace BusinessLogic.Común.Mappers
{
    public static class StockMovementMapper
    {
        public static StockMovement ToDomain(AddStockMovementRequest dto)
        {
            return new StockMovement(
                id: 0,
                date: dto.Date,
                quantity: dto.Quantity,
                type: dto.Type,
                reason: dto.Reason,
                shelve: new Shelve(dto.ShelveId),
                user: new User(dto.UserId),
                product: new Product(dto.ProductId)
            );
        }

        public static StockMovementResponse ToResponse(StockMovement domain)
        {
            return new StockMovementResponse
            {
                Id = domain.Id,
                Date = domain.Date,
                Quantity = domain.Quantity,
                Type = domain.Type,
                Reason = domain.Reason,
                Shelve = ShelveMapper.ToResponse(domain.Shelve),
                User = UserMapper.ToResponse(domain.User),
                Product = ProductMapper.ToResponse(domain.Product)
            };
        }
    }
}
