using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs;
using BusinessLogic.Común;

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

        public async Task<VehicleResponse> AddVehicleAsync(AddVehicleRequest request)
        {
            var newVehicle = VehicleMapper.ToDomain(request);
            newVehicle.Validate();

            var existing = await _vehicleRepository.GetByPlateAsync(newVehicle.Plate);
            if (existing != null)
                throw new ArgumentException("Ya existe un vehículo con esa matrícula.", nameof(newVehicle.Plate));

            var added = await _vehicleRepository.AddAsync(newVehicle);
            return VehicleMapper.ToResponse(added);
        }

        public async Task<VehicleResponse> UpdateVehicleAsync(UpdateVehicleRequest request)
        {
            var existingVehicle = await _vehicleRepository.GetByIdAsync(request.Id)
                                     ?? throw new ArgumentException("No se encontró el vehículo seleccionado.", nameof(request.Id));

            var other = await _vehicleRepository.GetByPlateAsync(request.Plate);
            if (existingVehicle.Plate != request.Plate && other != null)
                throw new ArgumentException("Ya existe un vehículo con esa matrícula.", nameof(request.Plate));

            var updatedData = VehicleMapper.ToUpdatableData(request);
            existingVehicle.Update(updatedData);

            var updated = await _vehicleRepository.UpdateAsync(existingVehicle);
            return VehicleMapper.ToResponse(updated);
        }
        public async Task<VehicleResponse> DeleteVehicleAsync(DeleteRequest request)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Id)
                           ?? throw new ArgumentException("No se encontró el vehículo seleccionado.", nameof(request.Id));

            var costs = await _costRepository.GetAllForVehicleAsync(request.Id);
            if (costs.Any())
                throw new InvalidOperationException("No se puede eliminar el vehículo porque tiene costos asociados.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            vehicle.SetDeletedAudit(auditInfo);

            await _vehicleRepository.DeleteAsync(vehicle);
            return VehicleMapper.ToResponse(vehicle);
        }

        public async Task<VehicleResponse> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id)
                          ?? throw new ArgumentException("No se encontró el vehículo seleccionado.", nameof(id));

            return VehicleMapper.ToResponse(vehicle);
        }

        public async Task<VehicleResponse> GetVehicleByPlateAsync(string plate)
        {
            var vehicle = await _vehicleRepository.GetByPlateAsync(plate)
                          ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(plate));

            return VehicleMapper.ToResponse(vehicle);
        }

        public async Task<List<VehicleResponse>> GetAllVehiclesAsync(QueryOptions options)
        {
            var vehicles = await _vehicleRepository.GetAllAsync(options);
            return vehicles.Select(VehicleMapper.ToResponse).ToList();
        }


        //Costs
        public async Task<VehicleCostResponse> AddVehicleCostAsync(AddVehicleCostRequest request)
        {
            _ = await _vehicleRepository.GetByIdAsync(request.VehicleId)
                ?? throw new ArgumentException("No se encontró el vehículo especificado.", nameof(request.VehicleId));

            var newCost = VehicleCostMapper.ToDomain(request);
            newCost.Validate();

            var added = await _costRepository.AddAsync(newCost);
            return VehicleCostMapper.ToResponse(added);
        }

        public async Task<VehicleCostResponse> UpdateVehicleCostAsync(UpdateVehicleCostRequest request)
        {
            var existing = await _costRepository.GetByIdAsync(request.Id)
                           ?? throw new ArgumentException("No se encontró el costo especificado.", nameof(request.Id));

            var updatedData = VehicleCostMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _costRepository.UpdateAsync(existing);
            return VehicleCostMapper.ToResponse(updated);
        }

        public async Task<VehicleCostResponse> DeleteVehicleCostAsync(DeleteRequest request)
        {
            var vehicleCost = await _costRepository.GetByIdAsync(request.Id)
                                ?? throw new InvalidOperationException("Costo de vehículo no encontrado.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            vehicleCost.SetDeletedAudit(auditInfo);

            await _costRepository.DeleteAsync(vehicleCost);
            return VehicleCostMapper.ToResponse(vehicleCost);
        }
        public async Task<List<VehicleCostResponse>> GetVehicleCostsAsync(int vehicleId)
        {
            var costs = await _costRepository.GetAllForVehicleAsync(vehicleId);
            return costs.Select(VehicleCostMapper.ToResponse).ToList();
        }

        public async Task<List<VehicleCostResponse>> GetCostsByDateRangeAsync(int vehicleId, DateTime from, DateTime to)
        {
            var costs = await _costRepository.GetCostsForVehicleInDateRangeAsync(vehicleId, from, to);
            return costs.Select(VehicleCostMapper.ToResponse).ToList();
        }

        public async Task<VehicleCostResponse> GetVehicleCostByIdAsync(int id)
        {
            var cost = await _costRepository.GetByIdAsync(id)
                       ?? throw new ArgumentException("No se encontró el costo especificado.", nameof(id));

            return VehicleCostMapper.ToResponse(cost);
        }
    }
}
