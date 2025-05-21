using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSupplier;
using BusinessLogic.DTOsCategory;
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


        private readonly ISupplierRepository _supplierRepository;
        public SuppliersSubSystem(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public void AddSupplier(AddSupplierRequest supplier)
        {
            var newSupplier = new Supplier(supplier.Name, supplier.RUT, supplier.BusinessName, supplier.Address, supplier.MapsAddress, supplier.Phone, supplier.ContactName, supplier.Email, supplier.BankAccount, supplier.Observation);
            newSupplier.Validate();
            _supplierRepository.Add(newSupplier);
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

            var supplier = _supplierRepository.GetById(id);
            if (supplier == null) throw new Exception("No se encontro el proveedor");
            var supplierResponse = new SupplierResponse
            {

                Name = supplier.Name,
                RUT = supplier.RUT,
                BusinessName = supplier.BusinessName,
                Address = supplier.Address,
                MapsAddress = supplier.MapsAddress,
                Phone = supplier.Phone,
                ContactName = supplier.ContactName,
                Email = supplier.Email,
                BankAccount = supplier.BankAccount,
                Observation = supplier.Observation

            };
            return supplierResponse;
        } 
        public List<SupplierResponse> GetAllSupplierResponse()
        {
            var suppliers = _supplierRepository.GetAll();
            if (suppliers == null || suppliers.Count == 0) throw new Exception("No se encontraron proveedores");
            var supplierResponses = suppliers.Select(s => new SupplierResponse
            {
                Name = s.Name,
                RUT = s.RUT,
                BusinessName = s.BusinessName,
                Address = s.Address,
                MapsAddress = s.MapsAddress,
                Phone = s.Phone,
                ContactName = s.ContactName,
                Email = s.Email,
                BankAccount = s.BankAccount,
                Observation = s.Observation
            }).ToList();
            return supplierResponses;
        }

    }
}

