using jini;
using Microsoft.Win32.SafeHandles;      //support SafeFileHandle
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;                            //support BindingFlags
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using ModuleLayer;

namespace Cheese
{
    public class LogDumpping
    {
        string MainSettingPath = GlobalData.MainSettingPath;
        const int byteMessage_max_Hex = 16;
        const int byteMessage_max_Ascii = 256;

        Queue<double> temperatureDouble = new Queue<double> { };

        public void LogDataReceiving(Mod_RS232 serialPort, string portConfig, ref string logText)
        {
            while (serialPort.IsOpen())
            {
                int data_to_read = serialPort.GetRxBytes();
                if (data_to_read > 0)
                {
                    byte[] dataset = new byte[data_to_read];
                    //GlobalData.LogQueue_A.Enqueue(dataset);

                    serialPort.ReadDataIn(dataset, data_to_read);

                    for (int index = 0; index < data_to_read; index++)
                    {
                        byte input_ch = dataset[index];
                        LogRecording(portConfig, ref logText, input_ch, true);
                        /*
                        if (TemperatureIsFound == true)
                        {
                            log_temperature(input_ch);
                        }
						*/
                    }
                    //else
                    //    logA_recorder(0x00,true); // tell log_recorder no more data for now.
                }
                //else
                //    logA_recorder(0x00,true); // tell log_recorder no more data for now.
            }
        }


        //byte[] byteMessage_A = new byte[Math.Max(byteMessage_max_Ascii, byteMessage_max_Hex)];
        //int byteMessage_length_A = 0;
        byte[] byteMessage = new byte[Math.Max(byteMessage_max_Ascii, byteMessage_max_Hex)];
        int byteMessage_length = 0;

        //private void LogRecording(string strPort, string strPortAll, byte ch, bool SaveToLog = false)
        private void LogRecording(string portConfig, ref string logText, byte ch, bool SaveToLog = false)
        {
            if (ini12.INIRead(MainSettingPath, "Record", "Displayhex", "") == "1")
            {
                // if (SaveToLog == false)
                {
                    byteMessage[byteMessage_length] = ch;
                    byteMessage_length++;
                }
                if ((ch == 0x0A) || (ch == 0x0D) || (byteMessage_length >= byteMessage_max_Hex))
                {
                    string strData = BitConverter.ToString(byteMessage).Replace("-", "").Substring(0, byteMessage_length * 2);
                    if (ini12.INIRead(MainSettingPath, "Record", "Timestamp", "") == "1")
                    {
                        DateTime dt = DateTime.Now;
                        strData = "[Receive_" + portConfig + "] [" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff") + "]  " + strData + "\r\n"; //OK
                    }

                    LogCat(ref logText, strData);
                    //LogCat(ref GlobalData.logAllText, strData);
                    byteMessage_length = 0;
                }
            }
            else
            {
                if ((ch == 0x0A) || (ch == 0x0D) || (byteMessage_length >= byteMessage_max_Ascii))
                {
                    string strData = Encoding.ASCII.GetString(byteMessage).Substring(0, byteMessage_length);
                    if (ini12.INIRead(MainSettingPath, "Record", "Timestamp", "") == "1")
                    {
                        DateTime dt = DateTime.Now;
                        strData = "[Receive_" + portConfig + "] [" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff") + "]  " + strData + "\r\n"; //OK
                    }

                    LogCat(ref logText, strData);
                    //LogCat(ref GlobalData.logAllText, strData);
                    byteMessage_length = 0;
                }
                else
                {
                    byteMessage[byteMessage_length] = ch;
                    byteMessage_length++;
                }
            }
        }
        
        // Log record function
        public void LogCat(ref string logComponent, string log)
        {
            //PortA, PortB, PortC, PortD, PortE, CA310, Canbus, KlinePort, All
            logComponent = string.Concat(logComponent, log);
        }

        public void LogCat(string logComponent, string log)
        {
            try
            {
                //PortA, PortB, PortC, PortD, PortE, CA310, Canbus, KlinePort, All
                logComponent = string.Concat(logComponent, log);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                //Serialportsave("All");
            }
        }

        public string ReturnLogCat(string logComponent, string log)
        {
            string allLog;
            allLog = string.Concat(logComponent, log);
            return allLog;
        }

        #region -- 換行符號置換 --
        //private void ReplaceNewLine(SerialPort port, string columns_serial, string columns_switch)
        public void ReplaceNewLine(Mod_RS232 serialPort, string columns_serial, string columns_switch)
        {
            List<string> originLineList = new List<string> { "\\r\\n", "\\n\\r", "\\r", "\\n" };
            List<string> newLineList = new List<string> { "\r\n", "\n\r", "\r", "\n" };
            int dataLength = columns_serial.Length + columns_switch.Length;
            var originAndNewLine = originLineList.Zip(newLineList, (o, n) => new { origin = o, newLine = n });
            foreach (var line in originAndNewLine)
            {
                if (columns_switch.Contains(line.origin))
                {
                    serialPort.WriteDataOut(columns_serial + columns_switch.Replace(line.origin, line.newLine), dataLength);    //Send RS232 data
                    return;
                }
            }
        }
        #endregion

        public void LogDumpToFile(string portConfig, string portName, ref string logText)
        {
            string fName = "";
            // 讀取ini中的路徑
            fName = ini12.INIRead(GlobalData.MainSettingPath, "Record", "LogPath", "");
            Console.WriteLine("[YFC]" + fName);
            string t = fName + "\\_" + portConfig + "[" + portName + "]_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + GlobalData.Loop_Number.ToString() + ".txt";
            StreamWriter MYFILE = new StreamWriter(t, false, Encoding.ASCII);
            MYFILE.Write(logText);
            MYFILE.Close();
            logText = String.Empty;
            
        }
    }

    public class DEV_TPE_RK2797
    {
        enum command_index
        {
            // Calibration Command
            SET_GAMMA_INDEX = 0,
            GET_GAMMA_INDEX,
            //SET_OUTPUT_GAMMA_TABLE_INDEX,
            GET_OUTPUT_GAMMA_TABLE_INDEX,
            SET_COLOR_GAMUT_INDEX,
            GET_COLOR_GAMUT_INDEX,
            //SET_INPUT_GAMMA_TABLE_INDEX,
            GET_INPUT_GAMMA_TABLE_INDEX,
            //SET_PCM_MARTIX_TABLE_INDEX,
            GET_PCM_MARTIX_TABLE_INDEX,
            SET_COLOR_TEMP_INDEX,
            GET_COLOR_TEMP_INDEX,
            SET_RGB_GAIN_INDEX,
            GET_RGB_GAIN_INDEX,

            // Control Command
            SET_BACKLIGHT_INDEX,
            GET_BACKLIGHT_INDEX,
            SET_PQ_ONOFF_INDEX,
            SET_INTERNAL_PATTERN_INDEX,
            SET_PATTERN_RGB_INDEX,
            SET_SHARPNESS_INDEX,
            GET_SHARPNESS_INDEX,
            GET_BACKLIGHT_SENSOR_INDEX,
            GET_THERMAL_SENSOR_INDEX,
            SET_SPI_PORT_INDEX,
            GET_SPI_PORT_INDEX,
            SET_UART_PORT_INDEX,
            GET_UART_PORT_INDEX,
            SET_BRIGHTNESS_INDEX,
            GET_BRIGHTNESS_INDEX,
            SET_CONTRAST_INDEX,
            GET_CONTRAST_INDEX,
            SET_MAIN_INPUT_INDEX,
            GET_MAIN_INPUT_INDEX,
            SET_SUB_INPUT_INDEX,
            GET_SUB_INPUT_INDEX,
            SET_PIP_MODE_INDEX,
            GET_PIP_MODE_INDEX,

            // Write Data Command
            GET_SCALER_TYPE_INDEX,
            //SET_MODEL_NAME_INDEX,
            GET_MODEL_NAME_INDEX,
            //SET_EDID_INDEX,
            GET_EDID_INDEX,
            //SET_HDCP14_INDEX,
            GET_HDCP14_INDEX,
            //SET_HDCP2x_INDEX,
            GET_HDCP2x_INDEX,
            //SET_SERIAL_NUMBER_INDEX,
            GET_SERIAL_NUMBER_INDEX,
            GET_FW_VERSION_INDEX,
            GET_FAC_EEPROM_DATA_INDEX,

            // BenQ Command
            //SET_BENQ_MODEL_NAME_INDEX,
            //SET_BENQ_SERIAL_NAME_INDEX,
            //SET_BENQ_FW_VERSION_INDEX,
            //SET_BENQ_MONITOR_ID_INDEX,
            //SET_BENQ_DNA_VERSION_INDEX,
            //SET_BENQ_MANUFACTURE_YEARANDDATE_INDEX,
            //SET_BENQ_EEPROM_INIT_INDEX,
            //GET_BENQ_EEPROM_INDEX,
        }

        byte[][] Command_Packet =
        {
			// Calibration Command
            new byte[] { 0x06, 0x00, 0xE0, 0x00, 0xff, 0xff },              ///SET_GAMMA_INDEX,
            new byte[] { 0x05, 0x00, 0xE0, 0x01, 0xff },                    ///GET_GAMMA_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x02, 0xff, 0xff, 0xff },      ///SET_OUTPUT_GAMMA_TABLE_INDEX,
            new byte[] { 0x07, 0x00, 0xE0, 0x03, 0xff, 0xff, 0xff },        ///GET_OUTPUT_GAMMA_TABLE_INDEX,
            new byte[] { 0x06, 0x00, 0xE0, 0x04, 0xff, 0xff },              ///SET_COLOR_GAMUT_INDEX,
            new byte[] { 0x05, 0x00, 0xE0, 0x05, 0xff },                    ///GET_COLOR_GAMUT_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x06, 0xff, 0xff, 0xff, 0xff, 0xff },						///SET_INPUT_GAMMA_TABLE_INDEX,
            new byte[] { 0x05, 0x00, 0xE0, 0x07, 0xff },              		///GET_INPUT_GAMMA_TABLE_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x08, 0xff, 0xff, 0xff },      ///SET_PCM_MARTIX_TABLE_INDEX,
            new byte[] { 0x05, 0x00, 0xE0, 0x09, 0xff },                    ///GET_PCM_MARTIX_TABLE_INDEX,
            new byte[] { 0x06, 0x00, 0xE0, 0x0A, 0xff, 0xff },              ///SET_COLOR_TEMP_INDEX,
            new byte[] { 0x05, 0x00, 0xE0, 0x0B, 0xff },              		///GET_COLOR_TEMP_INDEX,
            new byte[] { 0x07, 0x00, 0xE0, 0x0C, 0xff, 0xff, 0xff },		///SET_RGB_GAIN_INDEX,
            new byte[] { 0x05, 0x00, 0xE0, 0x0D, 0xff },              		///GET_RGB_GAIN_INDEX,		
            
            // Control Command
            new byte[] { 0x06, 0x01, 0xE0, 0x00, 0xff, 0xff },		    	///SET_BACKLIGHT_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x01, 0xff },           			///GET_BACKLIGHT_INDEX,			
            new byte[] { 0x07, 0x01, 0xE0, 0x02, 0xff, 0xff, 0xff },        ///SET_PQ_ONOFF_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x03, 0xff, 0xff },              ///SET_INTERNAL_PATTERN_INDEX,
            new byte[] { 0x0B, 0x01, 0xE0, 0x04, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff },            ///SET_PATTERN_RGB_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x05, 0xff, 0xff },              ///SET_SHARPNESS_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x06, 0xff },              		///GET_SHARPNESS_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x07, 0xff },              		///GET_BACKLIGHT_SENSOR_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x08, 0xff },                    ///GET_THERMAL_SENSOR_INDEX,
            //new byte[] { 0x06, 0x01, 0xE0, 0x08, 0xff, 0xff },            ///GET_THERMAL_SENSOR_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x09, 0xff, 0xff },              ///SET_SPI_PORT_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x0A, 0xff },              		///GET_SPI_PORT_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x0B, 0xff, 0xff },              ///SET_UART_PORT_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x0C, 0xff },              		///GET_UART_PORT_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x0D, 0xff, 0xff },              ///SET_BRIGHTNESS_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x0E, 0xff },        		    ///GET_BRIGHTNESS_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x0F, 0xff, 0xff },              ///SET_CONTRAST_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x10, 0xff },              		///GET_CONTRAST_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x11, 0xff, 0xff },              ///SET_MAIN_INPUT_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x12, 0xff },              		///GET_MAIN_INPUT_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x13, 0xff, 0xff },              ///SET_SUB_INPUT_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x14, 0xff },              		///GET_SUB_INPUT_INDEX,
            new byte[] { 0x06, 0x01, 0xE0, 0x15, 0xff, 0xff },              ///SET_PIP_MODE_INDEX,
            new byte[] { 0x05, 0x01, 0xE0, 0x16, 0xff },              		///GET_PIP_MODE_INDEX,

            // Write Data Command
            new byte[] { 0x05, 0x02, 0xE0, 0x00, 0xff },              		///GET_SCALER_TYPE_INDEX,
            //new byte[] { 0xff, 0x02, 0xE0, 0x01, 0xff, 0xff, 0xff },      ///SET_MODEL_NAME_INDEX,
            new byte[] { 0x05, 0x02, 0xE0, 0x02, 0xff },              		///GET_MODEL_NAME_INDEX,
            //new byte[] { 0xff, 0x02, 0xE0, 0x03, 0xff, 0xff, 0xff, 0xff, 0xff },              ///SET_EDID_INDEX,
            new byte[] { 0x06, 0x02, 0xE0, 0x04, 0xff, 0xff },              ///GET_EDID_INDEX,
            //new byte[] { 0xff, 0x02, 0xE0, 0x05, 0xff, 0xff, 0xff, 0xff },///SET_HDCP14_INDEX,
            new byte[] { 0x05, 0x02, 0xE0, 0x06, 0xff },              		///GET_HDCP14_INDEX,
            //new byte[] { 0xff, 0x02, 0xE0, 0x07, 0xff, 0xff, 0xff, 0xff },///SET_HDCP2x_INDEX,
            new byte[] { 0x06, 0x02, 0xE0, 0x08, 0xff },              		///GET_HDCP2x_INDEX,
            //new byte[] { 0xff, 0x02, 0xE0, 0x09, 0xff, 0xff, 0xff },      ///SET_SERIAL_NUMBER_INDEX,
            new byte[] { 0x05, 0x02, 0xE0, 0x0A, 0xff },              		///GET_SERIAL_NUMBER_INDEX,
            new byte[] { 0x05, 0x02, 0xE0, 0x0B, 0xff },              		///GET_FW_VERSION_INDEX,
            new byte[] { 0x05, 0x02, 0xE0, 0x0C, 0xff },              		///GET_FAC_EEPROM_DATA_INDEX,

            // BenQ Command
            //new byte[] { 0xff, 0x00, 0xE0, 0x00, 0xff, 0xff, 0xff },      ///SET_BENQ_MODEL_NAME_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x01, 0xff, 0xff, 0xff },      ///SET_BENQ_SERIAL_NAME_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x02, 0xff, 0xff, 0xff },      ///SET_BENQ_FW_VERSION_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x03, 0xff, 0xff, 0xff },      ///SET_BENQ_MONITOR_ID_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x04, 0xff, 0xff, 0xff },      ///SET_BENQ_DNA_VERSION_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x05, 0xff, 0xff, 0xff },      ///SET_BENQ_MANUFACTURE_YEARANDDATE_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x06, 0xff, 0xff, 0xff },      ///SET_BENQ_EEPROM_INIT_INDEX,
            //new byte[] { 0xff, 0x00, 0xE0, 0x07, 0xff, 0xff, 0xff },      ///GET_BENQ_EEPROM_INDEX,
        };
        // copy to mei

        byte[][] Parsing_Packet =
        {
            //// Calibration Command
            new byte [] { }, //SET_GAMMA_INDEX = 0,
            new byte [] { 0x06, 0x00, 0xE0, 0x01 }, //GET_GAMMA_INDEX,
            //new byte [] { }, //SET_OUTPUT_GAMMA_TABLE_INDEX,
            new byte [] { }, //GET_OUTPUT_GAMMA_TABLE_INDEX,
            new byte [] { }, //SET_COLOR_GAMUT_INDEX,
            new byte [] { 0x06, 0x00, 0xE0, 0x05 }, //GET_COLOR_GAMUT_INDEX,
            //new byte [] { }, //SET_INPUT_GAMMA_TABLE_INDEX,
            new byte [] { }, //GET_INPUT_GAMMA_TABLE_INDEX,
            //new byte [] { }, //SET_PCM_MARTIX_TABLE_INDEX,
            new byte [] { }, //GET_PCM_MARTIX_TABLE_INDEX,
            new byte [] { }, //SET_COLOR_TEMP_INDEX,
            new byte [] { 0x06, 0x00, 0xE0, 0x0B }, //GET_COLOR_TEMP_INDEX,
            new byte [] { }, //SET_RGB_GAIN_INDEX,
            new byte [] { 0x08, 0x00, 0xE0, 0x0D }, //GET_RGB_GAIN_INDEX,
            
            //// Control Command
            new byte [] { }, //SET_BACKLIGHT_INDEX,
            new byte [] { 0x06, 0x01, 0xE0, 0x01 },  //GET_BACKLIGHT_INDEX,
            new byte [] { }, //SET_PQ_ONOFF_INDEX,
            new byte [] { }, //SET_INTERNAL_PATTERN_INDEX,
            new byte [] { }, //SET_PATTERN_RGB_INDEX,
            new byte [] { }, //SET_SHARPNESS_INDEX,
            new byte [] { 0x06, 0x01, 0xE0, 0x06 }, //GET_SHARPNESS_INDEX,
            new byte [] { 0x07, 0x01, 0xE0, 0x07 }, //GET_BACKLIGHT_SENSOR_INDEX,
            new byte [] { 0x09, 0x01, 0xE0, 0x08 }, //GET_THERMAL_SENSOR_INDEX,
            new byte [] { }, //SET_SPI_PORT_INDEX,
            new byte [] { 0x07, 0x01, 0xE0, 0x0A }, //GET_SPI_PORT_INDEX,
            new byte [] { }, //SET_UART_PORT_INDEX,
            new byte [] { 0x07, 0x01, 0xE0, 0x0C }, //GET_UART_PORT_INDEX,
            new byte [] { }, //SET_BRIGHTNESS_INDEX,
            new byte [] { 0x06, 0x01, 0xE0, 0x0E }, //GET_BRIGHTNESS_INDEX,
            new byte [] { }, //SET_CONTRAST_INDEX,
            new byte [] { 0x06, 0x01, 0xE0, 0x10 }, //GET_CONTRAST_INDEX,
            new byte [] { }, //SET_MAIN_INPUT_INDEX,
            new byte [] { 0x06, 0x01, 0xE0, 0x12 }, //GET_MAIN_INPUT_INDEX,
            new byte [] { }, //SET_SUB_INPUT_INDEX,
            new byte [] { 0x06, 0x01, 0xE0, 0x14 }, //GET_SUB_INPUT_INDEX,
            new byte [] { }, //SET_PIP_MODE_INDEX,
            new byte [] { 0x06, 0x01, 0xE0, 0x16 }, //GET_PIP_MODE_INDEX,
            
            //new byte [] { }, // Write Data Command
            new byte [] { }, //GET_SCALER_TYPE_INDEX,
            //new byte [] { }, //SET_MODEL_NAME_INDEX,
            new byte [] { }, //GET_MODEL_NAME_INDEX,
            //new byte [] { }, //SET_EDID_INDEX,
            new byte [] { }, //GET_EDID_INDEX,
            //new byte [] { }, //SET_HDCP14_INDEX,
            new byte [] { }, //GET_HDCP14_INDEX,
            //new byte [] { }, //SET_HDCP2x_INDEX,
            new byte [] { }, //GET_HDCP2x_INDEX,
            //new byte [] { }, //SET_SERIAL_NUMBER_INDEX,
            new byte [] { }, //GET_SERIAL_NUMBER_INDEX,
            new byte [] { }, //GET_FW_VERSION_INDEX,
            new byte [] { }, //GET_FAC_EEPROM_DATA_INDEX,
            
            //// BenQ Command
            //new byte [] { }, //SET_BENQ_MODEL_NAME_INDEX,
            //new byte [] { }, //SET_BENQ_SERIAL_NAME_INDEX,
            //new byte [] { }, //SET_BENQ_FW_VERSION_INDEX,
            //new byte [] { }, //SET_BENQ_MONITOR_ID_INDEX,
            //new byte [] { }, //SET_BENQ_DNA_VERSION_INDEX,
            //new byte [] { }, //SET_BENQ_MANUFACTURE_YEARANDDATE_INDEX,
            //new byte [] { }, //SET_BENQ_EEPROM_INIT_INDEX,
            //new byte [] { }, //GET_BENQ_EEPROM_INDEX,
       };

        //  RK2797_cmd_list
        //List<byte> packetList = new List<byte>();
        //public Queue<List<byte>> packetQueueList = new Queue<List<byte>>();

        public void PacketAddedToQueue(Mod_RS232 serialPort)
        {
            List<byte> BackupDataList = new List<byte>();

            while (serialPort.ReceiveQueue.Count > 0)                   // Queue有資料就收取
            {
                //  Queue一個byte一個byte取出來被丟入List
                byte serial_byte = (byte)serialPort.ReceiveQueue.Dequeue();
                serialPort.ReceiveList.Add(serial_byte);                    // Queue一個byte一個byte取出來被丟入List
                BackupDataList.Add(serial_byte);                          // Queue debug list content
            }
            //GlobalData.RS232_receivedText = RawData_Output(BackupDataList);
        }

        //public void QueueAddedToList_DEV_TPE(Mod_RS232 serialPort)
        public void QueueAddedToList_DEV_TPE(ref List<byte> packetList, ref Queue<List<byte>> packetQueueList)
        {
            if (packetList.Count >= 3)
            {
                if (packetList.ElementAt(2) != 0xE0)
                {
                    packetList.RemoveAt(0);
                }
                else
                {
                    byte packet_len = packetList.ElementAt(0);
                    if (packet_len >= 4)
                    {
                        if (packetList.Count >= packet_len)
                        {
                            byte chksum_check_result = ChecksumCalculation.XOR_List(packetList, packet_len);

                            if (chksum_check_result == 0)
                            {
                                List<byte> CurrentDataList = new List<byte>();
                                CurrentDataList = packetList.GetRange(0, packet_len);
                                //GlobalData.RS232_receivedText = RawData_Output(CurrentDataList);
                                packetQueueList.Enqueue(CurrentDataList);                  // Enqueue list byte data
                                packetList.RemoveRange(0, packet_len);
                            }
                            else
                            {
                                packetList.RemoveAt(0);
                            }
                        }
                    }
                    else
                    {
                        packetList.RemoveAt(0);
                    }
                }
            }
            /*if (serialPort.ReceiveList.Count >= 3)
            {
                if (serialPort.ReceiveList.ElementAt(2) != 0xE0)
                {
                    serialPort.ReceiveList.RemoveAt(0);
                }
                else
                {
                    byte packet_len = serialPort.ReceiveList.ElementAt(0);
                    if (packet_len >= 4)
                    {
                        if (serialPort.ReceiveList.Count >= packet_len)
                        {
                            byte chksum_check_result = ChecksumCalculation.XOR_List(serialPort.ReceiveList, packet_len);

                            if (chksum_check_result == 0)
                            {
                                List<byte> CurrentDataList = new List<byte>();
                                CurrentDataList = serialPort.ReceiveList.GetRange(0, packet_len);
                                //GlobalData.RS232_receivedText = RawData_Output(CurrentDataList);
                                serialPort.ReceiveQueueList.Enqueue(CurrentDataList);                  // Enqueue list byte data
                                serialPort.ReceiveList.RemoveRange(0, packet_len);
                            }
                            else
                            {
                                serialPort.ReceiveList.RemoveAt(0);
                            }
                        }
                    }
                    else
                    {
                        serialPort.ReceiveList.RemoveAt(0);
                    }
                }
            }*/
        }


        public bool ListDequeuedToParse_DEV_TPE(Mod_RS232 serialPort)
        {
            bool status = false;
            List<byte> CurrentDataList = new List<byte>();
            int BACKLIGHT_SENSOR_INDEX = (int)command_index.GET_BACKLIGHT_SENSOR_INDEX;
            int THERMAL_SENSOR_INDEX = (int)command_index.GET_THERMAL_SENSOR_INDEX;

            if (serialPort.ReceiveQueueList.Count > 0)
            {
                CurrentDataList = serialPort.ReceiveQueueList.Dequeue();
                // update parsing here
                if (Parse_packet(CurrentDataList, Parsing_Packet[BACKLIGHT_SENSOR_INDEX].ToList()) == true)
                {
                    GlobalData.Measure_Backlight = raw_data(CurrentDataList);
                }
                else if (Parse_packet(CurrentDataList, Parsing_Packet[THERMAL_SENSOR_INDEX].ToList()) == true)
                {
                    GlobalData.Measure_Thermal = raw_data(CurrentDataList);
                }
                /*
                else
                    GlobalData.RS232_receivedText = RawData_Output(CurrentDataList);*/
            }
            return status;
        }

        private bool Parse_packet(List<byte> input_packet, List<byte> original_packet)
        {
            bool ret_value = true;

            for (int index = 0; index < original_packet.Count; index++)
            {
                if (input_packet.ElementAt(index) != original_packet[index])
                {
                    ret_value = false;
                    break;
                }
            }
            return ret_value;
        }

        public string GetDUTSensor(string measure_remark = "")
        {
            string log_content = "";
            int i = 1, measure_times = 1;
            try
            {
                DateTime dt = DateTime.Now;
                string DisplayMode = "None";
                string log = "None" + "," + "None" + "," +
                                "None" + "," + "None" + "," +
                                "None" + "," + DisplayMode + "," +
                                "None" + "," + "None" + "," + "None" + "," +
                                dt.ToString("yyyy/MM/dd") + "," + dt.ToString("HH:mm:ss") + "," +
                                measure_remark + "," + i + "," + measure_times + "," +
                                GlobalData.Measure_Backlight + "," + GlobalData.Measure_Thermal + "," + "\r\n";
                log_content = string.Concat(log_content, log);
            }
            catch (Exception)
            {
                log_content = "";
            }
            return log_content;
        }

        private string raw_data(List<byte> data)
        {
            string HexString = "";
            if (data != null)
            {
                foreach (byte sum in data)
                {
                    HexString += (sum.ToString("X2"));
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
                            //packetList.RemoveRange(0, packetLength);
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
                                    GlobalData.RS232_receivedText = "ACK";
                                }
                                //packetList.RemoveRange(0, packetLength);
                            }
                            else if (chkResult && ack == 0xAA)
                            {
                                packetQueueList.Enqueue(fullList);
                                lock (this)
                                {
                                    GlobalData.RS232_receivedText = "NACK";
                                }
                                //packetList.RemoveRange(0, packetLength);
                            }
                            else
                            {
                                packetQueueList.Enqueue(fullList);
                                MessageBox.Show("Checksum values are inconsistent!", "Reminding");
                            }

                            //packetList.RemoveRange(0, packetLength);
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
