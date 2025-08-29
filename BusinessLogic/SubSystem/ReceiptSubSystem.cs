using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.DTOs;
using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;

namespace BusinessLogic.SubSystem
{
    public class ReceiptSubSystem
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IClientRepository _clientRepository;

        public ReceiptSubSystem(IReceiptRepository receiptRepository, IClientRepository clientRepository)
        {
            _receiptRepository = receiptRepository;
            _clientRepository = clientRepository;
        }

        public async Task<ReceiptResponse> AddReceiptAsync(ReceiptAddRequest request)
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId)
                ?? throw new ArgumentException("No se encontró el cliente especificado.");

            var newReceipt = ReceiptMapper.ToDomain(request, client);
            newReceipt.Validate();

            var added = await _receiptRepository.AddAsync(newReceipt);
            return ReceiptMapper.ToResponse(added);
        }

        public async Task<ReceiptResponse> UpdateReceiptAsync(ReceiptUpdateRequest request)
        {
            var existingReceipt = await _receiptRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            var updatedData = ReceiptMapper.ToUpdatableData(request);
            existingReceipt.Update(updatedData);

            var updated = await _receiptRepository.UpdateAsync(existingReceipt);
            return ReceiptMapper.ToResponse(updated);
        }

        public async Task<ReceiptResponse> DeleteReceiptAsync(DeleteRequest request)
        {
            var receipt = await _receiptRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            receipt.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _receiptRepository.DeleteAsync(receipt);
            return ReceiptMapper.ToResponse(receipt);
        }

        public async Task<ReceiptResponse> GetReceiptByIdAsync(int id)
        {
            var receipt = await _receiptRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            return ReceiptMapper.ToResponse(receipt);
        }

        public async Task<List<ReceiptResponse>> GetAllReceiptsAsync(QueryOptions options)
        {
            var receipts = await _receiptRepository.GetAllAsync(options);
            return receipts.Select(ReceiptMapper.ToResponse).ToList();
        }
    }
}
