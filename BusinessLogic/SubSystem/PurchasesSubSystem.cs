using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class PurchasesSubSystem
    {
        // Guía temporal: entidades que maneja este subsistema

        private List<Purchase> Purchases = new List<Purchase>();
        private List<PurchaseItem> PurchaseItems = new List<PurchaseItem>();

        private IPurchaseRepository _purchaseRepository;
        public PurchasesSubSystem(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }
    }
}
