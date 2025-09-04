using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Domain
{
    public abstract class AccountStatement
    {
        public List<BalanceItem> BalanceItems { get; set; }

        protected AccountStatement()
        {
            BalanceItems = new List<BalanceItem>();
        }

        public decimal getBalance()
        {
            if (BalanceItems == null || BalanceItems.Count == 0)
                return 0m;
            return BalanceItems.Last().Balance;
        }
    }
}
