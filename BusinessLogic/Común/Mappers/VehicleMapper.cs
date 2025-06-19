using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsVehicle;


namespace BusinessLogic.Común.Mappers
{
    public static class VehicleMapper
    {
        
        public static Vehicle ToDomain(AddVehicleRequest data)
        {
            return new Vehicle(
                id: 0,
                plate: data.Plate,
                brand: data.Brand,
                model: data.Model,
                crateCapacity: data.CrateCapacity,
                costs: new List<VehicleCost>()
            );
        }
        public static Vehicle.UpdatableData ToUpdatableData(UpdateVehicleRequest data)
        {
           return new Vehicle.UpdatableData
           {
               Plate = data.Plate,
               Brand = data.Brand,
               Model = data.Model,
               CrateCapacity = data.CrateCapacity
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
                Costs = vehicle.VehicleCosts.Select(VehicleCostMapper.ToResponse).ToList()
            };
        }
    }
}
