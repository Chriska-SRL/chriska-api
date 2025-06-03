using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsCreditNote;
using BusinessLogic.DTOs.DTOsReturnRequest;

namespace BusinessLogic.SubSystem
{
    public class ReturnsSubSystem
    {
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICreditNoteRepository _creditNoteRepository;
        private readonly IOrderRepository _orderRepository;

        private readonly UserSubSystem _userSubSystem;
        private readonly ClientsSubSystem _clientsSubSystem;

        public ReturnsSubSystem(IReturnRequestRepository returnRequestRepository, IUserRepository userRepository, IClientRepository clientRepository, ICreditNoteRepository creditNoteRepository, IOrderRepository orderRepository, UserSubSystem userSubSystem, ClientsSubSystem clientsSubSystem)
        {
            _returnRequestRepository = returnRequestRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _creditNoteRepository = creditNoteRepository;
            _orderRepository = orderRepository;

            _userSubSystem = userSubSystem;
            _clientsSubSystem = clientsSubSystem;
        }

        public void AddReturnRequest(AddReturnRequest_Request request)
        {
            //Averiguar como se comportan en los dto los hijos.
            //var newRequest = new ReturnRequest(_creditNoteRepository.GetById(request.CreditNoteId), request.RequestDate, request.DeliveryDate, request.Status, request.Observation, _userRepository.GetById(request.UserId), _clientRepository.GetById(request.ClientId));
            //newRequest.Validate();
            //_returnRequestRepository.Add(newRequest);
        }

        public void UpdateReturnRequest(UpdateReturnRequest_Request request)
        {
            //var updateRequest = _returnRequestRepository.GetById(request.Id);
            //if (updateRequest == null) throw new Exception("La solicitud de devolucion no existe");
            //updateRequest.Update(request.DeliveryDate,request.Status,request.Observation,_userRepository.GetById(request.UserId),_clientRepository.GetById(request.ClientId));
            //_returnRequestRepository.Update(updateRequest);
        }
        public void DeleteReturnRequest (DeleteReturnRequest_Request request)
        {
            var deleteRequest = _returnRequestRepository.GetById(request.Id);
            if (deleteRequest == null) throw new Exception("La solicitud de devolucion no existe");
            _returnRequestRepository.Delete(deleteRequest.Id);

        }
        public ReturnRequestResponse GetReturnRequestById(int id)
        {
            var request = _returnRequestRepository.GetById(id);
            if (request == null) throw new Exception("La solicitud de devolucion no existe");
            var returnResponse = new ReturnRequestResponse
            {
                CreditNote = new CreditNoteResponse { 

                    IssueDate = request.CreditNote.IssueDate

                },
                RequestDate = request.RequestDate,
                DeliveryDate = request.DeliveryDate,
                Status = request.Status,
                Observation = request.Observation,
                User = _userSubSystem.GetUserById(request.User.Id),
                Client = _clientsSubSystem.GetClientById(request.Client.Id)
            };
            return returnResponse;
        }

        //public void AddOrderRequest(AddOrderRequest_Request request)
        //{
        //
        //    //Averiguar como se comportan en los dto los hijos.
        //    var newRequest = new OrderRequest(_orderRepository.GetById(request.OrderId), request.RequestDate, request.DeliveryDate, request.Status, request.Observation, _userRepository.GetById(request.UserId), _clientRepository.GetById(request.ClientId));
        //    newRequest.Validate();
        //    _returnRequestRepository.Add(newRequest);
        //    //Agregar un IRepository de OrderRequest para poder agregar los items del pedido a la solicitud de devolucion.
        //
        //}
        //public void UpdateOrderRequest()
        //{
        //
        //}

    }
}
