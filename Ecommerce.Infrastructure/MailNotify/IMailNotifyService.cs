﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.MailNotify
{
    public interface IMailNotifyService
    {
        Task SendMailAsync(string email, string subject, string htmlMessage);
    }
}
