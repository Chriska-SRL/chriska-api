using System.Collections.Generic;

namespace BusinessLogic.Domain
{
    public class ClientAccountStatement : AccountStatement
    {
        public Client Client { get; set; }
        public ClientAccountStatement(Client client, List<BalanceItem> items)
        {
            Client = client;
            BalanceItems = items ?? new List<BalanceItem>();
        }
    }
}
