using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class SuppliersSubSystem
    {
        // Guía temporal: entidades que maneja este subsistema

        private List<Supplier> Suppliers = new List<Supplier>();

        private ISupplierRepository _supplierRepository;
        public SuppliersSubSystem(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public void AddSupplier(Supplier supplier)
        {
            _supplierRepository.Add(supplier);
        }

    }
}
