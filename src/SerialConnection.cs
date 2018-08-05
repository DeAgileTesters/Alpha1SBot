using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace DATBot
{
    public class SerialConnection
    {
        private static SerialPort _serialPort;
        private static Thread _readSerialDataThread;
        private static Thread _keepAliveThread;
        private static bool _mustReadData = true;
        private static Queue<byte> _recievedDataQueue = new Queue<byte>();
        private static DateTime lastReceivedMessage;
        private static int WaitForMessageReceived = 5;

        public static void StartConnection(bool enableReceivingData)
        {
            // Get settings
            string serialPort = Helpers.HelperMethods.ReadSettingFromConfigFile("serialPort");
            WaitForMessageReceived = Convert.ToInt32(Helpers.HelperMethods.ReadSettingFromConfigFile("WaitForMessageReceived"));

            // Start serial connection
            _serialPort = new SerialPort();
            _serialPort.PortName = serialPort;
            _serialPort.BaudRate = 115200;

            try
            {
                _serialPort.Open();

                // When receivingdata is expected, start listening threads
                if (enableReceivingData)
                {
                    _readSerialDataThread = new Thread(ReadSerialMessagesFromRobotReadThreadMethod);
                    _keepAliveThread = new Thread(KeepAliveThread);
                    _keepAliveThread.Start();
                    _readSerialDataThread.Start();
                }

                Console.WriteLine($"   Serialport {serialPort} opened. Connection made to Alpha 1S Robot.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"   Error opening Serialport {serialPort}. Error: {e.Message}");
                Environment.Exit(-1);
            }
        }

        public static void CloseConnection()
        {
            try
            {
                _mustReadData = false;
                if (_keepAliveThread != null) _keepAliveThread.Abort();
                if (_readSerialDataThread != null) _readSerialDataThread.Abort();
                _serialPort.Close();

                Console.WriteLine($"   Serialport disconnected");
            }
            catch (Exception e)
            {
                Console.WriteLine($"   Error closing connection {e.Message}");
            }

            //Console.ReadLine();
        }

        public static void SendSerialMessageToRobot(byte[] message)
        {
            _serialPort.Write(message, 0, message.Length);
        }

        private static void ReadSerialMessagesFromRobotReadThreadMethod()
        {
            while (_mustReadData)
            {
                int messageSize = _serialPort.BytesToRead;
                byte[] incommingData = new byte[messageSize];
                _serialPort.Read(incommingData, 0, incommingData.Length);
                incommingData.ToList().ForEach(b => _recievedDataQueue.Enqueue(b));
                ProcessIncommingData();
            }
        }
        private static void KeepAliveThread()
        {
            while (_mustReadData)
            {
                // When we do not receive data we are done
                if (lastReceivedMessage.Year != 1)
                {
                    TimeSpan difference = DateTime.Now.Subtract(lastReceivedMessage);
                    if (difference.Seconds > WaitForMessageReceived)
                    {
                        CloseConnection();
                    }
                }
            }
        }

        public static void ProcessIncommingData()
        {
            // if endCharacter is found, we can process the message
            if (_recievedDataQueue.Contains(Helpers.Constants.ENDCHARACTER))
            {
                // Check actions
                if (_recievedDataQueue.Contains(Helpers.Constants.LISTACTIONSRESPONSE))
                {
                    ListActionCommand.ReportData(_recievedDataQueue.ToArray<byte>());
                }

                if (_recievedDataQueue.Contains(Helpers.Constants.BATTERYSTATIS))
                {
                    BatteryStatusCommand.ReportData(_recievedDataQueue.ToArray<byte>());
                }

                // Update stats and clear received input
                lastReceivedMessage = DateTime.Now;
                _recievedDataQueue = new Queue<byte>();
            }
        }
    }
}
