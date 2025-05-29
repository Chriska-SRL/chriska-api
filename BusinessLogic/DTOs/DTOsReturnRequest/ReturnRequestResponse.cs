using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsCreditNote;
using BusinessLogic.DTOs.DTOsUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsReturnRequest
{
    public class ReturnRequestResponse
    {
        public CreditNoteResponse CreditNote { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public UserResponse User { get; set; }
        public ClientResponse Client { get; set; }
        
    }
}
