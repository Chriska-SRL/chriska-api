using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsWarehouse;
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
            var newWarehouse = new Warehouse(warehouse.Name, warehouse.Description, warehouse.Address);
            newWarehouse.Validate();
            _warehouseRepository.Add(newWarehouse);
        }

        public void UpdateWarehouse(UpdateWarehouseRequest warehouse)
        {
            var existingWarehouse = _warehouseRepository.GetById(warehouse.Id);
            if (existingWarehouse == null) throw new Exception("No se encontro el almacen");
            existingWarehouse.Update(warehouse.Name, warehouse.Description, warehouse.Address);
            existingWarehouse.Validate();
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
            var warehouse = _warehouseRepository.GetById(id);
            if (warehouse == null) throw new Exception("No se encontro el almacen");
            var warehouseResponse = new WarehouseResponse
            {
                Name = warehouse.Name,
                Description = warehouse.Description,
                Address = warehouse.Address
            };
            return warehouseResponse;
        }

        public List<WarehouseResponse> GetAllWarehouses()
        {
            var warehouses = _warehouseRepository.GetAll();
            if (warehouses == null || !warehouses.Any()) throw new Exception("No hay almacenes registrados");
            return warehouses.Select(w => new WarehouseResponse
            {
                Name = w.Name,
                Description = w.Description,
                Address = w.Address
            }).ToList();
        }

        public void AddShelve(AddShelveRequest shelveRequest)
        {
            var newShelve = new Shelve(shelveRequest.Description, _warehouseRepository.GetById(shelveRequest.WarehouseId));
            newShelve.Validate();
            _shelveRepository.Add(newShelve);
        }
        public void UpdateShelve(UpdateShelveRequest shelveRequest)
        {
            var existingShelve = _shelveRepository.GetById(shelveRequest.Id);
            if (existingShelve == null) throw new Exception("No se encontro la estanteria");
            existingShelve.Update(shelveRequest.Description, _warehouseRepository.GetById(shelveRequest.WarehouseId));
            existingShelve.Validate();
            _shelveRepository.Update(existingShelve);
        }
        public void DeleteShelve(DeleteShelveRequest shelveRequest)
        {
            var existingShelve = _shelveRepository.GetById(shelveRequest.Id);
            if (existingShelve == null) throw new Exception("No se encontro la estanteria");
            _shelveRepository.Delete(existingShelve.Id);
        }
        public ShelveResponse GetShelveById(int id)
        {
            var shelve = _shelveRepository.GetById(id);
            if (shelve == null) throw new Exception("No se encontro la estanteria");
            var shelveResponse = new ShelveResponse
            {
                Description = shelve.Description,
                Warehouse = new WarehouseResponse
                {
                    Name = shelve.Warehouse.Name,
                    Description = shelve.Warehouse.Description,
                    Address = shelve.Warehouse.Address
                }
            };
            return shelveResponse;
        }


    }
}
