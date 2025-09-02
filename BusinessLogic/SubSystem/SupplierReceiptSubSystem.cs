using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class SupplierReceiptSubSystem
    {
        private readonly ISupplierReceiptRepository _supplierReceiptRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierBalanceItemRepository _supplierBalanceItemRepository;

        public SupplierReceiptSubSystem(ISupplierReceiptRepository receiptRepository, ISupplierRepository supplierRepository, ISupplierBalanceItemRepository supplierBalanceItemRepository)
        {
            _supplierReceiptRepository = receiptRepository;
            _supplierRepository = supplierRepository;
            _supplierBalanceItemRepository = supplierBalanceItemRepository;
        }

        public async Task<SupplierReceiptResponse> AddReceiptAsync(SupplierReceiptAddRequest request)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId)
                ?? throw new ArgumentException("No se encontró el proveedor especificado.");

            // 1) Estado de cuenta del proveedor
            List<BalanceItem> balanceItems =
                await _supplierBalanceItemRepository.GetBySupplierIdAsync(request.SupplierId);
            var accountStatement = new SupplierAccountStatement(supplier, balanceItems);

            // 2) Mapear y validar recibo
            var newReceipt = SupplierReceiptMapper.ToDomain(request, supplier);
            newReceipt.Validate();

            // 3) Persistir recibo
            var added = await _supplierReceiptRepository.AddAsync(newReceipt);

            // 4) Actualizar balance del proveedor
            var balanceItem = new ReceiptBalanceItem(added, accountStatement);
            await _supplierBalanceItemRepository.AddAsync(balanceItem);

            // 5) Respuesta
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
