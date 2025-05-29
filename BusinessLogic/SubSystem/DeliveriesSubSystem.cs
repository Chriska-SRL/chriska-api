using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.SubSystem
{
    public class DeliveriesSubSystem
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICostRepository _costRepository;
        public DeliveriesSubSystem(IDeliveryRepository deliveryRepository, IVehicleRepository vehicleRepository,ICostRepository costRepository)
        {
            _deliveryRepository = deliveryRepository;
            _vehicleRepository = vehicleRepository;
            _costRepository = costRepository;
        }
        public void AddDelivery(AddDeliveryRequest deliveryRequest)
        {
            var delivery = new Delivery(deliveryRequest.Date, deliveryRequest.DriverName, deliveryRequest.Observation, _vehicleRepository.GetById(deliveryRequest.VehicleId));
            delivery.Validate();
            _deliveryRepository.Add(delivery);
        }
        public void UpdateDelivery(UpdateDeliveryRequest deliveryRequest)
        {
            var delivery = _deliveryRepository.GetById(deliveryRequest.Id);
            if (delivery == null) throw new Exception("No se encontro la entrega");
            delivery.Update(deliveryRequest.DriverName, deliveryRequest.Observation, _vehicleRepository.GetById(deliveryRequest.VehicleId));
            _deliveryRepository.Update(delivery);
        }
        public void DeleteDelivery(DeleteDeliveryRequest deliveryRequest)
        {
            var delivery = _deliveryRepository.GetById(deliveryRequest.Id);
            if (delivery == null) throw new Exception("No se encontro la entrega");
            _deliveryRepository.Delete(deliveryRequest.Id);
        }
        public List<DeliveryResponse> GetAllDeliveries()
        {
            var deliveries = _deliveryRepository.GetAll();
            if (deliveries == null || !deliveries.Any()) throw new Exception("No se encontraron entregas");
            var deliveryResponses = new List<DeliveryResponse>();
            foreach (var delivery in deliveries)
            {
                var response = new DeliveryResponse
                {
                    Id = delivery.Id,
                    Date = delivery.Date,
                    DriverName = delivery.DriverName,
                    Observation = delivery.Observation,
                    Vehicle = new VehicleResponse
                    {
                        Plate = delivery.Vehicle.Plate,
                        Brand = delivery.Vehicle.Brand,
                        Model = delivery.Vehicle.Model,
                        CrateCapacity = delivery.Vehicle.CrateCapacity,
                        Cost = new CostResponse
                        {
                            Description = delivery.Vehicle.Cost.Description,
                            Amount = delivery.Vehicle.Cost.Amount,
                        }
                    }
                };
                deliveryResponses.Add(response);
            }
            return deliveryResponses;
        }
        public DeliveryResponse GetDeliveryById(int id)
        {
            var delivery = _deliveryRepository.GetById(id);
            if (delivery == null) throw new Exception("No se encontro la entrega");
            return new DeliveryResponse
            {
                Id = delivery.Id,
                Date = delivery.Date,
                DriverName = delivery.DriverName,
                Observation = delivery.Observation,
                Vehicle= new VehicleResponse
                {
                    Plate = delivery.Vehicle.Plate,
                    Brand = delivery.Vehicle.Brand,
                    Model = delivery.Vehicle.Model,
                    CrateCapacity = delivery.Vehicle.CrateCapacity,
                    Cost = new CostResponse
                    {
                        Description = delivery.Vehicle.Cost.Description,
                        Amount = delivery.Vehicle.Cost.Amount,
                    }
                }
            };
        }
        public void AddVehicle(AddVehicleRequest vehicle)
        {
            var newVehicle = new Vehicle(vehicle.Plate, vehicle.Brand, vehicle.Model, vehicle.CrateCapacity, _costRepository.GetById(vehicle.CostId));
            newVehicle.Validate();
            _vehicleRepository.Add(newVehicle);
        }
        public void UpdateVehicle(UpdateVehicleRequest vehicle)
        {
            var existingVehicle = _vehicleRepository.GetById(vehicle.Id);
            if (existingVehicle == null) throw new Exception("No se encontro el vehiculo");
            existingVehicle.Update(vehicle.Plate, vehicle.Brand, vehicle.Model, vehicle.CrateCapacity, _costRepository.GetById(vehicle.CostId));
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
            var vehicles = _vehicleRepository.GetAll();
            if (vehicles == null || !vehicles.Any()) throw new Exception("No se encontraron vehiculos");
            var vehicleResponses = new List<VehicleResponse>();
            foreach (var vehicle in vehicles)
            {
                var response = new VehicleResponse
                {
                    Plate = vehicle.Plate,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    CrateCapacity = vehicle.CrateCapacity,
                    Cost = new CostResponse
                    {
                        Description = vehicle.Cost.Description,
                        Amount = vehicle.Cost.Amount,
                    }
                };
                vehicleResponses.Add(response);
            }
            return vehicleResponses;
        }
        public VehicleResponse GetVehicleById(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);
            if (vehicle == null) throw new Exception("No se encontro el vehiculo");
            return new VehicleResponse
            {
                Plate = vehicle.Plate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                CrateCapacity = vehicle.CrateCapacity,
                Cost = new CostResponse
                {
                    Description = vehicle.Cost.Description,
                    Amount = vehicle.Cost.Amount,
                }
            };
        }
    }
}
