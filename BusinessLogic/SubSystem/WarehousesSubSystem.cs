using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
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

        // Warehouses

        public async Task<WarehouseResponse> AddWarehouseAsync(AddWarehouseRequest request)
        {
            var existing = await _warehouseRepository.GetByNameAsync(request.Name);
            if (existing != null)
                throw new ArgumentException("Ya existe un almacén con ese nombre.");

            var newWarehouse = WarehouseMapper.ToDomain(request);
            newWarehouse.Validate();

            var added = await _warehouseRepository.AddAsync(newWarehouse);
            return WarehouseMapper.ToResponse(added);
        }

        public async Task<WarehouseResponse> UpdateWarehouseAsync(UpdateWarehouseRequest request)
        {
            var existingWarehouse = await _warehouseRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el almacén seleccionado.");

            var existing = await _warehouseRepository.GetByNameAsync(request.Name);
            if (existingWarehouse.Name != request.Name && existing != null)
                throw new ArgumentException("Ya existe un almacén con ese nombre.");

            var updatedData = WarehouseMapper.ToUpdatableData(request);
            existingWarehouse.Update(updatedData);

            var updated = await _warehouseRepository.UpdateAsync(existingWarehouse);
            return WarehouseMapper.ToResponse(updated);
        }

        public async Task DeleteWarehouseAsync(DeleteRequest request)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el almacén seleccionado.");

            if (warehouse.Shelves.Any())
                throw new InvalidOperationException("No se puede eliminar el almacén porque tiene estanterías asociadas.");

            warehouse.MarkAsDeleted(request.getUserId(), request.Location);
            await _warehouseRepository.DeleteAsync(warehouse);
        }

        public async Task<WarehouseResponse> GetWarehouseByIdAsync(int id)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el almacén seleccionado.");

            return WarehouseMapper.ToResponse(warehouse);
        }

        public async Task<List<WarehouseResponse>> GetAllWarehousesAsync(QueryOptions options)
        {
            var warehouses = await _warehouseRepository.GetAllAsync(options);
            return warehouses.Select(WarehouseMapper.ToResponse).ToList();
        }

        // Shelves

        public async Task<ShelveResponse> AddShelveAsync(AddShelveRequest request)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
            if ( warehouse == null)
                throw new ArgumentException("El almacén seleccionado no existe.");

            if ( await _shelveRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe una estantería con ese nombre.");

            var newShelve = ShelveMapper.ToDomain(request, warehouse);
            newShelve.Validate();

            var added = await _shelveRepository.AddAsync(newShelve);
            return ShelveMapper.ToResponse(added);
        }

        public async Task<ShelveResponse> UpdateShelveAsync(UpdateShelveRequest request)
        {
            var existingShelve = await _shelveRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la estantería seleccionada.");

            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
            if (warehouse == null)
                throw new ArgumentException("El almacén seleccionado no existe.");

            var updatedData = ShelveMapper.ToUpdatableData(request, warehouse);
            existingShelve.Update(updatedData);

            var updated = await _shelveRepository.UpdateAsync(existingShelve);
            return ShelveMapper.ToResponse(updated);
        }

        public async Task DeleteShelveAsync(DeleteRequest request)
        {
            var shelve = await _shelveRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la estantería seleccionada.");

            shelve.MarkAsDeleted(request.getUserId(), request.Location);
            await _shelveRepository.DeleteAsync(shelve);
        }

        public async Task<ShelveResponse> GetShelveByIdAsync(int id)
        {
            var shelve = await _shelveRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró la estantería seleccionada.");

            return ShelveMapper.ToResponse(shelve);
        }

        public async Task<List<ShelveResponse>> GetAllShelvesAsync(QueryOptions options)
        {
            var shelves = await _shelveRepository.GetAllAsync(options);
            return shelves.Select(ShelveMapper.ToResponse).ToList();
        }
    }
}
