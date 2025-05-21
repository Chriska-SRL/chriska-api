using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsPurchaseItem;
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
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseItemRepository _purchaseItemRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly SuppliersSubSystem _suppliersSubSystem;
        private readonly IProductRepository _productRepository;
        public PurchasesSubSystem(IPurchaseRepository purchaseRepository, IPurchaseItemRepository purchaseItemRepository, SuppliersSubSystem suppliersSubSystem, ISupplierRepository supplierRepository, IProductRepository productRepository)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemRepository = purchaseItemRepository;
            _suppliersSubSystem = suppliersSubSystem;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }
        public void AddPurchase(AddPurchaseRequest purchase)
        {
            var newPurchase = new Purchase(purchase.Date, purchase.Status, _supplierRepository.GetById(purchase.SupplierId));
            newPurchase.Validate();
            _purchaseRepository.Add(newPurchase);
        }

        public void UpdatePurchase(UpdatePurchaseRequest purchase)
        {
            var existingPurchase = _purchaseRepository.GetById(purchase.Id);
            if (existingPurchase == null) throw new Exception("No se encontro la compra");

            existingPurchase.Update(purchase.Date, purchase.Status, _supplierRepository.GetById(purchase.SupplierId));
            _purchaseRepository.Update(existingPurchase);

        }
        public void DeletePurchase(DeletePurchaseRequest deletePurchaseRequest)
        {
            var existingPurchase = _purchaseRepository.GetById(deletePurchaseRequest.Id);
            if (existingPurchase == null) throw new Exception("No se encontro la compra");

            _purchaseRepository.Delete(deletePurchaseRequest.Id);

        }
        public PurchaseResponse GetPurchaseById(int id)
        {
            var purchase = _purchaseRepository.GetById(id);
            if (purchase == null) throw new Exception("No se encontro la compra");
            var purchaseResponse = new PurchaseResponse
            {
                Date = purchase.Date,
                Status = purchase.Status,
                Supplier = _suppliersSubSystem.GetSupplierById(purchase.Supplier.Id),
            };
            return purchaseResponse;
        }
        public List<PurchaseResponse> GetAllPurchases()
        {
            var purchases = _purchaseRepository.GetAll();
            var purchaseResponses = new List<PurchaseResponse>();
            foreach (var purchase in purchases)
            {
                purchaseResponses.Add(new PurchaseResponse
                {
                    Date = purchase.Date,
                    Status = purchase.Status,
                    Supplier = _suppliersSubSystem.GetSupplierById(purchase.Supplier.Id),
                });
            }
            return purchaseResponses;
        }

        public void AddPurchaseItem(AddPurchaseItemRequest purchaseItem)
        {
            var newPurchaseItem = new PurchaseItem(purchaseItem.Quantity, purchaseItem.UnitPrice, _productRepository.GetById(purchaseItem.ProductId));
            newPurchaseItem.Validate();
            _purchaseItemRepository.Add(newPurchaseItem);
        }
        public void UpdatePurchaseItem(UpdatePurchaseItemRequest purchaseItem)
        {
            var existingPurchaseItem = _purchaseItemRepository.GetById(purchaseItem.Id);
            if (existingPurchaseItem == null) throw new Exception("No se encontro el item de compra");

            existingPurchaseItem.Update(purchaseItem.Quantity, purchaseItem.UnitPrice);
            _purchaseItemRepository.Update(existingPurchaseItem);

        }
        public void DeletePurchaseItem(DeletePurchaseItemRequest purchaseItemRequest)
        {

            var existingPurchaseItem = _purchaseItemRepository.GetById(purchaseItemRequest.Id);
            if (existingPurchaseItem == null) throw new Exception("No se encontro el item de compra");

            _purchaseItemRepository.Delete(existingPurchaseItem.Id);



        }

    } 
}
