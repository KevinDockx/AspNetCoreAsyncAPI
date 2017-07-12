using Filenet.Apis.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Filenet.Apis.Services
{

    
    public class LocalMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        //public async Task Send(string subject, string message)
        //{
        //    // send mail - output to debug window
        //    Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService.");
        //    Debug.WriteLine($"Subject: {subject}");
        //    Debug.WriteLine($"Message: {message}");
        //}


        

        public async Task SendEmailAsync(string email, string subject, string message)
        {

            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Peter Santiago", "peter.santiago@its.ny.gov"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                client.LocalDomain = "mail.svc.ny.gov";
                await client.ConnectAsync("mail.svc.ny.gov", 587, SecureSocketOptions.None).ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }




    }
}

