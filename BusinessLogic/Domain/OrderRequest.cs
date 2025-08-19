using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class OrderRequest : Request
    {
        public Order? Order { get; set; }

        public OrderRequest(
          Client client,
          string? observation,
          User user,
          List<ProductItem> productItems
      ) : base(client, observation, user, productItems)
        {
        }

        public OrderRequest(
            int id,
            Client? client,
            Status status,
            DateTime? confirmedDate,
            DateTime date,
            string observation,
            User? user,
            List<ProductItem> productItems,
            Order? order,
            AuditInfo? auditInfo
           
        ) : base(id, client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
            Order = order;
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
