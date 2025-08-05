using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsVehicle;

namespace BusinessLogic.Common.Mappers
{
    public static class VehicleMapper
    {
        public static Vehicle ToDomain(AddVehicleRequest request)
        {
            var vehicle = new Vehicle(
                plate: request.Plate,
                brand: request.Brand,
                model: request.Model,
                crateCapacity: request.CrateCapacity
            );

            vehicle.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return vehicle;
        }

        public static Vehicle.UpdatableData ToUpdatableData(UpdateVehicleRequest request)
        {
            return new Vehicle.UpdatableData
            {
                Plate = request.Plate,
                Brand = request.Brand,
                Model = request.Model,
                CrateCapacity = request.CrateCapacity,
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static VehicleResponse ToResponse(Vehicle vehicle)
        {
            return new VehicleResponse
            {
                Id = vehicle.Id,
                Plate = vehicle.Plate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                CrateCapacity = vehicle.CrateCapacity,
                Costs = vehicle.VehicleCosts.Select(VehicleCostMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(vehicle.AuditInfo)
            };
        }
    }
}
