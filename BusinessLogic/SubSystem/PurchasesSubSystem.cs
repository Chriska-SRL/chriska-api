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

        public PurchaseResponse AddPurchase(AddPurchaseRequest request)
        {
            Purchase purchase = PurchaseMapper.ToDomain(request);
            purchase.Validate();

            Purchase added = _purchaseRepository.Add(purchase);
            return PurchaseMapper.ToResponse(added);
        }

        public PurchaseResponse UpdatePurchase(UpdatePurchaseRequest request)
        {
            Purchase existing = _purchaseRepository.GetById(request.Id)
                                 ?? throw new InvalidOperationException("Compra no encontrada.");

            Purchase.UpdatableData updatedData = PurchaseMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Purchase updated = _purchaseRepository.Update(existing);
            return PurchaseMapper.ToResponse(updated);
        }

        public PurchaseResponse DeletePurchase(DeletePurchaseRequest request)
        {
            Purchase deleted = _purchaseRepository.Delete(request.Id)
                                 ?? throw new InvalidOperationException("Compra no encontrada.");

            return PurchaseMapper.ToResponse(deleted);
        }

        public PurchaseResponse GetPurchaseById(int id)
        {
            Purchase purchase = _purchaseRepository.GetById(id)
                                ?? throw new InvalidOperationException("Compra no encontrada.");

            return PurchaseMapper.ToResponse(purchase);
        }

        public List<PurchaseResponse> GetAllPurchases()
        {
            List<Purchase> purchases = _purchaseRepository.GetAll();
            return purchases.Select(PurchaseMapper.ToResponse).ToList();
        }

        public PurchaseItemResponse AddPurchaseItem(AddPurchaseItemRequest request)
        {
            PurchaseItem item = PurchaseItemMapper.ToDomain(request);
            item.Validate();

            PurchaseItem added = _purchaseItemRepository.Add(item);
            return PurchaseItemMapper.ToResponse(added);
        }

        public PurchaseItemResponse UpdatePurchaseItem(UpdatePurchaseItemRequest request)
        {
            PurchaseItem existing = _purchaseItemRepository.GetById(request.Id)
                                       ?? throw new InvalidOperationException("Ítem de compra no encontrado.");

            PurchaseItem.UpdatableData updatedData = PurchaseItemMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            PurchaseItem updated = _purchaseItemRepository.Update(existing);
            return PurchaseItemMapper.ToResponse(updated);
        }

        public PurchaseItemResponse DeletePurchaseItem(DeletePurchaseItemRequest request)
        {
            PurchaseItem deleted = _purchaseItemRepository.Delete(request.Id)
                                       ?? throw new InvalidOperationException("Ítem de compra no encontrado.");

            return PurchaseItemMapper.ToResponse(deleted);
        }

        public PurchaseItemResponse GetPurchaseItemById(int id)
        {
            PurchaseItem item = _purchaseItemRepository.GetById(id)
                                    ?? throw new InvalidOperationException("Ítem de compra no encontrado.");

            return PurchaseItemMapper.ToResponse(item);
        }

        public List<PurchaseItemResponse> GetAllPurchaseItems()
        {
            List<PurchaseItem> purchaseItems = _purchaseItemRepository.GetAll();
            return purchaseItems.Select(PurchaseItemMapper.ToResponse).ToList();
        }
    }
}
