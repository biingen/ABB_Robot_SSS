using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DQASerailPortFunction;
using DQATestCoreFun;
using Rs232Drv;
using Camera_NET;
using DirectShowLib;
using Driver_Layer;
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
        int FlagPause;
        int FlagStop;
        string Cmdreceive;
        int Device, Resolution, Timeout;
        //------------------------------------------------------------------------------------------------//
        ProcessString ProcessStr = new ProcessString();
        DQACoreFun DQACoreFun = new DQACoreFun();
        ComPortFun ComPortHandle;
        Drv_TCPSocket_Client NetworkHandle;
        Thread ExecuteCmdThreadHandle;
        //------------------------------------------------------------------------------------------------//
        public DataGridView tempDataGrid;
        private delegate void UpdatDataGride(int x, int y, string data);
        private delegate void ProcessLoopText(int Cmd, ref int result);
        public delegate void UpdateUI(int status);
        public delegate void _UpdateUIBtn(int Btn, int Status);
        //------------------------------------------------------------------------------------------------//
        private void UpdatUIDataUI(int x,int y,string data)
        {
            int i;
            if (y == -1)
            {
                //add one row
                this.dataGridView1.Rows.Add();
                i = this.dataGridView1.Rows.Count;
                Console.WriteLine("Courrent Rows Count:" + i.ToString());
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
        public void Form1UPDateComportLedStatus(int status)
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
        public void Form1UPDateNetworkLedStatus(int status)
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
        //------------------------------------------------------------------------------------------------//
        public Main()
        {
            InitializeComponent();
            tempDataGrid = this.dataGridView1;
            FlagComPortStauts = 0;
            this.VerLabel.Text = "Ver:004";
            FlagPause = 0;
            FlagStop = 0;
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
                this.Txt_LoopCount.Enabled = false;
            }
            else if (Cmd == 1)
            {
                this.Txt_LoopCount.Enabled = true;
            }
            else if (Cmd == 2)
            {
                result = Convert.ToInt32(this.Txt_LoopCount.Text);
            }
            else if (Cmd == 3)
            {
                this.Label_LoopCounter.Visible = false;
            }
            else if (Cmd ==4)
            {
                this.Label_LoopCounter.Visible = false;
                this.Label_LoopCounter.Text = "Loop:" + result.ToString();
                this.Label_LoopCounter.Visible = true;
            }
            
        }
        private void UpdateDataGrid()
        {
            string InStrBuf;
            string[] tempStr;
            string[] cmdStr;
            char[] CRCStr = new char[5];
            int i = 0,j = 0, y = 0;
            int colIndex;
            byte HighHalfByte, LowHalfByte;
            byte[] tempData = new byte[100];
            ushort CRCResult;
            UpdatDataGride UpdataUIDataGrid = new UpdatDataGride(UpdatUIDataUI);

            System.IO.StreamReader rFile = new System.IO.StreamReader(@TargetFilePath);
            UpdataUIDataGrid.Invoke(0, -2, "");//Clear datagrid
            while ((InStrBuf = rFile.ReadLine()) != null)
            {
                try
                {
                    Console.WriteLine(InStrBuf + "\n");
                    colIndex = 0;
                    tempStr = InStrBuf.Split(',');
                    UpdataUIDataGrid.Invoke(0, -1, "");//Add one line
                    UpdataUIDataGrid.Invoke(0,y, tempStr[colIndex++]);
                    if (tempStr.Length >= 3)
                    {
                        //1.W/R filed
                        UpdataUIDataGrid.Invoke(1, y, tempStr[colIndex++]);
                        //2.Cmd string filed
                        UpdataUIDataGrid.Invoke(2, y, tempStr[colIndex++]);
                        //CRC filed
                        cmdStr = tempStr[2].Split(' ');
                        //convert string to byte array
                        if (cmdStr.Length >= 2)
                        {

                            j = 0;
                            for (i = 0; i <= (cmdStr.Length - 1); i++)
                            {
                                HighHalfByte = (byte)cmdStr[i][0];
                                LowHalfByte = (byte)cmdStr[i][1];
                                tempData[i] = (byte)((ProcessStr.ASCIIToByte(HighHalfByte) * 16) + ProcessStr.ASCIIToByte(LowHalfByte));
                                Console.Write("{0,2:X}", tempData[i]);
                                j++;
                            }
                            Console.Write("\n");
                            CRCResult = ProcessStr.CalculateCRC(j, tempData);
                            Console.Write("{0,4:X}\n", CRCResult);

                            CRCStr[0] = (char)ProcessStr.BytetoASCII((byte)((CRCResult & 0x00F0) >> 4));
                            CRCStr[1] = (char)ProcessStr.BytetoASCII((byte)(CRCResult & 0x000F));
                            CRCStr[2] = (char)0x20;
                            CRCStr[3] = (char)ProcessStr.BytetoASCII((byte)((CRCResult & 0xF000) >> 12));
                            CRCStr[4] = (char)ProcessStr.BytetoASCII((byte)((CRCResult & 0x0F00) >> 8));

                            UpdataUIDataGrid.Invoke(3, y, new string(CRCStr));
                        }
                        UpdataUIDataGrid.Invoke(4, y, tempStr[3]);
                        //judge criterion filed
                        if (tempStr.Length >= 5)
                        {
                            UpdataUIDataGrid.Invoke(9, y, tempStr[4]);
                        }


                    }
                    y++;
                }
                catch (Exception)
                {
                    MessageBox.Show("Cmd file has illegal field..");
                    break;
                }

            }
            rFile.Close();
            UpdataUIDataGrid.Invoke(0, -3, "");//Flush datagrid
        }
        //-------------------------------------------------------------------------------------------------//
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
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
        {

            Setting form2 = new Setting();
            UpdateUI UILED = new UpdateUI(Form1UPDateComportLedStatus);
            UpdateUI UINetworkLED = new UpdateUI(Form1UPDateNetworkLedStatus);
            string PortNumber;
            int BautRate;
            int ParryBit;
            int StopBit;
            int DataLen;
            string IP;
            int NetworkPort;

            //---------------- display RS232 setting panel ------------------//
            //form2.Owner = this;
            form2.ShowDialog(this);


            if (form2.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                ComPortHandle = new ComPortFun();
                NetworkHandle = new Drv_TCPSocket_Client();
                PortNumber = form2.getComPortSetting();
                BautRate = Convert.ToInt32(form2.getComPortBaudRate());
                ParryBit = form2.getComPortParrityBit();
                StopBit = form2.getComPortStopBit();
                DataLen = form2.getComPortByteCount();
                IP = form2.getNetworkIP();
                NetworkPort = int.Parse(form2.getNetworkPort());
                Timeout = form2.getNetworkTimeOut();
                Device = form2.getCameraDevice();
                Resolution = form2.getCameraResolution();

                if (ComPortHandle.OpenPort(PortNumber, BautRate, ParryBit, DataLen, StopBit) >= 1)
                {
                    UILED.Invoke(1);
                }
                else
                {
                    UILED.Invoke(0);
                    MessageBox.Show("Open Port fail");
                }

                if (IP != "" && NetworkPort > 0 && NetworkPort < 65536)
                {
                    UINetworkLED.Invoke(1);
                    NetworkHandle.SetIpAddr(IP);
                    NetworkHandle.SetPortNumber(NetworkPort);
                    NetworkHandle._updateTBRecvCallback = new Drv_TCPSocket_Client.UpdateTBRecvCallback(ShowMessageOnTBRecv);
                    NetworkHandle._updateTBSendCallback = new Drv_TCPSocket_Client.UpdateTBSendCallback(ShowMessageOnTBSend);
                    if (!NetworkHandle.IsConnected())
                        NetworkHandle.Start();
                }
                else
                {
                    UINetworkLED.Invoke(0);
                    MessageBox.Show("Open Socket fail");
                }

                //ComPortHandle.OpenPort();
            }
            else if (form2.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {

            }
            else
            {

            }
        }
        private void ExecuteCmd()
        {
            _UpdateUIBtn UpdateUIBtn = new _UpdateUIBtn(UpdateUIBtnFun);
            UpdatDataGride WriteDataGride = new UpdatDataGride(UpdatUIDataUI);
            UpdatDataGride UpdataUIDataGrid = new UpdatDataGride(UpdatUIDataUI);
            ProcessLoopText LoopText = new ProcessLoopText(UpdateLoopTxt);
            CameraChoice _CameraChoice = new CameraChoice();
            CameraControl cameraControl = new CameraControl();
            ProcessString ProStr = new ProcessString();
            Setting form2 = new Setting();
            string CmdLine = "";
            string ResultLine = "";
            string CmdType;
            string DateStr;
            string[] CmdString = new string[100];
            byte[] Cmdbuf = new byte[100];
            byte[] retBuf = new byte[100];
            byte[] finBuf = new byte[100];
            int DelayTime,retDataLen;
            ushort us_data,retCRC;
            string[] tempStr = new string[100];
            int loopCounter = 0;
            int loopIndex = 0;
            int loopFlag = 0;
            int i, j, RowCount,ExeIndex = 1;

            RowCount = this.dataGridView1.Rows.Count;
            if (RowCount <= 1) 
            {
                MessageBox.Show("Finish");
                //UpdateLoopTxt(1, ref loopCounter);//Enable Loop Text
                Invoke(LoopText, 1, loopCounter);
                Invoke(LoopText, 3, loopCounter);
            }
            else if (ComPortHandle == null)
            {
                MessageBox.Show("Check Comport Status First");

            }
            else
            {
                //
                Invoke(LoopText, 0, loopCounter);
                //UpdateLoopTxt(0, ref loopCounter);//disable Loop Test
                UpdateLoopTxt(2, ref loopCounter);//get Loop counter
                Invoke(LoopText, 2, loopCounter);
                if (loopCounter < 0)
                {
                    loopCounter = 0;
                }
               

                while (loopCounter > 0 && FlagStop == 0)
                {
                    if (this.checkBox1.Checked == true)
                    {
                        loopIndex++;
                        Invoke(LoopText, 4, loopIndex);
                    }

                    Invoke(UpdateUIBtn, 3, 1);//display testing
                    //-------------- Clear old data ------------------------//
                    for (ExeIndex = 0; ExeIndex < RowCount; ExeIndex++)
                    {
                        this.dataGridView1.Rows[ExeIndex].Cells[5].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[6].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[7].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[8].Value = "";
                        Invoke(UpdataUIDataGrid, ExeIndex, -5, "");//cleaer select status
                    }
                    
                    //-------------- Start to test -------------------------//



                    for (ExeIndex = this.dataGridView1.CurrentRow.Index; ExeIndex < RowCount; ExeIndex++)
                    {

                        CmdType = (string)this.dataGridView1.Rows[ExeIndex].Cells[0].Value;
                        if (ExeIndex >= 3)
                        {
                            Invoke(UpdataUIDataGrid, (ExeIndex - 2), -4, "");
                        }
                        else
                        {
                            Invoke(UpdataUIDataGrid, 0, -4, "");
                        }
                        if (ExeIndex >= 1)
                        {
                            Invoke(UpdataUIDataGrid, (ExeIndex - 1), -5, "");//cleaer select status
                        }
                        Invoke(UpdataUIDataGrid, ExeIndex, -6, "");
                        this.dataGridView1.Rows[ExeIndex].Cells[5].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[6].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[7].Value = "";
                        this.dataGridView1.Rows[ExeIndex].Cells[8].Value = "";
                        //
                        if ((CmdType == "PHOTO") || (CmdType == "photo"))
                        {
                            string Image_CurrentPath = System.Environment.CurrentDirectory;

                            _CameraChoice.UpdateDeviceList();
                            Bitmap bitmap = null;
                            Graphics g;
                            RectangleF rectf = new RectangleF(70, 90, 90, 50);
                            if (_CameraChoice.Devices.Count >= 1)
                            {
                                /*
                                var moniker = form2.cameraChoice.Devices[Device].Mon;
                                ResolutionList resolutions = Camera.GetResolutionList(form2.cameraChoice.Devices[Device].Mon);
                                form2.cameraControl.SetCamera(form2.cameraChoice.Devices[Device].Mon, resolutions[Resolution] - 1);

                                bitmap = form2.cameraControl.SnapshotSourceImage();
                                g = Graphics.FromImage(bitmap);
                                g.DrawString(DateTime.Now.ToString("yyyyMMdd_HHmmss"), new Font("Tahoma", 12), Brushes.Yellow, 0, 0);
                                g.Flush();
                                //Delay Time
                                if (this.dataGridView1.Rows[ExeIndex].Cells[4].Value != null)
                                {
                                    DelayTime = Convert.ToInt32(this.dataGridView1.Rows[ExeIndex].Cells[4].Value);
                                }
                                else
                                {
                                    DelayTime = 100;
                                }
                                Thread.Sleep(DelayTime);
                                bitmap.Save(Image_CurrentPath + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                form2.cameraControl.CloseCamera();
                                */

                                //Cmd line
                                CmdLine = (string)this.dataGridView1.Rows[ExeIndex].Cells[2].Value;
                                CmdString = CmdLine.PadLeft(2, '0').Split(' ');
                                Cmdbuf[0] = (byte)((ProStr.ASCIIToByte((byte)(CmdString[0][0])) * 16) + (ProStr.ASCIIToByte((byte)(CmdString[0][1]))));
                                //Get Camera count
                                int camera_Counter = _CameraChoice.Devices.Count;

                                for (i = 0; i <= (camera_Counter - 1); i++)
                                {
                                    try
                                    {
                                        if (((Cmdbuf[0] < 0x99) && (i == Cmdbuf[0])) || (Cmdbuf[0] == 0x99))
                                        {
                                            var moniker = _CameraChoice.Devices[i].Mon;
                                            ResolutionList resolutions = Camera.GetResolutionList(moniker);
                                            cameraControl.SetCamera(moniker, resolutions[resolutions.Count-1]);

                                            bitmap = cameraControl.SnapshotSourceImage();
                                            g = Graphics.FromImage(bitmap);
                                            g.DrawString(DateTime.Now.ToString("yyyyMMdd_HHmmss"), new Font("Tahoma", 12), Brushes.Yellow, 0, 0);
                                            g.Flush();
                                            //Delay Time
                                            if (this.dataGridView1.Rows[ExeIndex].Cells[4].Value != null)
                                            {
                                                DelayTime = Convert.ToInt32(this.dataGridView1.Rows[ExeIndex].Cells[4].Value);
                                            }
                                            else
                                            {
                                                DelayTime = 100;
                                            }
                                            Thread.Sleep(DelayTime);
                                            bitmap.Save(Image_CurrentPath + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cameraControl.CloseCamera();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        //Invoke(WriteDataGride, 5, ExeIndex, "Can't connect to Camera");
                                        // Invoke(WriteDataGride, 6, ExeIndex, "Fail");
                                        //Invoke(WriteDataGride, 7, ExeIndex, "Fail");
                                    }
                                }
                                if (Cmdbuf[0] < 99)
                                {

                                }
                                else
                                {//All camera capture 1 picture

                                }

                            }
                            else
                            {
                                Invoke(WriteDataGride, 5, ExeIndex, "Can't find Camera");
                                Invoke(WriteDataGride, 6, ExeIndex, "Fail");
                                Invoke(WriteDataGride, 7, ExeIndex, "Fail");
                            }

                        }
                        //else if (CmdType == "LOOP")
                        //{
                        //    Thread.Sleep(300);
                        //    CmdLine = (string)this.dataGridView1.Rows[ExeIndex].Cells[2].Value;
                        //    CmdString = CmdLine.Split(' ');
                        //    Cmdbuf[0] = Convert.ToByte(CmdString[0]);
                        //    Cmdbuf[1] = Convert.ToByte(CmdString[1]);

                        //    if (loopCounter == 0)
                        //    {
                        //        if (loopFlag == 0)
                        //        {
                        //            Invoke(UpdataUIDataGrid, ExeIndex, -5, "");//cleaer current select status   

                        //            loopIndex = ExeIndex = (int)(Cmdbuf[0] - 1);
                        //            loopCounter = (int)(Cmdbuf[1] - 1);
                        //            loopFlag = 1;

                        //            //Invoke(UpdataUIDataGrid, ExeIndex, -7, "");
                        //        }
                        //        else
                        //        {
                        //            loopFlag = 0;
                        //        }

                        //    }
                        //    else if (loopCounter >= 1)
                        //    {
                        //        Invoke(UpdataUIDataGrid, ExeIndex, -5, "");//cleaer current select status
                        //        loopCounter--;
                        //        ExeIndex = loopIndex;

                        //    }

                        //}
                        else if ((CmdType == "W") || (CmdType == "R"))
                        {
                            //-------------------------------- Process data string from csv file --------------------------------//
                            retDataLen = 7;
                            if (CmdType == "W")
                            {
                                retDataLen = 8;
                            }


                            //Cmd line
                            CmdLine = (string)this.dataGridView1.Rows[ExeIndex].Cells[2].Value;
                            CmdString = CmdLine.Split(' ');
                            j = 0;
                            for (i = 0; i <= (CmdString.Length - 1); i++)
                            {
                                //byte tempByte = (byte)CmdString[i][0];

                                Cmdbuf[i] = (byte)((ProStr.ASCIIToByte((byte)(CmdString[i][0])) * 16) + (ProStr.ASCIIToByte((byte)(CmdString[i][1]))));
                                j++;
                            }
                            //caluate CRC filed
                            CmdLine = (string)this.dataGridView1.Rows[ExeIndex].Cells[3].Value;
                            CmdString = CmdLine.Split(' ');
                            Cmdbuf[j++] = (byte)((ProStr.ASCIIToByte((byte)(CmdString[0][0])) * 16) + (ProStr.ASCIIToByte((byte)(CmdString[0][1]))));
                            Cmdbuf[j++] = (byte)((ProStr.ASCIIToByte((byte)(CmdString[1][0])) * 16) + (ProStr.ASCIIToByte((byte)(CmdString[1][1]))));
                            //Delay Time
                            if (this.dataGridView1.Rows[ExeIndex].Cells[4].Value != null)
                            {
                                DelayTime = Convert.ToInt32(this.dataGridView1.Rows[ExeIndex].Cells[4].Value);
                            }
                            else
                            {
                                DelayTime = 10;

                            }

                            //--------------------------------- Send cmd out via RS232 port ---------------------------------//
                            ComPortHandle.WriteDataOut(Cmdbuf, j);

                            //wait data return
                            j = 0;
                            while (ComPortHandle.GetReceiveBufLen() < 2)
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
                                Invoke(WriteDataGride, 5, ExeIndex, "Time out");
                                Invoke(WriteDataGride, 6, ExeIndex, "Time out");
                                Invoke(WriteDataGride, 7, ExeIndex, "Fail");
                                Invoke(WriteDataGride, 8, ExeIndex, "Fail");
                            }
                            else
                            {//process return data
                                ComPortHandle.GetReciveData(2, ref retBuf);
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
                                    while (ComPortHandle.GetReceiveBufLen() < (retDataLen - 2))
                                    {

                                        Thread.Sleep(1);
                                        j++;
                                        if (j >= 500)
                                        {
                                            break;
                                        }
                                    }
                                    ComPortHandle.GetReciveData(3, ref retBuf);

                                    for (i = 0; i <= 2; i++)
                                    {
                                        ResultLine += (char)ProStr.BytetoASCII((byte)((retBuf[i] >> 4) & 0x0F));
                                        ResultLine += (char)ProStr.BytetoASCII((byte)(retBuf[i] & 0x0F));
                                        ResultLine += ' ';
                                    }
                                    Invoke(WriteDataGride, 5, ExeIndex, ResultLine);
                                    Invoke(WriteDataGride, 6, ExeIndex, "NACK");
                                    Invoke(WriteDataGride, 7, ExeIndex, "Fail");
                                    Invoke(WriteDataGride, 8, ExeIndex, "Fail");

                                }
                                else
                                {
                                    while (ComPortHandle.GetReceiveBufLen() < (retDataLen - 2))
                                    {

                                        Thread.Sleep(1);
                                        j++;
                                        if (j >= 500)
                                        {
                                            break;
                                        }
                                    }

                                    ComPortHandle.GetReciveData((retDataLen - 2), ref retBuf);

                                    for (i = 0; i <= (retDataLen - 3); i++)
                                    {
                                        finBuf[i + 2] = retBuf[i];
                                        ResultLine += (char)ProStr.BytetoASCII((byte)((retBuf[i] >> 4) & 0x0F));
                                        ResultLine += (char)ProStr.BytetoASCII((byte)(retBuf[i] & 0x0F));
                                        ResultLine += ' ';
                                    }
                                    Invoke(WriteDataGride, 5, ExeIndex, ResultLine);


                                    retCRC = (ushort)((finBuf[retDataLen - 1] * 256) + finBuf[retDataLen - 2]);
                                    us_data = ProcessStr.CalculateCRC((retDataLen - 2), finBuf);

                                    if (retCRC != us_data)
                                    {//CRC fail
                                        Invoke(WriteDataGride, 6, ExeIndex, "CRC Fail");
                                        Invoke(WriteDataGride, 7, ExeIndex, "Fail");
                                        Invoke(WriteDataGride, 8, ExeIndex, "Fail");
                                    }
                                    else
                                    {
                                        //out put result string
                                        us_data = (ushort)((finBuf[retDataLen - 4] * 256) + finBuf[retDataLen - 3]);
                                        ResultLine = String.Format("0x{0:X}", us_data, us_data);
                                        Invoke(WriteDataGride, 6, ExeIndex, ResultLine);
                                        ResultLine = String.Format("{0}", us_data);
                                        Invoke(WriteDataGride, 7, ExeIndex, ResultLine);

                                        if (CmdType == "W")
                                        {
                                            if ((Cmdbuf[4] == finBuf[4]) && (Cmdbuf[5] == finBuf[5]))
                                            {
                                                Invoke(WriteDataGride, 8, ExeIndex, "Pass");
                                            }
                                            else
                                            {
                                                Invoke(WriteDataGride, 8, ExeIndex, "Pass");
                                            }
                                        }
                                        else if (CmdType == "R")
                                        {
                                            CmdLine = (string)this.dataGridView1.Rows[ExeIndex].Cells[9].Value;
                                            if ((CmdLine != null) && (CmdLine.Length >= 1))
                                            {
                                                CmdString = CmdLine.Split('/');
                                                if (CmdString.Length >= 1)
                                                {
                                                    j = 0;
                                                    for (i = 0; i <= (CmdString.Length - 1); i++)
                                                    {
                                                        tempStr = CmdString[i].Split(':');
                                                        retCRC = Convert.ToUInt16(tempStr[0]);
                                                        if (retCRC == us_data)
                                                        {
                                                            Invoke(WriteDataGride, 8, ExeIndex, tempStr[1]);
                                                            j = 1;
                                                            break;
                                                        }
                                                    }
                                                    if (j == 0)
                                                    {
                                                        Invoke(WriteDataGride, 8, ExeIndex, "Unknow");
                                                    }
                                                }


                                            }
                                        }


                                    }



                                }






                            }


                            Invoke(UpdataUIDataGrid, 0, -3, " ");//reflush data gride
                            Thread.Sleep(DelayTime);
                        }
                        else if((CmdType == "ROBOT") || (CmdType == "robot"))
                        {
                            Cmdreceive = (string)this.dataGridView1.Rows[ExeIndex].Cells[2].Value;
                            NetworkHandle.Send(Cmdreceive);
                            Counter_Delay(Convert.ToInt32(this.dataGridView1.Rows[ExeIndex].Cells[4].Value));
                        }

                        //Invoke(UpdataUIDataGrid, ExeIndex, -3, "");//Flush datagrid

                        if (FlagPause == 1)
                        {
                            Invoke(UpdateUIBtn, 0, 1);//this.BTN_StartTest.Enabled = true;
                            Invoke(UpdateUIBtn, 3, 2);//display pause
                            while (FlagPause == 1)
                            {
                                Thread.Sleep(10);
                            }
                            Invoke(UpdateUIBtn, 3, 1);//display testing
                            Invoke(UpdateUIBtn, 0, 0);//this.BTN_StartTest.Enabled = true;
                        }
                        else if (FlagStop == 1)
                        {

                            break;//stop for loop
                        }
                    }


                    //--------------------- Save test result ----------------//
                    DateStr = Directory.GetCurrentDirectory() + "\\" + "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    System.IO.StreamWriter wFile = new System.IO.StreamWriter(DateStr);
                    ResultLine = "CMD,Out String,CRC Field,Delay(ms),Reply String,Result_1,Result_2,Judge,Judge Criterion";
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
                    wFile.Close();
                    loopCounter--;
                }
                //----------------------------------------------------//
                MessageBox.Show("Finish");
                //UpdateUIBtn(3, 3);//display finish
                Invoke(UpdateUIBtn, 3,3);//display finish
                Invoke(LoopText, 3, loopCounter);
                if (this.checkBox1.Checked == true)
                {
                    Invoke(LoopText, 1, loopCounter);
                    Invoke(LoopText, 3, loopCounter);
                }
                

            }
            



            Invoke(UpdateUIBtn, 0, 1);//this.BTN_StartTest.Enabled = true;
            Invoke(UpdateUIBtn, 1, 0);//this.BTN_Pause.Enabled = false;
            Invoke(UpdateUIBtn, 2, 0);//this.BTN_Stop.Enabled = false;





        }

        public void ShowMessageOnTBRecv(string msg)
        {
            if (InvokeRequired)
            {
                Drv_TCPSocket_Client.UpdateTBRecvCallback updateTBCallback = new Drv_TCPSocket_Client.UpdateTBRecvCallback(ShowMessageOnTBRecv);
                Invoke(updateTBCallback, new object[] { msg });
            }
        }

        public void ShowMessageOnTBSend(string msg)
        {
            if (InvokeRequired)
            {
                Drv_TCPSocket_Client.UpdateTBSendCallback updateTBCallback = new Drv_TCPSocket_Client.UpdateTBSendCallback(ShowMessageOnTBSend);
                Invoke(updateTBCallback, new object[] { msg });
            }

        }

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

     
        }

        private void BTN_Pause_Click(object sender, EventArgs e)
        {
            _UpdateUIBtn UpdateUIBtn = new _UpdateUIBtn(UpdateUIBtnFun);
            FlagPause = 1;
            Invoke(UpdateUIBtn, 1, 0);//this.BTN_Pause.Enabled = false;
        }

        private void BTN_Stop_Click(object sender, EventArgs e)
        {
            _UpdateUIBtn UpdateUIBtn = new _UpdateUIBtn(UpdateUIBtnFun);
            FlagStop = 1;
            Invoke(UpdateUIBtn, 2, 0);//this.BTN_Pause.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                this.Txt_LoopCount.Enabled = true;
            }
            else
            {
                this.Txt_LoopCount.Enabled = false;
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
                    /*
                    string strHeader = "";
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        strHeader += dataGridView1.Columns[i].HeaderText + delimiter;
                    }
                    sw.WriteLine(strHeader.Replace("\r\n", "~"));
                    */
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
                            if (k != 3)
                                strRowValue += scheduleOutput + delimiter;
                        }
                        sw.WriteLine(strRowValue);
                    }
                    sw.Close();
                }
            }
        }

        // 這個Counter專用的delay的內部資料與function
        static bool Counter_Delay_TimeOutIndicator = false;
        static UInt64 Counter_Count = 0;
        private void Counter_Delay_UsbOnTimedEvent(object source, ElapsedEventArgs e)
        {
            Counter_Count++;
            Counter_Delay_TimeOutIndicator = true;
        }

        private void Counter_Delay(int delay_ms)
        {
            if (Timeout <= 0) return;
            bool network_receive = false;
            network_receive = true;
            System.Timers.Timer Counter_Timer = new System.Timers.Timer(Timeout);
            Counter_Timer.Interval = Timeout;
            Counter_Timer.Elapsed += new ElapsedEventHandler(Counter_Delay_UsbOnTimedEvent);
            Counter_Timer.Enabled = true;
            Counter_Timer.Start();
            Counter_Timer.AutoReset = true;

            while (Counter_Delay_TimeOutIndicator == false && network_receive == true)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(1);//釋放CPU//
                if (NetworkHandle.Receive() == Cmdreceive + "_Finished")
                {
                    Counter_Timer.Stop();
                    Counter_Timer.Dispose();
                    network_receive = false;
                    Thread.Sleep(delay_ms);
                }
            }

            while (Counter_Delay_TimeOutIndicator == true && network_receive == true)
            {
                Setting form2 = new Setting();
                sendMail(form2.getMailAddress());
                Counter_Timer.Stop();
                Counter_Timer.Dispose();
                network_receive = false;
            }
        }

        public void sendMail(string mailTo)
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

        public void SendMail(List<string> MailList, string Subject, string Body)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(string.Join(",", MailList.ToArray()));       //收件者，以逗號分隔不同收件者
            msg.From = new MailAddress("tpdqatest@gmail.com", "TP_DQA_Test", System.Text.Encoding.UTF8);
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
                MySmtp.Credentials = new System.Net.NetworkCredential("tpdqatest@gmail.com", "Auoasc2019");     //設定你的帳號密碼
                MySmtp.EnableSsl = true;      //Gmail 的 smtp 需打開 SSL
                MySmtp.Send(msg);
            }
            catch (Exception)
            {
                MessageBox.Show("Connect the google smtp server error and mail setting value is disabled. Please check the network connect status.", "Mail send error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

