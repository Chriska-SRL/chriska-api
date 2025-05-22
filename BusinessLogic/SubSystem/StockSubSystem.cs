using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class StockSubSystem
    {
        // Guía temporal: entidades que maneja este subsistema

        private readonly List<StockMovement> Movements = new List<StockMovement>();

        private readonly IStockMovementRepository _stockMovementRepository;
        
        public StockSubSystem(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }
        public void AddStockMovement(StockMovement stockMovement)
        {
            _stockMovementRepository.Add(stockMovement);
        }

      

    }
}
