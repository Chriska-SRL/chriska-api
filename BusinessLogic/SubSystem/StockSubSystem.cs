using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class StockSubSystem
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShelveRepository _shelveRespository;
        private readonly IProductRepository _productRepository;

        public StockSubSystem(IStockMovementRepository stockMovementRepository, IUserRepository userRepository, IShelveRepository shelveRespository, IProductRepository productRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _userRepository = userRepository;
            _shelveRespository = shelveRespository;
            _productRepository = productRepository;
        }

        public void AddStockMovement(AddStockMovementRequest stockMovement)
        {
            StockMovement newStockMovement = StockMovementMapper.ToDomain(stockMovement);
            newStockMovement.Validate();
            _stockMovementRepository.Add(newStockMovement);
        }
        public void UpdateStockMovement(UpdateStockMovementRequest stockMovement)
        {
            StockMovement existingStockMovement = _stockMovementRepository.GetById(stockMovement.Id);
            if (existingStockMovement == null) throw new Exception("Movimiento de stock no encontrado");
            existingStockMovement.Update(StockMovementMapper.ToDomain(stockMovement));
            _stockMovementRepository.Update(existingStockMovement);
        }
        public void DeleteStockMovement(DeleteStockMovementRequest stockMovement)
        {
            StockMovement existingStockMovement = _stockMovementRepository.GetById(stockMovement.Id);
            if (existingStockMovement == null) throw new Exception("Movimiento de stock no encontrado");
            _stockMovementRepository.Delete(existingStockMovement.Id);
        }
        public StockMovementResponse GetStockMovementById(int id)
        {
            StockMovement stockMovement = _stockMovementRepository.GetById(id);
            if (stockMovement == null)
                throw new Exception("Movimiento de stock no encontrado");
            return StockMovementMapper.ToResponse(stockMovement);
        }

        public List<StockMovementResponse> GetAllStockMovements()
        {
            List<StockMovement> stockMovements = _stockMovementRepository.GetAll();
            if (stockMovements == null || !stockMovements.Any())
                throw new Exception("No hay movimientos de stock disponibles");
            return stockMovements.Select(StockMovementMapper.ToResponse).ToList();
        }
    }
}
