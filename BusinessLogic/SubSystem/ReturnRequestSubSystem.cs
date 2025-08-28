using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsReturnRequest;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class ReturnRequestSubSystem
    {
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IProductRepository _productRepository;

        public ReturnRequestSubSystem(
            IReturnRequestRepository returnRequestRepository,
            IUserRepository userRepository,
            IDeliveryRepository deliveryRepository,
            IProductRepository productRepository)
        {
            _returnRequestRepository = returnRequestRepository;
            _userRepository = userRepository;
            _deliveryRepository = deliveryRepository;
            _productRepository = productRepository;
        }

        public async Task<ReturnRequestResponse> AddReturnRequestAsync(ReturnRequestAddRequest request)
        {
            Delivery delivery = await _deliveryRepository.GetByIdAsync(request.DeliveryId)
                ?? throw new ArgumentException("No se encontró la entrega asociada.");

            if (delivery.Status != Status.Confirmed)
                throw new ArgumentException("La entrega asociada debe estar en estado confirmado para crear una solicitud de devolución.");

            if (!delivery.ProductItems.Any())
                throw new ArgumentException("La entrega no tiene productos para devolver.");

            int userId = request.getUserId() ?? 0;
            User user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la solicitud no existe.");

            var entity = ReturnRequestMapper.ToDomain(request, delivery,user);

            var added = await _returnRequestRepository.AddAsync(entity);
            return ReturnRequestMapper.ToResponse(added);
        }

        public async Task<ReturnRequestResponse> UpdateReturnRequestAsync(ReturnRequestUpdateRequest request)
        {
            var existing = await _returnRequestRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException($"No se encontró una solicitud de devolución con el ID {request.Id}.");

            if (existing.Status != Status.Pending)
              throw new ArgumentException("La solicitud de devolución no se puede modificar porque no está en estado pendiente.");

            var userId = request.getUserId() ?? 0;
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la modificación no existe.");

            var delivery = await _deliveryRepository.GetByIdAsync(existing.Delivery.Id)
                ?? throw new ArgumentException("El delivery seleccionado no existe.");

            var productItems = new List<ProductItem>();
            foreach (var item in request.ProductItems)
            {
                ProductItem? existingItem = existing.ProductItems.FirstOrDefault(p => p.Product.Id == item.ProductId);
                ProductItem? deliveryItem = delivery.ProductItems.FirstOrDefault(p => p.Product.Id == item.ProductId);
                if (existingItem != null)
                {
                    if (item.Quantity > deliveryItem.Quantity)
                        throw new ArgumentException($"La cantidad solicitada para la devolucion del producto {item.ProductId} no puede ser mayor que la cantidad entregada.");
                    if (item.Quantity <= 0)
                        throw new ArgumentException($"La cantidad solicitada para la devolucion del producto {item.ProductId} debe ser mayor que cero.");
                    existingItem.Quantity = item.Quantity;
                    productItems.Add(existingItem);
                }
            }

            ReturnRequest.UpdatableData updatedData = ReturnRequestMapper.ToUpdatableData(request, user, productItems);

            
            existing.Update(updatedData);

            var updated = await _returnRequestRepository.UpdateAsync(existing);
            return ReturnRequestMapper.ToResponse(updated);
        }

        public async Task DeleteReturnRequestAsync(DeleteRequest request)
        {
            var entity = await _returnRequestRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la solicitud de devolución seleccionada.");

            entity.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            await _returnRequestRepository.DeleteAsync(entity);
        }

        public async Task<ReturnRequestResponse> GetReturnRequestByIdAsync(int id)
        {
            var entity = await _returnRequestRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró la solicitud de devolución seleccionada.");

            return ReturnRequestMapper.ToResponse(entity);
        }

        public async Task<List<ReturnRequestResponse>> GetAllReturnRequestsAsync(QueryOptions options)
        {
            var list = await _returnRequestRepository.GetAllAsync(options);
            return list.Select(ReturnRequestMapper.ToResponse).ToList();
        }
        internal async Task<ReturnRequestResponse?> ChangeStatusReturnRequestAsync(int id, DocumentClientChangeStatusRequest request)
        {
            ReturnRequest? returnRequest = await _returnRequestRepository.GetByIdAsync(id)
                ?? throw new ArgumentException($"No se encontró una solicitud de pedido con el ID {id}.");

            returnRequest.AuditInfo.SetUpdated(request.getUserId(), request.AuditLocation);

            if (request.Status == Status.Confirmed)
            {
                returnRequest.Confirm();

                Delivery delivery = await _deliveryRepository.GetByIdAsync(returnRequest.Delivery.Id)
             ?? throw new ArgumentException("No se encontró la entrega asociada a la devolución.");

                foreach (var item in returnRequest.ProductItems)
                {
                    await _productRepository.UpdateStockAsync(item.Product.Id, item.Quantity, item.Quantity);
                }

            }
            else if (request.Status == Status.Cancelled)
            {
                returnRequest.Cancel();
            }
            else
            {
                throw new ArgumentException("El estado de la solicitud de pedido no es válido para cambiar.");
            }
            returnRequest = await _returnRequestRepository.ChangeStatusReturnRequestAsync(returnRequest);
            return ReturnRequestMapper.ToResponse(returnRequest);
        }
    }
}
