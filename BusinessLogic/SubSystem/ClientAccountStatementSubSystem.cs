using BusinessLogic.DTOs.DTOsAccountStatement;
using BusinessLogic.Repository;
using BusinessLogic.Domain;

namespace BusinessLogic.SubSystem
{
    public class ClientAccountStatementSubSystem
    {
        private readonly IClientBalanceItemRepository _balanceItemRepository;
        private readonly IClientRepository _clientRepository;

        public ClientAccountStatementSubSystem(
            IClientBalanceItemRepository balanceItemRepository,
            IClientRepository clientRepository)
        {
            _balanceItemRepository = balanceItemRepository;
            _clientRepository = clientRepository;
        }

        // Agrega un movimiento de balance para el cliente
        public async Task AddBalanceItemAsync(BalanceItem item)
        {
            await _balanceItemRepository.AddAsync(item);
        }

        // Obtiene el estado de cuenta de un cliente
        public async Task<ClientAccountStatementResponse?> GetAccountStatementAsync(AccountStatementRequest request)
        {
            var client = await _clientRepository.GetByIdAsync(request.EntityId);
            if (client == null) return null;

            // Obtiene los movimientos del cliente en el rango solicitado
            var items = await _balanceItemRepository.GetByClientIdAsync(request.EntityId, request.From, request.To);

            // Usa el dominio para calcular el estado de cuenta
            var statement = new ClientAccountStatement(client, items);

            // Mapea a DTO de respuesta
            return new ClientAccountStatementResponse
            {
                ClientId = client.Id,
                ClientName = client.Name,
                TotalBalance = statement.getBalance(),
                Items = statement.BalanceItems.Select(i => new AccountStatementItemResponse
                {
                    Id = i.Id,
                    Date = i.Date,
                    Description = i.Description,
                    Amount = i.Amount,
                    Balance = i.Balance,
                    DocumentType = i.DocumentType
                }).ToList()
            };
        }
    }
}