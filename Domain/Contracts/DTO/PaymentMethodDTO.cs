﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }
}
