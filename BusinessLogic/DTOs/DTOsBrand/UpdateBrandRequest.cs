﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsBrand
{
    public class UpdateBrandRequest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }

    }
}
