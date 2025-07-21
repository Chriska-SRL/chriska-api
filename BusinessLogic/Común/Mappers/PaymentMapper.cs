using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsPayment;
using BusinessLogic.Mappers;

namespace Repository.Mappers
{
    public static class PaymentMapper
    {
        public static Payment FromAddRequest(AddPaymentRequest dto)
        {
            return new Payment(
                id: 0,
                date: dto.Date,
                amount: dto.Amount,
                note: dto.Note,
                auditInfo: AuditMapper.ToDomain(dto.AuditInfo)

            );
        }

        public static Payment.UpdatableData FromUpdateRequest(UpdatePaymentRequest dto)
        {
            return new Payment.UpdatableData
            {

                Date = dto.Date,
                Amount = dto.Amount,
                Note = dto.Note,
                AuditInfo = AuditMapper.ToDomain(dto.AuditInfo)

            };
        }

        public static PaymentResponse ToResponse(Payment entity)
        {
            return new PaymentResponse
            {
                Date = entity.Date,
                Amount = entity.Amount,
                Note = entity.Note,
                AuditInfo = AuditMapper.ToResponse(entity.AuditInfo)
            };
        }
    }
}
