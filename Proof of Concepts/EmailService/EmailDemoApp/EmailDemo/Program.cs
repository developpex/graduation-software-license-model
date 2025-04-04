using System;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core;
using FluentEmail.Smtp;

namespace EmailDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var sender = new SmtpSender(() => new SmtpClient("localhost")
            {
                // data via appsettings.jsno
                // for demo otherwise true for production 
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25
                
            });

            Email.DefaultSender = sender;

            var email = await Email
                .From("patrick.van.nieuwburg@tbwb.nl")
                .To("test@test.com", "Test")
                .Subject("Tanks!")
                .Body("Thanks for testing the email")
                .SendAsync();
        }
    }
}
