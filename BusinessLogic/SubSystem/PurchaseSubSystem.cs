using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsOrderRequest;
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
        private readonly StockSubSystem _stockSubSystem;

        public PurchaseSubSystem(
            IPurchaseRepository purchaseRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            StockSubSystem stockSubSystem)
        {
            _purchaseRepository = purchaseRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _stockSubSystem = stockSubSystem;
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

            int supplierId = request.SupplierId ?? existing.Supplier.Id;

            var supplier = await _supplierRepository.GetByIdAsync(supplierId)
                ?? throw new ArgumentException("El proveedor no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"El producto con ID {item.ProductId} no existe.");
                productItems.Add(new ProductItem(item.Quantity, null, item.UnitPrice, item.Discount, product));
            }

            var updatedData = PurchaseMapper.ToUpdatableData(request, productItems, supplier);
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

        internal async Task<PurchaseResponse?> ChangeStatusPurchaseAsync(int id, DocumentClientChangeStatusRequest request)
        {
            Purchase purchase = await _purchaseRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una compra con el ID {id}.");

            if (purchase.Status == Status.Cancelled)
                throw new ArgumentException("La solicitud de pedido no se puede modificar porque ya esta cancelada.");

            int userId = request.getUserId() ?? 0;
            User? user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza el cambio de estado no existe.");
            purchase.AuditInfo.SetUpdated(userId, request.AuditLocation);
            purchase.User = user;

            if (purchase.Status == Status.Pending)
            {
                if (request.Status == Status.Confirmed)
                {
                    purchase.Confirm();
                    foreach (var item in purchase.ProductItems)
                    {
                        await _stockSubSystem.AddStockMovementAsync(DateTime.Now, item.Product, item.Quantity, StockMovementType.Inbound, ReasonType.Purchase, $"Compra por factura {purchase.InvoiceNumber}", user);
                    }
                }
                else if (request.Status == Status.Cancelled)
                {
                    purchase.Cancel();
                }
                else
                {
                    throw new ArgumentException("El estado proporcionado no es válido para la operación.");
                }
            }
            else if(purchase.Status == Status.Confirmed)
            {
                if(request.Status == Status.Cancelled)
                {
                    purchase.Cancel();
                    foreach (var item in purchase.ProductItems)
                    {
                        await _stockSubSystem.AddStockMovementAsync(DateTime.Now, item.Product, item.Quantity, StockMovementType.Outbound, ReasonType.Purchase, $"Cancelacion de compra {purchase.Id}", user);
                    }
                }
            }
            await _purchaseRepository.ChangeStatusPurchase(purchase);

            return PurchaseMapper.ToResponse(purchase);
        }
    }
}