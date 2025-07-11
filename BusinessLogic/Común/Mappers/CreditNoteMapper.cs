﻿using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCreditNote;

namespace BusinessLogic.Común.Mappers
{
    public static class CreditNoteMapper
    {
        public static CreditNote ToDomain (AddCreditNoteRequest dto)
        {
            return new CreditNote(
                id: 0,
                issueDate: dto.IssueDate,
                returnRequest: null
            );
        }
        public static CreditNote.UpdatableData ToDomain(UpdateCreditNoteRequest dto)
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
        public static CreditNoteResponse ToResponse(CreditNote domain)
        {
            return new CreditNoteResponse
            {
                Id = domain.Id,
                IssueDate = domain.IssueDate
            };
        }

    }
}
