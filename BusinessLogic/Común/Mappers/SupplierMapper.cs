using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSupplier;

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
                bank: request.Bank,
                bankAccount: request.BankAccount,
                observations: request.Observation
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
                Bank = request.Bank,
                BankAccount = request.BankAccount,
                Observations = request.Observations
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
                Bank = domain.Bank,
                BankAccount = domain.BankAccount,
                Observations = domain.Observations
            };
        }
    }
}
