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

        public void AsignShelve(Warehouse warehouse, Shelve shelve)
        {
            //Logica para asignar estante a bodega
            // warehouse.AddShelve(shelve);
            // _warehouseRepository.Update(warehouse);
        }

    }
}
