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
        private readonly ISupplierRepository _supplierResponse;

        public PurchasesSubSystem(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository, ISupplierRepository supplierResponse)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemRepository = purchaseItemRepository;
            _supplierResponse = supplierResponse;
        }

        public PurchaseResponse AddPurchase(AddPurchaseRequest request)
        {
            Supplier supplier = _supplierResponse.GetById(request.SupplierId)
                           ?? throw new ArgumentException("Proveedor no encontrado.", nameof(request.SupplierId));

            Purchase newPurchase = PurchaseMapper.ToDomain(request);
            newPurchase.Supplier = supplier;
            newPurchase.Validate();

            Purchase added = _purchaseRepository.Add(newPurchase);
            return PurchaseMapper.ToResponse(added);
        }

        public PurchaseResponse UpdatePurchase(UpdatePurchaseRequest request)
        {
            Purchase existing = _purchaseRepository.GetById(request.Id)
                               ?? throw new InvalidOperationException("Compra no encontrada.");

            Supplier supplier = _supplierResponse.GetById(request.SupplierId)
                        ?? throw new ArgumentException("Proveedor no encontrado.", nameof(request.SupplierId));
      
            Purchase.UpdatableData updatedData = PurchaseMapper.ToUpdatableData(request);
            updatedData.Supplier = supplier;
            existing.Update(updatedData);

            Purchase updated = _purchaseRepository.Update(existing);
            return PurchaseMapper.ToResponse(updated);
        }

        public PurchaseResponse DeletePurchase(int id)
        {
            Purchase deleted = _purchaseRepository.Delete(id)
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

        public PurchaseItemResponse DeletePurchaseItem(int id)
        {
            PurchaseItem deleted = _purchaseItemRepository.Delete(id)
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
