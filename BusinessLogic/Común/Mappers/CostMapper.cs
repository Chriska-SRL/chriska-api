using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.Común.Mappers
{
    public static class CostMapper
    {
        public static Cost toDomain(AddCostRequest dto)
        {
            return new Cost(
                id: 0,
                description: dto.Description,
                amount: dto.Amount
            );
        }
        public static Cost toDomain(UpdateCostRequest dto)
        {
            return new Cost(
                id: dto.Id,
                description: dto.Description,
                amount: dto.Amount
            );
        }
        public static CostResponse toResponse(Cost domain)
        {
            return new CostResponse
            {
                Id = domain.Id,
                Description = domain.Description,
                Amount = domain.Amount
            };
        }
    }
}
