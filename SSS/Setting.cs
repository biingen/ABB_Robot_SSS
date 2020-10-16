using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using Camera_NET;
using DirectShowLib;

namespace SerialPortTest_002
{
    public partial class Setting : Form
    {
        //创建摄像头操作对象
        private CameraChoice cameraChoice = new CameraChoice();
        private CameraControl cameraControl = new CameraControl();

        public string getComPortSetting()
        {
            return ((string)this.comboBox1.SelectedItem);
        }
        public string getComPortBaudRate()
        {
            return ((string)this.ComboBox_BaudRate.SelectedItem);
        }
        public int getComPortByteCount()
        {
            return (Convert.ToInt32((string)this.ComboBox_ByteCount.SelectedItem));
        }
        public int getComPortParrityBit()
        {
            return (this.ComboBox_ParrityBit.SelectedIndex);
        }
        public int getComPortStopBit()
        {
            return (Convert.ToInt32((string)this.ComboBox_StopBit.SelectedItem));
        }
        public string getNetworkIP()
        {
            return ((string)this.textBox_IPAddr.Text);
        }
        public string getNetworkPort()
        {
            return ((string)this.textBox_PortNum.Text);
        }
        public int getNetworkTimeOut()
        {
            return (Convert.ToInt32((string)this.textBox_Timeout.Text));
        }
        public string getMailAddress()
        {
            return ((string)this.textBox_MailAddress.Text);
        }
        public int getCameraDevice()
        {
            return (this.cboCameraTypeList.SelectedIndex);
        }
        public int getCameraResolution()
        {
            return (this.cboResolutionList.SelectedIndex);
        }

        public Setting()
        {
            //
            string[] SerialPortName;
            int i;

            InitializeComponent();
            this.comboBox1.Items.Clear();
            this.BTN_Connect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ComboBox_BaudRate.SelectedIndex = 1;
            this.ComboBox_ByteCount.SelectedIndex = 1;
            this.ComboBox_ParrityBit.SelectedIndex = 0;
            this.ComboBox_StopBit.SelectedIndex = 0;

            SerialPortName = System.IO.Ports.SerialPort.GetPortNames();
            if (SerialPortName.Length >= 1)
            {
                for(i = 0; i <= (SerialPortName.Length - 1); i++)
                {
                    this.comboBox1.Items.Add(SerialPortName[i]);
                }
                this.comboBox1.SelectedIndex = 0;
            }

        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            SerialPort PortHandle = new SerialPort();
            //string tempStr;
            PortHandle.PortName = (string)this.comboBox1.SelectedItem;
            try
            {
                PortHandle.Open();
                System.Threading.Thread.Sleep(1);
                PortHandle.Close();

            }
            catch (Exception)
            {
                
                MessageBox.Show((string)this.comboBox1.SelectedItem + " is opened by other app.");
            }

        }

        //找到当前计算机上可用的摄像头
        private void FillCameraList()
        {
            cboCameraTypeList.Items.Clear();//首先清空下拉列表
            cameraChoice.UpdateDeviceList();//更新设备列表
            //循环把设备列表添加到下拉框
            foreach (var device in cameraChoice.Devices)
            {
                cboCameraTypeList.Items.Add(device.Name);
            }
        }
        //填充可用分辨率的下拉框
        private void FillResolutionList()
        {
            cboResolutionList.Items.Clear();//清空下拉框
            if (!this.cameraControl.CameraCreated) return; //如果没有摄像头则退出
            //获取可用分辨率列表
            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);
            if (resolutions == null) return;
            int selectedIndex = -1;
            for (int i = 0; i < resolutions.Count; i++)
            {
                cboResolutionList.Items.Add(resolutions[i].ToString());
                //如果当前的可用分辨率和摄像头分辨率一样，则默认选择最佳分辨率
                if (resolutions[i].CompareTo(cameraControl.Resolution) == 0)
                {
                    selectedIndex = i;
                }
            }
            //设置当前默认的分辨率
            if (selectedIndex >= 0)
            {
                cboResolutionList.SelectedIndex = selectedIndex;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            /*
            //填充摄像头下拉框和设置默认摄像头
            FillCameraList();
            if (cboCameraTypeList.Items.Count > 0)
            {
                cboCameraTypeList.SelectedIndex = 0;
            }
            */
        }

        private void cboCameraTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCameraTypeList.SelectedIndex < 0)
            {
                cameraControl.CloseCamera();
            }
            else
            {
                // Set camera
                cameraControl.SetCamera(cameraChoice.Devices[cboCameraTypeList.SelectedIndex].Mon, null);
                //SetCamera(_CameraChoice.Devices[ comboBoxCameraList.SelectedIndex ].Mon, null);
            }
            FillResolutionList();//显示可用的分辨率
        }

        //分辨率变化，同时摄像头要重新设置
        private void cboResolutionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cameraControl.CameraCreated)
                return;
            int comboBoxResolutionIndex = cboResolutionList.SelectedIndex;
            if (comboBoxResolutionIndex < 0) return;
            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);
            if (resolutions == null) return;
            if (comboBoxResolutionIndex >= resolutions.Count) return;
            if (0 == resolutions[comboBoxResolutionIndex].CompareTo(cameraControl.Resolution))
            {
                // this resolution is already selected
                return;
            }
            // Recreate camera
            //SetCamera(_Camera.Moniker, resolutions[comboBoxResolutionIndex]);
            cameraControl.SetCamera(cameraControl.Moniker, resolutions[comboBoxResolutionIndex]);
        }
    }
}
