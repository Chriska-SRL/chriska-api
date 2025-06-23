using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.Común.Mappers
{
    public static class VehicleCostMapper
    {
        public static VehicleCost ToDomain(AddVehicleCostRequest dto)
        {
            return new VehicleCost(0, dto.VehicleId, dto.Type, dto.Description, dto.Amount, dto.Date);
        }
        public static VehicleCost.UpdatableData ToUpdatableData(UpdateVehicleCostRequest dto)
        {
            return new VehicleCost.UpdatableData
            {
                Type = dto.Type,
                Description = dto.Description,
                Amount = dto.Amount,
                Date = dto.Date
            };
        }
        public static VehicleCostResponse ToResponse(VehicleCost domain)
        {
            return new VehicleCostResponse
            {
                VehicleId = domain.VehicleId,
                Id = domain.Id,
                Date = domain.Date,
                Type = domain.Type,
                Description = domain.Description,
                Amount = domain.Amount
            };
        }
    }
}
