using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCreditNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Común.Mappers
{
    public static class CreditNoteMapper
    {
        public static CreditNote toDomain (AddCreditNoteRequest dto)
        {
            return new CreditNote(
                id: 0,
                issueDate: dto.IssueDate,
                returnRequest: null
            );
        }
        public static CreditNote.UpdatableData toDomain(UpdateCreditNoteRequest dto)
        {
            return new CreditNote.UpdatableData
            {
                IssueDate = dto.IssueDate,
                ReturnRequest = new ReturnRequest
                (
                    id: dto.ReturnRequestId,
                    creditNote: null,
                    requestDate: DateTime.Now,
                    deliveryDate: DateTime.Now,
                    status: null,
                    observation: null,
                    user: null,
                    client: null,
                    requestItems: new List<RequestItem>()

                )
            };
        }       
        public static CreditNoteResponse toResponse(CreditNote domain)
        {
            return new CreditNoteResponse
            {
                Id = domain.Id,
                IssueDate = domain.IssueDate
            };
        }

    }
}
