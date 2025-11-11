using SerialPortWorker.Application.Interfaces;

namespace SerialPortWorker;

public class Worker(ILogger<Worker> logger, ISerialPortService serialPortService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SerialPortWorker started...");

        serialPortService.Connect();
        serialPortService.DataReceivedEvent += (data) =>
        {
            logger.LogInformation("Received data: {data}", data);
        };
            
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(5000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        serialPortService.Dispose();
        base.Dispose();
    }
}
