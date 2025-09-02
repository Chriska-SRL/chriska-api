using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.DTOs;
using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;

namespace BusinessLogic.SubSystem
{
    public class ClientReceiptSubSystem
    {
        private readonly IClientReceiptRepository _clientReceiptRepository;
        private readonly IClientRepository _clientRepository;

        public ClientReceiptSubSystem(IClientReceiptRepository receiptRepository, IClientRepository clientRepository)
        {
            _clientReceiptRepository = receiptRepository;
            _clientRepository = clientRepository;
        }

        public async Task<ClientReceiptResponse> AddReceiptAsync(ClientReceiptAddRequest request)
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId)
                ?? throw new ArgumentException("No se encontró el cliente especificado.");

            var newReceipt = ClientReceiptMapper.ToDomain(request, client);
            newReceipt.Validate();

            var added = await _clientReceiptRepository.AddAsync(newReceipt);
            return ClientReceiptMapper.ToResponse(added);
        }

        public async Task<ClientReceiptResponse> UpdateReceiptAsync(ReceiptUpdateRequest request)
        {
            var existingReceipt = await _clientReceiptRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            var updatedData = ClientReceiptMapper.ToUpdatableData(request);
            existingReceipt.Update(updatedData);

            var updated = await _clientReceiptRepository.UpdateAsync(existingReceipt);
            return ClientReceiptMapper.ToResponse(updated);
        }

        public async Task<ClientReceiptResponse> DeleteReceiptAsync(DeleteRequest request)
        {
            var receipt = await _clientReceiptRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            receipt.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _clientReceiptRepository.DeleteAsync(receipt);
            return ClientReceiptMapper.ToResponse(receipt);
        }

        public async Task<ClientReceiptResponse> GetReceiptByIdAsync(int id)
        {
            var receipt = await _clientReceiptRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el recibo seleccionado.");

            return ClientReceiptMapper.ToResponse(receipt);
        }

        public async Task<List<ClientReceiptResponse>> GetAllReceiptsAsync(QueryOptions options)
        {
            var receipts = await _clientReceiptRepository.GetAllAsync(options);
            return receipts.Select(ClientReceiptMapper.ToResponse).ToList();
        }
    }
}
