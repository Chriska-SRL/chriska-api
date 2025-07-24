using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsSupplier;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Común;

namespace BusinessLogic.SubSystem
{
    public class SuppliersSubSystem
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersSubSystem(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<SupplierResponse> AddSupplierAsync(AddSupplierRequest request)
        {
            if (await _supplierRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un proveedor con el mismo nombre.", nameof(request.Name));

            if (await _supplierRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un proveedor con el mismo RUT.", nameof(request.RUT));

            Supplier newSupplier = SupplierMapper.ToDomain(request);
            newSupplier.Validate();

            Supplier added = await _supplierRepository.AddAsync(newSupplier);
            return SupplierMapper.ToResponse(added);
        }

        public async Task<SupplierResponse> UpdateSupplierAsync(UpdateSupplierRequest request)
        {
            var existing = await _supplierRepository.GetByIdAsync(request.Id)
                            ?? throw new ArgumentException("Proveedor no encontrado.", nameof(request.Id));

            if (existing.RUT != request.RUT && await _supplierRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un proveedor con el mismo RUT.", nameof(request.RUT));

            if (existing.Name != request.Name && await _supplierRepository.GetByNameAsync(request.Name) != null)
                throw new ArgumentException("Ya existe un proveedor con el mismo nombre.", nameof(request.Name));

            Supplier.UpdatableData data = SupplierMapper.ToUpdatableData(request);
            existing.Update(data);

            Supplier updated = await _supplierRepository.UpdateAsync(existing);
            return SupplierMapper.ToResponse(updated);
        }

        public async Task<SupplierResponse> DeleteSupplierAsync(DeleteSupplierRequest request)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id)
                           ?? throw new InvalidOperationException("Proveedor no encontrado.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            supplier.SetDeletedAudit(auditInfo);

            await _supplierRepository.DeleteAsync(supplier);
            return SupplierMapper.ToResponse(supplier);
        }
        public async Task<SupplierResponse> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id)
                           ?? throw new InvalidOperationException("Proveedor no encontrado.");

            return SupplierMapper.ToResponse(supplier);
        }

        public async Task<List<SupplierResponse>> GetAllSuppliersAsync(QueryOptions options)
        {
            var suppliers = await _supplierRepository.GetAllAsync(options);
            if (suppliers.Count == 0)
                throw new InvalidOperationException("No hay proveedores registrados.");

            return suppliers.Select(SupplierMapper.ToResponse).ToList();
        }
    }
}
