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
        private IPurchaseRepository _purchaseRepository;
        private IPurchaseItemRepository _purchaseItemRepository;
        public PurchasesSubSystem(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemRepository = purchaseItemRepository;
        }
        public void AddPurchase(Purchase purchase)
        {
            _purchaseRepository.Add(purchase);
        }

        public void AddPurchaseItem(PurchaseItem purchaseItem)
        {
            _purchaseItemRepository.Add(purchaseItem);
        }
    }
}
