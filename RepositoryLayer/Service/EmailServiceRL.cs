using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;

namespace RepositoryLayer.Service
{
    public class EmailServiceRL: IEmailServiceRL
    {


        private readonly EmailSettings _emailSettings;

        public EmailServiceRL(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings=emailSettings.Value;
        }

         
        public async Task<int> SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var res = 0;
            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.FromEmail),
                        Subject = subject,
                        Body = htmlMessage,
                        IsBodyHtml = true,

                    };
                    mailMessage.To.Add(to);

                    await client.SendMailAsync(mailMessage);
                    res = 1;
               
                }
            }
            catch(Exception)
            {
                res = 0;
            }
            finally
            {
                //
            }
            return res;
        }
    }
}
