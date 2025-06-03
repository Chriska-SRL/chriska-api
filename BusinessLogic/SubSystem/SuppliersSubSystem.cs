using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsSupplier;
using System.Diagnostics;

namespace BusinessLogic.SubSystem
{
    public class SuppliersSubSystem
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersSubSystem(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public void AddSupplier(AddSupplierRequest supplier)
        {
            //var newSupplier = new Supplier(supplier.Name, supplier.RUT, supplier.BusinessName, supplier.Address, supplier.MapsAddress, supplier.Phone, supplier.ContactName, supplier.Email, supplier.BankAccount, supplier.Observation);
            //newSupplier.Validate();
            //_supplierRepository.Add(newSupplier);
        }

        public void UpdateSupplier(UpdateSupplierRequest supplier)
        {
            var existingSupplier = _supplierRepository.GetById(supplier.Id);
            if (existingSupplier == null) throw new Exception("No se encontro el proveedor");
            existingSupplier.Update(supplier.Name, supplier.RUT, supplier.BusinessName, supplier.Address, supplier.MapsAddress, supplier.Phone, supplier.ContactName, supplier.Email, supplier.BankAccount, supplier.Observation);
            existingSupplier.Validate();
            _supplierRepository.Update(existingSupplier);
        }

        public void DeleteSupplier(DeleteSupplierRequest supplier)
        {
            var existingSupplier = _supplierRepository.GetById(supplier.Id);
            if (supplier == null) throw new Exception("No se encontro el proveedor");
            _supplierRepository.Delete(existingSupplier.Id);
        }

        public SupplierResponse GetSupplierById(int id)
        {
            throw new NotImplementedException();
        } 

        public List<SupplierResponse> GetAllSupplierResponse()
        {
            throw new NotImplementedException();
        }
    }
}

