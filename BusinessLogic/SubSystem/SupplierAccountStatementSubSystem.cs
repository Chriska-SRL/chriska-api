using BusinessLogic.DTOs.DTOsAccountStatement;
using BusinessLogic.Repository;
using BusinessLogic.Domain;

namespace BusinessLogic.SubSystem
{
    public class SupplierAccountStatementSubSystem
    {
        private readonly ISupplierBalanceItemRepository _balanceItemRepository;
        private readonly ISupplierRepository _supplierRepository;

        public SupplierAccountStatementSubSystem(
            ISupplierBalanceItemRepository balanceItemRepository,
            ISupplierRepository supplierRepository)
        {
            _balanceItemRepository = balanceItemRepository;
            _supplierRepository = supplierRepository;
        }

        // Agrega un movimiento de balance para el proveedor
        public async Task AddBalanceItemAsync(BalanceItem item)
        {
            await _balanceItemRepository.AddAsync(item);
        }

        // Obtiene el estado de cuenta de un proveedor
        public async Task<SupplierAccountStatementResponse?> GetAccountStatementAsync(AccountStatementRequest request)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.EntityId);
            if (supplier == null) return null;

            // Obtiene los movimientos del proveedor en el rango solicitado
            var items = await _balanceItemRepository.GetBySupplierIdAsync(request.EntityId, request.From, request.To);

            // Usa el dominio para calcular el estado de cuenta
            var statement = new SupplierAccountStatement(supplier, items);

            // Mapea a DTO de respuesta
            return new SupplierAccountStatementResponse
            {
                SupplierId = supplier.Id,
                SupplierName = supplier.Name,
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