using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Smtp;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;

namespace LicenseService.Domain.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSender _sender;

    public EmailService()
    {
        _sender = new SmtpSender(() => new SmtpClient("localhost")
        {
            // data via appsettings.jsno
            // for demo otherwise true for production 
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 25
        });
    }

    public async void SendUpdateLicenseAmountEmail(License license)
    {
        Email.DefaultSender = _sender;
        await Email
            .From("tbwb@tbwb.nl")
            .To("klant@test.com", $"{license.Company}")
            .Subject($"Update users to: {license.ActualAmount} users")
            .Body($"Updated the users to {license.ActualAmount}")
            .SendAsync();
    }

    public async void SendUpdatePayment(License license)
    {
        Email.DefaultSender = _sender;
        await Email
            .From("administration@tbwb.nl")
            .To("klant@test.com", "Klant")
            .Subject($"Approved payment on: {license.LastPayment}")
            .Body(
                $"Payment is processed successfully on {license.LastPayment}, certificate is valid for another moth")
            .SendAsync();
    }

    public async void SendWatchdogEmail()
    {
        Email.DefaultSender = _sender;
        await Email
            .From("service@tbwb.nl")
            .To("service@test.com", "Test")
            .Subject("No connection with LicenseManager")
            .Body(
                "There is nog connection between the LicenseService and LicenseManager")
            .SendAsync();
    }
}
