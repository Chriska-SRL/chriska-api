﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsCost
{
    public class CostResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
