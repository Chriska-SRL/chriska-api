using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsSupplier;
using System.Diagnostics;
using BusinessLogic.Común.Mappers;

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
            Supplier newSupplier= SupplierMapper.ToDomain(supplier);
            newSupplier.Validate();
            _supplierRepository.Add(newSupplier);
        }

        public void UpdateSupplier(UpdateSupplierRequest supplier)
        {
            Supplier existingSupplier = _supplierRepository.GetById(supplier.Id);
            if (existingSupplier == null) throw new Exception("No se encontro el proveedor");
            existingSupplier.Update(SupplierMapper.ToDomain(supplier));
            _supplierRepository.Update(existingSupplier);
        }

        public void DeleteSupplier(DeleteSupplierRequest supplier)
        {
            Supplier existingSupplier = _supplierRepository.GetById(supplier.Id);
            if (supplier == null) throw new Exception("No se encontro el proveedor");
            _supplierRepository.Delete(existingSupplier.Id);
        }

        public SupplierResponse GetSupplierById(int id)
        {
            Supplier supplier = _supplierRepository.GetById(id);
            if (supplier == null) throw new Exception("No se encontro el proveedor");
            return SupplierMapper.ToResponse(supplier);
        } 

        public List<SupplierResponse> GetAllSupplierResponse()
        {
            List<Supplier> suppliers = _supplierRepository.GetAll();
            if (!suppliers.Any()) throw new Exception("No se encontraron proveedores");
            return suppliers.Select(SupplierMapper.ToResponse).ToList();
        }
    }
}

