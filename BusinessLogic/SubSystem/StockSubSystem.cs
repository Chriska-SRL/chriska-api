using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.SubSystem
{
    public class StockSubSystem
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public StockSubSystem(
            IStockMovementRepository stockMovementRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<StockMovementResponse> AddStockMovementAsync(AddStockMovementRequest request)
        {
            int userId = request.getUserId() ?? 0;

            var user = await _userRepository.GetByIdAsync(userId) ?? throw new InvalidOperationException("Usuario no encontrado.");
            var product = await _productRepository.GetByIdAsync(request.ProductId) ?? throw new InvalidOperationException("Producto no encontrado.");

            StockMovement stockMovement = await AddStockMovementAsync(request.Date ?? DateTime.Now, product, request.Quantity, request.Type, RasonType.Adjustment, request.Reason, user);

            return StockMovementMapper.ToResponse(stockMovement);

        }
        public async Task<StockMovement> AddStockMovementAsync(DateTime date, Product product, decimal quantity, StockMovementType stockMovementType, RasonType rasonType, string Rason, User user)
        {
            StockMovement stockMovement = new StockMovement(date, quantity, stockMovementType, rasonType, Rason, user, product);
            stockMovement.AuditInfo.SetCreated(user.Id, null);
            var added = await _stockMovementRepository.AddAsync(stockMovement);
            if (stockMovement.Type == Common.Enums.StockMovementType.Inbound)
            {
                await _productRepository.UpdateStockAsync(product.Id, quantity, quantity);
            }
            else
            {
                if(rasonType == RasonType.Adjustment) 
                    await _productRepository.UpdateStockAsync(product.Id, -quantity, -quantity);
                else
                    await _productRepository.UpdateStockAsync(product.Id, -quantity, 0);
               
            }

            return added;
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
