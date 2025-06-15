using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsVehicle;

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

        public void AddDelivery(AddDeliveryRequest deliveryRequest)
        {
            Delivery delivery = DeliveryMapper.ToDomain(deliveryRequest);
            delivery.Validate();
            _deliveryRepository.Add(delivery);
        }

        public void UpdateDelivery(UpdateDeliveryRequest deliveryRequest)
        {
            Delivery exisitingDelivery = _deliveryRepository.GetById(deliveryRequest.Id);
            if (exisitingDelivery == null) throw new Exception("No se encontro la entrega");
            exisitingDelivery.Update(DeliveryMapper.ToDomain(deliveryRequest));
            _deliveryRepository.Update(exisitingDelivery);
        }

        public void DeleteDelivery(DeleteDeliveryRequest deliveryRequest)
        {
            var delivery = _deliveryRepository.GetById(deliveryRequest.Id);
            if (delivery == null) throw new Exception("No se encontro la entrega");
            _deliveryRepository.Delete(deliveryRequest.Id);
        }

        public List<DeliveryResponse> GetAllDeliveries()
        {
            List<Delivery> deliveries = _deliveryRepository.GetAll();
            if (!deliveries.Any()) throw new Exception("No se encontraron entregas");
            return deliveries.Select(DeliveryMapper.ToResponse).ToList();
        }


        public void AddVehicle(AddVehicleRequest vehicle)
        {
            Vehicle newVehicle = VehicleMapper.ToDomain(vehicle);
            newVehicle.Validate();
            _vehicleRepository.Add(newVehicle);
        }

        public void UpdateVehicle(UpdateVehicleRequest vehicle)
        {
            Vehicle existingVehicle = _vehicleRepository.GetById(vehicle.Id);
            if (existingVehicle == null) throw new Exception("No se encontro el vehiculo");
            existingVehicle.Update(VehicleMapper.ToDomain(vehicle));
            _vehicleRepository.Update(existingVehicle);
        }

        public void DeleteVehicle(DeleteVehicleRequest vehicle)
        {
            var existingVehicle = _vehicleRepository.GetById(vehicle.Id);
            if (existingVehicle == null) throw new Exception("No se encontro el vehiculo");
            _vehicleRepository.Delete(vehicle.Id);
        }
        public List<VehicleResponse> GetAllVehicles()
        {
            List<Vehicle> listVehicles = _vehicleRepository.GetAll();
            if (!listVehicles.Any()) throw new Exception("No se encontraron vehiculos");
            return listVehicles.Select(VehicleMapper.ToResponse).ToList();
        }
        public VehicleResponse GetVehicleById(int id)
        {
            Vehicle vehicle = _vehicleRepository.GetById(id);
            if (vehicle == null) throw new Exception("No se encontro el vehiculo");
            VehicleResponse vehicleResponse = VehicleMapper.ToResponse(vehicle);
            return vehicleResponse;
        }
    }
}
