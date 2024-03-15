using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IEmailServiceRL
    {
        Task<int> SendEmailAsync(string to, string subject, string htmlMessage);
    }
}
