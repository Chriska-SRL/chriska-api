﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsShelve
{
    public class UpdateShelveRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int WarehouseId { get; set; }
    }
}
