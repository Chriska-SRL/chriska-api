using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.Common.Mappers
{
    public static class VehicleCostMapper
    {
        public static VehicleCost ToDomain(AddVehicleCostRequest request,Vehicle vehicle)
        {
            var cost = new VehicleCost(
                vehicle: vehicle,
                type: request.Type,
                description: request.Description,
                amount: request.Amount,
                date: request.Date
            );

            cost.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return cost;
        }

        public static VehicleCost.UpdatableData ToUpdatableData(UpdateVehicleCostRequest request)
        {
            return new VehicleCost.UpdatableData
            {
                Type = request.Type,
                Description = request.Description,
                Amount = request.Amount,
                Date = request.Date,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static VehicleCostResponse ToResponse(VehicleCost cost)
        {
            return new VehicleCostResponse
            {
                Vehicle= VehicleMapper.ToResponse(cost.Vehicle),
                Id = cost.Id,
                Date = cost.Date,
                Type = cost.Type,
                Description = cost.Description,
                Amount = cost.Amount,
                AuditInfo = AuditMapper.ToResponse(cost.AuditInfo)
            };
        }
    }
}
