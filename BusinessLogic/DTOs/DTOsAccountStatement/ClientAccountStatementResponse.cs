using BusinessLogic.Common.Enums;
using System;
using System.Collections.Generic;

namespace BusinessLogic.DTOs.DTOsAccountStatement
{
    public class ClientAccountStatementResponse
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal TotalBalance { get; set; }
        public List<AccountStatementItemResponse> Items { get; set; } = new();
    }

    public class AccountStatementItemResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}