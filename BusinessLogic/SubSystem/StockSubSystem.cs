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

        private List<StockMovement> Movements = new List<StockMovement>();

        private IStockMovementRepository _stockMovementRepository;
        
        public StockSubSystem(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }
        public void AddStockMovement(StockMovement stockMovement)
        {
            _stockMovementRepository.Add(stockMovement);
        }
        //Fabriccio: Preguntar por si el stock subsistema tambien tienen el repositorio de stock de producto.


    }
}
