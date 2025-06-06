﻿using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsWarehouse;

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
            throw new NotImplementedException("Metodo no implementado");
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

        //public void AddShelve(Shelve shelve)
        //{
        //    _shelveRepository.Add(shelve);
        //}
    }
}
