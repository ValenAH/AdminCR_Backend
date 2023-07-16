﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO
{
    public class SaleDetailsDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public int SaleId { get; set; }
    }
}