using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsStockMovement;

namespace BusinessLogic.SubSystem
{
    public class StockSubSystem
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShelveRepository _shelveRespository;
        private readonly IProductRepository _productRepository;

        private readonly UserSubSystem _userSubSystem;
        private readonly ProductsSubSystem _productsSubSystem;

        public StockSubSystem(IStockMovementRepository stockMovementRepository, IUserRepository userRepository, IShelveRepository shelveRespository, IProductRepository productRepository, UserSubSystem userSubSystem, ProductsSubSystem productsSubSystem)
        {
            _stockMovementRepository = stockMovementRepository;
            _userRepository = userRepository;
            _shelveRespository = shelveRespository;
            _productRepository = productRepository;

            _userSubSystem = userSubSystem;
            _productsSubSystem = productsSubSystem;
        }

        public void AddStockMovement(AddStockMovementRequest stockMovement)
        {
            var newStockMovement = new StockMovement(stockMovement.Date, stockMovement.Quantity, stockMovement.Type, stockMovement.Reason, _shelveRespository.GetById(stockMovement.ShelveId), _userRepository.GetById(stockMovement.UserId), _productRepository.GetById(stockMovement.ProductId));
            newStockMovement.Validate();
            _stockMovementRepository.Add(newStockMovement);
        }

        public StockMovementResponse GetStockMovementById(int id)
        {
            var stockMovement = _stockMovementRepository.GetById(id);
            if (stockMovement == null) throw new Exception("Movimiento de stock no encontrado");

            var stockMovementResponse= new StockMovementResponse
            {
                Date = stockMovement.Date,
                Quantity = stockMovement.Quantity,
                Type = stockMovement.Type,
                Reason = stockMovement.Reason,
                Shelve = new ShelveResponse
                {
                    Description = stockMovement.Shelve.Description
                },
                User = _userSubSystem.GetUserById(stockMovement.User.Id),
                Product = _productsSubSystem.GetProductById(stockMovement.Product.Id)
            };
            return stockMovementResponse;
        }

        public List<StockMovementResponse> GetAllStockMovements()
        {
            var stockMovements = _stockMovementRepository.GetAll();
            if (stockMovements == null) throw new Exception("No hay movimientos de stock registrados");
            var stockMovementResponses = new List<StockMovementResponse>();
            foreach (var stockMovement in stockMovements)
            {
                var stockMovementResponse = new StockMovementResponse
                {
                    Date = stockMovement.Date,
                    Quantity = stockMovement.Quantity,
                    Type = stockMovement.Type,
                    Reason = stockMovement.Reason,
                    Shelve = new ShelveResponse
                    {
                        Description = stockMovement.Shelve.Description
                    },
                    User = _userSubSystem.GetUserById(stockMovement.User.Id),
                    Product = _productsSubSystem.GetProductById(stockMovement.Product.Id)
                };
                stockMovementResponses.Add(stockMovementResponse);
            }
            return stockMovementResponses;
        }
    }
}
