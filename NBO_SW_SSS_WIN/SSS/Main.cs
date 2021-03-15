using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModuleLayer;
using Camera_NET;
using DirectShowLib;
using System.Net.Sockets;
using System.Timers;
using System.Net.Mail;
using System.Net.Mime;

namespace SSS
{
    public partial class Main : Form
    {
        string TargetFilePath;
        public int FlagComPortStauts;
        int FlagPause, FlagStop, loopTimes = 0, loopCounter = 0;
        static string Cmdsend, Cmdreceive;
        int Device, Resolution;
        public double timeout;
        public static double tmout = 0.0;
        public TimeoutTimer timeOutTimer;
        Thread SerialPort_Receive_Thread;

        // ----------------------------------------------------------------------------------------------- //
        private static CameraChoice cameraChoice = new CameraChoice();
        private static CameraControl cameraControl = new CameraControl();
        // ----------------------------------------------------------------------------------------------- //
        DataTypeConversion ProcessStr = new DataTypeConversion();
        //Mod_RS232 SerialPortHandle = new Mod_RS232();
        static Mod_TCPIP_Client NetworkHandle = new Mod_TCPIP_Client();
        Thread ExecuteCmdThreadHandle;
        // ----------------------------------------------------------------------------------------------- //
        public DataGridView tempDataGrid;
        private delegate void dUpdateDataGrid(int x, int y, string data);
        private delegate void ProcessLoopText(int Cmd, ref int result);
        public delegate void dUpdateUI(int status);
        public delegate void dUpdateUIBtn(int Btn, int Status);

        // ----------------------------------------------------------------------------------------------- //
        private void UpdateUiData(int x, int y, string data)
        {
            int i;
            if (y == -1)
            {
                //add one row
                this.dataGridView1.Rows.Add();
                i = this.dataGridView1.Rows.Count;
                Console.WriteLine("Current Rows Count:" + i.ToString());
            }
            else if (y == -2)
            {
                this.dataGridView1.Rows.Clear();
            }
            else if (y == -3)
            {
                this.dataGridView1.Refresh();
            }
            else if (y == -4)
            {
                this.dataGridView1.FirstDisplayedScrollingRowIndex = x;
            }
            else if (y == -5)
            {//unslect target row
                this.dataGridView1.Rows[x].Selected = false;
                //this.dataGridView1.Refresh();
            }
            else if (y == -6)
            {//slect target row
                this.dataGridView1.Rows[x].Selected = true;
            }
            else if (y == -7)
            {
                this.dataGridView1.Refresh();
            }
            else
            {
                this.dataGridView1.Rows[y].Cells[x].Value = data;
            }
            
        }
        
        public void Form1UpdateArduinoLedStatus(int status)
        {
            if (status == 1)
            {
                this.PIC_Arduino.Image = ImageResource.GleenLed;
            }
            else
            {
                this.PIC_Arduino.Image = ImageResource.BlackLED;
            }
        }
        public void Form1UpdateComportLedStatus(int status)
        {
            if (status == 1)
            {
                this.PIC_ComPortStatus.Image = ImageResource.GleenLed;
            }
            else
            {
                this.PIC_ComPortStatus.Image = ImageResource.BlackLED;
            }
        }
        public void Form1UpdateNetworkLedStatus(int status)
        {
            if (status == 1)
            {
                this.PIC_NetworkStatus.Image = ImageResource.GleenLed;
            }
            else
            {
                this.PIC_NetworkStatus.Image = ImageResource.BlackLED;
            }
        }
        // ---------------------------------- Arduino parameter ---------------------------------- //
        string Read_Arduino_Data = "";
        bool serial_receive = false;
        // ----------------------------------------------------------------------------------------------- //
        public Main()
        {
            InitializeComponent();
            tempDataGrid = this.dataGridView1;
            FlagComPortStauts = 0;
            this.VerLabel.Text = "Version: 006.001";
            FlagPause = 0;
            FlagStop = 0;

            //GlobalData.sp_Arduino.m_ArduinoOpenLED = new Mod_RS232.dArduinoOpenLED(Form1UpdateArduinoLedStatus);
        }
        private void UpdateUIBtnFun(int Btn,int Status)
        {
            switch (Btn)
            {
                case 0://Start BTN
                    if (Status == 1)
                    {
                        this.BTN_StartTest.Enabled = true;
                    }
                    else
                    {
                        this.BTN_StartTest.Enabled = false;
                    }
                    break;
                case 1://Pause BTN
                    if (Status == 1)
                    {
                        this.BTN_Pause.Enabled = true;
                    }
                    else
                    {
                        this.BTN_Pause.Enabled = false;
                    }
                    break;
                case 2://Stop BTN
                    if (Status == 1)
                    {
                        this.BTN_Stop.Enabled = true;
                    }
                    else
                    {
                        this.BTN_Stop.Enabled = false;
                    }
                    break;
                case 3://Update status picture box
                    if (Status == 1)
                    {
                        //this.
                        this.Picbox_CurrentStyatus.Image = ImageResource.Testing;
                    }
                    else if (Status == 2)
                    {
                        //this.
                        this.Picbox_CurrentStyatus.Image = ImageResource.pause;
                    }
                    else if (Status == 3)
                    {
                        //this.
                        this.Picbox_CurrentStyatus.Image = ImageResource.Finish;
                    }
                    else
                    {
                        this.Picbox_CurrentStyatus.Image = null;

                    }
                    //this.Picbox_CurrentStyatus.Refresh();
                    break;

            }
            this.Refresh();
        }
        private void UpdateLoopTxt(int Cmd,ref int result)
        {
            if (Cmd == 0)
            {
                this.Txt_LoopTimes.Enabled = false;
                this.Txt_LoopCounter.Visible = false;
            }
            else if (Cmd == 1)
            {
                this.Txt_LoopTimes.Enabled = true;
                this.Txt_LoopCounter.Visible = true;
            }
            else if (Cmd == 2)
            {
                result = Convert.ToInt32(this.Txt_LoopTimes.Text);
            }
            else if (Cmd == 3)
            {
                //this.Txt_LoopCounter.Visible = false;
            }
            else if (Cmd ==4)
            {
                //this.Txt_LoopCounter.Visible = false;
                this.Txt_LoopCounter.Text = result.ToString();
                //this.Txt_LoopCounter.Visible = true;
            }
            
        }
        private void UpdateDataGrid()
        {
            string contentLine; //headerLine
            string[] tempStr;
            string[] cmdStr;
            char[] CRCStr = new char[5];
            int i = 0,j = 0, y = 0;
            int colIndex, colBoundary = 9;
            byte HighHalfByte, LowHalfByte;
            byte[] tempData = new byte[100];
            ushort CRCResult;
            DataTypeConversion dataConv = new DataTypeConversion();
            dUpdateDataGrid updateDataGrid = new dUpdateDataGrid(UpdateUiData);
            System.IO.StreamReader rFile = new System.IO.StreamReader(@TargetFilePath);
            updateDataGrid.Invoke(0, -2, "");   //Clear datagrid

            rFile.ReadLine();      //just read the header line and do nothing with it
            while ((contentLine = rFile.ReadLine()) != null)
            {
                try
                {
                    //Console.WriteLine(contentLine + "\n");     //print first row of grid content
                    tempStr = contentLine.Split(',');
                    updateDataGrid.Invoke(0, -1, "");   //Add one line

                    for (colIndex = 0; colIndex <= colBoundary; colIndex++)
                    {
                        updateDataGrid.Invoke(colIndex, y, tempStr[colIndex]);
                        if (tempStr.Length >= 1)
                        {
                            if (colIndex == 6 && tempStr[colIndex] != null || tempStr[colIndex] != "")
                            {
                                //1.W/R field
                                //updateDataGrid.Invoke(1, y, tempStr[colIndex++]);
                                //2.Cmd string field
                                //updateDataGrid.Invoke(2, y, tempStr[colIndex++]);
                                //CRC field - convert string to byte array
                                cmdStr = tempStr[colIndex].Split(' ');

                                if (tempStr[4] == "XOR8" && cmdStr.Length >= 2)
                                {
                                    string CmdLine = tempStr[colIndex];
                                    //calculate XOR8 CRC
                                    string[] CmdStringArray = CmdLine.Split(' ');
                                    byte[] CmdBytes = new byte[CmdStringArray.Count() + 1];     //Plus 1 is reserved for checksum Byte
                                    var tstStr = dataConv.XOR8_BytesWithChksum(CmdLine, CmdBytes, CmdBytes.Length);

                                    updateDataGrid.Invoke(13, y, tstStr);    //Auto fill CRC Field column after opening schedule file
                                }
                                else if (tempStr[4] != "XOR8" && cmdStr.Length >= 2)
                                {
                                    j = 0;
                                    for (i = 0; i <= (cmdStr.Length - 1); i++)
                                    {
                                        HighHalfByte = (byte)cmdStr[i][0];
                                        LowHalfByte = (byte)cmdStr[i][1];
                                        tempData[i] = (byte)((ProcessStr.AsciiToByte(HighHalfByte) * 16) + ProcessStr.AsciiToByte(LowHalfByte));
                                        //Console.Write("{0,2:X}", tempData[i]);
                                        j++;
                                    }
                                    //Console.Write("\n");
                                    CRCResult = ProcessStr.CalculateCRC(j, tempData);
                                    //Console.Write("{0,4:X}\n", CRCResult);

                                    CRCStr[0] = (char)ProcessStr.BytetoAscii((byte)((CRCResult & 0x00F0) >> 4));
                                    CRCStr[1] = (char)ProcessStr.BytetoAscii((byte)(CRCResult & 0x000F));
                                    CRCStr[2] = (char)0x20;
                                    CRCStr[3] = (char)ProcessStr.BytetoAscii((byte)((CRCResult & 0xF000) >> 12));
                                    CRCStr[4] = (char)ProcessStr.BytetoAscii((byte)((CRCResult & 0x0F00) >> 8));

                                    updateDataGrid.Invoke(13, y, new string(CRCStr));    //Auto fill CRC Field column after opening schedule file
                                    /*  previous SerialPortTest judgement
                                    if (tempStr.Length >= 5)
                                    {
                                        updateDataGrid.Invoke(9, y, tempStr[4]);
                                    }*/
                                }
                            }
                        }
                        
                    }   //end of For loop
                    
                    y++;
                }
                catch (Exception)
                {
                    MessageBox.Show("Cmd file has illegal field..");
                    break;
                }
            }   //end of While loop

            rFile.Close();
            updateDataGrid.Invoke(0, -3, "");   //Fresh datagrid
        }
        // ------------------------------------------------------------------------------------------------ //
        
        private void ExecuteCmd()
        {
            dUpdateUIBtn UpdateUIBtn = new dUpdateUIBtn(UpdateUIBtnFun);
            dUpdateDataGrid WriteDataGrid = new dUpdateDataGrid(UpdateUiData);
            dUpdateDataGrid updateDataGrid = new dUpdateDataGrid(UpdateUiData);
            ProcessLoopText LoopText = new ProcessLoopText(UpdateLoopTxt);
            //CameraChoice _CameraChoice = new CameraChoice();
            //CameraControl cameraControl = new CameraControl();
            DataTypeConversion ProStr = new DataTypeConversion();
            Setting form2 = new Setting();
            string resultLine = "";
            string dateStr;
            string[] CmdStringArray = new string[100];
            string[] tempStr = new string[100];
            byte[] Cmdbuf = new byte[100];
            byte[] retBuf = new byte[100];
            byte[] finBuf = new byte[100];
            ushort us_data, retCRC;
            int delayTime, retDataLen;
            //int loopCounter = 0;
            int loopIndex = 0;
            int sysDelay = 0;
            int i, j, RowCount, ExeIndex = 0;
            int GPIO_Read_IntValue = -1;

            RowCount = this.dataGridView1.Rows.Count;
            if (RowCount <= 1) 
            {
                MessageBox.Show("Finish");
                //UpdateLoopTxt(1, ref loopCounter);//Enable Loop Text
                //Invoke(LoopText, 1, loopCounter);
                //Invoke(LoopText, 3, loopCounter);
            }
            else if (GlobalData.m_SerialPort == null)
            {
                MessageBox.Show("Check Comport Status First");
            }
            else
            {
                //Invoke(LoopText, 0, loopCounter);
                //UpdateLoopTxt(0, ref loopCounter);  //disable Loop Test
                UpdateLoopTxt(2, ref loopCounter);  //get Loop counter
                //Invoke(LoopText, 2, loopCounter);
                if (loopCounter < 0)
                {
                    loopCounter = 0;
                }

                while (loopCounter > 0 && FlagStop == 0)
                {
                    if (this.chkBox_LoopTimes.Checked == true)
                    {
                        loopIndex++;
                        //Invoke(LoopText, 4, loopIndex);
                        Invoke(LoopText, 4, loopCounter);
                    }

                    Invoke(UpdateUIBtn, 3, 1);  //display testing
                    /* ============= Clear old data ============= */
                    for (ExeIndex = 0; ExeIndex < (RowCount-1); ExeIndex++)
                    {
                        dataGridView1.Rows[ExeIndex].Cells[10].Value = "";
                        dataGridView1.Rows[ExeIndex].Cells[11].Value = "";
                        dataGridView1.Rows[ExeIndex].Cells[12].Value = "";
                        dataGridView1.Rows[ExeIndex].Cells[13].Value = "";
                        Invoke(updateDataGrid, ExeIndex, -5, "");   //clear select status
                    }
                    /* ============= Start to test ============= */
                    //for (ExeIndex = this.dataGridView1.CurrentRow.Index; ExeIndex < RowCount; ExeIndex++)
                    for (ExeIndex = 0; ExeIndex < (RowCount-1); ExeIndex++)
                    {
                        string columns_command = dataGridView1.Rows[ExeIndex].Cells[0].Value.ToString().Trim();
                        string columns_times = dataGridView1.Rows[ExeIndex].Cells[1].Value.ToString().Trim();
                        string columns_interval = dataGridView1.Rows[ExeIndex].Cells[2].Value.ToString().Trim();
                        string columns_comport = dataGridView1.Rows[ExeIndex].Cells[3].Value.ToString().Trim();
                        string columns_function = dataGridView1.Rows[ExeIndex].Cells[4].Value.ToString().Trim();
                        string columns_subFunction = dataGridView1.Rows[ExeIndex].Cells[5].Value.ToString().Trim();
                        string columns_cmdLine = dataGridView1.Rows[ExeIndex].Cells[6].Value.ToString().Trim();
                        string columns_switch = dataGridView1.Rows[ExeIndex].Cells[7].Value.ToString().Trim();
                        string columns_wait = dataGridView1.Rows[ExeIndex].Cells[8].Value.ToString().Trim();
                        string columns_remark = dataGridView1.Rows[ExeIndex].Cells[9].Value.ToString().Trim();

                        //IO_INPUT();                 //先讀取IO值，避免schedule第一行放IO CMD會出錯//
                        //Arduino_IO_INPUT();  //先讀取Arduino_IO值，避免schedule第一行放IO CMD會出錯//

                        if (ExeIndex >= 3)
                        {
                            Invoke(updateDataGrid, (ExeIndex - 2), -4, "");
                        }
                        else
                        {
                            Invoke(updateDataGrid, 0, -4, "");
                        }
                        
                        if (ExeIndex >= 1)
                        {
                            Invoke(updateDataGrid, (ExeIndex - 1), -5, "");     //clear select status
                        }
                        Invoke(updateDataGrid, ExeIndex, -6, "");
                        this.dataGridView1.Rows[ExeIndex].Cells[10].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[11].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[12].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[13].Value = "";

                        #region -- Schedule for Snapshot Command --
                        if ((columns_command == "_shot") || (columns_command == "PHOTO") || (columns_command == "photo"))
                        {
                            int cameraIdx = 0;
                            byte[] decodeFromStr = new byte[2];
                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                            if (columns_subFunction == "all" || columns_subFunction == "")
                                cameraIdx = -1;
                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                cameraIdx = Convert.ToInt32(columns_subFunction);

                            if (cameraChoice.Devices.Count >= 1)
                            {
                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                /*
                                //Cmd line
                                CmdStringArray = columns_cmdLine.PadLeft(2, '0').Split(' ');
                                Cmdbuf[0] = (byte)((ProStr.ASCIIToByte((byte)(CmdStringArray[0][0])) * 16) + (ProStr.ASCIIToByte((byte)(CmdStringArray[0][1]))));
                                //Get Camera count
                                int camera_Counter = cameraChoice.Devices.Count;

                                for (i = 0; i <= (camera_Counter - 1); i++)
                                {
                                    try
                                    {
                                        if (((Cmdbuf[0] < 0x99) && (i == Cmdbuf[0])) || (Cmdbuf[0] == 0x99))
                                        {
                                            var moniker = cameraChoice.Devices[i].Mon;
                                            ResolutionList resolutions = Camera.GetResolutionList(moniker);
                                            cameraControl.SetCamera(moniker, resolutions[resolutions.Count-1]);

                                            bitmap = cameraControl.SnapshotSourceImage();
                                            g = Graphics.FromImage(bitmap);
                                            g.DrawString(DateTime.Now.ToString("yyyyMMdd_HHmmss"), new Font("Tahoma", 12), Brushes.Yellow, 0, 0);
                                            g.Flush();
                                            //Delay Time
                                            if (this.dataGridView1.Rows[ExeIndex].Cells[8].Value != null)
                                            {
                                                delayTime = Convert.ToInt32(this.dataGridView1.Rows[ExeIndex].Cells[8].Value);
                                            }
                                            else
                                            {
                                                delayTime = 100;
                                            }
                                            Thread.Sleep(delayTime);
                                            bitmap.Save(Image_CurrentPath + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cameraControl.CloseCamera();
                                        }
                                        Invoke(WriteDataGrid, 10, ExeIndex, "Snapshot done.");
                                    }
                                    catch (Exception)
                                    {
                                        //Invoke(WriteDataGride, 10, ExeIndex, "Can't connect to Camera");
                                        // Invoke(WriteDataGride, 11, ExeIndex, "Fail");
                                        //Invoke(WriteDataGride, 12, ExeIndex, "Fail");
                                    }
                                }
                                if (Cmdbuf[0] < 99)
                                {

                                }
                                else
                                {//All camera capture 1 picture

                                }
                                */
                            }
                            else
                            {
                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                            }

                        }
                        #endregion
                        #region -- Schedule for Robot Command --
                        else if ((columns_command == "_tcpip") || (columns_command == "ROBOT") || (columns_command == "robot"))
                        {
                            if (NetworkHandle.IsConnected())
                            {
                                Cmdsend = (string)this.dataGridView1.Rows[ExeIndex].Cells[6].Value;
                                NetworkHandle.Send(Cmdsend);
                                
                                Invoke(WriteDataGrid, 10, ExeIndex, "");
                                Thread.Sleep(100);
                                timeOutTimer = new TimeoutTimer(timeout);
                                timeOutTimer.StartTimeoutTimer();
                                Invoke(WriteDataGrid, 10, ExeIndex, Cmdreceive);
                                Thread.Sleep(Convert.ToInt32(this.dataGridView1.Rows[ExeIndex].Cells[8].Value));
                                Cmdreceive = "";
                            }
                        }
                        #endregion
                        #region -- Schedule for HEX W/R --
                        else if ((columns_command == "_HEX") || (columns_command == "_HEX_R"))
                        {
                            // ----------------------- Process data string from csv file ----------------------- //
                            if (columns_command == "_HEX")
                            {
                                //caluate CRC field
                                if (columns_function == "XOR8")
                                {
                                    CmdStringArray = columns_cmdLine.Split(' ');
                                    byte[] cmdBytes = new byte[CmdStringArray.Count() + 1];     //Plus 1 is reserved for checksum Byte
                                    var tstStr = ProStr.XOR8_BytesWithChksum(columns_cmdLine, cmdBytes, cmdBytes.Length);
                                    GlobalData.m_SerialPort.WriteDataOut(cmdBytes, cmdBytes.Length);
                                }
                                else if (columns_function != "XOR8")
                                {
                                    //retDataLen = 8;
                                    j = 0;
                                    for (i = 0; i <= (CmdStringArray.Length - 1); i++)
                                    {
                                        Cmdbuf[i] = (byte)((ProStr.AsciiToByte((byte)(CmdStringArray[i][0])) * 16) + (ProStr.AsciiToByte((byte)(CmdStringArray[i][1]))));
                                        j++;
                                    }

                                    CmdStringArray = columns_cmdLine.Split(' ');
                                    Cmdbuf[j++] = (byte)((ProStr.AsciiToByte((byte)(CmdStringArray[0][0])) * 16) + (ProStr.AsciiToByte((byte)(CmdStringArray[0][1]))));
                                    Cmdbuf[j++] = (byte)((ProStr.AsciiToByte((byte)(CmdStringArray[1][0])) * 16) + (ProStr.AsciiToByte((byte)(CmdStringArray[1][1]))));
                                    GlobalData.m_SerialPort.WriteDataOut(Cmdbuf, Cmdbuf.Length);
                                    // --- Clear local Cmdbuf after writing Serial Data --- //
                                    for (i = 0; i < CmdStringArray.Length; i++)
                                        Cmdbuf[i] = 0x00;
                                }
                            }

                            if (columns_command == "_HEX_R")
                            {
                                //retDataLen = 7;
                                int rxLength = GlobalData.m_SerialPort.ReceivedBufferLength();

                                if (columns_function == "XOR8")
                                {
                                    for (int index = 0; index < rxLength; index++)
                                    {
                                        GlobalData.returnBytes[index] = GlobalData.m_SerialPort.GeneralDequeue();
                                        resultLine += GlobalData.returnBytes[index].ToString("X2").PadLeft(2, '0');
                                        if (index != (rxLength - 1))
                                            resultLine += ' ';
                                    }
                                }
                                else if (columns_function != "XOR8")
                                {
                                    for (int index = 0; index < rxLength; index++)
                                    {
                                        resultLine += (char)ProStr.BytetoAscii((byte)((GlobalData.returnBytes[index] >> 4) & 0x0F));
                                        resultLine += (char)ProStr.BytetoAscii((byte)(GlobalData.returnBytes[index] & 0x0F));
                                        if (index != (rxLength - 1))
                                            resultLine += ' ';
                                    }
                                }

                                Invoke(WriteDataGrid, 10, ExeIndex, resultLine);
                            }
                            
                            //Delay Time
                            if (this.dataGridView1.Rows[ExeIndex].Cells[8].Value != null)
                            {
                                delayTime = Convert.ToInt32(this.dataGridView1.Rows[ExeIndex].Cells[8].Value);
                            }
                            else
                            {
                                delayTime = 1000;
                            }

                            #region -- Send cmd out via RS232 port (original SerialPortTest design) --
                            /*
                            //SerialPortHandle.WriteDataOut(Cmdbuf, j);

                            //wait data return
                            j = 0;
                            while (SerialPortHandle.ReceivedBufferLength() < 2)
                            {

                                Thread.Sleep(1);
                                j++;
                                if (j >= 500)
                                {
                                    break;
                                }
                            }

                            if (j >= 500)
                            {//time out
                                Invoke(WriteDataGrid, 10, ExeIndex, "Time out");
                                Invoke(WriteDataGrid, 11, ExeIndex, "Time out");
                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                Invoke(WriteDataGrid, 13, ExeIndex, "Fail");
                            }
                            else
                            {//process return data
                                SerialPortHandle.SpecificDequeue(2, ref retBuf);
                                ResultLine = "";
                                ResultLine += (char)ProStr.BytetoASCII((byte)((retBuf[0] >> 4) & 0x0F));
                                ResultLine += (char)ProStr.BytetoASCII((byte)(retBuf[0] & 0x0F));
                                ResultLine += ' ';
                                ResultLine += (char)ProStr.BytetoASCII((byte)((retBuf[1] >> 4) & 0x0F));
                                ResultLine += (char)ProStr.BytetoASCII((byte)(retBuf[1] & 0x0F));
                                ResultLine += ' ';
                                finBuf[0] = retBuf[0];
                                finBuf[1] = retBuf[1];
                                if ((retBuf[1] == 0x83) || (retBuf[1] == 0x86) || (retBuf[1] == 0x90))
                                {
                                    while (SerialPortHandle.ReceivedBufferLength() < (retDataLen - 2))
                                    {

                                        Thread.Sleep(1);
                                        j++;
                                        if (j >= 500)
                                        {
                                            break;
                                        }
                                    }
                                    SerialPortHandle.SpecificDequeue(3, ref retBuf);

                                    for (i = 0; i <= 2; i++)
                                    {
                                        ResultLine += (char)ProStr.BytetoASCII((byte)((retBuf[i] >> 4) & 0x0F));
                                        ResultLine += (char)ProStr.BytetoASCII((byte)(retBuf[i] & 0x0F));
                                        ResultLine += ' ';
                                    }
                                    Invoke(WriteDataGrid, 10, ExeIndex, ResultLine);
                                    Invoke(WriteDataGrid, 11, ExeIndex, "NACK");
                                    Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                    Invoke(WriteDataGrid, 13, ExeIndex, "Fail");
                                }
                                else
                                {
                                    while (SerialPortHandle.ReceivedBufferLength() < (retDataLen - 2))
                                    {

                                        Thread.Sleep(1);
                                        j++;
                                        if (j >= 500)
                                        {
                                            break;
                                        }
                                    }

                                    SerialPortHandle.SpecificDequeue((retDataLen - 2), ref retBuf);

                                    for (i = 0; i <= (retDataLen - 3); i++)
                                    {
                                        finBuf[i + 2] = retBuf[i];
                                        ResultLine += (char)ProStr.BytetoASCII((byte)((retBuf[i] >> 4) & 0x0F));
                                        ResultLine += (char)ProStr.BytetoASCII((byte)(retBuf[i] & 0x0F));
                                        ResultLine += ' ';
                                    }
                                    Invoke(WriteDataGride, 10, ExeIndex, ResultLine);

                                    retCRC = (ushort)((finBuf[retDataLen - 1] * 256) + finBuf[retDataLen - 2]);
                                    us_data = ProcessStr.CalculateCRC((retDataLen - 2), finBuf);

                                    if (retCRC != us_data)
                                    {   //CRC fail
                                        Invoke(WriteDataGrid, 11, ExeIndex, "CRC Fail");
                                        Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                        Invoke(WriteDataGrid, 13, ExeIndex, "Fail");
                                    }
                                    else
                                    {
                                        //output result string
                                        us_data = (ushort)((finBuf[retDataLen - 4] * 256) + finBuf[retDataLen - 3]);
                                        ResultLine = String.Format("0x{0:X}", us_data, us_data);
                                        Invoke(WriteDataGride, 11, ExeIndex, ResultLine);
                                        ResultLine = String.Format("{0}", us_data);
                                        Invoke(WriteDataGride, 12, ExeIndex, ResultLine);

                                        if (CmdType == "W")
                                        {
                                            if ((Cmdbuf[4] == finBuf[4]) && (Cmdbuf[5] == finBuf[5]))
                                            {
                                                Invoke(WriteDataGrid, 13, ExeIndex, "Pass");
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 13, ExeIndex, "Pass");
                                            }
                                        }
                                        else if (CmdType == "R")
                                        {
                                            

                                            CmdLine = (string)this.dataGridView1.Rows[ExeIndex].Cells[9].Value;
                                            if ((CmdLine != null) && (CmdLine.Length >= 1))
                                            {
                                                CmdStringArray = CmdLine.Split('/');
                                                if (CmdStringArray.Length >= 1)
                                                {
                                                    j = 0;
                                                    for (i = 0; i <= (CmdStringArray.Length - 1); i++)
                                                    {
                                                        tempStr = CmdStringArray[i].Split(':');
                                                        retCRC = Convert.ToUInt16(tempStr[0]);
                                                        if (retCRC == us_data)
                                                        {
                                                            Invoke(WriteDataGrid, 13, ExeIndex, tempStr[1]);
                                                            j = 1;
                                                            break;
                                                        }
                                                    }
                                                    if (j == 0)
                                                    {
                                                        Invoke(WriteDataGrid, 13, ExeIndex, "Unknow");
                                                    }
                                                }
                                            }
                                        }

                                    }   //else (return CRC = us_data)

                                }   //else (while loop)

                            }   //else (process return data)
                            */
                            #endregion

                            Invoke(updateDataGrid, 0, -3, " ");   //refresh datagrid
                            Thread.Sleep(delayTime);
                        }
                        #endregion
                        #region -- GPIO_INPUT_OUTPUT --
                        else if (columns_command == "_Arduino_Input")
                        {
                            //debug_process("GPIO control: _Arduino_Input");
                            //Arduino_IO_INPUT(sysDelay);
                            
                            Arduino_Get_GPIO_Input(ref GPIO_Read_IntValue, sysDelay);
                        }
                        else if (columns_command == "_Arduino_Output")
                        {
                            Arduino_Get_GPIO_Input(ref GPIO_Read_IntValue, sysDelay);
                            //debug_process("GPIO control: _Arduino_Output");
                            //string GPIO = "01010101";
                            //Arduino_IO_INPUT();
                            string GPIO_string = columns_times;
                            byte GPIO_B = Convert.ToByte(GPIO_string, 2);
                            if ((GPIO_B & 0x02) == 0x00)
                                GlobalData.Arduino_relay_status = false;
                            else
                                GlobalData.Arduino_relay_status = true;
                            Arduino_Set_GPIO_Output(GPIO_B, sysDelay);
                            //label_Command.Text = "(" + columns_command + ") " + columns_times;
                            Thread.Sleep(Convert.ToInt32(columns_wait));
                        }
                        #endregion
                        #region -- Schedule for Normal I/O --
                        else if (columns_command == "_Pin")
                        {

                        }
                        #endregion
                        #region -- Schedule for Arduino I/O --
                        else if (columns_command == "_Arduino_Pin"
                            && columns_comport.Length >=6 && columns_comport.Substring(0,3) == "_P0")
                        {
                            switch (columns_comport.Substring(3, 1))
                            {
                                #region -- No.1 / P02 --
                                case "2":   //0x00FD
                                    // --- pin_0: expect getting Low value when monitor turns On --- //
                                    if (columns_comport.Substring(5, 1) == "0" && (ushort)(GlobalData.Arduino_IO_INPUT_value & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino2_0_COUNT++;
                                            //label_Command.Text = "IO CMD_ACCUMULATE";
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);
                                            
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    // --- pin 1: expect getting High value when monitor turns off --- //
                                    //else if (columns_comport.Substring(5, 1) == "1" && GlobalData.Arduino_IO_INPUT.Substring(12, 1) == "1")
                                    else if (columns_comport.Substring(5, 1) == "1" && (ushort)(GlobalData.Arduino_IO_INPUT_value & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino2_1_COUNT++;
                                            //label_Command.Text = "IO CMD_ACCUMULATE";
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if(Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                #endregion

                                #region -- No.2 / P03 --
                                case "3":
                                    //ushort tst = (ushort)(GlobalData.Arduino_IO_INPUT_value >> 1);
                                    //ushort notTst = (ushort)(tst & 0x01);
                                    if (columns_comport.Substring(5, 1) == "0" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 1) & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino3_0_COUNT++;
                                            //label_Command.Text = "IO CMD_ACCUMULATE";
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    else if (columns_comport.Substring(5, 1) == "1" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 1) & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino3_1_COUNT++;
                                            //label_Command.Text = "IO CMD_ACCUMULATE";
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                #endregion

                                #region -- No.3 / P04 --
                                case "4":
                                    if (columns_comport.Substring(5, 1) == "0" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 2) & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino4_0_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    // --- pin 1: expect getting High value when monitor turns off --- //
                                    else if (columns_comport.Substring(5, 1) == "1" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 2) & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino4_1_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                #endregion

                                #region -- No.4 / P05 --
                                case "5":
                                    if (columns_comport.Substring(5, 1) == "0" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 3) & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino5_0_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    else if (columns_comport.Substring(5, 1) == "1" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 3) & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino5_1_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                #endregion

                                #region -- No.5 / P06 --
                                case "6":
                                    if (columns_comport.Substring(5, 1) == "0" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 4) & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino6_0_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    else if (columns_comport.Substring(5, 1) == "1" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 4) & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino6_1_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                #endregion

                                #region -- No.6 / P07 --
                                case "7":
                                    if (columns_comport.Substring(5, 1) == "0" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 5) & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino7_0_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    else if (columns_comport.Substring(5, 1) == "1" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 5) & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino7_1_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                #endregion

                                #region -- No.7 / P08 --
                                case "8":
                                    if (columns_comport.Substring(5, 1) == "0" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 6) & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino8_0_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    else if (columns_comport.Substring(5, 1) == "1" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 6) & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino8_1_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                #endregion

                                #region -- No.8 / P09 --
                                case "9":
                                    if (columns_comport.Substring(5, 1) == "0" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 7) & 0x01) == 0)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino9_0_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            byte[] decodeFromStr = new byte[2];
                                            decodeFromStr = Encoding.ASCII.GetBytes(columns_subFunction);

                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (columns_subFunction != "" && decodeFromStr[0] > 0x30 && decodeFromStr[0] < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD();
                                        }
                                    }
                                    else if (columns_comport.Substring(5, 1) == "1" &&
                                            (ushort)((GlobalData.Arduino_IO_INPUT_value >> 7) & 0x01) == 1)
                                    {
                                        if (columns_cmdLine == "_accumulate")
                                        {
                                            GlobalData.IO_Arduino9_1_COUNT++;
                                        }
                                        else if (columns_cmdLine == "_shot")
                                        {
                                            int cameraIdx = 0;
                                            if (columns_subFunction == "all" || columns_subFunction == "")
                                                cameraIdx = -1;
                                            else if (Convert.ToChar(columns_subFunction[0]) > 0x30 && Convert.ToChar(columns_subFunction[0]) < 0x39)
                                                cameraIdx = Convert.ToInt32(columns_subFunction);

                                            if (cameraChoice.Devices.Count >= 1)
                                            {
                                                Snapshot(cameraIdx, columns_wait, columns_remark);
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGrid, 10, ExeIndex, "Can't find Camera");
                                                Invoke(WriteDataGrid, 11, ExeIndex, "Fail");
                                                Invoke(WriteDataGrid, 12, ExeIndex, "Fail");
                                            }
                                        }
                                        else
                                        {
                                            //IO_CMD(columns_cmdLine, cameraIdx, columns_wait);
                                        }
                                    }
                                    else
                                    {
                                        //SysDelay = 0;
                                    }
                                    break;
                                    #endregion
                            }
                        }
                        #endregion

                        if (FlagPause == 1)
                        {
                            Invoke(UpdateUIBtn, 0, 1);  //this.BTN_StartTest.Enabled = true;
                            Invoke(UpdateUIBtn, 3, 2);  //display pause
                            while (FlagPause == 1)
                            {
                                Thread.Sleep(10);
                            }
                            Invoke(UpdateUIBtn, 3, 1);  //display testing
                        }
                        else if (FlagStop == 1)
                        {
                            //All other stop actions are going at BTN_Stop
                            break;  //stop for loop
                        }
                    }

                    //--------------------- Save test result ----------------//
                    /*
                    DateStr = Directory.GetCurrentDirectory() + "\\" + "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    System.IO.StreamWriter wFile = new System.IO.StreamWriter(DateStr);
                    ResultLine = "CMD,Out String,Delay(ms),CRC Field,Reply String,Result_1,Result_2,Judge,Judge Criterion";
                    wFile.WriteLine(ResultLine);
                    for (ExeIndex = 0; ExeIndex < RowCount; ExeIndex++)
                    {
                        ResultLine = "";
                        for (i = 0; i <= 9; i++)
                        {
                            ResultLine += (((string)this.dataGridView1.Rows[ExeIndex].Cells[i].Value) + ",");
                        }
                        wFile.WriteLine(ResultLine);

                    }
                    wFile.Close();*/

                    loopCounter--;
					Invoke(LoopText, 4, loopCounter);
                }
                //----------------------------------------------------//
                MessageBox.Show("All schedules finished");
                //UpdateUIBtn(3, 3);        //display finish
                Invoke(UpdateUIBtn, 3,3);   //display finish
            }

            Invoke(UpdateUIBtn, 0, 1);  //this.BTN_StartTest.Enabled = true;
            Invoke(UpdateUIBtn, 1, 0);  //this.BTN_Pause.Enabled = false;
            Invoke(UpdateUIBtn, 2, 0);  //this.BTN_Stop.Enabled = false;
        }

        private void Arduino_IO_INPUT(int delay_time = 1000)
        {
            int retry_cnt;
            int GPIO_input_value;
            bool aGpio = false;
            retry_cnt = 3;
            do
            {
                String modified0 = "";
                //Arduino_Get_GPIO_Input(out GPIO_input_value, delay_time);
                
                //modified0 = Convert.ToString(GPIO_input_value, 2);

                string modified1 = modified0.Insert(1, ",");
                string modified2 = modified1.Insert(3, ",");
                string modified3 = modified2.Insert(5, ",");
                string modified4 = modified3.Insert(7, ",");
                string modified5 = modified4.Insert(9, ",");
                string modified6 = modified5.Insert(11, ",");
                string modified7 = modified6.Insert(13, ",");

                GlobalData.Arduino_IO_INPUT = modified7;
            }
            //while ((aGpio == false) && (--retry_cnt > 0));
            while (false);

            string dataValue = "Arduino_GPIO_INPUT=" + GlobalData.Arduino_IO_INPUT;

            
            //ushort tstNum = (ushort)((GlobalData.Arduino_IO_INPUT_value & 0xFF)>>2);
            //ushort tst = (ushort)((~0x00FB)&0x00FF);
            /*
            if (ini12.INIRead(MainSettingPath, "Record", "Timestamp", "") == "1")
            {
                DateTime dt = DateTime.Now;
                dataValue = "[Receive_Port_Arduino_IO_INPUT] [" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff") + "]  " + dataValue + "\r\n"; //OK
            }
            log_process("Arduino", dataValue);
            log_process("All", dataValue);
            */
        }

        public void Arduino_Get_GPIO_Input(ref int GPIO_Read_Data, int delay_time)
        {
            //bool aGpio = false;
            int retry_cnt = 5;

            if (GlobalData.sp_Arduino.IsOpen())
            {
                try
                {
                    string dataString = "io i";
                    byte[] tmpBt = new byte[10];
					
					//serial_receive = true;
                    do
                    {
                        //serialPort_Arduino.WriteLine(dataValue);
                        GlobalData.sp_Arduino.WriteDataOut(dataString, dataString.Length);
                        
                        /*
                        int tmpLen = GlobalData.sp_Arduino.ReceiveQueue.Count;
                        for (int i = 0; i < tmpLen; i++)
                        {
                            tmpBt[i] = (byte)GlobalData.m_SerialPort.ReceiveQueue.Dequeue();
                        }
                        for (int i = 0; i < 6; i++)
                        {
                            if (i <= 4)
                                chkStr += tmpBt[i].ToString();
                            else if (i >= 5)
                                valStr += tmpBt[i].ToString();
                        }*/
                        
                        Thread.Sleep(300);
                        
                        serial_receive = GlobalData.Arduino_recFlag;
                        if (!serial_receive && retry_cnt == 0)
                        {
                            MessageBox.Show("Arduino_IO_INPUT: no data received. Please plug the USB wire again.", "Error");
                        }
                        else if (serial_receive == true)
                        {
                            Read_Arduino_Data = GlobalData.Arduino_Read_String;
                            string l_strResult = Read_Arduino_Data.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("ioi", "");
                            GPIO_Read_Data = Convert.ToInt32(l_strResult, 16);
                            //GlobalData.Arduino_Read_Byte = (short)(GPIO_Read_Data & 0x00FF);
                            if (GPIO_Read_Data < 0x100)
                                GlobalData.Arduino_IO_INPUT_value = (ushort)(GPIO_Read_Data & 0X00FF);
                            else
                                MessageBox.Show("Received byte value format error", "Format Error");
                        }
                        retry_cnt--;
                    }
                    while (!serial_receive && retry_cnt > 0);
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Received byte value format error", "Format Error");
                }
            }
            
        }

        public void Arduino_Set_GPIO_Output(byte outputbyte, int delay_time)
        {
            int retry_cnt = 5;

            if (GlobalData.sp_Arduino.IsOpen())
            {
                try
                {
                    string dataString = "io x " + outputbyte;
                    
                    do
                    {
                        //serialPort_Arduino.WriteLine(dataValue);
                        GlobalData.sp_Arduino.WriteDataOut(dataString, dataString.Length);
                        Thread.Sleep(300);

                        serial_receive = GlobalData.Arduino_recFlag;
                        if (serial_receive && retry_cnt == 0)
                        {
                            MessageBox.Show("Arduino_IO_OUTPUT_ERROR, Please replug the Arduino board.", "Error");
                        }
                        else if (serial_receive == true)
                        {
                            /*
                            if (ini12.INIRead(MainSettingPath, "Record", "Timestamp", "") == "1")
                            {
                                DateTime dt = DateTime.Now;
                                dataValue = "[Send_Port_Arduino_IO_OUTPUT] [" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff") + "]  " + dataValue + "\r\n"; //OK
                            }
                            log_process("Arduino", dataValue);
                            log_process("All", dataValue);
                            */
                        }
                        retry_cnt--;
                    }
                    while (!serial_receive && retry_cnt > 0);
                }
                catch (System.FormatException)
                {
                    //MessageBox.Show("Received byte value format error", "Format Error");
                }
            }

        }

        bool PauseFlag = false;
        bool ShotFlag = false;
        #region -- IO CMD 指令集 --
        private void IO_CMD(string cmdLine, int cameraIdx, string columns_wait, string columns_remark)
        {
            if (cmdLine == "_shot")
            {
                /*
                ShotFlag = true;
                GlobalData.caption_Num++;
                if (GlobalData.Loop_Number == 1)
                    GlobalData.caption_Sum = GlobalData.caption_Num;
                    */
                Snapshot(cameraIdx, columns_wait, columns_remark);
                //label_Command.Text = "IO CMD_SHOT";
            }/*
            else if (cmdLine == "_pause")
            {
                PauseFlag = true;
                button_Pause.PerformClick();
                label_Command.Text = "IO CMD_PAUSE";
            }
            else if (cmdLine == "_stop")
            {
                button_Start.PerformClick();
                label_Command.Text = "IO CMD_STOP";
            }
            else if (cmdLine == "_ac_restart")
            {
                GP0_GP1_AC_OFF_ON();
                Thread.Sleep(10);
                GP0_GP1_AC_OFF_ON();
                label_Command.Text = "IO CMD_AC_RESTART";
            }
            */
        }
        #endregion

        private void BTN_StartTest_Click(object sender, EventArgs e)
        {
            this.BTN_StartTest.Enabled = false;
            this.BTN_Pause.Enabled = true;
            this.BTN_Stop.Enabled = true;
            FlagPause = 0;
            FlagStop = 0;

            if (ExecuteCmdThreadHandle == null)
            {
                ExecuteCmdThreadHandle = new Thread(ExecuteCmd);
                ExecuteCmdThreadHandle.Start();
            }
            else if (ExecuteCmdThreadHandle.IsAlive == false)
            {
                ExecuteCmdThreadHandle.Abort();
                ExecuteCmdThreadHandle = new Thread(ExecuteCmd);
                ExecuteCmdThreadHandle.Start();
            }

            if (SerialPort_Receive_Thread == null)
            {
                SerialPort_Receive_Thread = new Thread(new ThreadStart(SerialRxThread));
                SerialPort_Receive_Thread.Start();
            }
            else if (SerialPort_Receive_Thread.IsAlive == false)
            {
                SerialPort_Receive_Thread.Abort();
                SerialPort_Receive_Thread = new Thread(new ThreadStart(SerialRxThread));
                SerialPort_Receive_Thread.Start();
            }
        }

        private void BTN_Pause_Click(object sender, EventArgs e)
        {
            dUpdateUIBtn UpdateUIBtn = new dUpdateUIBtn(UpdateUIBtnFun);
            Invoke(UpdateUIBtn, 1, 0);  //this.BTN_Pause.Enabled = false;
            Invoke(UpdateUIBtn, 2, 0);  //this.BTN_Stop.Enabled = false;
            Invoke(UpdateUIBtn, 0, 0);  //this.BTN_StartTest.Enabled = false;
            FlagPause = 1;
        }

        private void BTN_Stop_Click(object sender, EventArgs e)
        {
            dUpdateUIBtn UpdateUIBtn = new dUpdateUIBtn(UpdateUIBtnFun);
            Invoke(UpdateUIBtn, 2, 0);  //this.BTN_Stop.Enabled = false;
            Invoke(UpdateUIBtn, 1, 0);  //this.BTN_Pause.Enabled = false;
            if (timeOutTimer != null && timeOutTimer.TimerOnIndicator())
            {
                timeOutTimer.StopTimeoutTimer(-9.9);
                timeOutTimer.DisposeTimeoutTimer();
            }

            if (FlagPause == 1)
                FlagPause = 0;
            FlagStop = 1;

            //SerialPort_Receive_Thread.Abort();
        }
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {   // == Open File BTN == //
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "csv files (*.*) |*.csv";
            dialog.Title = "Select Souce File";
            dialog.InitialDirectory = "D:\\";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TargetFilePath = dialog.FileName;
                Console.WriteLine(TargetFilePath);
                //Task task = new Task();
                //task.Start();
                UpdateDataGrid();
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {   // == Setting BTN == //
            Setting form2 = new Setting();
            dUpdateUI ArduinoLED = new dUpdateUI(Form1UpdateArduinoLedStatus);
            dUpdateUI UILED = new dUpdateUI(Form1UpdateComportLedStatus);
            dUpdateUI UINetworkLED = new dUpdateUI(Form1UpdateNetworkLedStatus);
            int ComportStatus;
            string PortNumber;
            int BautRate;
            int ParryBit;
            int StopBit;
            int DataLen;
            int NetworkStatus;
            string IP;
            int NetworkPort;

            //---------------- display RS232 setting panel ------------------//
            //form2.Owner = this;
            if (NetworkHandle.IsConnected())
            {
                NetworkHandle.Close();
                UINetworkLED.Invoke(0);
            }

            if (GlobalData.m_SerialPort.IsOpen())
            {
                GlobalData.m_SerialPort.ClosePort();
                UILED.Invoke(0);
            }
            if (GlobalData.sp_Arduino.IsOpen())
            {
                GlobalData.sp_Arduino.ClosePort();
                ArduinoLED.Invoke(0);
            }

            form2.ShowDialog(this);
            if (form2.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                ComportStatus = form2.getComPortChecked();
                PortNumber = form2.getComPortSetting();
                BautRate = Convert.ToInt32(form2.getComPortBaudRate());
                ParryBit = form2.getComPortParrityBit();
                StopBit = form2.getComPortStopBit();
                DataLen = form2.getComPortByteCount();
                NetworkStatus = form2.getNetworkChecked();
                IP = form2.getNetworkIP();
                NetworkPort = int.Parse(form2.getNetworkPort());
                timeout = form2.getNetworkTimeOut();
                Device = form2.getCameraDevice();
                Resolution = form2.getCameraResolution();

                if (ComportStatus == 1)
                {
                    if (GlobalData.m_SerialPort.OpenPort(PortNumber, BautRate, ParryBit, DataLen, StopBit) >= 1)
                    {
                        UILED.Invoke(1);
                    }
                    else
                    {
                        UILED.Invoke(0);
                        MessageBox.Show("Open Port fail");
                    }
                }

                if (NetworkStatus == 1)
                {
                    //if (NetworkHandle.TestConnection(IP, NetworkPort, 500) == true)
                    if (!NetworkHandle.IsConnected())
                    {
                        NetworkHandle.SetIpAddr(IP);
                        NetworkHandle.SetPortNumber(NetworkPort);
                        //if (!NetworkHandle.IsConnected())
                        {
                            NetworkHandle.Start();
                            UINetworkLED.Invoke(1);
                        }
                    }
                    //else if (NetworkHandle.TestConnection(IP, NetworkPort, 500) == false)
                    else if (NetworkHandle.IsConnected())
                    {
                        UINetworkLED.Invoke(0);
                        MessageBox.Show("Please Check the TCPIP server status.");
                    }
                    else
                    {
                        UINetworkLED.Invoke(0);
                        MessageBox.Show("Open Socket fail.");
                    }
                }
                //ComPortHandle.OpenPort();
            }
            else if (form2.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                if (!GlobalData.sp_Arduino.IsOpen())
                {
                    GlobalData.sp_Arduino.OpenPort_Arduino(GlobalData.Arduino_Comport);
                    ArduinoLED.Invoke(1);
                }

            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string delimiter = ",";

            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName, false))
                {
                    //output header data
                    string strHeader = "";
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        strHeader += dataGridView1.Columns[i].HeaderText + delimiter;
                    }
                    sw.WriteLine(strHeader.Replace("\r\n", "~"));
                    
                    //output rows data
                    for (int j = 0; j < dataGridView1.Rows.Count - 1; j++)
                    {
                        string strRowValue = "";

                        for (int k = 0; k < dataGridView1.Columns.Count; k++)
                        {
                            string scheduleOutput = dataGridView1.Rows[j].Cells[k].Value + "";
                            if (scheduleOutput.Contains(","))
                            {
                                scheduleOutput = String.Format("\"{0}\"", scheduleOutput);
                            }
                            strRowValue += scheduleOutput + delimiter;
                        }
                        sw.WriteLine(strRowValue);
                    }
                    sw.Close();
                }
            }
        }
        
        private void GetSerialData(Mod_RS232 SpHandler)
        {
            while (SpHandler.IsOpen())
            {
                int data_to_read = SpHandler.GetRxBytes();
                if (data_to_read > 0)
                {
                    byte[] dataset = new byte[data_to_read];
                    SpHandler.ReadDataIn(dataset, data_to_read);
                }
            }
        }

        public void SerialRxThread()
        {
            GetSerialData(GlobalData.m_SerialPort);
        }

        public void Snapshot(int cameraIndex, string delayTimeString, string remark)
        {
            string image_currentPath = System.Environment.CurrentDirectory;
            int delayTime;

            cameraChoice.UpdateDeviceList();
            Bitmap bitmap = null;
            Graphics g;
            RectangleF rectf = new RectangleF(70, 90, 90, 50);

            int camera_counter = 0, camera_startNum = 0;
            if (cameraIndex > 0)
            {
                camera_counter = cameraIndex - 1;
                camera_startNum = cameraIndex - 1;
            }  
            else if (cameraIndex < 0)
            {
                camera_counter = cameraChoice.Devices.Count - 1;
                camera_startNum = 0;
            }

            for (int i = camera_startNum; i <= camera_counter; i++)
            {
                try
                {
                    var moniker = cameraChoice.Devices[i].Mon;
                    ResolutionList resolutions = Camera.GetResolutionList(moniker);
                    cameraControl.SetCamera(moniker, resolutions[resolutions.Count - 1]);

                    bitmap = cameraControl.SnapshotSourceImage();
                    g = Graphics.FromImage(bitmap);
                    DateTime dt = DateTime.Now;
                    g.DrawString(string.Format("{0:R}", dt), new Font("Arial", 12), Brushes.Yellow, 0, 0);
                    if(GlobalData.Arduino_relay_status)
                        g.DrawString("AC Source: On", new Font("Arial", 12), Brushes.Yellow, 0, 15);
                    else
                        g.DrawString("AC Source: Off", new Font("Arial", 12), Brushes.Yellow, 0, 15);
                    if (remark != "")
                        g.DrawString(remark, new Font("Arial", 12), Brushes.Yellow, 0, 30);
                    g.Flush();
                    
                    //Delay Time
                    if (delayTimeString != null)
                    {
                        delayTime = Convert.ToInt32(delayTimeString);
                    }
                    else
                    {
                        delayTime = 500;
                    }
                    Thread.Sleep(delayTime);
                    if (createCamerafolder(cameraChoice.Devices[i].Name.ToString()))
                        bitmap.Save(image_currentPath + "\\" + cameraChoice.Devices[i].Name.ToString() + "\\" + cameraChoice.Devices[i].Name.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    else
                        MessageBox.Show("Camera picture save folder error.", "Save error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    cameraControl.CloseCamera();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Snapshot error: " + ex);
                }
            }
        }

        public class TimeoutTimer
        {
            private System.Timers.Timer Counter_Timer;
            public TimeoutTimer(double tmOut)
            {
                Counter_Timer = new System.Timers.Timer(tmOut);
                Counter_Timer.Interval = tmOut;
                Counter_Timer.Elapsed += new ElapsedEventHandler(Counter_Delay_OnTimedEvent);
            }

            static bool TimeoutIndicator = false;
            static UInt64 TimeoutCounter_Count = 0;
            private void Counter_Delay_OnTimedEvent(object source, ElapsedEventArgs e)
            {
                TimeoutCounter_Count++;
                TimeoutIndicator = true;
            }

            public bool TimerOnIndicator()
            {
                return TimeoutIndicator;
            }

            public void StartTimeoutTimer()
            {
                if (Counter_Timer.Interval >= 0.0)
                {
                    Counter_Timer.Enabled = true;
                    Counter_Timer.Start();
                    Counter_Timer.AutoReset = true;

                    TimeoutCounter_Delay();
                }
            }

            public void StopTimeoutTimer(double tout)
            {
                if (tout == -9.9)
                    Counter_Timer.Stop();
            }

            public void DisposeTimeoutTimer()
            {
                Counter_Timer.Dispose();
            }

            private void TimeoutCounter_Delay()
            {
                bool network_receive = false;
                if (tmout >= 0)
                    network_receive = true;
                
                while (TimeoutIndicator == false && network_receive == true)
                {
                    //Application.DoEvents();
                    //Thread.Sleep(50);
                    Cmdreceive = NetworkHandle.Receive();
                    
                    if (Cmdreceive == Cmdsend + "_RobotDone")     //e.g. Path_10_RobotDone"
                    {
                        network_receive = false;
                    	Cmdsend = "";
                        StopTimeoutTimer(-9.9);
                        DisposeTimeoutTimer();
                    }
                }

                while (TimeoutIndicator == true && network_receive == true)
                {
                    Setting form2 = new Setting();
                    sendMail(form2.getMailAddress());
                    StopTimeoutTimer(-9.9);
                    DisposeTimeoutTimer();
                    network_receive = false;
                    TimeoutIndicator = false;
                    Cmdreceive = "Mail notification already sends.";
                }

            }   //The end of TimeoutTimer.TimeoutCounter_Delay()
        }

        private void button_AcOn_Click(object sender, EventArgs e)
        {
            string dataString = "io x 7";     //  111: pin12/pin11/pin10
            GlobalData.sp_Arduino.WriteDataOut(dataString, dataString.Length);
            button_AcOn.Enabled = false;
            button_AcOff.Enabled = true;
        }

        private void button_AcOff_Click(object sender, EventArgs e)
        {
            string dataString = "io x 5";     //  101: pin12/pin11/pin10
            GlobalData.sp_Arduino.WriteDataOut(dataString, dataString.Length);
            button_AcOff.Enabled = false;
            button_AcOn.Enabled = true;
        }

        public static void sendMail(string mailTo)
        {
            string To = mailTo + ",";
            int z = 0;
            string[] to = To.Split(new char[] { ',' });
            List<string> MailList = new List<string> { };

            while (to[z] != "")
            {
                MailList.Add(to[z]);
                z++;
            }

            string Subject = "Robot client receive timeout !!!";

            string Body =
                                    "<br>" + "<br>" +

                                    "Robot client received message timeout !!! " + "<br>" + "<br>" +

                                    "Please note this E-mail is sent by Google mail system automatically, do not reply. If you have any questions please contact system administrator.";

            SendMail(MailList, Subject, Body);
        }

        public static void SendMail(List<string> MailList, string Subject, string Body)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(string.Join(",", MailList.ToArray()));       //收件者，以逗號分隔不同收件者
            msg.From = new MailAddress("nbosss.dqa@gmail.com", "NBOSSS_DQA", System.Text.Encoding.UTF8);
            msg.Subject = Subject;      //郵件標題 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;        //郵件標題編碼  
            msg.Body = Body;        //郵件內容

            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;       //郵件內容編碼 
            msg.Priority = MailPriority.High;     //郵件優先級 

            //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 

            #region 其它 Host
            /*
            ~~~~~~~~~~~~~~~~~       outlook.com smtp.live.com port:25
            ~~~~~~~~~~~~~~~~~       yahoo smtp.mail.yahoo.com.tw port:465
            ~~~~~~~~~~~~~~~~~       smtp.gmail.com port:587
            ~~~~~~~~~~~~~~~~~       tpmx.tpvaoc.com port: 25        //公司內部的SMTP
            ~~~~~~~~~~~~~~~~~       msa.hinet.net port: 25
            */
            #endregion

            try
            {
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                MySmtp.Credentials = new System.Net.NetworkCredential("nbosss.dqa@gmail.com", "Auo+1234");     //設定你的帳號密碼
                MySmtp.EnableSsl = true;      //Gmail 的 smtp 需打開 SSL
                MySmtp.Send(msg);
            }
            catch (Exception)
            {
                MessageBox.Show("Connect the google smtp server error and mail setting value is disabled. Please check the network connect status.", "Mail send error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Fill Camera ListBox and set default value
            FillCameraList();
            if (cboxCameraList.Items.Count > 0)
            {
                cboxCameraList.SelectedIndex = 0;
            }

            dUpdateUI ArduinoLED = new dUpdateUI(Form1UpdateArduinoLedStatus);
            if (GlobalData.Arduino_openFlag)
                ArduinoLED.Invoke(1);
            else
                ArduinoLED.Invoke(0);
        }

        private void chkBox_Loop_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkBox_LoopTimes.Checked == true)
            {
                this.Txt_LoopTimes.Enabled = true;
                this.Txt_LoopCounter.Enabled = true;
                loopTimes = Convert.ToInt32(Txt_LoopTimes.Text);
                loopCounter = Convert.ToInt32(Txt_LoopCounter.Text);
            }
            else
            {
                this.Txt_LoopTimes.Enabled = false;
                this.Txt_LoopCounter.Enabled = false;
                loopTimes = 0;
                loopCounter = 0;
            }
        }

        private void Txt_LoopTimes_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            if (this.chkBox_LoopTimes.Checked == true)
            {
                Txt_LoopCounter.Text = txtBox.Text;
                //loopCounter = Convert.ToInt32(Txt_LoopCounter.Text);
            }
        }

        //Update all the camera list
        private void FillCameraList()
        {
            cboxCameraList.Items.Clear();
            cameraChoice.UpdateDeviceList();
            //Iterate all cameras and put into list
            foreach (var device in cameraChoice.Devices)
            {
                cboxCameraList.Items.Add(device.Name);
            }
        }

        private void cboxCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxCameraList.SelectedIndex < 0)
            {
                cameraControl.CloseCamera();
            }
            else
            {
                // Set camera
                cameraControl.SetCamera(cameraChoice.Devices[cboxCameraList.SelectedIndex].Mon, null);
                //SetCamera(_CameraChoice.Devices[ comboBoxCameraList.SelectedIndex ].Mon, null);
            }
        }

        private bool createCamerafolder(string camera)
        {
            bool status = false;
            string picFolder = System.Environment.CurrentDirectory + "\\" + camera;

            if (Directory.Exists(picFolder))
            {
                status = true;
            }
            else
            {
                Directory.CreateDirectory(picFolder);
                status = true;
            }
            return status;
        }
    }
}
