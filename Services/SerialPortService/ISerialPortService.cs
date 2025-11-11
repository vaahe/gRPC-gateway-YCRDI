namespace SerialPortService
{
    public interface ISerialPortService
    {
        void Send(string data);
        void Receive();
    }
}