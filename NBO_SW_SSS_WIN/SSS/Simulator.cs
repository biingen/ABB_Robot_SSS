using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cheese
{
    public partial class Simulator : Form
    {
        const int NORMAL_PACKET_COUNT = 8;
        const int ACK_PACKET_COUNT = 6;
        DEV_TPE_RK2797 rk2797 = new DEV_TPE_RK2797();
        ODM_BenQ_RS232 benQ = new ODM_BenQ_RS232();
        string updateStr = "";
        enum TxtBoxItem { receiving = 0, sending }

        Thread PcEnd_Thread = null, Processing_Thread = null;

        private delegate void dUpdateTbText(int myTbItem, string updateStr);
        dUpdateTbText updateTbText;// = new dUpdateTbText(UpdateTxtBoxText);

        public Simulator()
        {
            InitializeComponent();

            
            /*
            if (Processing_Thread == null)
            {
                Processing_Thread = new Thread(new ThreadStart(InstantDataProcessing));
                Processing_Thread.Start();
            }
            else if (Processing_Thread.IsAlive == false)
            {
                Processing_Thread.Abort();
                Processing_Thread = new Thread(new ThreadStart(InstantDataProcessing));
                Processing_Thread.Start();
            }*/

            
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
            List<byte> cmdByteList = new List<byte>();
            Queue<List<byte>> packetQueueList = new Queue<List<byte>>();

            string outputString = "";
            //rk2797.PacketAddedToQueue(GlobalData.m_SerialPort);
            GlobalData.m_SerialPort.PacketDequeuedToList(ref GlobalData.m_SerialPort.ReceiveList);
            
            outputString = RawData_Output(GlobalData.m_SerialPort.ReceiveList);

            updateTbText = new dUpdateTbText(UpdateTxtBoxText);
            int item_recv = (int)TxtBoxItem.receiving;
            int item_send = (int)TxtBoxItem.sending;
            updateTbText(item_recv, outputString);

            //rk2797.QueueAddedToList_DEV_TPE(ref GlobalData.m_SerialPort.ReceiveList, ref packetQueueList);
            //rk2797.ListDequeuedToParse_DEV_TPE(GlobalData.m_SerialPort);
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
            
            
        }

        private void button_Recv_Click(object sender, EventArgs e)
        {
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
            }*/
            List<byte> cmdByteList = new List<byte>();
            Queue<List<byte>> packetQueueList = new Queue<List<byte>>();

            string outputString = "";
            //rk2797.PacketAddedToQueue(GlobalData.m_SerialPort);
            while (GlobalData.m_SerialPort.ReceiveQueue.Count > 0)
                GlobalData.m_SerialPort.PacketDequeuedToList(ref GlobalData.m_SerialPort.ReceiveList);

            outputString = RawData_Output(GlobalData.m_SerialPort.ReceiveList);

            updateTbText = new dUpdateTbText(UpdateTxtBoxText);
            int item_recv = (int)TxtBoxItem.receiving;
            int item_send = (int)TxtBoxItem.sending;
            updateTbText(item_recv, outputString);

            byte chksum_calc = ChecksumCalculation.Mod256_Byte(GlobalData.m_SerialPort.ReceiveList);
            int numOfCount = GlobalData.m_SerialPort.ReceiveList.Count;
            //rk2797.QueueAddedToList_DEV_TPE(ref GlobalData.m_SerialPort.ReceiveList, ref packetQueueList);
            benQ.QueueAddedToList_ODM_BenQ(ref GlobalData.m_SerialPort.ReceiveList, ref packetQueueList);

            if (packetQueueList.Count > 0 && numOfCount >= ACK_PACKET_COUNT)
            {
                if (numOfCount == ACK_PACKET_COUNT && GlobalData.RS232_receivedText != "")
                {
                    outputString = RawData_Output(packetQueueList.ElementAt(0)) + "( " + GlobalData.RS232_receivedText + " )";
                    GlobalData.RS232_receivedText = "";
                }
                else if (numOfCount == ACK_PACKET_COUNT && GlobalData.RS232_receivedText != "")
                {
                    outputString = RawData_Output(packetQueueList.ElementAt(0)) + "( " + GlobalData.RS232_receivedText + " )";
                    GlobalData.RS232_receivedText = "";
                }
                else
                    outputString = RawData_Output(packetQueueList.ElementAt(0));                    
            }
            else if (packetQueueList.Count > 0 && numOfCount < ACK_PACKET_COUNT)
                outputString = "Packet length is not legal!";
            else if (packetQueueList.Count == 0 && GlobalData.Measure_Backlight != "")
            {
                outputString += " ( " + GlobalData.Measure_Backlight + " )";
                GlobalData.Measure_Backlight = "";
            }
            else
                outputString = "Packet data cannot be recognized!";

            updateTbText(item_send, outputString);
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

    public class ODM_BenQ_RS232
    {
        public bool dequeueResult = false;
        public void QueueAddedToList_ODM_BenQ(ref List<byte> packetList, ref Queue<List<byte>> packetQueueList)
        {   //test packet_Stop byte incoorect_: 40 42 07 08 01 00 00 02 03 50; _Checksum incorrect_: 40 42 07 08 01 00 00 02 FF 51
            const int NORMAL_PACKET_COUNT = 8;
            const int ACK_PACKET_COUNT = 6;
            if (packetList.Count >= NORMAL_PACKET_COUNT)
            {
                byte stx = packetList.ElementAt(0);
                if (stx == 0x42)
                {
                    byte packetLength = packetList.ElementAt(1);
                    if (packetLength == packetList.Count)
                    {
                        byte etx = packetList.ElementAt(packetLength - 1);
                        if (etx == 0x51)
                        {
                            List<byte> tempList = packetList.GetRange(2, packetLength - 4);
                            List<byte> fullList = new List<byte>();
                            byte chksumByte_packet = packetList.ElementAt(packetLength - 2);
                            bool chkResult = ChecksumCalculation.Mod256(packetLength, chksumByte_packet, tempList, ref fullList);
                            
                            if (chkResult)
                            {
                                packetQueueList.Enqueue(fullList);
                                if (fullList.Count > NORMAL_PACKET_COUNT)
                                    dequeueResult = ListDequeuedToParse_ODM_BenQ(ref packetQueueList, 6, packetLength - NORMAL_PACKET_COUNT);
                            }
                            else
                            {
                                packetQueueList.Enqueue(fullList);
                                MessageBox.Show("Checksum values are inconsistent!", "Reminding");
                            }
                            packetList.RemoveRange(0, packetLength);
                        }
                        else
                            //  (etx != 0x51)
                            //packetList.RemoveAt(packetLength - 1);
                            MessageBox.Show("Stop byte is not correct!", "Reminding");
                    }
                    else
                    //  (packetLength != packetList.Count)
                        packetList.RemoveAt(1);
                }
                else
                    //  (stx != 0x42)
                    packetList.RemoveAt(0);
            }
            //else if (packetList.Count >= 8)

            if (packetList.Count >= ACK_PACKET_COUNT && packetList.Count < NORMAL_PACKET_COUNT)
            {
                byte stx = packetList.ElementAt(0);
                if (stx == 0x42)
                {
                    byte packetLength = packetList.ElementAt(1);
                    if (packetLength == packetList.Count)
                    {
                        byte etx = packetList.ElementAt(packetLength - 1);
                        byte ack = packetList.ElementAt(packetLength - 3);
                        if (etx == 0x51)
                        {
                            List<byte> tempList = packetList.GetRange(2, packetLength - 4);
                            List<byte> fullList = new List<byte>();
                            byte chksumByte_packet = packetList.ElementAt(packetLength - 2);
                            bool chkResult = ChecksumCalculation.Mod256(packetLength, chksumByte_packet, tempList, ref fullList);
                            if (chkResult && ack == 0x55)
                            {
                                packetQueueList.Enqueue(fullList);
                                lock (this)
                                {
                                    GlobalData.RS232_receivedText = "Ack";
                                }
                                //packetList.RemoveRange(0, packetLength);
                            }
                            else if (chkResult && ack == 0xAA)
                            {
                                packetQueueList.Enqueue(fullList);
                                lock (this)
                                {
                                    GlobalData.RS232_receivedText = "Nack";
                                }
                                //packetList.RemoveRange(0, packetLength);
                            }
                            else
                            {
                                packetQueueList.Enqueue(fullList);
                                MessageBox.Show("Checksum values are inconsistent!", "Reminding");
                            }
                            /*
                            packetList.RemoveRange(0, packetLength);
                            if (fullList != null)
                                fullList = null;

                            if (packetQueueList != null)
                                packetQueueList.Dequeue();*/
                        }
                        else
                            //  (etx != 0x51)
                            //packetList.RemoveAt(packetLength - 1);
                            MessageBox.Show("Stop byte is not correct!", "Reminding");
                    }
                    else
                        //  (packetLength != packetList.Count)
                        packetList.RemoveAt(1);
                }
                else
                    //  (stx != 0x42)
                    packetList.RemoveAt(0);
            }
            //else if (packetList.Count >= 8)
        }

        public bool ListDequeuedToParse_ODM_BenQ(ref Queue<List<byte>> pktQueueList, int dataStartByte, int dataCount)
        {
            bool result = false;
            List<byte> tempList = new List<byte>();

            tempList = pktQueueList.Dequeue();
            tempList = tempList.GetRange(dataStartByte, dataCount);
            lock (this)
            {
                GlobalData.Measure_Backlight = RawData_Output(tempList);
            }

            return result;
        }

        public string RawData_Output(List<byte> data)
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

    }   //public class ODM_BenQ_RS232

}
