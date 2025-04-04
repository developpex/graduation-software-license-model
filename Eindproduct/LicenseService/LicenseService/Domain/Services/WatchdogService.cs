using LicenseService.Domain.Interfaces;

namespace LicenseService.Domain.Services;

public class WatchdogService : IHostedService, IDisposable
{
    private readonly IWatchdogHelper _watchdogHelper;
    private Timer _timer = null!;

    public WatchdogService(IWatchdogHelper watchdogHelper)
    {
        _watchdogHelper = watchdogHelper;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckConnection, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    private void CheckConnection(object? state)
    {
        _watchdogHelper.CheckConnection();
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