using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class WarehousesSubSystem
    {
        // Guía temporal: entidades que maneja este subsistema

        private List<Warehouse> Warehouses = new List<Warehouse>();

        private List<Shelve> Shelves = new List<Shelve>();

        private List<ProductStock> WarehouseStocks = new List<ProductStock>();

        private IWarehouseRepository _warehouseRepository;
        public WarehousesSubSystem(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }
        public void AddWarehouse(Warehouse warehouse)
        {
            _warehouseRepository.Add(warehouse);
        }

    }
}
