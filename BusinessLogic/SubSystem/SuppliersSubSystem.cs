using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsSupplier;
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

        public SupplierResponse AddSupplier(AddSupplierRequest request)
        {
            Supplier newSupplier = SupplierMapper.ToDomain(request);
            newSupplier.Validate();

            Supplier added = _supplierRepository.Add(newSupplier);
            return SupplierMapper.ToResponse(added);
        }

        public SupplierResponse UpdateSupplier(UpdateSupplierRequest request)
        {
            Supplier existing = _supplierRepository.GetById(request.Id)
                                ?? throw new InvalidOperationException("Proveedor no encontrado.");

            Supplier.UpdatableData updatedData = SupplierMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Supplier updated = _supplierRepository.Update(existing);
            return SupplierMapper.ToResponse(updated);
        }

        public SupplierResponse DeleteSupplier(DeleteSupplierRequest request)
        {
            Supplier deleted = _supplierRepository.Delete(request.Id)
                                ?? throw new InvalidOperationException("Proveedor no encontrado.");

            return SupplierMapper.ToResponse(deleted);
        }

        public SupplierResponse GetSupplierById(int id)
        {
            Supplier supplier = _supplierRepository.GetById(id)
                               ?? throw new InvalidOperationException("Proveedor no encontrado.");

            return SupplierMapper.ToResponse(supplier);
        }

        public List<SupplierResponse> GetAllSupplierResponse()
        {
            List<Supplier> suppliers = _supplierRepository.GetAll();
            return suppliers.Select(SupplierMapper.ToResponse).ToList();
        }
    }
}
