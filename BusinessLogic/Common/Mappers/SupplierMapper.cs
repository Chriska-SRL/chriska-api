using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier ToDomain(AddSupplierRequest request)
        {
            var supplier = new Supplier(
                name: request.Name,
                rut: request.RUT,
                razonSocial: request.BusinessName,
                address: request.Address,
                mapsAddress: request.MapsAddress,
                phone: request.Phone,
                contactName: request.ContactName,
                email: request.Email,
                observations: request.Observation,
                bankAccounts: new List<BankAccount>()
            );

            supplier.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return supplier;
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
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static SupplierResponse ToResponse(Supplier supplier)
        {
            return new SupplierResponse
            {
                Id = supplier.Id,
                Name = supplier.Name,
                RUT = supplier.RUT,
                RazonSocial = supplier.RazonSocial,
                Address = supplier.Address,
                MapsAddress = supplier.MapsAddress,
                Phone = supplier.Phone,
                ContactName = supplier.ContactName,
                Email = supplier.Email,
                Observations = supplier.Observations,
                Products = supplier.Products.Select(ProductMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(supplier.AuditInfo)
            };
        }
    }
}
