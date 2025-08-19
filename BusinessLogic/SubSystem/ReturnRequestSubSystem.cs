using BusinessLogic.Common;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsReturnRequest;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class ReturnRequestSubSystem
    {
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryRepository _deliveryRepository;

        public ReturnRequestSubSystem(
            IReturnRequestRepository returnRequestRepository,
            IUserRepository userRepository,
            IDeliveryRepository deliveryRepository)
        {
            _returnRequestRepository = returnRequestRepository;
            _userRepository = userRepository;
            _deliveryRepository = deliveryRepository;
        }

        public async Task<ReturnRequestResponse> AddReturnRequestAsync(ReturnRequestAddRequest request)
        {
            Delivery delivery = await _deliveryRepository.GetByIdAsync(request.DeliveryId)
                ?? throw new ArgumentException("No se encontró la entrega asociada.");

            int userId = request.getUserId() ?? 0;
            User user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("El usuario que realiza la solicitud no existe.");

            var entity = ReturnRequestMapper.ToDomain(request, delivery,user);
            entity.Validate();

            var added = await _returnRequestRepository.AddAsync(entity);
            return ReturnRequestMapper.ToResponse(added);
        }

        public async Task<ReturnRequestResponse> UpdateReturnRequestAsync(ReturnRequestUpdateRequest request)
        {
            throw new NotImplementedException("UpdateOrderRequestAsync is not implemented yet.");
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
        }

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
