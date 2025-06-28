using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Común.Enums;

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
            IStockMovementRepository stockMovementRepository, IShelveRepository shelveRepository, IWarehouseRepository warehouseRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _shelveRepository = shelveRepository;
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public StockMovementResponse AddStockMovement(AddStockMovementRequest request)
        {
            Product product = _productRepository.GetById(request.ProductId)
                ?? throw new ArgumentException("Producto no encontrado.", nameof(request.ProductId));
            Shelve shelve = _shelveRepository.GetById(request.ShelveId)
                ?? throw new ArgumentException("Estantería no encontrada.", nameof(request.ShelveId));
            User user = _userRepository.GetById(request.UserId)
                ?? throw new ArgumentException("Usuario no encontrado.", nameof(request.UserId));
            int shelveStock = shelve.Stocks.FirstOrDefault(s => s.Product.Id == product.Id)?.Quantity ?? 0;
            if(request.Type == StockMovementType.Egreso && shelveStock < request.Quantity)
                throw new InvalidOperationException($"No hay suficiente stock en la estantería para realizar el movimiento. El stock disponible es: {shelveStock}");
            

            StockMovement newStockMovement = StockMovementMapper.ToDomain(request);
            newStockMovement.Validate();

            StockMovement added = _stockMovementRepository.Add(newStockMovement);
            return StockMovementMapper.ToResponse(added);
        }
        public StockMovementResponse GetStockMovementById(int id)
        {
            StockMovement stockMovement = _stockMovementRepository.GetById(id)
                                            ?? throw new InvalidOperationException("Movimiento de stock no encontrado.");

            return StockMovementMapper.ToResponse(stockMovement);
        }

        public List<StockMovementResponse> GetAllStockMovements(DateTime from, DateTime to)
        {
            checkRangeDate(from, to);
            List<StockMovement> stockMovements = _stockMovementRepository.GetAll(from, to);
            return stockMovements.Select(StockMovementMapper.ToResponse).ToList();
        }

        internal List<StockMovementResponse> GetAllStockMovementsByShelve(int id, DateTime from, DateTime to)
        {
            checkRangeDate(from, to);
            var shelve = _shelveRepository.GetById(id)
                         ?? throw new ArgumentException("Estantería no encontrada.", nameof(id));

            var movements = _stockMovementRepository.GetAllByShelve(id, from, to);

            return movements
                         .Select(StockMovementMapper.ToResponse)
                         .ToList();
        }

        internal List<StockMovementResponse> GetAllStockMovementsByWarehouse(int id, DateTime from, DateTime to)
        {
            checkRangeDate(from, to);
            var shelve = _warehouseRepository.GetById(id)
                         ?? throw new ArgumentException("deposito no encontrada.", nameof(id));

            var movements = _stockMovementRepository.GetAllByWarehouse(id, from, to);

            return movements
                         .Select(StockMovementMapper.ToResponse)
                         .ToList();
        }
        private void checkRangeDate(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.", nameof(from));
            }
            if (from < DateTime.MinValue || to > DateTime.MaxValue)
            {
                throw new ArgumentOutOfRangeException("Las fechas deben estar dentro de los límites permitidos.");
            }
            if(to-from > TimeSpan.FromDays(60))
            {
                throw new ArgumentException("El rango de fechas no puede ser mayor a 60 dias", nameof(from));
            }
        }
    }
}
