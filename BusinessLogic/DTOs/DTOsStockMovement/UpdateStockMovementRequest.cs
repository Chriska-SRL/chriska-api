using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsStockMovement
{
    public class UpdateStockMovementRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public int ShelveId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
