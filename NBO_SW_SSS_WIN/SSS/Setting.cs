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

namespace Cheese
{
    public partial class Setting : Form
    {
        public int getComPortChecked()
        {
            if (Comport_checkBox.Checked == true)
                return 1;
            else
                return 0;
        }
        public string getComPortSetting()
        {
            return ((string)this.comboBox_Portname.SelectedItem);
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
        public int getNetworkChecked()
        {
            if (Network_checkBox.Checked == true)
                return 1;
            else
                return 0;
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
            this.comboBox_Portname.Items.Clear();
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
                    this.comboBox_Portname.Items.Add(SerialPortName[i]);
                }
                this.comboBox_Portname.SelectedIndex = 0;
            }

            textBox_IPAddr.Enabled = false;
            textBox_PortNum.Enabled = false;
            textBox_Timeout.Enabled = false;
            textBox_MailAddress.Enabled = false;
            comboBox_Portname.Enabled = false;
            ComboBox_BaudRate.Enabled = false;
            ComboBox_ParrityBit.Enabled = false;
            ComboBox_ByteCount.Enabled = false;
            ComboBox_StopBit.Enabled = false;
        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            if (Comport_checkBox.Checked == true)
            {
                SerialPort PortHandle = new SerialPort();
                
                PortHandle.PortName = (string)this.comboBox_Portname.SelectedItem;
                try
                {
                    PortHandle.Open();
                    System.Threading.Thread.Sleep(1);
                    PortHandle.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show((string)this.comboBox_Portname.SelectedItem + " is opened by other application.");
                }
            }	

        }

        private void Network_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Network_checkBox.Checked == true)
            {
                textBox_IPAddr.Enabled = true;
                textBox_PortNum.Enabled = true;
                textBox_Timeout.Enabled = true;
                textBox_MailAddress.Enabled = true;
            }
            else
            {
                textBox_IPAddr.Enabled = false;
                textBox_PortNum.Enabled = false;
                textBox_Timeout.Enabled = false;
                textBox_MailAddress.Enabled = false;
            }
        }

        private void Comport_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Comport_checkBox.Checked == true)
            {
                comboBox_Portname.Enabled = true;
                ComboBox_BaudRate.Enabled = true;
                ComboBox_ParrityBit.Enabled = true;
                ComboBox_ByteCount.Enabled = true;
                ComboBox_StopBit.Enabled = true;
            }
            else
            {
                comboBox_Portname.Enabled = false;
                ComboBox_BaudRate.Enabled = false;
                ComboBox_ParrityBit.Enabled = false;
                ComboBox_ByteCount.Enabled = false;
                ComboBox_StopBit.Enabled = false;
            }
        }
    }
}
