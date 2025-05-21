using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class ReturnsSubSystem
    {

        private List<Request> Requests = new List<Request>();

        private IReturnRequestRepository _returnRequestRepository;
        public ReturnsSubSystem(IReturnRequestRepository returnRequestRepository)
        {
            _returnRequestRepository = returnRequestRepository;
        }

        public void AddRequest(ReturnRequest request)
        {
            _returnRequestRepository.Add(request);
        }



    }
}
