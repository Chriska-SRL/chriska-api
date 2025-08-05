using BusinessLogic.Domain;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.SubSystem
{
    public class StockSubSystem
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IShelveRepository _shelveRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public StockSubSystem(
            IStockMovementRepository stockMovementRepository,
            IShelveRepository shelveRepository,
            IWarehouseRepository warehouseRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _shelveRepository = shelveRepository;
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<StockMovementResponse> AddStockMovementAsync(AddStockMovementRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<StockMovementResponse> GetStockMovementByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StockMovementResponse>> GetAllStockMovementsAsync(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StockMovementResponse>> GetAllStockMovementsByShelveAsync(int shelveId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StockMovementResponse>> GetAllStockMovementsByWarehouseAsync(int warehouseId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        private void CheckRangeDate(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
