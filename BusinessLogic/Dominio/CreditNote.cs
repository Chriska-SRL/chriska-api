using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class CreditNote
    {
        private int CreditNoteId { get; set; }
        private DateTime IssueDate { get; set; }
        private ReturnRequest ReturnRequest { get; set; }
    }
}
