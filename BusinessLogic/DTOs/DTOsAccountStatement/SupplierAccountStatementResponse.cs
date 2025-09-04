using System;
using System.Collections.Generic;

namespace BusinessLogic.DTOs.DTOsAccountStatement
{
    public class SupplierAccountStatementResponse
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalBalance { get; set; }
        public List<AccountStatementItemResponse> Items { get; set; } = new();
    }

    // Reutiliza AccountStatementItemResponse del DTO anterior
}