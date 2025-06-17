using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class StockSubSystem
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public StockSubSystem(
            IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        public StockMovementResponse AddStockMovement(AddStockMovementRequest request)
        {
            StockMovement newStockMovement = StockMovementMapper.ToDomain(request);
            newStockMovement.Validate();

            StockMovement added = _stockMovementRepository.Add(newStockMovement);
            return StockMovementMapper.ToResponse(added);
        }

        public StockMovementResponse UpdateStockMovement(UpdateStockMovementRequest request)
        {
            StockMovement existing = _stockMovementRepository.GetById(request.Id)
                                        ?? throw new InvalidOperationException("Movimiento de stock no encontrado.");

            StockMovement.UpdatableData updatedData = StockMovementMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            StockMovement updated = _stockMovementRepository.Update(existing);
            return StockMovementMapper.ToResponse(updated);
        }

        public StockMovementResponse DeleteStockMovement(DeleteStockMovementRequest request)
        {
            StockMovement deleted = _stockMovementRepository.Delete(request.Id)
                                        ?? throw new InvalidOperationException("Movimiento de stock no encontrado.");

            return StockMovementMapper.ToResponse(deleted);
        }

        public StockMovementResponse GetStockMovementById(int id)
        {
            StockMovement stockMovement = _stockMovementRepository.GetById(id)
                                            ?? throw new InvalidOperationException("Movimiento de stock no encontrado.");

            return StockMovementMapper.ToResponse(stockMovement);
        }

        public List<StockMovementResponse> GetAllStockMovements()
        {
            List<StockMovement> stockMovements = _stockMovementRepository.GetAll();
            return stockMovements.Select(StockMovementMapper.ToResponse).ToList();
        }
    }
}
