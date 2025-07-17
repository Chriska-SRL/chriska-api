using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsCategory
{
    public class DeleteCategoryRequest
    {
        public int Id { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();
    }
}
