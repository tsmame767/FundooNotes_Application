using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class EmailServiceBL:IEmailServiceBL
    {
        private readonly IEmailServiceRL serviceRL;

        public EmailServiceBL(IEmailServiceRL serviceRL)
        {
            this.serviceRL = serviceRL;
        }
        public Task<int> SendEmailAsync(string toEmail, string subject, string body)
        {
            return serviceRL.SendEmailAsync(toEmail, subject, body);
        }
    }
}
