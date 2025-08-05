using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Repository;

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

        // Vehículos

        public async Task<VehicleResponse> AddVehicleAsync(AddVehicleRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<VehicleResponse> UpdateVehicleAsync(UpdateVehicleRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteVehicleAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<VehicleResponse> GetVehicleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<VehicleResponse> GetVehicleByPlateAsync(string plate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VehicleResponse>> GetAllVehiclesAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        // Costos

        public async Task<VehicleCostResponse> AddVehicleCostAsync(AddVehicleCostRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<VehicleCostResponse> UpdateVehicleCostAsync(UpdateVehicleCostRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteVehicleCostAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VehicleCostResponse>> GetVehicleCostsAsync(int vehicleId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VehicleCostResponse>> GetCostsByDateRangeAsync(int vehicleId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public async Task<VehicleCostResponse> GetVehicleCostByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
