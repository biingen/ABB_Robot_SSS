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

namespace SerialPortTest_002
{
    public partial class Form2 : Form
    {
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

        public Form2()
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


    }
}
