using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class ClientReceipt : Receipt
    {
        public Client? Client { get; set; }
        public ClientReceipt(DateTime date, decimal amount, string notes, PaymentMethod paymentMethod, Client client) : base(date, amount, notes, paymentMethod)
        {
            Client = client ?? throw new ArgumentException(nameof(client), "El cliente es obligatorio.");
        }

        public ClientReceipt(int id, DateTime date, decimal amount, string notes, PaymentMethod paymentMethod, AuditInfo? auditInfo, Client? client) : base(id, date, amount, notes, paymentMethod, auditInfo)
        {
            Client = client;
        }
    }
}
