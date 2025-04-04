using LicenseManager.Domain.Interfaces;

namespace LicenseManager.Infrastructure.Helpers;

public class CertificateGetter : IHostedService, IDisposable
{
    private Timer _timer = null!;
    private readonly ICertificateHelper _certificateHelper;

    public CertificateGetter(ICertificateHelper certificateHelper)
    {
        _certificateHelper = certificateHelper;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        //_timer = new Timer(GetCertificate!, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    private void GetCertificate(object state)
    {
        _certificateHelper.GetCertificateFromLicenseService();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}