using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using Microsoft.Win32.SafeHandles;           //support SafeFileHandle
using System.Runtime.InteropServices;      //support DIIImport
using System.Reflection;                                //support BindingFlags
using jini;
using Cheese;

namespace ModuleLayer
{
    public class Mod_RS232
    {
        //private int iPortNmuber = 0;
        private SerialPort _SerialPortHandle = new SerialPort();
        private Stream _internalSerialStream;
        public Queue<byte> ReceiveQueue = new Queue<byte>();
        public List<byte> ReceiveList = new List<byte>();
        public Queue<List<byte>> ReceiveQueueList = new Queue<List<byte>>();
        
        //=== main Read function ===//
		public int ReadTerm(Byte[] ResultDataBuf, ref int Count, char TermByte)
        {
            int DataLen = _SerialPortHandle.BytesToRead;
            Count = 0;

            if (DataLen >= 1)
            {
                for (int i = 0; i <= (DataLen - 1); i++)
                {
                    
                    ResultDataBuf[i] = (Byte)_SerialPortHandle.ReadByte();
                    Count++;

                    if (ResultDataBuf[i] == TermByte)
                    {
                        Array.Resize(ref ResultDataBuf, (i+1));
                        return 1;
                    }
                }

                return -1;
            }
            return -1;

        }
		
        public int ReadDataIn(Byte[] inBuf, int Length)
        {
            try
            {
                _SerialPortHandle.Read(inBuf, 0, Length);
                for (int i = 0; i < Length; i++)
                    ReceiveQueue.Enqueue(inBuf[i]);
            }
            catch (System.TimeoutException)
            {//Time out
                return -1;
            }
            catch (System.ArgumentException)
            {
                return -1;
            }
            catch (System.IO.IOException)
            {//Port number error
                return -1;
            }

            return 1;
        }
        public int GetRxBytes()
        {
            return (_SerialPortHandle.BytesToRead);
        }
        public void Receive()
        {
            int data_to_read = GetRxBytes();
            if (data_to_read > 0)
            {
                byte[] dataset = new byte[data_to_read];
                ReadDataIn(dataset, data_to_read);
            }
        }

        //=== Used along with DataReceived Event ===//
        public void ReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int i, j;
            byte[] InBuf = new byte[512];
            //byte tempByte = new byte();
            try
            {
                j = _SerialPortHandle.BytesToRead;
                //j = sp.BytesToRead;
                byte[] tempBuf = new byte[j];
                for (i = 0; i < j; j++)
                {
                    tempBuf[i] = (byte)_SerialPortHandle.ReadByte();
                    //Console.Write("{0,2:X},", tempBuf[i]);
                    ReceiveQueue.Enqueue(tempBuf[i]);
                }

            }
            catch (Exception)
            {

            }
        }

        public void DataReceivedByEvent(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;   //this is used with Forms-Serialport component inserted
            try
            {
                if (sp.IsOpen)
                {
                    byte[] byteRead = new byte[sp.BytesToRead];
					//it must use ReadLine() to do carriage-return and line-feed instead of using /r/n.
                    string dataValue = sp.ReadLine(); 
                    if (dataValue.Contains("io i"))
                        GlobalData.Arduino_Read_String = dataValue;

                    int recByteCount = byteRead.Count();
                    if (recByteCount > 0)
                        GlobalData.Arduino_recFlag = true;
                    else
                        GlobalData.Arduino_recFlag = false;

                    sp.DiscardInBuffer();                      //Clear buffer of SerialPort component
                }
                else
                {
                    Console.WriteLine("Arduino serialport is not opened!");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message, "DataReceivedEvent error!");
            }
        }
		
		//=== main Write function ===//
        public int WriteDataOut(string InBuf, int DataLength)
        {
            if (_SerialPortHandle.IsOpen == true)
            {
                try
                {
                    ReceiveQueue.Clear();   //clear input buffer
                    //_SerialPortHandle.Write(InBuf);
                    _SerialPortHandle.WriteLine(InBuf);     //for IO cmd doing \r\n
                }
                catch (System.ArgumentException)
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
            return -1;
        }
        public int WriteDataOut(char[] InBuf, int DataLength)
        {

            if (_SerialPortHandle.IsOpen == true)
            {
                try
                {
                    ReceiveQueue.Clear();   //clear input buffer
					_SerialPortHandle.DiscardInBuffer();
					_SerialPortHandle.DiscardOutBuffer();
                    _SerialPortHandle.Write(InBuf, 0, DataLength);
                }
                catch (System.ArgumentException)
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
            return -1;
        }
		
		public int WriteDataOut(byte[] InBuf, int DataLength)
        {
            if (_SerialPortHandle.IsOpen == true)
            {
                try
                {
                    _SerialPortHandle.DiscardInBuffer();
                    _SerialPortHandle.DiscardOutBuffer();
                    _SerialPortHandle.Write(InBuf, 0, DataLength);
                }
                catch (System.ArgumentException)
                {
                    return -1;
                }
            }
            else
                return -1;
            
            return -1;
        }
		
		public int GetDataFromQueue(int Len, ref byte[] retBuf)
        {
            int i, j;
            if ((ReceiveQueue.Count <= 0) || (retBuf.Length < Len))
            {
                return -1;
            }
            else
            {
                try
                {
                    if (Len > ReceiveQueue.Count)
                    {
                        j = ReceiveQueue.Count;
                    }
                    else
                    {
                        j = Len;
                    }
                    Console.Write("\nInBuf:");

                    for (i = 0; i <= (j - 1); i++)
                    {
                        retBuf[i] = (byte)ReceiveQueue.Dequeue();
                        Console.Write("{0,2:X},", retBuf[i]);
                    }
                    Console.Write("\n");
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 1;
        }

        public void PacketDequeuedToList(ref List<byte> byteList)
        {
            while (ReceiveQueue.Count > 0)                   // Queue有資料就收取
            {
                //  Queue一個byte一個byte取出來被丟入List
                byte serial_byte = (byte)ReceiveQueue.Dequeue();
                ReceiveList.Add(serial_byte);
                //byteList = ReceiveList;                 // Queue debug list content
            }
        }

        public int SpecificDequeue(int Len, ref byte[] retBuf)
        {
            int i, j;
            if ((ReceiveQueue.Count <= 0) || (retBuf.Length < Len))
            {
                return -1;
            }
            else
            {
                try
                {
                    if (Len > ReceiveQueue.Count)
                    {
                        j = ReceiveQueue.Count;
                    }
                    else
                    {
                        j = Len;
                    }
                    Console.Write("\nInBuf:");

                    for (i = 0; i <= (j - 1); i++)
                    {
                        retBuf[i] = (byte)ReceiveQueue.Dequeue();
                        Console.Write("{0,2:X},", retBuf[i]);
                    }
                    Console.Write("\n");
                }
                catch (Exception)
                {
                    return 1;
                }

            }
            return 1;
        }
        public byte GeneralDequeue()
        {
            return ((byte)ReceiveQueue.Dequeue());
        }
        public int ReceivedBufferLength()
        {
            return (ReceiveQueue.Count);
        }
		
        public int OpenSerialPort(string port_Name, string port_BR)
        {
            try
            {
                if (_SerialPortHandle.IsOpen == false)
                {
                    _SerialPortHandle.PortName = port_Name;
                    _SerialPortHandle.BaudRate = int.Parse(port_BR);
                    _SerialPortHandle.DataBits = 8;
                    _SerialPortHandle.StopBits = StopBits.One;
                    /*
                    string stopbits = ini12.INIRead(GlobalData.MainSettingPath, "Port A", "StopBits", "");
                    switch (stopbits)
                    {
                        case "One":
                            _SerialPortHandle.StopBits = StopBits.One;
                            break;
                        case "Two":
                            _SerialPortHandle.StopBits = StopBits.Two;
                            break;
                    }
                    */
                    _SerialPortHandle.Handshake = Handshake.None;
                    _SerialPortHandle.Parity = Parity.None;
                    _SerialPortHandle.ReadTimeout = 2000;
                    _SerialPortHandle.WriteTimeout = 100;
                    _SerialPortHandle.ReadBufferSize = 1024;
                    ReceiveQueue.Clear();
                    _SerialPortHandle.Open();
                    Console.WriteLine("[Mod_RS232] " + _SerialPortHandle.PortName + " is successfully opened.");

                    //_SerialPortHandle.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
                }
                else
                {
                    Console.WriteLine("[Mod_RS232] " + _SerialPortHandle.PortName + " is not yet opened.");
                    return -3;
                }
            }
            catch (System.IO.IOException)
            {   //Port number error
                return -1;
            }
            catch (System.UnauthorizedAccessException)
            {   //Port is used by another application
                return -2;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Mod_RS232]" + e.Message);
            }

            return 1;
        }

        public int OpenPort_Arduino(string port_Name, string port_BR = "9600")
        {
            try
            {
                if (_SerialPortHandle.IsOpen == false)
                {
                    _SerialPortHandle.PortName = port_Name;
                    _SerialPortHandle.BaudRate = int.Parse(port_BR);
                    _SerialPortHandle.DataBits = 8;
                    _SerialPortHandle.StopBits = StopBits.One;
                    _SerialPortHandle.Handshake = Handshake.None;
                    _SerialPortHandle.Parity = Parity.None;
                    _SerialPortHandle.ReadTimeout = 2000;
                    _SerialPortHandle.WriteTimeout = 100;
                    _SerialPortHandle.ReadBufferSize = 1024;
                    ReceiveQueue.Clear();
                    _SerialPortHandle.Open();
                    Console.WriteLine("[Mod_RS232] " + _SerialPortHandle.PortName + " is successfully opened.");

                    _SerialPortHandle.DataReceived += new SerialDataReceivedEventHandler(DataReceivedByEvent);
                }
                else
                {
                    Console.WriteLine("[Mod_RS232] " + _SerialPortHandle.PortName + " is not yet opened.");
                    return -3;
                }
            }
            catch (System.IO.IOException)
            {   //Port number error
                return -1;
            }
            catch (System.UnauthorizedAccessException)
            {   //Port is used by another application
                return -2;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Mod_RS232]" + e.Message);
            }

            return 1;
        }

        public int OpenPort(string PortNumber, int BaudRate, int ParityBit, int DataLen, int StopBit)
        {
            try
            {
                int i = 0;
                _SerialPortHandle.PortName = PortNumber;
                _SerialPortHandle.BaudRate = BaudRate;
                _SerialPortHandle.DataBits = DataLen;
                _SerialPortHandle.StopBits = (StopBits)(StopBit);
                _SerialPortHandle.Handshake = (Handshake)(i);
                _SerialPortHandle.Parity = (Parity)(ParityBit);
                _SerialPortHandle.ReadTimeout = 100;
                _SerialPortHandle.WriteTimeout = 100;
                _SerialPortHandle.ReadBufferSize = 1024;
                ReceiveQueue.Clear();
                _SerialPortHandle.Open();
                 
                _SerialPortHandle.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
            }
            catch (System.IO.IOException)
            {//Port number error
                return -1;
            }
            catch (System.UnauthorizedAccessException)
            {//Port is used by  another APP
                return -2;
            }
            return 1;
        }

        public int ClosePort()
        {
            _SerialPortHandle.Dispose();    //added by YFC
            _SerialPortHandle.Close();
			
            return 1;
        }

        public bool IsOpen()
        {
            if (_SerialPortHandle.IsOpen == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
		
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes,
                                                            uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        private SafeFileHandle handle_Com = null;
        //private static object stream = typeof(SerialPort).GetField("internalSerialStream", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_ComPort);
        //private static SafeFileHandle handle_Com = (SafeFileHandle)stream.GetType().GetField("_handle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stream);

        public SafeFileHandle Handle
        {
            get
            {
                object stream = typeof(SerialPort).GetField("internalSerialStream", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_SerialPortHandle);
                // If the handle is valid, return it.
                if (!handle_Com.IsInvalid)
                {
                   
                    handle_Com = (SafeFileHandle)stream.GetType().GetField("_handle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stream);
                    return handle_Com;
                }
                else
                    return null;
            }
        }

    }
}
