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
        // Guía temporal: entidades que maneja este subsistema

        private List<Request> Requests = new List<Request>();

        private List<RequestItem> RequestItems = new List<RequestItem>();

        private IReturnRequestRepository _returnRequestRepository;
        public ReturnsSubSystem(IReturnRequestRepository returnRequestRepository)
        {
            _returnRequestRepository = returnRequestRepository;
        }

    }
}
