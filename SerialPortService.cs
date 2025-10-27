using System;
using System.IO.Ports;
using SerialPortService;

namespace SerialPortService
{
    public class SerialPortService : ISerialPortService, IDisposable
    {
        private readonly SerialPort _serialPort;
        public event Action<string>? DataReceivedEvent;

        public SerialPortService(string portName)
        {
            _serialPort = new SerialPort(portName, 57600);
            _serialPort.DataReceived += DataReceived;
        }

        /// <summary>
        /// Open the serial port connection
        /// </summary>
        public void Connect()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                Console.WriteLine($"Serial port {_serialPort.PortName} connected");
            }
        }

        /// <summary>
        /// Close the serial port connection
        /// </summary>
        public void Disconnect()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Console.WriteLine($"Serial port {_serialPort.PortName} disconnected");
            }
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = _serialPort.ReadExisting();
            Console.WriteLine($"Received {data}");
            DataReceivedEvent?.Invoke(data);
        }

        /// <summary>
        /// Send data to serial port
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort._WriteLine(data);
            }
        }

        /// <summary>
        /// Receive data from serial port
        /// </summary>
        public void Receive()
        {
            if (_serialPort.IsOpen)
            {
                var data = _serialPort.ReadExisting();
                Console.WriteLine($"Buffer: {data}");
            }
        }
    
        public void Dispose()
        {
            Disconnect();
            _serialPort?.Dispose();
        }
    }
}