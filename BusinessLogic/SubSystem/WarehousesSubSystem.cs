using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsShelve;

namespace BusinessLogic.SubSystem
{
    public class WarehousesSubSystem
    {
        private IWarehouseRepository _warehouseRepository;
        private IShelveRepository _shelveRepository;

        public WarehousesSubSystem(IWarehouseRepository warehouseRepository, IShelveRepository shelveRepository)
        {
            _warehouseRepository = warehouseRepository;
            _shelveRepository = shelveRepository;
        }

        public void AddWarehouse(AddWarehouseRequest warehouse)
        {
            Warehouse newWareHouse= WarehouseMapper.toDomain(warehouse);
            newWareHouse.Validate();
            _warehouseRepository.Add(newWareHouse);
        }

        public void UpdateWarehouse(UpdateWarehouseRequest warehouse)
        {
            var existingWarehouse = _warehouseRepository.GetById(warehouse.Id);
            if (existingWarehouse == null) throw new Exception("No se encontro el almacen");
            existingWarehouse.Update(WarehouseMapper.toDomain(warehouse));
            _warehouseRepository.Update(existingWarehouse);
        }

        public void DeleteWarehouse(DeleteWarehouseRequest warehouse)
        {
            var existingWarehouse = _warehouseRepository.GetById(warehouse.Id);
            if (existingWarehouse == null) throw new Exception("No se encontro el almacen");
            _warehouseRepository.Delete(existingWarehouse.Id);
        }

        public WarehouseResponse GetWarehouseById(int id)
        {
            Warehouse warehouse = _warehouseRepository.GetById(id);
            if (warehouse == null) throw new Exception("No se encontro el almacen");
            return WarehouseMapper.toResponse(warehouse);
        }

        public List<WarehouseResponse> GetAllWarehouses()
        {
            List<Warehouse> warehouses = _warehouseRepository.GetAll();
            if (warehouses == null || warehouses.Count == 0) throw new Exception("No se encontraron almacenes");
            return warehouses.Select(WarehouseMapper.toResponse).ToList();
        }

        public void AddShelve(AddShelveRequest addShelveRequest)
        {
            Shelve newShelve= ShelveMapper.toDomain(addShelveRequest);
            newShelve.Validate();
            _shelveRepository.Add(newShelve);
        }
        public void UpdateShelve(UpdateShelveRequest updateShelveRequest)
        {
            Shelve existingShelve = _shelveRepository.GetById(updateShelveRequest.Id);
            if (existingShelve == null) throw new Exception("No se encontro la estanteria");
            existingShelve.Update(ShelveMapper.toDomain(updateShelveRequest));
            _shelveRepository.Update(existingShelve);
        }
        public void DeleteShelve(DeleteShelveRequest deleteShelveRequest)
        {
            Shelve existingShelve = _shelveRepository.GetById(deleteShelveRequest.Id);
            if (existingShelve == null) throw new Exception("No se encontro la estanteria");
            _shelveRepository.Delete(existingShelve.Id);
        }
        public ShelveResponse GetShelveById(int id)
        {
            Shelve shelve = _shelveRepository.GetById(id);
            if (shelve == null) throw new Exception("No se encontro la estanteria");
            return ShelveMapper.toResponse(shelve);
        }
        public List<ShelveResponse> GetAllShelves()
        {
            List<Shelve> shelves = _shelveRepository.GetAll();
            if (shelves == null || shelves.Count == 0) throw new Exception("No se encontraron estanterias");
            return shelves.Select(ShelveMapper.toResponse).ToList();
        }
    }
}
