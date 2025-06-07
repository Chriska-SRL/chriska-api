using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsPurchaseItem;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class PurchasesSubSystem
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseItemRepository _purchaseItemRepository;

        public PurchasesSubSystem(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemRepository = purchaseItemRepository;
        }

        public void AddPurchase(AddPurchaseRequest purchase)
        {
            Purchase newPurchase = PurchaseMapper.toDomain(purchase);
            newPurchase.Validate();
            _purchaseRepository.Add(newPurchase);
        }

        public void UpdatePurchase(UpdatePurchaseRequest purchase)
        {
            Purchase existingPurchase = _purchaseRepository.GetById(purchase.Id);
            if (existingPurchase == null) throw new Exception("No se encontro la compra");
            existingPurchase.Update(PurchaseMapper.toDomain(purchase));
            _purchaseRepository.Update(existingPurchase);
        }

        public void DeletePurchase(DeletePurchaseRequest deletePurchaseRequest)
        {
            Purchase existingPurchase = _purchaseRepository.GetById(deletePurchaseRequest.Id);
            if (existingPurchase == null) throw new Exception("No se encontro la compra");
            _purchaseRepository.Delete(deletePurchaseRequest.Id);
        }

        public PurchaseResponse GetPurchaseById(int id)
        {
            Purchase purchase = _purchaseRepository.GetById(id);
            if (purchase == null) throw new Exception("No se encontro la compra");
            PurchaseResponse purchaseResponse = PurchaseMapper.toResponse(purchase);
            return purchaseResponse;
        }

        public List<PurchaseResponse> GetAllPurchases()
        {
            List<Purchase> purchases = _purchaseRepository.GetAll();
            if(!purchases.Any()) throw new Exception("No se encontraron compras");
            return purchases.Select(PurchaseMapper.toResponse).ToList();
        }

        public void AddPurchaseItem(AddPurchaseItemRequest purchaseItem)
        {
            PurchaseItem newPurchaseItem = PurchaseItemMapper.toDomain(purchaseItem);
            newPurchaseItem.Validate();
            _purchaseItemRepository.Add(newPurchaseItem);
        }

        public void UpdatePurchaseItem(UpdatePurchaseItemRequest purchaseItem)
        {
            PurchaseItem existingPurchaseItem = _purchaseItemRepository.GetById(purchaseItem.Id);
            if (existingPurchaseItem == null) throw new Exception("No se encontro el item de compra");
            existingPurchaseItem.Update(PurchaseItemMapper.toDomain(purchaseItem));
            _purchaseItemRepository.Update(existingPurchaseItem);
        }

        public void DeletePurchaseItem(DeletePurchaseItemRequest purchaseItemRequest)
        {
            var existingPurchaseItem = _purchaseItemRepository.GetById(purchaseItemRequest.Id);
            if (existingPurchaseItem == null) throw new Exception("No se encontro el item de compra");
            _purchaseItemRepository.Delete(existingPurchaseItem.Id);
        }
        public PurchaseItemResponse GetPurchaseItemById(int id)
        {
            PurchaseItem purchaseItem = _purchaseItemRepository.GetById(id);
            if (purchaseItem == null) throw new Exception("No se encontro el item de compra");
            PurchaseItemResponse purchaseItemResponse = PurchaseItemMapper.toResponse(purchaseItem);
            return purchaseItemResponse;
        }
        public List<PurchaseItemResponse> GetAllPurchaseItems()
        {
            List<PurchaseItem> purchaseItems = _purchaseItemRepository.GetAll();
            if (!purchaseItems.Any()) throw new Exception("No se encontraron items de compra");
            return purchaseItems.Select(PurchaseItemMapper.toResponse).ToList();
        }
    } 
}
