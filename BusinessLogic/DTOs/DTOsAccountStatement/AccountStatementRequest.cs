using System;

namespace BusinessLogic.DTOs.DTOsAccountStatement
{
    public class AccountStatementRequest
    {
        public int EntityId { get; set; } // ClientId o SupplierId
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}