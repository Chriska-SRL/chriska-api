using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Común;
using BusinessLogic.DTOs;

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
            if (await _warehouseRepository.GetByNameAsync(request.Name) is not null)
                throw new ArgumentException("Ya existe un almacén con el mismo nombre.", nameof(request.Name));

            var newWarehouse = WarehouseMapper.ToDomain(request);
            newWarehouse.Validate();

            var added = await _warehouseRepository.AddAsync(newWarehouse);
            return WarehouseMapper.ToResponse(added);
        }

        public async Task<WarehouseResponse> UpdateWarehouseAsync(UpdateWarehouseRequest request)
        {
            var existing = await _warehouseRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("Almacén no encontrado.", nameof(request.Id));

            if (existing.Name != request.Name &&
                await _warehouseRepository.GetByNameAsync(request.Name) is not null)
                throw new ArgumentException("Ya existe un almacén con el mismo nombre.", nameof(request.Name));

            var updatedData = WarehouseMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _warehouseRepository.UpdateAsync(existing);
            return WarehouseMapper.ToResponse(updated);
        }

        public async Task<WarehouseResponse> DeleteWarehouseAsync(DeleteRequest request)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.Id)
                              ?? throw new InvalidOperationException("Depósito no encontrado.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            warehouse.SetDeletedAudit(auditInfo);

            await _warehouseRepository.DeleteAsync(warehouse);
            return WarehouseMapper.ToResponse(warehouse);
        }

        public async Task<WarehouseResponse> GetWarehouseByIdAsync(int id)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("Almacén no encontrado.", nameof(id));

            return WarehouseMapper.ToResponse(warehouse);
        }

        public async Task<List<WarehouseResponse>> GetAllWarehousesAsync(QueryOptions options)
        {
            var warehouses = await _warehouseRepository.GetAllAsync(options);
            return warehouses.Select(WarehouseMapper.ToResponse).ToList();
        }

        // Estanterías

        public async Task<ShelveResponse> AddShelveAsync(AddShelveRequest request)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId)
                ?? throw new ArgumentException("Almacén no encontrado.", nameof(request.WarehouseId));

            if (await _shelveRepository.GetByNameAsync(request.Name) is not null)
                throw new ArgumentException("Ya existe una estantería con el mismo nombre.", nameof(request.Name));

            var newShelve = ShelveMapper.ToDomain(request);
            newShelve.Validate();

            var added = await _shelveRepository.AddAsync(newShelve);
            return ShelveMapper.ToResponse(added);
        }

        public async Task<ShelveResponse> UpdateShelveAsync(UpdateShelveRequest request)
        {
            var existing = await _shelveRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("Estantería no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name &&
                await _shelveRepository.GetByNameAsync(request.Name) is not null)
                throw new ArgumentException("Ya existe una estantería con el mismo nombre.", nameof(request.Name));

            var updatedData = ShelveMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            var updated = await _shelveRepository.UpdateAsync(existing);
            return ShelveMapper.ToResponse(updated);
        }

        public async Task<ShelveResponse> DeleteShelveAsync(DeleteRequest request)
        {
            var shelve = await _shelveRepository.GetByIdAsync(request.Id)
                ?? throw new InvalidOperationException("Estantería no encontrada.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            shelve.SetDeletedAudit(auditInfo);

            await _shelveRepository.DeleteAsync(shelve);
            return ShelveMapper.ToResponse(shelve);
        }
        public async Task<ShelveResponse> GetShelveByIdAsync(int id)
        {
            var shelve = await _shelveRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("Estantería no encontrada.", nameof(id));

            return ShelveMapper.ToResponse(shelve);
        }
        public async Task<List<ShelveResponse>> GetAllShelvesAsync(QueryOptions options)
        {
            var shelves = await _shelveRepository.GetAllAsync(options);
            return shelves.Select(ShelveMapper.ToResponse).ToList();
        }
    }
}
