using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class Order : ClientDocument
    {
        public int Crates { get; set; }
        public OrderRequest? OrderRequest { get; set; }
        public Delivery? Delivery { get; set; }
        public Order(
           OrderRequest orderRequest
       ) : base(orderRequest.Client, "", orderRequest.User, orderRequest.ProductItems)
        {
            Crates = 0;
            OrderRequest = orderRequest;
        }
        public Order(
          int id,
          Client client,
          Status status,
          DateTime date,
          string observations,
          User? user,
          List<ProductItem> productItems,
          AuditInfo? auditInfo,
          DateTime confirmedDate,
          int crates,
          OrderRequest? orderRequest = null,
          Delivery? delivery = null
      ) : base(id, client, status, confirmedDate, date, observations, user, productItems, auditInfo)
        {
            Crates = crates;
            OrderRequest = orderRequest;
            Delivery = delivery;
        }



        public override void Validate()
        {
            throw new NotImplementedException();
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