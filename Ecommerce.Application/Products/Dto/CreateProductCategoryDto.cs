﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products.Dto
{
    public class CreateProductCategoryDto
    {
        public string Name { get; init; }
        public string Image { get; init; }
        public decimal Price {get; init;}
    }
}
