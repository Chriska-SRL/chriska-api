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
            if (_supplierRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe un proveedor con el mismo nombre.", nameof(request.Name));
            if (_supplierRepository.GetByRUT(request.RUT) != null)          
                throw new ArgumentException("Ya existe un proveedor con el mismo RUT.", nameof(request.RUT));                    

            Supplier newSupplier = SupplierMapper.ToDomain(request);
            newSupplier.Validate();

            Supplier added = _supplierRepository.Add(newSupplier);
            return SupplierMapper.ToResponse(added);
        }

        public SupplierResponse UpdateSupplier(UpdateSupplierRequest request)
        {
            var existing = _supplierRepository.GetById(request.Id)
                            ?? throw new ArgumentException("Proveedor no encontrado.", nameof(request.Id));

            if (existing.RUT != request.RUT && _supplierRepository.GetByRUT(request.RUT) != null)
                throw new ArgumentException("Ya existe un proveedor con el mismo RUT.", nameof(request.RUT));

            if (existing.Name != request.Name && _supplierRepository.GetByName(request.Name) != null)
                throw new ArgumentException("Ya existe un proveedor con el mismo nombre.", nameof(request.Name));

            Supplier.UpdatableData data = SupplierMapper.ToUpdatableData(request);
            existing.Update(data);

            Supplier updated = _supplierRepository.Update(existing);
            return SupplierMapper.ToResponse(updated);
        }

        public SupplierResponse DeleteSupplier(int id)
        {
            Supplier deleted = _supplierRepository.Delete(id)
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
            if (_supplierRepository.GetAll().Count == 0)
                throw new InvalidOperationException("No hay proveedores registrados.");
            List<Supplier> suppliers = _supplierRepository.GetAll();
            return suppliers.Select(SupplierMapper.ToResponse).ToList();
        }
    }
}
