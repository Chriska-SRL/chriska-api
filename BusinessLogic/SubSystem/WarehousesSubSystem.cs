using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class WarehousesSubSystem
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IShelveRepository _shelveRepository;

        public WarehousesSubSystem(IWarehouseRepository warehouseRepository, IShelveRepository shelveRepository)
        {
            _warehouseRepository = warehouseRepository;
            _shelveRepository = shelveRepository;
        }

        // Almacenes

        public async Task<WarehouseResponse> AddWarehouseAsync(AddWarehouseRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<WarehouseResponse> UpdateWarehouseAsync(UpdateWarehouseRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteWarehouseAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<WarehouseResponse> GetWarehouseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WarehouseResponse>> GetAllWarehousesAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        // Estanterías

        public async Task<ShelveResponse> AddShelveAsync(AddShelveRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ShelveResponse> UpdateShelveAsync(UpdateShelveRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteShelveAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ShelveResponse> GetShelveByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShelveResponse>> GetAllShelvesAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
