using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Repository;
using BusinessLogic.Services;

namespace BusinessLogic.SubSystem
{
    public class ProductsSubSystem
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IShelveRepository _shelveRepository;
        private readonly IAzureBlobService _blobService;
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IDeliveryRepository _deliveriesRepository;
        private readonly IOrderRepository _orderRepository;

        public ProductsSubSystem(
            IProductRepository productRepository,
            ISubCategoryRepository subCategoryRepository,
            IBrandRepository brandRepository,
            ISupplierRepository supplierRepository,
            IShelveRepository shelveRepository,
            IAzureBlobService blobService,
            IDiscountRepository discountRepository,
            IOrderRequestRepository orderRequestRepository,
            IDeliveryRepository deliveriesRepository,
            IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _brandRepository = brandRepository;
            _supplierRepository = supplierRepository;
            _shelveRepository = shelveRepository;
            _blobService = blobService;
            _discountRepository = discountRepository;
            _orderRequestRepository = orderRequestRepository;
            _deliveriesRepository = deliveriesRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ProductResponse> AddProductAsync(ProductAddRequest request)
        {
            SubCategory subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId)
                ?? throw new ArgumentException("No se encontró la subcategoría asociada.");

            Brand brand = await _brandRepository.GetByIdAsync(request.BrandId)
                ?? throw new ArgumentException("No se encontró la marca asociada.");

            Shelve shelve = await _shelveRepository.GetByIdAsync(request.ShelveId)
                ?? throw new ArgumentException("No se encontró la estantería asociada.");

            List<Supplier> suppliers = new List<Supplier>();
            foreach (var supplierId in request.SupplierIds ?? new List<int>())
            {
                var supplier = await _supplierRepository.GetByIdAsync(supplierId)
                    ?? throw new ArgumentException($"No se encontró el proveedor con ID {supplierId}.");
                suppliers.Add(supplier);
            }

            if(request.UnitType == Common.Enums.UnitType.Kilo && (request.EstimatedWeight == null || request.EstimatedWeight <= 0))
                throw new ArgumentException("El peso estimado es obligatorio para productos vendidos por kilo.");

            var product = ProductMapper.ToDomain(request, subCategory, brand, suppliers, shelve);
            product.Validate();

            var added = await _productRepository.AddAsync(product);
            return ProductMapper.ToResponse(added);
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request)
        {
            var existing = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            var subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId ?? existing.SubCategory.Id)
                ?? throw new ArgumentException("No se encontró la subcategoría asociada.");

            var brand = await _brandRepository.GetByIdAsync(request.BrandId ?? existing.Brand.Id)
                ?? throw new ArgumentException("No se encontró la marca asociada.");

            var shelve = await _shelveRepository.GetByIdAsync(request.ShelveId ?? existing.Shelve.Id)
                ?? throw new ArgumentException("No se encontró la estantería asociada.");

            List<Supplier> suppliers = new List<Supplier>();
            foreach (var supplierId in request.SupplierIds ?? new List<int>())
            {
                var supplier = await _supplierRepository.GetByIdAsync(supplierId)
                    ?? throw new ArgumentException($"No se encontró el proveedor con ID {supplierId}.");
                suppliers.Add(supplier);
            }

            Product.UpdatableData updatedData = ProductMapper.ToUpdatableData(request, subCategory, brand, suppliers, shelve);
            existing.Update(updatedData);

            var updated = await _productRepository.UpdateAsync(existing);
            return ProductMapper.ToResponse(updated);
        }

        public async Task DeleteProductAsync(DeleteRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            var options = new QueryOptions
            {
                Filters = new Dictionary<string, string>
                {
                    { "ProductId", request.Id.ToString() }
                }
            };
            List<Discount> discounts = await _discountRepository.GetAllAsync(options);
            if (discounts.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene descuentos asociados.");
            }

            List<OrderRequest> orderRequests = await _orderRequestRepository.GetAllAsync(options);
            if (orderRequests.Any(or => or.Status == Common.Enums.Status.Pending))
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene pedidos pendientes asociados.");

            
            List<Order> orders = await _orderRepository.GetAllAsync(options);
            if (orders.Any(o => o.Status == Common.Enums.Status.Pending))
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene órdenes pendientes asociadas.");

           
            List<Delivery> deliveries = await _deliveriesRepository.GetAllAsync(options);
            if (deliveries.Any(d => d.Status == Common.Enums.Status.Pending))
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene entregas pendientes asociadas.");


            product.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            return ProductMapper.ToResponse(product);
        }

        public async Task<ProductResponse> GetProductByIdWithDiscountsAsync(int id)
        {
            var product = await _productRepository.GetByIdWithDiscountsAsync(id)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            return ProductMapper.ToResponse(product);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(QueryOptions options)
        {
            var products = await _productRepository.GetAllAsync(options);
            return products.Select(ProductMapper.ToResponse).ToList();
        }

        public async Task<string> UploadProductImageAsync(AddImageRequest request)
        {
            Product product = await _productRepository.GetByIdAsync(request.EntityId)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");
            product.AuditInfo.SetUpdated(request.getUserId(), request.AuditLocation);

            var url = await _blobService.UploadFileAsync(request.File, "products", $"product{product.Id}");
            return await _productRepository.UpdateImageUrlAsync(product, url);
        }

        public async Task DeleteProductImageAsync(int productId)
        {
            Product product = await _productRepository.GetByIdAsync(productId)
                ?? throw new ArgumentException("No se encontró el producto seleccionado.");

            await _blobService.DeleteFileAsync(product.ImageUrl, "products"); 
            await _productRepository.UpdateImageUrlAsync(product, "");
        }
    }
}
