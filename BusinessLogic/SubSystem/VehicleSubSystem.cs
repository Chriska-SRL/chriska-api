using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem;

public class VehicleSubSystem
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IVehicleCostRepository _costRepository;
    private readonly IDistributionRepository _distributionRepository;

    public VehicleSubSystem(IVehicleRepository vehicleRepository, IVehicleCostRepository costRepository, IDeliveryRepository deliveryRepository,IDistributionRepository distributionRepository)
    {
        _vehicleRepository = vehicleRepository;
        _costRepository = costRepository;
        _distributionRepository = distributionRepository;
    }

    // VEHICLE


    public async Task<VehicleResponse> AddVehicleAsync(AddVehicleRequest request)
    {
        var newVehicle = VehicleMapper.ToDomain(request);
        newVehicle.Validate();

        var added = await _vehicleRepository.AddAsync(newVehicle);
        return VehicleMapper.ToResponse(added);
    }

    public async Task<VehicleResponse> UpdateVehicleAsync(UpdateVehicleRequest request)
    {
        var existingVehicle = await _vehicleRepository.GetByIdAsync(request.Id)
            ?? throw new ArgumentException("No se encontró el vehículo seleccionado.");

        var updatedData = VehicleMapper.ToUpdatableData(request);
        existingVehicle.Update(updatedData);

        var updated = await _vehicleRepository.UpdateAsync(existingVehicle);
        return VehicleMapper.ToResponse(updated);
    }

    public async Task<VehicleResponse> DeleteVehicleAsync(DeleteRequest request)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id)
            ?? throw new ArgumentException("No se encontró el vehículo seleccionado.");

        if (vehicle.VehicleCosts.Any())
            throw new InvalidOperationException("No se puede eliminar un vehículo con costos asociados activos.");

        var options = new QueryOptions
        {
            Filters = new Dictionary<string, string>
                {
                    { "VehicleId", request.Id.ToString() }
                }
        };
        List<Distribution> distributions = await _distributionRepository.GetAllAsync(options);
        if (distributions.Any())
        {
            throw new InvalidOperationException("No se puede eliminar la el vehiculo porque tiene repartos asociados.");
        }
        List<VehicleCost> costs = await _costRepository.GetAllAsync(options);
        if (costs.Any())
        {
            throw new InvalidOperationException("No se puede eliminar la el vehiculo porque tiene costos asociados.");
        }

        vehicle.MarkAsDeleted(request.getUserId(), request.AuditLocation);
        await _vehicleRepository.DeleteAsync(vehicle);
        return VehicleMapper.ToResponse(vehicle);
    }

    public async Task<VehicleResponse> GetVehicleByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id)
            ?? throw new ArgumentException("No se encontró el vehículo seleccionado.");

        return VehicleMapper.ToResponse(vehicle);
    }

    public async Task<List<VehicleResponse>> GetAllVehiclesAsync(QueryOptions options)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(options);
        return vehicles.Select(VehicleMapper.ToResponse).ToList();
    }

    // COSTS
    public async Task<VehicleCostResponse> AddVehicleCostAsync(AddVehicleCostRequest request)
    {
        var existingVehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId)
            ?? throw new ArgumentException("No se encontró el vehículo seleccionado.");
        var cost = VehicleCostMapper.ToDomain(request,existingVehicle);
        cost.Validate();

        var added = await _costRepository.AddAsync(cost);
        return VehicleCostMapper.ToResponse(added);
    }

    public async Task<VehicleCostResponse> UpdateVehicleCostAsync(UpdateVehicleCostRequest request)
    {
        var exisitingCost = await _costRepository.GetByIdAsync(request.Id)
            ?? throw new ArgumentException("No se encontró el costo seleccionado.");

        var updated = VehicleCostMapper.ToUpdatableData(request);
        exisitingCost.Update(updated);

        var saved = await _costRepository.UpdateAsync(exisitingCost);
        return VehicleCostMapper.ToResponse(saved);
    }

    public async Task<VehicleCostResponse> DeleteVehicleCostAsync(DeleteRequest request)
    {
        var cost = await _costRepository.GetByIdAsync(request.Id)
            ?? throw new ArgumentException("No se encontró el costo seleccionado.");

        cost.MarkAsDeleted(request.getUserId(), request.AuditLocation);
        await _costRepository.DeleteAsync(cost);

        return VehicleCostMapper.ToResponse(cost);
    }


    public async Task<VehicleCostResponse> GetVehicleCostByIdAsync(int id)
    {
        var cost = await _costRepository.GetByIdAsync(id)
            ?? throw new ArgumentException("No se encontró el costo seleccionado.");

        return VehicleCostMapper.ToResponse(cost);
    }

    public async Task<List<VehicleCostResponse>> GetAllCosts(QueryOptions options)
    {
        var costs = await _costRepository.GetAllAsync(options);
        return costs.Select(VehicleCostMapper.ToResponse).ToList();
    }


}
