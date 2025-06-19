using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsCost;

namespace BusinessLogic.SubSystem
{
    public class VehicleSubSystem
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleCostRepository _costRepository;

        public VehicleSubSystem(IVehicleRepository vehicleRepository, IVehicleCostRepository costRepository)
        {
            _vehicleRepository = vehicleRepository;
            _costRepository = costRepository;
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

            var costs = _costRepository.GetAllForVehicle(id);
            if (costs.Any())
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


        //Costs
        public VehicleCostResponse AddVehicleCost(AddVehicleCostRequest request)
        {
            _ = _vehicleRepository.GetById(request.VehicleId)
                ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(request.VehicleId));

            var newCost = VehicleCostMapper.ToDomain(request);
            newCost.Validate();

            var added = _costRepository.Add(newCost);
            return VehicleCostMapper.ToResponse(added);
        }

        public VehicleCostResponse UpdateVehicleCost(UpdateVehicleCostRequest request)
        {
            var existing = _costRepository.GetById(request.Id)
                           ?? throw new ArgumentException("No se encontró el costo especificado.", nameof(request.Id));

            var updatedData = VehicleCostMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = _costRepository.Update(existing);
            return VehicleCostMapper.ToResponse(updated);
        }

        public VehicleCostResponse DeleteVehicleCost(int costId)
        {
            var deleted = _costRepository.Delete(costId)
                           ?? throw new ArgumentException("No se encontró el costo especificado.", nameof(costId));

            return VehicleCostMapper.ToResponse(deleted);
        }

        public List<VehicleCostResponse> GetVehicleCosts(int vehicleId)
        {
            return _costRepository.GetAllForVehicle(vehicleId)
                                  .Select(VehicleCostMapper.ToResponse)
                                  .ToList();
        }

        public List<VehicleCostResponse> GetCostsByDateRange(int vehicleId, DateTime from, DateTime to)
        {
            return _costRepository.GetCostsForVehicleInDateRange(vehicleId, from, to)
                                  .Select(VehicleCostMapper.ToResponse)
                                  .ToList();
        }
        public VehicleCostResponse GetVehicleCostById(int id)
        {
            var cost = _costRepository.GetById(id)
                       ?? throw new ArgumentException("No se encontró el costo especificado.", nameof(id));

            return VehicleCostMapper.ToResponse(cost);
        }

    }
}
