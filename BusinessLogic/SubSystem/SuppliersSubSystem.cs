using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsSupplier;
using BusinessLogic.Repository;

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
            var newSupplier = SupplierMapper.ToDomain(request);
            newSupplier.Validate();

            if (await _supplierRepository.GetByRUTAsync(newSupplier.RUT) != null)
                throw new ArgumentException("Ya existe un proveedor con ese RUT.");

            if (await _supplierRepository.GetByNameAsync(newSupplier.Name) != null)
                throw new ArgumentException("Ya existe un proveedor con ese nombre.");

            if (await _supplierRepository.GetByRazonSocialAsync(newSupplier.RazonSocial) != null)
                throw new ArgumentException("Ya existe un proveedor con esa razón social.");

            Supplier added = await _supplierRepository.AddAsync(newSupplier);
            return SupplierMapper.ToResponse(added);
        }

        public async Task<SupplierResponse> UpdateSupplierAsync(UpdateSupplierRequest request)
        {
            var existingSupplier = await _supplierRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el proveedor seleccionado.");

            if (existingSupplier.RUT != request.RUT && await _supplierRepository.GetByRUTAsync(request.RUT) != null)
                throw new ArgumentException("Ya existe un proveedor con ese RUT.");

            var updatedData = SupplierMapper.ToUpdatableData(request);
            existingSupplier.Update(updatedData);

            var updated = await _supplierRepository.UpdateAsync(existingSupplier);
            return SupplierMapper.ToResponse(updated);
        }

        public async Task DeleteSupplierAsync(DeleteRequest request)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el proveedor seleccionado.");

            supplier.MarkAsDeleted(request.getUserId(), request.Location);
            await _supplierRepository.DeleteAsync(supplier);
        }

        public async Task<SupplierResponse> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el proveedor seleccionado.");

            return SupplierMapper.ToResponse(supplier);
        }

        public async Task<List<SupplierResponse>> GetAllSuppliersAsync(QueryOptions options)
        {
            var suppliers = await _supplierRepository.GetAllAsync(options);
            return suppliers.Select(SupplierMapper.ToResponse).ToList();
        }
    }
}
