using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class PurchaseSubSystem
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public PurchaseSubSystem(
            IPurchaseRepository purchaseRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _purchaseRepository = purchaseRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<PurchaseResponse> AddPurchaseAsync(PurchaseAddRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.getUserId() ?? 0)
                ?? throw new ArgumentException("El usuario no existe.");

            var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId)
                ?? throw new ArgumentException("El proveedor no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");
                productItems.Add(new ProductItem(item.Quantity, item.Weight, item.UnitPrice, item.Discount, product));
            }

            var purchase = PurchaseMapper.ToDomain(request,supplier, productItems, user);
            purchase.Validate();

            var added = await _purchaseRepository.AddAsync(purchase);
            return PurchaseMapper.ToResponse(added);
        }

        public async Task<PurchaseResponse> UpdatePurchaseAsync(PurchaseUpdateRequest request)
        {
            var existing = await _purchaseRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la compra seleccionada.");

            var user = await _userRepository.GetByIdAsync(request.getUserId() ?? 0)
                ?? throw new ArgumentException("El usuario no existe.");

            var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId)
                ?? throw new ArgumentException("El proveedor no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");
                productItems.Add(new ProductItem(item.Quantity, null, item.UnitPrice, item.Discount, product));
            }

            var updatedData = PurchaseMapper.ToUpdatableData(request, productItems);
            existing.Update(updatedData);

            var updated = await _purchaseRepository.UpdateAsync(existing);
            return PurchaseMapper.ToResponse(updated);
        }

        public async Task<PurchaseResponse> DeletePurchaseAsync(DeleteRequest request)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la compra seleccionada.");
            purchase.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _purchaseRepository.DeleteAsync(purchase);
            return PurchaseMapper.ToResponse(purchase);
        }

        public async Task<PurchaseResponse> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró la compra seleccionada.");
            return PurchaseMapper.ToResponse(purchase);
        }

        public async Task<List<PurchaseResponse>> GetAllPurchasesAsync(QueryOptions options)
        {
            var purchases = await _purchaseRepository.GetAllAsync(options);
            return purchases.Select(PurchaseMapper.ToResponse).ToList();
        }
    }
}