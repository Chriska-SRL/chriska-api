using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.Common;

namespace BusinessLogic.SubSystem
{
    public class StockSubSystem
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IShelveRepository _shelveRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public StockSubSystem(
            IStockMovementRepository stockMovementRepository,
            IShelveRepository shelveRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _shelveRepository = shelveRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<StockMovementResponse> AddStockMovementAsync(AddStockMovementRequest request)
        {

            int userId = request.getUserId() ?? 0;

            var user = await _userRepository.GetByIdAsync(userId) ?? throw new InvalidOperationException("Usuario no encontrado.");
            var product = await _productRepository.GetByIdAsync(request.ProductId) ?? throw new InvalidOperationException("Producto no encontrado.");

            StockMovement stockMovement = StockMovementMapper.ToDomain(request, user, product);

            var added = await _stockMovementRepository.AddAsync(stockMovement);
            if (stockMovement.Type == Common.Enums.StockMovementType.Inbound)
            {
                product.Stock += stockMovement.Quantity;
                product.AvailableStocks += stockMovement.Quantity;
            }
            else
            {
                product.Stock -= stockMovement.Quantity;
                product.AvailableStocks -= stockMovement.Quantity;
            }
            product.AuditInfo.SetUpdated(request.getUserId(), request.AuditLocation);

            await _productRepository.UpdateAsync(product);
            added.Product = product;
            return StockMovementMapper.ToResponse(added);

        }

        public async Task<StockMovementResponse> GetStockMovementByIdAsync(int id)
        {
            var stockMovement = await _stockMovementRepository.GetByIdAsync(id)
                         ?? throw new InvalidOperationException("Movimiento no encontrado.");

            return StockMovementMapper.ToResponse(stockMovement);
        }

        public async Task<List<StockMovementResponse>> GetAllStockMovementsAsync(QueryOptions options)
        {
            QueryOptions.CheckRangeDate(options, 60); // Verifica que el rango de fechas no exceda los 60 días
            var entities = await _stockMovementRepository.GetAllAsync(options);
            return entities.Select(StockMovementMapper.ToResponse).ToList();
        }

    }
}
