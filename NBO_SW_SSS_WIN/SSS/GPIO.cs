using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using jini;

namespace Cheese
{
    public partial class GPIO : Form
    {
        private string button1_down = "";
        private string button1_up = "";
        private string button2_down = "";
        private string button2_up = "";
        private string button3_down = "";
        private string button3_up = "";
        private string button4_down = "";
        private string button4_up = "";
        private string button5_down = "";
        private string button5_up = "";
        private string button6_down = "";
        private string button6_up = "";
        private string button7_down = "";
        private string button7_up = "";
        private string button8_down = "";
        private string button8_up = "";
        private string button9_down = "";
        private string button9_up = "";

        public GPIO()
        {
            InitializeComponent();
        }

        private void LoadGpioDB(string xmlfile)
        {
            try
            {
                // Gpio指令檔案匯入
                if (System.IO.File.Exists(xmlfile) == true)
                {
                    var allGPIO = XDocument.Load(xmlfile).Root.Element("Keypad_Setting").Elements("GPIO");
                    foreach (var GPIOCode in allGPIO)
                    {
                        switch (GPIOCode.Attribute("Name").Value)
                        {
                            case "GPIO1":
                                button1.Text = GPIOCode.Element("GPIO_N").Value;
                                button1_down = GPIOCode.Element("GPIO_D").Value;
                                button1_up = GPIOCode.Element("GPIO_U").Value;
                                button1.MouseDown += new MouseEventHandler(button_MouseDown);
                                button1.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO2":
                                button2.Text = GPIOCode.Element("GPIO_N").Value;
                                button2_down = GPIOCode.Element("GPIO_D").Value;
                                button2_up = GPIOCode.Element("GPIO_U").Value;
                                button2.MouseDown += new MouseEventHandler(button_MouseDown);
                                button2.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO3":
                                button3.Text = GPIOCode.Element("GPIO_N").Value;
                                button3_down = GPIOCode.Element("GPIO_D").Value;
                                button3_up = GPIOCode.Element("GPIO_U").Value;
                                button3.MouseDown += new MouseEventHandler(button_MouseDown);
                                button3.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO4":
                                button4.Text = GPIOCode.Element("GPIO_N").Value;
                                button4_down = GPIOCode.Element("GPIO_D").Value;
                                button4_up = GPIOCode.Element("GPIO_U").Value;
                                button4.MouseDown += new MouseEventHandler(button_MouseDown);
                                button4.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO5":
                                button5.Text = GPIOCode.Element("GPIO_N").Value;
                                button5_down = GPIOCode.Element("GPIO_D").Value;
                                button5_up = GPIOCode.Element("GPIO_U").Value;
                                button5.MouseDown += new MouseEventHandler(button_MouseDown);
                                button5.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO6":
                                button6.Text = GPIOCode.Element("GPIO_N").Value;
                                button6_down = GPIOCode.Element("GPIO_D").Value;
                                button6_up = GPIOCode.Element("GPIO_U").Value;
                                button6.MouseDown += new MouseEventHandler(button_MouseDown);
                                button6.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO7":
                                button7.Text = GPIOCode.Element("GPIO_N").Value;
                                button7_down = GPIOCode.Element("GPIO_D").Value;
                                button7_up = GPIOCode.Element("GPIO_U").Value;
                                button7.MouseDown += new MouseEventHandler(button_MouseDown);
                                button7.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO8":
                                button8.Text = GPIOCode.Element("GPIO_N").Value;
                                button8_down = GPIOCode.Element("GPIO_D").Value;
                                button8_up = GPIOCode.Element("GPIO_U").Value;
                                button8.MouseDown += new MouseEventHandler(button_MouseDown);
                                button8.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                            case "GPIO9":
                                button9.Text = GPIOCode.Element("GPIO_N").Value;
                                button9_down = GPIOCode.Element("GPIO_D").Value;
                                button9_up = GPIOCode.Element("GPIO_U").Value;
                                button9.MouseDown += new MouseEventHandler(button_MouseDown);
                                button9.MouseUp += new MouseEventHandler(button_MouseUp);
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("GPIO code file does not exist", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), "Keypad_GPIO library error!");
            }
        }

        private void button_MouseDown(object sender, EventArgs e)
        {
            int index = int.Parse(((Button)(sender)).Name.ToString().Replace("button", ""));

            switch (index)
            {
                case 1:
                    GlobalData.sp_Arduino.WriteDataOut(button1_down, button1_down.Length);
                    break;
                case 2:
                    GlobalData.sp_Arduino.WriteDataOut(button2_down, button2_down.Length);
                    break;
                case 3:
                    GlobalData.sp_Arduino.WriteDataOut(button3_down, button3_down.Length);
                    break;
                case 4:
                    GlobalData.sp_Arduino.WriteDataOut(button4_down, button4_down.Length);
                    break;
                case 5:
                    GlobalData.sp_Arduino.WriteDataOut(button5_down, button5_down.Length);
                    break;
                case 6:
                    GlobalData.sp_Arduino.WriteDataOut(button6_down, button6_down.Length);
                    break;
                case 7:
                    GlobalData.sp_Arduino.WriteDataOut(button7_down, button7_down.Length);
                    break;
                case 8:
                    GlobalData.sp_Arduino.WriteDataOut(button8_down, button8_down.Length);
                    break;
                case 9:
                    GlobalData.sp_Arduino.WriteDataOut(button9_down, button9_down.Length);
                    break;
            }
        }

        private void button_MouseUp(object sender, EventArgs e)
        {
            int index = int.Parse(((Button)(sender)).Name.ToString().Replace("button", ""));

            switch (index)
            {
                case 1:
                    GlobalData.sp_Arduino.WriteDataOut(button1_up, button1_up.Length);
                    break;
                case 2:
                    GlobalData.sp_Arduino.WriteDataOut(button2_up, button2_up.Length);
                    break;
                case 3:
                    GlobalData.sp_Arduino.WriteDataOut(button3_up, button3_up.Length);
                    break;
                case 4:
                    GlobalData.sp_Arduino.WriteDataOut(button4_up, button4_up.Length);
                    break;
                case 5:
                    GlobalData.sp_Arduino.WriteDataOut(button5_up, button5_up.Length);
                    break;
                case 6:
                    GlobalData.sp_Arduino.WriteDataOut(button6_up, button6_up.Length);
                    break;
                case 7:
                    GlobalData.sp_Arduino.WriteDataOut(button7_up, button7_up.Length);
                    break;
                case 8:
                    GlobalData.sp_Arduino.WriteDataOut(button8_up, button8_up.Length);
                    break;
                case 9:
                    GlobalData.sp_Arduino.WriteDataOut(button9_up, button9_up.Length);
                    break;
            }
        }

        private void button_XmlFile_Click(object sender, EventArgs e)
        {
            // Generator Command Path
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "")
            {
                textBox_XmlPath.Text = textBox_XmlPath.Text;
            }
            else
            {
                textBox_XmlPath.Text = openFileDialog1.FileName;
                LoadGpioDB(textBox_XmlPath.Text);
            }
        }

        private void GPIO_Shown(object sender, EventArgs e)
        {
            GlobalData.GPIO = true;
        }

        private void GPIO_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalData.GPIO = false;
        }
    }
}
