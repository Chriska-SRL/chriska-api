using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSupplier;
using System.Runtime;

namespace BusinessLogic.Común.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier ToDomain(AddSupplierRequest request)
        {
            return new Supplier(
                id: 0,
                name: request.Name,
                rut: request.RUT,
                razonSocial: request.BusinessName,
                address: request.Address,
                mapsAddress: request.MapsAddress,
                phone: request.Phone,
                contactName: request.ContactName,
                email: request.Email,
                observations: request.Observation,
                products: new List<Product>(),
                purchases: new List<Purchase>(),
                bankAccounts: new List<BankAccount>(),
                auditInfo: AuditMapper.ToDomain(request.AuditInfo)
                );
        }
        public static Supplier.UpdatableData ToUpdatableData(UpdateSupplierRequest request)
        {
            return new Supplier.UpdatableData
            {
                Name = request.Name,
                RUT = request.RUT,
                RazonSocial = request.RazonSocial,
                Address = request.Address,
                MapsAddress = request.MapsAddress,
                Phone = request.Phone,
                ContactName = request.ContactName,
                Email = request.Email,
                Observations = request.Observations,
                AuditInfo = AuditMapper.ToDomain(request.AuditInfo)
            };
        }
        public static SupplierResponse ToResponse(Supplier domain)
        {
            return new SupplierResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                RUT = domain.RUT,
                RazonSocial = domain.RazonSocial,
                Address = domain.Address,
                MapsAddress = domain.MapsAddress,
                Phone = domain.Phone,
                ContactName = domain.ContactName,
                Email = domain.Email,
                Observations = domain.Observations,
                Products = domain.Products.Select(ProductMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(domain.AuditInfo)
            };
        }
    }
}
