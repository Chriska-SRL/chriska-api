using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsVehicle;


namespace BusinessLogic.Común.Mappers
{
    public static class VehicleMapper
    {
        
        public static Vehicle toDomain(AddVehicleRequest data)
        {
            return new Vehicle(
                id: 0,
                plate: data.Plate,
                brand: data.Brand,
                model: data.Model,
                crateCapacity: data.CrateCapacity,
                cost: new Cost(
                    id: data.CostId,
                    description: string.Empty,
                    amount: 0
                )
            );
        }
        public static Vehicle.UpdatableData toDomain(UpdateVehicleRequest data)
        {
           return new Vehicle.UpdatableData
           {
               Plate = data.Plate,
               Brand = data.Brand,
               Model = data.Model,
               CrateCapacity = data.CrateCapacity,
               Cost = new Cost(
                    id: data.CostId,
                    description: string.Empty,
                    amount: 0
                )
           };
        }
        public static VehicleResponse toResponse(Vehicle vehicle)
        {
            return new VehicleResponse
            {
                Id = vehicle.Id,
                Plate = vehicle.Plate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                CrateCapacity = vehicle.CrateCapacity,
                Cost = new CostResponse
                {
                    Id = vehicle.Cost.Id,
                    Description = vehicle.Cost.Description,
                    Amount = vehicle.Cost.Amount
                }
            };
        }
    }
}
