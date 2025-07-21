using BusinessLogic.Común;
using BusinessLogic.Común.Audits;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.Mappers;

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
                vehicle: new Vehicle(id: addDeliveryRequest.VehicleId))
            {
                AuditInfo = AuditMapper.ToDomain(addDeliveryRequest.AuditInfo)
            };
        }

        public static Delivery.UpdatableData ToUpdatableData(UpdateDeliveryRequest updateDeliveryRequest)
        {
            return new Delivery.UpdatableData
            {
                Date = updateDeliveryRequest.Date,
                DriverName = updateDeliveryRequest.DriverName,
                Observation = updateDeliveryRequest.Observation,
                Vehicle = new Vehicle(updateDeliveryRequest.VehicleId),
                AuditInfo = AuditMapper.ToDomain(updateDeliveryRequest.AuditInfo)
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
                AuditInfo = AuditMapper.ToResponse(delivery.AuditInfo)
            };
        }
    }
}
