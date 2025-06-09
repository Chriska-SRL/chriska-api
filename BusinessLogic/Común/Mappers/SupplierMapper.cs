using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier toDomain(AddSupplierRequest request)
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
                bankAccount: request.BankAccount,
                observations: request.Observation,
                products: new List<Product>(),
                payments: new List<Payment>(),
                purchases: new List<Purchase>(),
                daysToDeliver: new List<Day>()
                );
        }
        public static Supplier.UpdatableData toDomain(UpdateSupplierRequest request)
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
                BankAccount = request.BankAccount,
                Observations = request.Observations
            };
        }
        public static SupplierResponse toResponse(Supplier domain)
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
                BankAccount = domain.BankAccount,
                Observations = domain.Observations
            };
        }
    }
}
