using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICustomEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlBody);
    }
}
