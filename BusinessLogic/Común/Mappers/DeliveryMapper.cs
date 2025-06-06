using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsVehicle;

namespace BusinessLogic.Común.Mappers
{
    public static class DeliveryMapper
    {
        public static Delivery toDomain(AddDeliveryRequest addDeliveryRequest)
        {
            return new Delivery(

                id: 0,
                date: addDeliveryRequest.Date,
                driverName: addDeliveryRequest.DriverName,
                observation: addDeliveryRequest.Observation,
                orders: new List<Order>(),
                vehicle: new Vehicle(

                    id: addDeliveryRequest.VehicleId,
                    plate: "",
                    brand: "",
                    model: "",
                    crateCapacity: 0,
                    cost: null
                )

            );
        }
        public static Delivery.UpdatableData toDomain(UpdateDeliveryRequest updateDeliveryRequest)
        {
            return new Delivery.UpdatableData
            {
                Date = updateDeliveryRequest.Date,
                DriverName = updateDeliveryRequest.DriverName,
                Observation = updateDeliveryRequest.Observation,
                Vehicle = new Vehicle
                (
                    id : updateDeliveryRequest.VehicleId,
                    plate : "",
                    brand : "",
                    model: "",
                    crateCapacity : 0,
                    cost: null
                )
            };
        }

        public static DeliveryResponse toResponse(Delivery delivery)
        {
            return new DeliveryResponse
            {
                Id = delivery.Id,
                Date = delivery.Date,
                DriverName = delivery.DriverName,
                Observation = delivery.Observation,
                Vehicle = new VehicleResponse
                {
                    Id = delivery.Vehicle.Id,
                    Plate = delivery.Vehicle.Plate,
                    Brand = delivery.Vehicle.Brand,
                    Model = delivery.Vehicle.Model,
                    CrateCapacity = delivery.Vehicle.CrateCapacity,
                    Cost = new CostResponse
                    {
                        Id = delivery.Vehicle.Cost.Id,
                        Description = delivery.Vehicle.Cost.Description,
                        Amount = delivery.Vehicle.Cost.Amount
                    }
                }
            };
        }
    }
}
