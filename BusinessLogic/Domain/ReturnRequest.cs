using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class ReturnRequest:Request,IEntity<ReturnRequest.UpdatableData>
    {
        public Delivery Delivery { get; set; }

        public ReturnRequest(
             Client? client,
             Status status,
             DateTime? date,
             DateTime confirmedDate,
             string? observation,
             User? user,
             List<ProductItem> productItems,
             AuditInfo? auditInfo,
             Delivery delivery
         ) : base(client, status, date, confirmedDate, observation, user, productItems, auditInfo)
        {
            Delivery = delivery;
        }

        public ReturnRequest(
            int id,
            Client? client,
            Status status,
            DateTime? date,
            DateTime confirmedDate,
            string? observation,
            User? user,
            List<ProductItem> productItems,
            AuditInfo? auditInfo,
            Delivery delivery
        ) : base(id, client, status, date, confirmedDate, observation, user, productItems, auditInfo)
        {
            Delivery = delivery;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
        public void Update(UpdatableData data)
        {
            Delivery = data.Delivery;
            Date = data.Date;
            Observation = data.Observation;
            User = data.User;
        }

        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public string Observation { get; set; }
            public User User { get; set; }
            public Delivery Delivery { get; set; }
        }
    

    }

}
