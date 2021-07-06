using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Text;

namespace Selector
{
    

    public class SelectorControl
    {

        private byte[] _chanals = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120 };
        SerialPort _comPort;
        private bool _isConnected;
        private string _portName;

        public SelectorControl()
        {
            _comPort = null;
            _isConnected = false;
        }

        public bool IsConnected => _isConnected;
        public string NameComPort
        {
            get { return _portName; }
            set 
            {
                if (!IsConnected)
                {
                    _portName = value;
                }
            }
        }


        public string[] SearchComPorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool Connect()
        {
            if (_comPort == null)
            {
                _comPort = new SerialPort(_portName, 115200, Parity.None, 8, StopBits.One);
                _comPort.WriteTimeout = 3000;
                _comPort.ReadTimeout = 3000;
            }
            else
                Disconnect();
            try
            {
                _comPort.PortName = _portName;
                _comPort.Open();

                _isConnected = TestConnection();
            }
            catch (Exception ex)
            {
                
                _isConnected = false;
            }
            return _isConnected;
        }


        public void Disconnect()
        {
            if (_comPort != null)
            {
                _comPort.Close();
                _isConnected = false;
            }
        }

        

        private bool TestConnection()
        {
            int byteReadNumber = 1;
            byte[] bytesToWrite = {200};
            byte[] bytesToRead = new byte[byteReadNumber];

            try
            {
                _comPort.Write(bytesToWrite, 0, 1);
                _comPort.Read(bytesToRead, 0, byteReadNumber);
            }
            catch (Exception)
            {
                return false; 
            }
            


            if (!(bytesToWrite[0] == bytesToRead[0]))
            {
                return false;
            }

            return true;
        }

        public void ActivationSHDN(bool activate)
        {
            int byteReadNumber = 1;
            byte[] bytesToWrite = { 5, 0 };
            byte[] bytesToRead = new byte[byteReadNumber];


            try
            {
                _comPort.Write(bytesToWrite, 0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Write error", ex);
            }

            try
            {
                _comPort.Read(bytesToRead, 0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Read error", ex);
            }

            if (activate)
            {
                bytesToWrite[1] = 1;
                try
                {
                    _comPort.Write(bytesToWrite, 1, 1);
                }
                catch (Exception ex)
                {
                    throw new Exception("Write error", ex);
                }
                
            }
            else
            {
                try
                {
                    _comPort.Write(bytesToWrite, 1, 1);
                }
                catch (Exception ex)
                {
                    throw new Exception("Write error", ex);
                }
               
            }
        }

        /// <summary>
        /// Активация выбранного канала Селектора
        /// </summary>
        /// <param name="channal">номер канала от 0 до 11 (0 - это 1-ый канал...11 - это 12-ый канал)</param>
        public void ActivationChannels(int channal)
        {

            int N = 1;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = _chanals[channal];

            try
            {
                _comPort.Write(bytesToWrite, 0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Write error", ex);
            }
            try
            {
                _comPort.Read(bytesToRead, 0, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Read error", ex);
            }
            
        }
    }
}
