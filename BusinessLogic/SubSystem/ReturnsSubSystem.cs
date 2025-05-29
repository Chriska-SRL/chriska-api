using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsCreditNote;
using BusinessLogic.DTOs.DTOsReturnRequest;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsOrder;

namespace BusinessLogic.SubSystem
{
    public class ReturnsSubSystem
    {
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICreditNoteRepository _creditNoteRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IOrderRepository _orderRepository;

        private readonly UserSubSystem _userSubSystem;
        private readonly ClientsSubSystem _clientsSubSystem;

        public ReturnsSubSystem(IReturnRequestRepository returnRequestRepository, IUserRepository userRepository, IClientRepository clientRepository, ICreditNoteRepository creditNoteRepository, IOrderRequestRepository orderRequestRepository, IOrderRepository orderRepository)
        {
            _returnRequestRepository = returnRequestRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _creditNoteRepository = creditNoteRepository;
            _orderRequestRepository = orderRequestRepository;
            _orderRepository = orderRepository;
        }

        public void AddReturnRequest(AddReturnRequest request)
        {
            var newRequest = new ReturnRequest(_creditNoteRepository.GetById(request.CreditNoteId), request.RequestDate, request.DeliveryDate, request.Status, request.Observation, _userRepository.GetById(request.UserId), _clientRepository.GetById(request.ClientId));
            newRequest.Validate();
            _returnRequestRepository.Add(newRequest);
        }

        public void UpdateReturnRequest(UpdateReturnRequest request)
        {
            var updateRequest = _returnRequestRepository.GetById(request.Id);
            if (updateRequest == null) throw new Exception("La solicitud de devolucion no existe");
            updateRequest.Update(request.DeliveryDate,request.Status,request.Observation,_userRepository.GetById(request.UserId),_clientRepository.GetById(request.ClientId));
            _returnRequestRepository.Update(updateRequest);
        }
        public void DeleteReturnRequest (DeleteReturnRequest request)
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
         public void AddOrderRequest(AddOrderRequest_Request orderRequest)
        {
            var orderRequestReturn = new OrderRequest(
                _orderRepository.GetById(orderRequest.OrderId),
                orderRequest.RequestDate,
                orderRequest.DeliveryDate,
                orderRequest.Status,
                orderRequest.Observation,
                _userRepository.GetById(orderRequest.UserId),
                _clientRepository.GetById(orderRequest.ClientId)
            );
            orderRequestReturn.Validate();
            _orderRequestRepository.Add(orderRequestReturn);
        }
        public void UpdateOrderRequest(UpdateOrderRequest_Request orderRequest)
        {
            var updateOrderRequest = _orderRequestRepository.GetById(orderRequest.Id);
            if (updateOrderRequest == null) throw new Exception("La solicitud de pedido no existe");
            updateOrderRequest.Update(orderRequest.DeliveryDate, orderRequest.Status, orderRequest.Observation, _userRepository.GetById(orderRequest.UserId), _clientRepository.GetById(orderRequest.ClientId));
            _orderRequestRepository.Update(updateOrderRequest);
        }
        public void DeleteOrderRequest(DeleteOrderRequest_Request orderRequest)
        {
            var deleteOrderRequest = _orderRequestRepository.GetById(orderRequest.Id);
            if (deleteOrderRequest == null) throw new Exception("La solicitud de pedido no existe");
            _orderRequestRepository.Delete(deleteOrderRequest.Id);
        }
        public OrderRequestResponse GetOrderRequestById(int id)
        {
            var orderRequest = _orderRequestRepository.GetById(id);
            if (orderRequest == null) throw new Exception("La solicitud de pedido no existe");
            var orderRequestResponse = new OrderRequestResponse
            {
                Order = new OrderResponse
                {
                    Id = orderRequest.Order.Id,
                    Date = orderRequest.Order.Date,
                    ClientName = orderRequest.Order.ClientName,
                    Crates = orderRequest.Order.Crates,
                    Status = orderRequest.Order.Status
                },
                RequestDate = orderRequest.RequestDate,
                DeliveryDate = orderRequest.DeliveryDate,
                Status = orderRequest.Status,
                Observation = orderRequest.Observation,
                User = _userSubSystem.GetUserById(orderRequest.User.Id),
                Client = _clientsSubSystem.GetClientById(orderRequest.Client.Id)
            };
            return orderRequestResponse;
        }
    }
}
