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

        public void AddReturnRequest(AddReturnRequest_Request request)
        {
            ReturnRequest newReturnRequest = ReturnRequestMapper.ToDomain(request);
            newReturnRequest.Validate();
            _returnRequestRepository.Add(newReturnRequest);
        }

        public void UpdateReturnRequest(UpdateReturnRequest_Request request)
        {
            ReturnRequest existingRequest = _returnRequestRepository.GetById(request.Id);
            if (existingRequest == null) throw new Exception("La solicitud de devolucion no existe");
            existingRequest.Update(ReturnRequestMapper.ToDomain(request));
            _returnRequestRepository.Update(existingRequest);
        }
        public void DeleteReturnRequest(DeleteReturnRequest_Request request)
        {
            var deleteRequest = _returnRequestRepository.GetById(request.Id);
            if (deleteRequest == null) throw new Exception("La solicitud de devolucion no existe");
            _returnRequestRepository.Delete(deleteRequest.Id);

        }
        public ReturnRequestResponse GetReturnRequestById(int id)
        {
            ReturnRequest returnRequest = _returnRequestRepository.GetById(id);
            if (returnRequest == null) throw new Exception("La solicitud de devolucion no existe");
            return ReturnRequestMapper.ToResponse(returnRequest);
        }
        public List<ReturnRequestResponse> GetAllReturnRequests()
        {
            var returnRequests = _returnRequestRepository.GetAll();
            if (!returnRequests.Any()) throw new Exception("No se encontraron solicitudes de devolucion");
            return returnRequests.Select(ReturnRequestMapper.ToResponse).ToList();
        }
    }
}
