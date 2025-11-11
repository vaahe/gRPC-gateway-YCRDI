using SerialPortWorker;
using SerialPortWorker.Application.Interfaces;
using SerialPortWorker.Application.Services;

Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
{
   var portName = context.Configuration["SerialPort:PortName"] ?? "COM3";
   var baudRate = int.TryParse(context.Configuration["SerialPort:BaudRate"], out var baudRateValue) ? baudRateValue : 57600;

   services.AddSingleton<ISerialPortService>(_ => new SerialPortService(portName, baudRate));
   services.AddHostedService<Worker>();
})
.Build()
.Run();