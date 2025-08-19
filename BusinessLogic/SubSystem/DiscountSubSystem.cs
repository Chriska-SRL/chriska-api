using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDiscount;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class DiscountsSubSystem
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;

        public DiscountsSubSystem(
            IDiscountRepository discountRepository,
            IBrandRepository brandRepository,
            ISubCategoryRepository subCategoryRepository,
            IZoneRepository zoneRepository,
            IClientRepository clientRepository,
            IProductRepository productRepository)
        {
            _discountRepository = discountRepository;
            _brandRepository = brandRepository;
            _subCategoryRepository = subCategoryRepository;
            _zoneRepository = zoneRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
        }

        public async Task<DiscountResponse?> AddDiscountAsync(DiscountAddRequest request)
        {
            Brand? brand = null;
            if (request.BrandId.HasValue)
            {
                brand = await _brandRepository.GetByIdAsync(request.BrandId.Value)
                    ?? throw new ArgumentException("No se encontró la marca asociada.");
            }

            SubCategory? subCategory = null;
            if (request.SubCategoryId.HasValue)
            {
                subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId.Value)
                    ?? throw new ArgumentException("No se encontró la subcategoría asociada.");
            }

            Zone? zone = null;
            if (request.ZoneId.HasValue)
            {
                zone = await _zoneRepository.GetByIdAsync(request.ZoneId.Value)
                    ?? throw new ArgumentException("No se encontró la zona asociada.");
            }

            var clients = new List<Client>();
            foreach (var clientId in request.DiscountClientId)
            {
                var client = await _clientRepository.GetByIdAsync(clientId)
                    ?? throw new ArgumentException($"No se encontró el cliente con ID {clientId}.");
                clients.Add(client);
            }

            var products = new List<Product>();
            foreach (var productId in request.DiscountProductId)
            {
                var product = await _productRepository.GetByIdAsync(productId)
                    ?? throw new ArgumentException($"No se encontró el producto con ID {productId}.");
                products.Add(product);
            }

            if (clients.Count == 0 && zone == null) { 
                throw new ArgumentException("Debe asignar al menos un cliente o una zona para el descuento.");
            }
            if(products.Count == 0 && (brand == null && subCategory == null)) { 
                throw new ArgumentException("Debe asignar al menos un producto, una marca o una subcategoría para el descuento.");
            }

            if (zone != null && clients.Count > 0) { 
                throw new ArgumentException("No se puede asignar una zona y clientes al mismo tiempo. El descuento debe ser para todos los clientes de la zona o para clientes específicos.");
            }
            if (products.Count > 0 && (brand != null || subCategory != null)) {
                throw new ArgumentException("No se puede asignar productos y marca o subcategoría al mismo tiempo. El descuento debe ser para todos los productos de la marca o subcategoría, o para productos específicos.");
            }

            var discount = DiscountMapper.ToDomain(request, brand, subCategory, zone, clients, products);
            discount.Validate();

            var added = await _discountRepository.AddAsync(discount);
            return DiscountMapper.ToResponse(added);
        }

        public async Task<DiscountResponse?> UpdateDiscountAsync(DiscountUpdateRequest request)
        {
            var existing = await _discountRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el descuento seleccionado.");

            Brand? brand = null;
            if (request.BrandId.HasValue)
            {
                brand = await _brandRepository.GetByIdAsync(request.BrandId.Value)
                    ?? throw new ArgumentException("No se encontró la marca asociada.");
            }

            SubCategory? subCategory = null;
            if (request.SubCategoryId.HasValue)
            {
                subCategory = await _subCategoryRepository.GetByIdAsync(request.SubCategoryId.Value)
                    ?? throw new ArgumentException("No se encontró la subcategoría asociada.");
            }

            Zone? zone = null;
            if (request.ZoneId.HasValue)
            {
                zone = await _zoneRepository.GetByIdAsync(request.ZoneId.Value)
                    ?? throw new ArgumentException("No se encontró la zona asociada.");
            }

            var clients = new List<Client>();
            foreach (var clientId in request.DiscountClientId)
            {
                var client = await _clientRepository.GetByIdAsync(clientId)
                    ?? throw new ArgumentException($"No se encontró el cliente con ID {clientId}.");
                clients.Add(client);
            }

            var products = new List<Product>();
            foreach (var productId in request.DiscountProductId)
            {
                var product = await _productRepository.GetByIdAsync(productId)
                    ?? throw new ArgumentException($"No se encontró el producto con ID {productId}.");
                products.Add(product);
            }

            Discount.UpdatableData updatedData = DiscountMapper.ToUpdatableData(request, brand, subCategory, zone, clients, products);
            existing.Update(updatedData);

            var updated = await _discountRepository.UpdateAsync(existing);
            return DiscountMapper.ToResponse(updated);
        }

        public async Task DeleteDiscountAsync(DeleteRequest request)
        {
            var discount = await _discountRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el descuento seleccionado.");

            discount.MarkAsDeleted(request.getUserId(), request.Location);
            await _discountRepository.DeleteAsync(discount);
        }

        public async Task<DiscountResponse?> GetDiscountByIdAsync(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el descuento seleccionado.");

            return DiscountMapper.ToResponse(discount);
        }

        public async Task<List<DiscountResponse>?> GetAllDiscountsAsync(QueryOptions options)
        {
            var discounts = await _discountRepository.GetAllAsync(options);
            return discounts.Select(DiscountMapper.ToResponse).OfType<DiscountResponse>().ToList();
        }
        public async Task<DiscountResponse?> GetBestDiscountAsync(int productId, int clientId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new ArgumentException($"No se encontró el producto con ID {productId}.");

            var client = await _clientRepository.GetByIdAsync(clientId)
                ?? throw new ArgumentException($"No se encontró el cliente con ID {clientId}.");

            var bestDiscount = await _discountRepository.GetBestByProductAndClientAsync(product, client);

            return bestDiscount != null ? DiscountMapper.ToResponse(bestDiscount) : null;
        }
    }
}
