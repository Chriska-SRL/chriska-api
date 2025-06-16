using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsReturnRequest;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class ReturnsSubSystem
    {
        private readonly IReturnRequestRepository _returnRequestRepository;

        public ReturnsSubSystem(IReturnRequestRepository returnRequestRepository)
        {
            _returnRequestRepository = returnRequestRepository;
        }

        public ReturnRequestResponse AddReturnRequest(AddReturnRequest_Request request)
        {
            ReturnRequest newReturnRequest = ReturnRequestMapper.ToDomain(request);
            newReturnRequest.Validate();

            ReturnRequest added = _returnRequestRepository.Add(newReturnRequest);
            return ReturnRequestMapper.ToResponse(added);
        }

        public ReturnRequestResponse UpdateReturnRequest(UpdateReturnRequest_Request request)
        {
            ReturnRequest existing = _returnRequestRepository.GetById(request.Id)
                                        ?? throw new InvalidOperationException("Solicitud de devolución no encontrada.");

            ReturnRequest.UpdatableData updatedData = ReturnRequestMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            ReturnRequest updated = _returnRequestRepository.Update(existing);
            return ReturnRequestMapper.ToResponse(updated);
        }

        public ReturnRequestResponse DeleteReturnRequest(DeleteReturnRequest_Request request)
        {
            ReturnRequest deleted = _returnRequestRepository.Delete(request.Id)
                                        ?? throw new InvalidOperationException("Solicitud de devolución no encontrada.");

            return ReturnRequestMapper.ToResponse(deleted);
        }

        public ReturnRequestResponse GetReturnRequestById(int id)
        {
            ReturnRequest returnRequest = _returnRequestRepository.GetById(id)
                                            ?? throw new InvalidOperationException("Solicitud de devolución no encontrada.");

            return ReturnRequestMapper.ToResponse(returnRequest);
        }

        public List<ReturnRequestResponse> GetAllReturnRequests()
        {
            List<ReturnRequest> returnRequests = _returnRequestRepository.GetAll();
            return returnRequests.Select(ReturnRequestMapper.ToResponse).ToList();
        }
    }
}
