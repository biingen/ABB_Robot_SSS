using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Rs232Drv
{
    class ComPortFun
    {
        private SerialPort ComPortHandle = new SerialPort();
        private Queue ReceiveQueue = new Queue();
        public int WriteDataOut(byte[] InBuf, int DataLength)
        {
            if (ComPortHandle.IsOpen == true)
            {
                try
                {
                    ReceiveQueue.Clear();//clear input buffer
                    ComPortHandle.Write(InBuf, 0, DataLength);

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
        public int WriteDataOut(string InBuf, int DataLength)
        {
            if (ComPortHandle.IsOpen == true)
            {
                try
                {
                    ReceiveQueue.Clear();//clear input buffer

                    ComPortHandle.Write(InBuf);

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

        public int GetReciveData(int Len,ref byte[] retBuf)
        {
            int i,j;
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
        public int GetReceiveBufLen()
        {
            return (ReceiveQueue.Count);
        }
        private void ReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int i,j;
            byte[] InBuf = new byte[100];
            //byte tempByte = new byte();
            try
            {
                j = sp.BytesToRead;
                byte[] tempBuf = new byte[j];
                for (i = 0; i < j; j++)
                {
                    tempBuf[i] = (byte)sp.ReadByte();
                    //Console.Write("{0,2:X},", tempBuf[i]);
                    ReceiveQueue.Enqueue(tempBuf[i]);
                }

            }
            catch (Exception)
            {

            }




        }
        public int OpenPort(string PortNumber, int BaudRate, int ParityBit, int DataLen, int StopBit)
        {
            try
            {
                int i = 0;
                ComPortHandle.PortName = PortNumber;// "COM" + Convert.ToString(PortNumber);
                ComPortHandle.BaudRate = BaudRate;
                ComPortHandle.DataBits = DataLen;
                ComPortHandle.StopBits = (StopBits)(StopBit);
                ComPortHandle.Handshake = (Handshake)(i);
                ComPortHandle.Parity = (Parity)(ParityBit);
                ComPortHandle.ReadTimeout = 100;
                ComPortHandle.WriteTimeout = 100;
                ComPortHandle.ReadBufferSize = 1024;
                ReceiveQueue.Clear();
                ComPortHandle.Open();
                
                ComPortHandle.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
                //ComPortHandle
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
            ComPortHandle.Close();
            return 1;
        }

    }
}
