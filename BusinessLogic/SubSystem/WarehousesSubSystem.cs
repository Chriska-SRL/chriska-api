using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsShelve;

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

        public WarehouseResponse AddWarehouse(AddWarehouseRequest request)
        {
            Warehouse newWarehouse = WarehouseMapper.ToDomain(request);
            newWarehouse.Validate();

            Warehouse added = _warehouseRepository.Add(newWarehouse);
            return WarehouseMapper.ToResponse(added);
        }

        public WarehouseResponse UpdateWarehouse(UpdateWarehouseRequest request)
        {
            Warehouse existing = _warehouseRepository.GetById(request.Id)
                                ?? throw new InvalidOperationException("Almacén no encontrado.");

            Warehouse.UpdatableData updatedData = WarehouseMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Warehouse updated = _warehouseRepository.Update(existing);
            return WarehouseMapper.ToResponse(updated);
        }

        public WarehouseResponse DeleteWarehouse(DeleteWarehouseRequest request)
        {
            Warehouse deleted = _warehouseRepository.Delete(request.Id)
                               ?? throw new InvalidOperationException("Almacén no encontrado.");

            return WarehouseMapper.ToResponse(deleted);
        }

        public WarehouseResponse GetWarehouseById(int id)
        {
            Warehouse warehouse = _warehouseRepository.GetById(id)
                                 ?? throw new InvalidOperationException("Almacén no encontrado.");

            return WarehouseMapper.ToResponse(warehouse);
        }

        public List<WarehouseResponse> GetAllWarehouses()
        {
            List<Warehouse> warehouses = _warehouseRepository.GetAll();
            return warehouses.Select(WarehouseMapper.ToResponse).ToList();
        }

        public ShelveResponse AddShelve(AddShelveRequest request)
        {
            Shelve newShelve = ShelveMapper.ToDomain(request);
            newShelve.Validate();

            Shelve added = _shelveRepository.Add(newShelve);
            return ShelveMapper.ToResponse(added);
        }

        public ShelveResponse UpdateShelve(UpdateShelveRequest request)
        {
            Shelve existing = _shelveRepository.GetById(request.Id)
                              ?? throw new InvalidOperationException("Estantería no encontrada.");

            Shelve.UpdatableData updatedData = ShelveMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Shelve updated = _shelveRepository.Update(existing);
            return ShelveMapper.ToResponse(updated);
        }

        public ShelveResponse DeleteShelve(DeleteShelveRequest request)
        {
            Shelve deleted = _shelveRepository.Delete(request.Id)
                              ?? throw new InvalidOperationException("Estantería no encontrada.");

            return ShelveMapper.ToResponse(deleted);
        }

        public ShelveResponse GetShelveById(int id)
        {
            Shelve shelve = _shelveRepository.GetById(id)
                             ?? throw new InvalidOperationException("Estantería no encontrada.");

            return ShelveMapper.ToResponse(shelve);
        }

        public List<ShelveResponse> GetAllShelves()
        {
            List<Shelve> shelves = _shelveRepository.GetAll();
            return shelves.Select(ShelveMapper.ToResponse).ToList();
        }
    }
}
