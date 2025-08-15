using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsReturnRequest;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class RequestSubSystem
    {
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IProductRepository _productRepository;

        public RequestSubSystem(
            IReturnRequestRepository returnRequestRepository,
            IClientRepository clientRepository,
            IUserRepository userRepository,
            IDeliveryRepository deliveryRepository,
            IProductRepository productRepository)
        {
            _returnRequestRepository = returnRequestRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _deliveryRepository = deliveryRepository;
            _productRepository = productRepository;
        }

        public async Task<ReturnRequestResponse> AddReturnRequestAsync(ReturnRequestAddRequest request)
        {
            User user = await _userRepository.GetByIdAsync(request.UserId)
                ?? throw new ArgumentException("No se encontró el usuario asociado.");

            Delivery delivery = await _deliveryRepository.GetByIdAsync(request.DeliveryId)
                ?? throw new ArgumentException("No se encontró la entrega asociada.");

            var entity = ReturnRequestMapper.ToDomain(request, delivery, user);
            entity.Validate();

            var added = await _returnRequestRepository.AddAsync(entity);
            return ReturnRequestMapper.ToResponse(added);
        }

        //public async Task<ReturnRequestResponse> UpdateReturnRequestAsync(ReturnRequestUpdateRequest request)
        //{
        //    var existing = await _returnRequestRepository.GetByIdAsync(request.Id)
        //        ?? throw new ArgumentException("No se encontró la solicitud de devolución seleccionada.");

        //    // Si tu Update permite cambiar Delivery/User, los resolvemos acá
        //    User user = await _userRepository.GetByIdAsync(request.UserId ?? 0)
        //        ?? throw new ArgumentException("No se encontró el usuario asociado.");

        //    Delivery delivery = await _deliveryRepository.GetByIdAsync(request.DeliveryId)
        //        ?? throw new ArgumentException("No se encontró la entrega asociada.");

        //    var updatable = ReturnRequestMapper.ToUpdatableData(request, user, delivery);
        //    existing.Update(updatable);

        //    var updated = await _returnRequestRepository.UpdateAsync(existing);
        //    return ReturnRequestMapper.ToResponse(updated);
        //}

        public async Task DeleteReturnRequestAsync(DeleteRequest request)
        {
            var entity = await _returnRequestRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la solicitud de devolución seleccionada.");

            entity.MarkAsDeleted(request.getUserId(), request.Location);
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
    }
}
