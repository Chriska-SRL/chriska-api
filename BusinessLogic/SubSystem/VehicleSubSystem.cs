using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem;

public class VehicleSubSystem
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IVehicleCostRepository _costRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;

    public VehicleSubSystem(IVehicleRepository vehicleRepository, IVehicleCostRepository costRepository,IDeliveryRepository deliveryRepository, IUserRepository userRepository, IOrderRepository orderRepository)
    {
        _vehicleRepository = vehicleRepository;
        _costRepository = costRepository;
        _deliveryRepository = deliveryRepository;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
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

        vehicle.MarkAsDeleted(request.getUserId(), request.Location);
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

        cost.MarkAsDeleted(request.getUserId(), request.Location);
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

    // DELIVERY
    public async Task<DeliveryResponse> AddDeliveryAsync(DeliveryAddRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new ArgumentException("No se encontró el usuario asociado.");

        var order = await _orderRepository.GetByIdAsync(request.OrderId)
            ?? throw new ArgumentException("No se encontró la orden asociada.");

        var delivery = DeliveryMapper.ToDomain(request, user, order);
        delivery.Validate();

        var added = await _deliveryRepository.AddAsync(delivery);
        return DeliveryMapper.ToResponse(added);
    }

    //public async Task<DeliveryResponse> UpdateDeliveryAsync(DeliveryUpdateRequest request)
    //{
    //    var existing = await _deliveryRepository.GetByIdAsync(request.Id)
    //        ?? throw new ArgumentException("No se encontró la entrega seleccionada.");

    //    var updatedData = DeliveryMapper.ToUpdatableData(request);
    //    existing.Update(updatedData);

    //    var saved = await _deliveryRepository.UpdateAsync(existing);
    //    return DeliveryMapper.ToResponse(saved);
    //}

    public async Task<DeliveryResponse> DeleteDeliveryAsync(DeleteRequest request)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(request.Id)
            ?? throw new ArgumentException("No se encontró la entrega seleccionada.");

        delivery.MarkAsDeleted(request.getUserId(), request.Location);
        await _deliveryRepository.DeleteAsync(delivery);

        return DeliveryMapper.ToResponse(delivery);
    }

    public async Task<DeliveryResponse> GetDeliveryByIdAsync(int id)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(id)
            ?? throw new ArgumentException("No se encontró la entrega seleccionada.");

        return DeliveryMapper.ToResponse(delivery);
    }

    public async Task<List<DeliveryResponse>> GetAllDeliveriesAsync(QueryOptions options)
    {
        var deliveries = await _deliveryRepository.GetAllAsync(options);
        return deliveries.Select(DeliveryMapper.ToResponse).ToList();
    }

}
