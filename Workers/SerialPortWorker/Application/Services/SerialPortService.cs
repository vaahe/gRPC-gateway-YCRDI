using System.IO.Ports;
using SerialPortWorker.Application.Interfaces;

namespace SerialPortWorker.Application.Services
{
    public class SerialPortService : ISerialPortService
    {
        private readonly SerialPort _serialPort;
        public event Action<string>? DataReceivedEvent;

        public SerialPortService(string portName, int baudRate = 57600)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += DataReceived;
        }

        /// <summary>
        /// Open the serial port connection
        /// </summary>
        public void Connect()
        {
            if (_serialPort.IsOpen) return;

            _serialPort.Open();
            Console.WriteLine($"Serial port {_serialPort.PortName} connected");
        }
        
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = _serialPort.ReadExisting();
            Console.WriteLine($"[SerialPort] Received: {data}");
            DataReceivedEvent?.Invoke(data);
        }

        /// <summary>
        /// Send data to serial port
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            if (!_serialPort.IsOpen) return;

            _serialPort.WriteLine(data);
            Console.WriteLine($"[SerialPort] Sent: {data}");
        }
    
        public void Dispose()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Console.WriteLine($"[SerialPort] Disconnected {_serialPort.PortName}");
            }

            _serialPort.Dispose();
        }
    }
}