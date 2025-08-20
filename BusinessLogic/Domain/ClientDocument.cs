using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public abstract class ClientDocument : ProductDocument
    {

        public Client? Client { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public DateTime? ConfirmedDate { get; set; }

        public ClientDocument(Client client, string? observation, User user, List<ProductItem> productItems) :
            base(observation, user, productItems)
        {
            Client = client;
            Status = Status.Pending;
            ConfirmedDate = null;
            Validate();
        }


        public ClientDocument(int id, Client? client, Status status, DateTime? confirmedDate, DateTime date, string observation, User? user, List<ProductItem> productItems, AuditInfo? auditInfo)
            : base(id, date, observation, user, productItems, auditInfo)
        {
            Client = client;
            Status = status;
            ConfirmedDate = confirmedDate ?? null;
        }

        public void ChangeStatus(Status status, int userId, Location? location)
        {
            Status = status;
            if (status == Status.Confirmed)
            {
                ConfirmedDate = DateTime.Now;
            }
            AuditInfo?.SetUpdated(userId, location);
        }
        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo?.SetDeleted(userId, location);
        }
        public override void Validate()
        {
            if (!string.IsNullOrWhiteSpace(Observations) && Observations.Length > 255)
                throw new ArgumentOutOfRangeException("La observación no puede superar los 255 caracteres.");
            if (Client == null) throw new ArgumentNullException("El cliente es obligatorio.");
        }
    }
}
