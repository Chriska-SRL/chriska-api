using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class ReturnRequest : Request
    {
        public Delivery Delivery { get; set; }

        public ReturnRequest(
             User user,
             Delivery delivery
         ) : base(delivery.Client, "", user, delivery.ProductItems)
        {
            Delivery = delivery;
        }

        public ReturnRequest(
            int id,
            Client client,
            Status status,
            DateTime date,
            DateTime? confirmedDate,
            string observation,
            User? user,
            List<ProductItem> productItems,
            AuditInfo? auditInfo,
            Delivery delivery
        ) : base(id, client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
            Delivery = delivery;
        }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Observations))
                throw new InvalidOperationException("La observación es obligatoria.");

        }

        internal void Confirm()
        {
            Status = Status.Confirmed;
            ConfirmedDate = DateTime.Now;
        }

        internal void Cancel()
        {
            Status = Status.Cancelled;
        }
    }

}