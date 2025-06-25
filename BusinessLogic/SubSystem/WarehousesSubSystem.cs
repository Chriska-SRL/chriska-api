using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.Común.Mappers;

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

        public WarehouseResponse AddWarehouse(AddWarehouseRequest request)
        {
            if (_warehouseRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe un almacén con el mismo nombre.", nameof(request.Name));

            Warehouse newWarehouse = WarehouseMapper.ToDomain(request);
            newWarehouse.Validate();

            Warehouse added = _warehouseRepository.Add(newWarehouse);
            return WarehouseMapper.ToResponse(added);
        }

        public WarehouseResponse UpdateWarehouse(UpdateWarehouseRequest request)
        {
            Warehouse existing = _warehouseRepository.GetById(request.Id)
                ?? throw new ArgumentException("Almacén no encontrado.", nameof(request.Id));

            if (existing.Name != request.Name &&
                _warehouseRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe un almacén con el mismo nombre.", nameof(request.Name));

            Warehouse.UpdatableData updatedData = WarehouseMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Warehouse updated = _warehouseRepository.Update(existing);
            return WarehouseMapper.ToResponse(updated);
        }

        public WarehouseResponse DeleteWarehouse(int id)
        {
            Warehouse existing = _warehouseRepository.GetById(id)
                ?? throw new ArgumentException("Almacén no encontrado.", nameof(id));

            if (existing.Shelves.Any())
                throw new InvalidOperationException("No se puede eliminar un almacén con estanterías asociadas.");

            Warehouse deleted = _warehouseRepository.Delete(existing.Id)
                ?? throw new InvalidOperationException("Error al eliminar el almacén.");

            return WarehouseMapper.ToResponse(deleted);
        }

        public WarehouseResponse GetWarehouseById(int id)
        {
            Warehouse warehouse = _warehouseRepository.GetById(id)
                ?? throw new ArgumentException("Almacén no encontrado.", nameof(id));

            return WarehouseMapper.ToResponse(warehouse);
        }

        public List<WarehouseResponse> GetAllWarehouses()
        {
            List<Warehouse> warehouses = _warehouseRepository.GetAll();
            return warehouses.Select(WarehouseMapper.ToResponse).ToList();
        }

        // Estanterías

        public ShelveResponse AddShelve(AddShelveRequest request)
        {
            Warehouse warehouse = _warehouseRepository.GetById(request.WarehouseId)
                ?? throw new ArgumentException("Almacén no encontrado.", nameof(request.WarehouseId));
            if (_shelveRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una estantería con el mismo nombre.", nameof(request.Name));

            Shelve newShelve = ShelveMapper.ToDomain(request);
            newShelve.Validate();

            Shelve added = _shelveRepository.Add(newShelve);
            return ShelveMapper.ToResponse(added);
        }

        public ShelveResponse UpdateShelve(UpdateShelveRequest request)
        {
            Shelve existing = _shelveRepository.GetById(request.Id)
                ?? throw new ArgumentException("Estantería no encontrada.", nameof(request.Id));

            if (existing.Name != request.Name && _shelveRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe una estantería con el mismo nombre.", nameof(request.Name));
            

            Shelve.UpdatableData updatedData = ShelveMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Shelve updated = _shelveRepository.Update(existing);
            return ShelveMapper.ToResponse(updated);
        }

        public ShelveResponse DeleteShelve(int id)
        {
            Shelve deleted = _shelveRepository.Delete(id)
                ?? throw new ArgumentException("Estantería no encontrada.", nameof(id));

            return ShelveMapper.ToResponse(deleted);
        }

        public ShelveResponse GetShelveById(int id)
        {
            Shelve shelve = _shelveRepository.GetById(id)
                ?? throw new ArgumentException("Estantería no encontrada.", nameof(id));

            return ShelveMapper.ToResponse(shelve);
        }

        public List<ShelveResponse> GetAllShelves()
        {
            List<Shelve> shelves = _shelveRepository.GetAll();
            return shelves.Select(ShelveMapper.ToResponse).ToList();
        }
    }
}
