using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Common
{
    public abstract class AuditData
    {
        public int? UserId { get; set; } = null;
        public Location? Location { get; set; } = null;
    }
}
