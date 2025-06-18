using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsVehicle;

namespace BusinessLogic.Común.Mappers
{
    public static class DeliveryMapper
    {
        public static Delivery ToDomain(AddDeliveryRequest addDeliveryRequest)
        {
            return new Delivery(

                id: 0,
                date: addDeliveryRequest.Date,
                driverName: addDeliveryRequest.DriverName,
                observation: addDeliveryRequest.Observation,
                orders: new List<Order>(),
                vehicle: new Vehicle(id: addDeliveryRequest.VehicleId));
        }
        public static Delivery.UpdatableData ToUpdatableData(UpdateDeliveryRequest updateDeliveryRequest)
        {
            return new Delivery.UpdatableData
            {
                Date = updateDeliveryRequest.Date,
                DriverName = updateDeliveryRequest.DriverName,
                Observation = updateDeliveryRequest.Observation,
                Vehicle = new Vehicle(updateDeliveryRequest.VehicleId)
            };
        }

        public static DeliveryResponse ToResponse(Delivery delivery)
        {
            return new DeliveryResponse
            {
                Id = delivery.Id,
                Date = delivery.Date,
                DriverName = delivery.DriverName,
                Observation = delivery.Observation,
                Vehicle = VehicleMapper.ToResponse(delivery.Vehicle),
            };
        }
    }
}
