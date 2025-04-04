using LicenseService.Domain.Interfaces;

namespace LicenseService.Domain.Helpers;

public class WatchdogHelper : IWatchdogHelper
{
    private readonly IEmailService _emailService;

    public WatchdogHelper(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public void CheckConnection()
    {
        using var httpClient = new HttpClient();
        try
        {
            _ = httpClient.GetAsync("https://localhost:44356/watchdog").Result;
        }
        catch (Exception)
        {
            _emailService.SendWatchdogEmail();
        }
    }
}