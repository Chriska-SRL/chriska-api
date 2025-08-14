using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public abstract class ClientDocument : ProductDocument
    {
     
        public Client? Client { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public DateTime ConfirmedDate { get; set; }

        public ClientDocument(Client? client,Status status,DateTime confirmedDate,DateTime? date,string? observation,User? user,List<ProductItem> productItems,AuditInfo? auditInfo ) : base(date, observation, user, productItems, auditInfo)
        {
            Client = client;
            Status = status;
            ConfirmedDate = confirmedDate;
        }
        public ClientDocument(int id, Client? client, Status status, DateTime confirmedDate, DateTime? date, string? observation, User? user, List<ProductItem> productItems, AuditInfo? auditInfo)
            : base(id, date, observation, user, productItems, auditInfo)
        {
            Client = client;
            Status = status;
            ConfirmedDate = confirmedDate;
        }
        public abstract void Validate();
    }
}
