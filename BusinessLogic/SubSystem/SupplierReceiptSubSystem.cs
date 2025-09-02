using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.DTOs;
using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;

namespace BusinessLogic.SubSystem
{
    public class SupplierReceiptSubSystem
    {
        private readonly ISupplierReceiptRepository _supplierReceiptRepository;
        private readonly ISupplierRepository _supplierRepository;

        public SupplierReceiptSubSystem(ISupplierReceiptRepository receiptRepository, ISupplierRepository supplierRepository)
        {
            _supplierReceiptRepository = receiptRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<SupplierReceiptResponse> AddReceiptAsync(SupplierReceiptAddRequest request)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId)
                ?? throw new ArgumentException("No se encontró el proveedor especificado.");

            var newReceipt = SupplierReceiptMapper.ToDomain(request, supplier);
            newReceipt.Validate();

            var added = await _supplierReceiptRepository.AddAsync(newReceipt);
            return SupplierReceiptMapper.ToResponse(added);
        }

        public async Task<SupplierReceiptResponse> UpdateReceiptAsync(ReceiptUpdateRequest request)
        {
            var existingReceipt = await _supplierReceiptRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            var updatedData = SupplierReceiptMapper.ToUpdatableData(request);
            existingReceipt.Update(updatedData);

            var updated = await _supplierReceiptRepository.UpdateAsync(existingReceipt);
            return SupplierReceiptMapper.ToResponse(updated);
        }

        public async Task<SupplierReceiptResponse> DeleteReceiptAsync(DeleteRequest request)
        {
            var receipt = await _supplierReceiptRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            receipt.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _supplierReceiptRepository.DeleteAsync(receipt);
            return SupplierReceiptMapper.ToResponse(receipt);
        }

        public async Task<SupplierReceiptResponse> GetReceiptByIdAsync(int id)
        {
            var receipt = await _supplierReceiptRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            return SupplierReceiptMapper.ToResponse(receipt);
        }

        public async Task<List<SupplierReceiptResponse>> GetAllReceiptsAsync(QueryOptions options)
        {
            var receipts = await _supplierReceiptRepository.GetAllAsync(options);
            return receipts.Select(SupplierReceiptMapper.ToResponse).ToList();
        }
    }
}
