using BusinessLogic.Común.Enums;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsBankAccount;
using BusinessLogic.DTOs.DTOsProduct;


namespace BusinessLogic.DTOs.DTOsSupplier
{
    public class SupplierResponse : AuditableResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RUT { get; set; }
        public string RazonSocial { get; set; }
        public string Address { get; set; }
        public string MapsAddress { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Observations { get; set; }
        public List<ProductResponse> Products { get; set; } = new List<ProductResponse>();
    }
}
