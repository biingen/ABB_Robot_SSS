using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ModuleLayer;

namespace Cheese
{
    public partial class Simulator : Form
    {
        const int NORMAL_PACKET_COUNT = 8;
        const int ACK_PACKET_COUNT = 6;
        DEV_TPE_RK2797 rk2797 = new DEV_TPE_RK2797();
        ODM_BenQ_RS232 benQ = new ODM_BenQ_RS232();
        List<byte> cmdByteList = new List<byte>();
        Queue<List<byte>> packetQueueList = new Queue<List<byte>>();
        string outputString = "";
        int numOfCount = GlobalData.m_SerialPort.QueueLength();
        
        enum TxtBoxItem { receiving = 0, sending }
        int item_recv = (int)TxtBoxItem.receiving;
        int item_send = (int)TxtBoxItem.sending;

        Thread PcEnd_Thread = null, Processing_Thread = null;

        private delegate void dUpdateTbText(int myTbItem, string updateStr);
        dUpdateTbText updateTbText;// = new dUpdateTbText(UpdateTxtBoxText);

        public Simulator()
        {
            InitializeComponent();

        }

        private void UpdateTxtBoxText(int myTbItem, string updateStr)
        {
            //lock(this)
            {
                switch (myTbItem)
                {
                    case 0:
                        this.textBox_receive.Text = updateStr;
                        break;
                    case 1:
                        this.textBox_send.Text = updateStr;
                        break;
                }
            }
        }

        private void Log_PcEnd()
        {
            while (GlobalData.m_SerialPort.IsOpen())
            {
                if (GlobalData.m_SerialPort.GetRxBytes() > 0)
                {
                    PacketDequeuedToList(GlobalData.m_SerialPort, ref cmdByteList);
                    outputString = RawData_Output(cmdByteList);

                    //byte chksum_calc = ChecksumCalculation.Mod256_Byte(cmdByteList);
                    //rk2797.QueueAddedToList_DEV_TPE(ref GlobalData.m_SerialPort.ReceiveList, ref packetQueueList);
                    benQ.QueueAddedToList_ODM_BenQ(ref cmdByteList, ref packetQueueList);

                    //rk2797.ListDequeuedToParse_DEV_TPE(GlobalData.m_SerialPort);
                }
            }
        }

        private void InstantDataProcessing()
        {
            /*
            updateTbText = new dUpdateTbText(UpdateTxtBoxText);
            int item_recv = (int)TxtBoxItem.receiving;
            int item_send = (int)TxtBoxItem.sending;
            updateTbText(item_recv, GlobalData.RS232_receivedText);
            
            //{ STX, Len, ID, Group, Cmd, Sub-Cmd, Data, Chksum, ETX }
            //byte[] replyData = { 0x42, 0x09, 0x01, 0xAA, 0x02, 0x02, 0x00, 0xAF, 0x51 };
            
            string outputCmdString = "06 01 E0 11 03";
            string xorByte = Algorithm.Medical_XOR8(outputCmdString);
            string outputCmd = outputCmdString + xorByte;
            byte[] outputCmdByte = new byte[outputCmd.Split(' ').Count()];
            outputCmdByte = HexConverter.StrToByte(outputCmd);      //06 01 E0 0D 64 8E     //06 01 E0 11 03 F5
            GlobalData.m_SerialPort.WriteDataOut(outputCmdByte, outputCmdByte.Length);
            //string outputString = "STX\tLen\tID\tGroup\tCmd\tSub-Cmd\tData\tChksum\tETX\r\n" + RawData_Output(replyData);
            updateTbText( item_send, outputCmd);*/
        }

        private string RawData_Output(byte[] data)
        {
            string HexString = "";
            if (data != null)
            {
                foreach (byte sum in data)
                {
                    HexString += (sum.ToString("X2"));
                    HexString += "\t";
                }
            }
            return HexString;
        }

        private string RawData_Output(List<byte> data)
        {
            string HexString = "";
            int i = 0;
            if (data != null)
            {
                foreach (byte sum in data)
                {
                    HexString += (sum.ToString("X2"));
                    if (i < data.Count - 1)
                        HexString += " ";

                    i++;
                }
            }

            return HexString;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            updateTbText = new dUpdateTbText(UpdateTxtBoxText);
            string updateString = "";
            
            
            //Invoke(updateTbText, 0, updateString);
            //Invoke(updateTbText, 1, updateString);
            updateTbText(0, updateString);
            updateTbText(1, updateString);
            //textBox_receive.Text = "";
            //textBox_send.Text = "";
        }

        private void Simulator_Load(object sender, EventArgs e)
        {
            if (PcEnd_Thread == null)
            {
                PcEnd_Thread = new Thread(new ThreadStart(Log_PcEnd));
                PcEnd_Thread.Start();
            }
            else if (PcEnd_Thread.IsAlive == false)
            {
                PcEnd_Thread.Abort();
                PcEnd_Thread = new Thread(new ThreadStart(Log_PcEnd));
                PcEnd_Thread.Start();
            }
        }

        private void button_Recv_Click(object sender, EventArgs e)
        {
            updateTbText = new dUpdateTbText(UpdateTxtBoxText);
            /*
            if (PcEnd_Thread == null)
            {
                PcEnd_Thread = new Thread(new ThreadStart(Log_PcEnd));
                PcEnd_Thread.Start();
            }
            else if (PcEnd_Thread.IsAlive == false)
            {
                PcEnd_Thread.Abort();
                PcEnd_Thread = new Thread(new ThreadStart(Log_PcEnd));
                PcEnd_Thread.Start();
            }
            */
            //Log_PcEnd();
            
            updateTbText(item_recv, outputString);
            //outputString = "";
            
            if (packetQueueList.Count > 0)// && numOfCount >= ACK_PACKET_COUNT)
            {
                //if (numOfCount == ACK_PACKET_COUNT && GlobalData.RS232_receivedText != "")
                if (GlobalData.RS232_receivedText != "")
                {
                    //outputString = RawData_Output(packetQueueList.ElementAt(0)) + "( " + GlobalData.RS232_receivedText + " )";
                    outputString = "(" + GlobalData.RS232_receivedText + ")";
                    GlobalData.RS232_receivedText = "";
                }
                //else if (numOfCount < ACK_PACKET_COUNT)
                    //outputString = "Packet length is not legal!";
                else
                    outputString = RawData_Output(packetQueueList.ElementAt(0));

                packetQueueList.Dequeue();
                if (cmdByteList.Count > 0)
                    cmdByteList.Clear();
            }
            else if (packetQueueList.Count == 0 && GlobalData.Measure_Backlight != "")
            {
                outputString += " (" + GlobalData.Measure_Backlight + ")";
                GlobalData.Measure_Backlight = "";
            }
            else
                outputString = "Packet data cannot be recognized!";

            updateTbText(item_send, outputString);
            outputString = "";
        }

        private void PacketDequeuedToList(Mod_RS232 spHandle, ref List<byte> ReceiveList)
        {
            while (spHandle.QueueLength() > 0)
            {
                byte serial_byte = spHandle.GeneralDequeue();
                ReceiveList.Add(serial_byte);
            }
        }

        private void Simulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            updateTbText = null;

            if (PcEnd_Thread.IsAlive == true)
            {
                PcEnd_Thread.Abort();
            }

            if (Processing_Thread.IsAlive == true)
            {
                Processing_Thread.Abort();
            }
        }
    }

    

}
