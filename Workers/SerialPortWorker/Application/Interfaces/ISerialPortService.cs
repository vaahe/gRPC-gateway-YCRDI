namespace SerialPortWorker.Application.Interfaces
{
    public interface ISerialPortService : IDisposable
    {
        event Action<string> DataReceivedEvent;
        void Connect();
    }
}