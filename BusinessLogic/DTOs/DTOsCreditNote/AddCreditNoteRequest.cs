using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsCreditNote
{
    public class AddCreditNoteRequest
    {
        public DateTime IssueDate { get; set; }
        public int ReturnRequestId { get; set; }
    }
}
