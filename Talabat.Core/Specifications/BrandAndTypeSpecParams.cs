﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class BrandAndTypeSpecParams
    {
        private const int MaxPageSize = 5;
        private int pageSize = 3;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;
       
       
    }
}
