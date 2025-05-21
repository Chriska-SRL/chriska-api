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
        private IShelveRepository _shelveRepository;
        public WarehousesSubSystem(IWarehouseRepository warehouseRepository, IShelveRepository shelveRepository)
        {
            _warehouseRepository = warehouseRepository;
            _shelveRepository = shelveRepository;
        }
        public void AddWarehouse(Warehouse warehouse)
        {
            _warehouseRepository.Add(warehouse);
        }
        public void AddShelve(Shelve shelve)
        {
            _shelveRepository.Add(shelve);
        }

    }
}
