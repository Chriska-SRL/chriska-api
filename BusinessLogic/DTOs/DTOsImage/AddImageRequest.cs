using BusinessLogic.DTOs.DTOsAudit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsImage
{
    public class AddImageRequest:AuditableRequest
    {
        public int EntityId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
