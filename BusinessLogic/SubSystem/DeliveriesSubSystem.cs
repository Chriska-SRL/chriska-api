using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Dominio;

namespace BusinessLogic.SubSystem
{
    public class DeliveriesSubSystem
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public DeliveriesSubSystem(IDeliveryRepository deliveryRepository, IVehicleRepository vehicleRepository)
        {
            _deliveryRepository = deliveryRepository;
            _vehicleRepository = vehicleRepository;
        }

        // Entregas

        public DeliveryResponse AddDelivery(AddDeliveryRequest request)
        {
            Delivery delivery = DeliveryMapper.ToDomain(request);
            delivery.Validate();

            Delivery added = _deliveryRepository.Add(delivery);
            return DeliveryMapper.ToResponse(added);
        }

        public DeliveryResponse UpdateDelivery(UpdateDeliveryRequest request)
        {
            Delivery existing = _deliveryRepository.GetById(request.Id)
                                  ?? throw new InvalidOperationException("Entrega no encontrada.");

            Delivery.UpdatableData updatedData = DeliveryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Delivery updated = _deliveryRepository.Update(existing);
            return DeliveryMapper.ToResponse(updated);
        }

        public DeliveryResponse DeleteDelivery(DeleteDeliveryRequest request)
        {
            Delivery deleted = _deliveryRepository.Delete(request.Id)
                                  ?? throw new InvalidOperationException("Entrega no encontrada.");

            return DeliveryMapper.ToResponse(deleted);
        }

        public DeliveryResponse GetDeliveryById(int id)
        {
            Delivery delivery = _deliveryRepository.GetById(id)
                                   ?? throw new InvalidOperationException("Entrega no encontrada.");

            return DeliveryMapper.ToResponse(delivery);
        }

        public List<DeliveryResponse> GetAllDeliveries()
        {
            return _deliveryRepository.GetAll()
                                      .Select(DeliveryMapper.ToResponse)
                                      .ToList();
        }

        // Vehículos

        public VehicleResponse AddVehicle(AddVehicleRequest request)
        {
            Vehicle vehicle = VehicleMapper.ToDomain(request);
            vehicle.Validate();

            Vehicle added = _vehicleRepository.Add(vehicle);
            return VehicleMapper.ToResponse(added);
        }

        public VehicleResponse UpdateVehicle(UpdateVehicleRequest request)
        {
            Vehicle existing = _vehicleRepository.GetById(request.Id)
                                  ?? throw new InvalidOperationException("Vehículo no encontrado.");

            Vehicle.UpdatableData updatedData = VehicleMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Vehicle updated = _vehicleRepository.Update(existing);
            return VehicleMapper.ToResponse(updated);
        }

        public VehicleResponse DeleteVehicle(DeleteVehicleRequest request)
        {
            Vehicle deleted = _vehicleRepository.Delete(request.Id)
                                  ?? throw new InvalidOperationException("Vehículo no encontrado.");

            return VehicleMapper.ToResponse(deleted);
        }

        public VehicleResponse GetVehicleById(int id)
        {
            Vehicle vehicle = _vehicleRepository.GetById(id)
                                  ?? throw new InvalidOperationException("Vehículo no encontrado.");

            return VehicleMapper.ToResponse(vehicle);
        }

        public List<VehicleResponse> GetAllVehicles()
        {
            return _vehicleRepository.GetAll()
                                     .Select(VehicleMapper.ToResponse)
                                     .ToList();
        }
    }
}
