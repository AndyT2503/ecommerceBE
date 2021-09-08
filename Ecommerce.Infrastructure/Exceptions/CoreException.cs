﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException() : base() { }

        public CoreException(string message) : base(message)
        {
        }
    }
}
