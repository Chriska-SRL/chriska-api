using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.SubSystem
{
    public class VehicleSubSystem
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleSubSystem(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public VehicleResponse AddVehicle(AddVehicleRequest request)
        {
            var newVehicle = VehicleMapper.ToDomain(request);
            newVehicle.Validate();

            var existing = _vehicleRepository.GetByPlate(newVehicle.Plate);
            if (existing != null)
                throw new ArgumentException("Ya existe un vehículo con esa matrícula.", nameof(newVehicle.Plate));

            var added = _vehicleRepository.Add(newVehicle);
            return VehicleMapper.ToResponse(added);
        }

        public VehicleResponse UpdateVehicle(UpdateVehicleRequest request)
        {
            var existingVehicle = _vehicleRepository.GetById(request.Id)
                                     ?? throw new ArgumentException("No se encontró el vehículo seleccionado.", nameof(request.Id));

            var other = _vehicleRepository.GetByPlate(request.Plate);
            if (existingVehicle.Plate != request.Plate && other != null)
                throw new ArgumentException("Ya existe un vehículo con esa matrícula.", nameof(request.Plate));

            var updatedData = VehicleMapper.ToUpdatableData(request);
            existingVehicle.Update(updatedData);

            var updated = _vehicleRepository.Update(existingVehicle);
            return VehicleMapper.ToResponse(updated);
        }

        public VehicleResponse DeleteVehicle(int id)
        {
            var vehicle = _vehicleRepository.GetById(id)
                          ?? throw new ArgumentException("No se encontró el vehículo seleccionado.", nameof(id));

            if (vehicle.VehicleCosts.Any())
                 throw new InvalidOperationException("No se puede eliminar el vehículo porque tiene costos asociados.");

            _vehicleRepository.Delete(id);
            return VehicleMapper.ToResponse(vehicle);
        }

        public VehicleResponse GetVehicleById(int id)
        {
            var vehicle = _vehicleRepository.GetById(id)
                          ?? throw new ArgumentException("No se encontró el vehículo seleccionado.", nameof(id));

            return VehicleMapper.ToResponse(vehicle);
        }
        public VehicleResponse GetVehicleByPlate(string plate)
        {
            var vehicle = _vehicleRepository.GetByPlate(plate)
                          ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(plate));

            return VehicleMapper.ToResponse(vehicle);
        }

        public List<VehicleResponse> GetAllVehicles()
        {
            return _vehicleRepository.GetAll()
                                     .Select(VehicleMapper.ToResponse)
                                     .ToList();
        }

        public VehicleCostResponse AddVehicleCost(AddVehicleCostRequest request)
        {
            var vehicle = _vehicleRepository.GetById(request.VehicleId)
                          ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(request.VehicleId));

            var newCost = VehicleCostMapper.ToDomain(request);
            newCost.Validate();

            vehicle.VehicleCosts.Add(newCost);
            _vehicleRepository.Update(vehicle);

            return VehicleCostMapper.ToResponse(newCost);
        }

        public VehicleCostResponse UpdateVehicleCost(UpdateVehicleCostRequest request)
        {
            var vehicle = _vehicleRepository.GetById(request.VehicleId)
                          ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(request.VehicleId));

            VehicleCost cost = vehicle.VehicleCosts.FirstOrDefault(c => c.Id == request.Id)
                          ?? throw new ArgumentException("No se encontró el costo especificado.", nameof(request.Id));

            var updatedData = VehicleCostMapper.ToUpdatableData(request);
            vehicle.UpdateCost(cost.Id, updatedData);

            _vehicleRepository.Update(vehicle);

            return VehicleCostMapper.ToResponse(cost);
        }

        public VehicleCostResponse DeleteVehicleCost(int vehicleId, int costId)
        {
            var vehicle = _vehicleRepository.GetById(vehicleId)
                          ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(vehicleId));

            VehicleCost cost = vehicle.VehicleCosts.FirstOrDefault(c => c.Id == costId)
                          ?? throw new ArgumentException("No se encontró el costo especificado.", nameof(costId));

            vehicle.RemoveCost(costId);
            _vehicleRepository.Update(vehicle);

            return VehicleCostMapper.ToResponse(cost);
        }

        public List<VehicleCostResponse> GetVehicleCosts(int vehicleId)
        {
            var vehicle = _vehicleRepository.GetById(vehicleId)
                          ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(vehicleId));

            return vehicle.VehicleCosts
                          .Select(VehicleCostMapper.ToResponse)
                          .ToList();
        }
    }
}
