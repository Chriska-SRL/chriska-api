using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class PurchaseMapper
    {
        public static Purchase ToDomain(AddPurchaseRequest dto)
        {
            return new Purchase(
                id: 0,
                date: dto.Date,
                referece: dto.Reference,
                supplier: new Supplier(dto.SupplierId),
                payments: new List<Payment>(),
                auditInfo: AuditMapper.ToDomain(dto.AuditInfo)
            );
        }

        public static Purchase.UpdatableData ToUpdatableData(UpdatePurchaseRequest dto)
        {
            return new Purchase.UpdatableData
            {
                Date = dto.Date,
                Status = dto.Status,
                AuditInfo = AuditMapper.ToDomain(dto.AuditInfo)
            };
        }

        public static PurchaseResponse ToResponse(Purchase entity)
        {
            return new PurchaseResponse
            {
                Id = entity.Id,
                Date = entity.Date,
                Reference = entity.Reference,
                Supplier = new SupplierResponse
                {
                    Id = entity.Supplier.Id,
                    Name = entity.Supplier.Name,
                    RUT = entity.Supplier.RUT,
                    RazonSocial = entity.Supplier.RazonSocial,
                    Address = entity.Supplier.Address,
                    MapsAddress = entity.Supplier.MapsAddress,
                    Phone = entity.Supplier.Phone,
                    ContactName = entity.Supplier.ContactName,
                    Email = entity.Supplier.Email,
                    Observations = entity.Supplier.Observations
                },
                AuditInfo = AuditMapper.ToResponse(entity.AuditInfo)
            };
        }
    }
}
