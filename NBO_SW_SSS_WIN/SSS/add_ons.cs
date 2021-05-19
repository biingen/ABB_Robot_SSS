using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using jini;
using ModuleLayer;

namespace Cheese
{
    class Add_ons
    {
        private string MainSettingPath = GlobalData.MainSettingPath;    //Application.StartupPath + "\\Config.ini";
        private string MailPath = GlobalData.MailSettingPath;                  //Application.StartupPath + "\\Mail.ini";
        private string RcPath = GlobalData.RcSettingPath;                         //Application.StartupPath + "\\RC.ini";

        //Thread UsbDetecting = new Thread(new ThreadStart(USB_Read));
        
        #region -- 讀取USB裝置 --
        public void USB_Read()
        {
            //調整Building version: All為全功能, Car為車功能
            ini12.INIWrite(GlobalData.MainSettingPath, "Device", "Software", "All");
            //預設AutoKit沒接上
            ini12.INIWrite(GlobalData.MainSettingPath, "Device", "ArduinoExist", "0");
            ini12.INIWrite(GlobalData.MainSettingPath, "Device", "ArduinoPort", "");

            ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
            ManagementObjectCollection collection = search.Get();

            var usbList = from u in collection.Cast<ManagementBaseObject>()
                          select new
                          {
                              id = u.GetPropertyValue("DeviceID"),
                              name = u.GetPropertyValue("Name"),
                              description = u.GetPropertyValue("Description"),
                              status = u.GetPropertyValue("Status"),
                              system = u.GetPropertyValue("SystemName"),
                              caption = u.GetPropertyValue("Caption"),
                              pnp = u.GetPropertyValue("PNPDeviceID"),
                          };

            foreach (var usbDevice in usbList)
            {
                string deviceId = (string)usbDevice.id;
                string deviceTp = (string)usbDevice.name;
                string deviecDescription = (string)usbDevice.description;

                string deviceStatus = (string)usbDevice.status;
                string deviceSystem = (string)usbDevice.system;
                string deviceCaption = (string)usbDevice.caption;
                string devicePnp = (string)usbDevice.pnp;

                if (deviecDescription != null)
                {
                    #region 偵測相機
                    if (deviecDescription.IndexOf("USB 視訊裝置", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        deviecDescription.IndexOf("USB 视频设备", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        deviceTp.IndexOf("Webcam", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        deviceTp.IndexOf("Camera", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        deviceTp.IndexOf("LifeCam", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (deviceId.IndexOf("VID_", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            int vidIndex = deviceId.IndexOf("VID_");
                            string startingAtVid = deviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"
                            string vid = startingAtVid.Substring(0, 4); // vid is four characters long
                            //GlobalData.VidList.Add(vid);
                        }

                        if (deviceId.IndexOf("PID_", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            int pidIndex = deviceId.IndexOf("PID_");
                            string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"
                            string pid = startingAtPid.Substring(0, 4); // pid is four characters long
                            //GlobalData.PidList.Add(pid);
                        }

                        Console.WriteLine("-----------------Camera------------------");
                        Console.WriteLine("DeviceID: {0}\n" +
                                              "Name: {1}\n" +
                                              "Description: {2}\n" +
                                              "Status: {3}\n" +
                                              "System: {4}\n" +
                                              "Caption: {5}\n" +
                                              "Pnp: {6}\n"
                                              , deviceId, deviceTp, deviecDescription, deviceStatus, deviceSystem, deviceCaption, devicePnp);

                        //Camera存在
                        ini12.INIWrite(GlobalData.MainSettingPath, "Device", "CameraExist", "1");
                    }
                    #endregion

                    #region 偵測Arduino
                    if (deviceId.IndexOf("FTDIBUS\\VID_0403+PID_6001", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine("-----------------Arduino------------------");
                        Console.WriteLine("DeviceID: {0}\n" +
                                              "Name: {1}\n" +
                                              "Description: {2}\n" +
                                              "Status: {3}\n" +
                                              "System: {4}\n" +
                                              "Caption: {5}\n" +
                                              "Pnp: {6}\n"
                                              , deviceId, deviceTp, deviecDescription, deviceStatus, deviceSystem, deviceCaption, devicePnp);

                        int FirstIndex = deviceTp.IndexOf("(");
                        string ArduinoPortSubstring = deviceTp.Substring(FirstIndex + 1);
                        string ArduinoPort = ArduinoPortSubstring.Substring(0);

                        int ArduinoPortLengh = ArduinoPort.Length;
                        string ArduinoPortFinal = ArduinoPort.Remove(ArduinoPortLengh - 1);
                        GlobalData.Arduino_Comport = ArduinoPortFinal;

                        if (ArduinoPortSubstring.Substring(0, 3) == "COM")
                        {
                            ini12.INIWrite(GlobalData.MainSettingPath, "Device", "ArduinoExist", "1");
                            ini12.INIWrite(GlobalData.MainSettingPath, "Device", "ArduinoPort", ArduinoPortFinal);
                        }

                        if (!GlobalData.sp_Arduino.IsOpen())
                        {
                            GlobalData.sp_Arduino.OpenPort_Arduino(ArduinoPortFinal);
                            GlobalData.Arduino_openFlag = true;
                            //GlobalData.m_SerialPort.OpenSerialPort(ArduinoPortFinal, "9600");
                        }
                        else
                        {
                            //GlobalData.sp_Arduino.ClosePort();
                            GlobalData.Arduino_openFlag = false;
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region -- Schedule CSV File --
        public void ScheduleCSV_InitialFlag()
        {
            if (ini12.INIRead(MainSettingPath, "Schedule1", "Exist", "") == "")
            {
                ini12.INIWrite(MainSettingPath, "Schedule1", "Exist", "0");
                ini12.INIWrite(MainSettingPath, "Schedule1", "Path", "Not yet opened");
            }
        }
        #endregion
        /*
        public void SaveSXPReport()
        {
            StreamReader Log = new StreamReader(@"D:\AUTO BOX\LOG\_Log2_20160906132452_1.txt");
            StreamWriter SXP = new StreamWriter(@"D:\AUTO BOX\LOG\SXP.txt");

            string line = string.Empty;
            while ((line = Log.ReadLine()) != null)
            {
                Console.WriteLine(line);
                Console.WriteLine(line.Length);
            }
            Log.Close();
        }*/

        #region -- 創建Config.ini --
        public void CreateConfig()
        {
            string[] Device = { "Software", "ArduinoExist", "ArduinoPort", "CameraExist", "RedRatExist", "DOS", "RunAfterStartUp", "CAx10Exist" };
            string[] RedRat = { "RedRatIndex", "DBFile", "Brands", "SerialNumber" };
            string[] Camera = { "VideoIndex", "VideoNumber", "VideoName", "AudioIndex", "AudioNumber", "AudioName", "CameraDevice", "Resolution" };
            string[] SerialPort = { "Selected", "PortName", "BaudRate", "DataBit", "StopBits", "DisplayHex" };
            string[] Schedule1 = { "Exist", "Loop", "OnTimeStart", "Timer", "Path" };
            string[] LogSearch = { "StartTime", "Comport1", "Comport2", "Comport3", "Comport4", "Comport5", "TextNum", "Camerarecord", "Camerashot",
                                                "Sendmail", "Savelog", "Showmessage", "ACcontrol", "Stop", "AC OFF", "Nowvalue",
                                               "Text0", "Text1", "Text2", "Text3", "Text4",
                                               "Times0", "Times1", "Times2", "Times3", "Times4",
                                               "Display0", "Display1", "Display2", "Display3", "Display4" };

            if (File.Exists(MainSettingPath) == false)
            {
                for (int i = 0; i < Device.Length; i++)
                {
                    if (i == (Device.Length - 1))
                    {
                        ini12.INIWrite(MainSettingPath, "Device", Device[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MainSettingPath, "Device", Device[i], "");
                    }
                }

                for (int i = 0; i < RedRat.Length; i++)
                {
                    if (i == (RedRat.Length - 1))
                    {
                        ini12.INIWrite(MainSettingPath, "RedRat", RedRat[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MainSettingPath, "RedRat", RedRat[i], "");
                    }
                }

                for (int i = 0; i < Camera.Length; i++)
                {
                    if (i == (Camera.Length - 1))
                    {
                        ini12.INIWrite(MainSettingPath, "Camera", Camera[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MainSettingPath, "Camera", Camera[i], "");
                    }
                }

                for (int i = 0; i < SerialPort.Length; i++)
                {
                    if (i == (SerialPort.Length - 1))
                    {
                        ini12.INIWrite(MainSettingPath, "Serial Port", SerialPort[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MainSettingPath, "Serial Port", SerialPort[i], "");
                    }
                }

                for (int i = 0; i < Schedule1.Length; i++)
                {
                    if (i == (Schedule1.Length - 1))
                    {
                        ini12.INIWrite(MainSettingPath, "Schedule1", Schedule1[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MainSettingPath, "Schedule1", Schedule1[i], "");
                    }
                }

                for (int i = 0; i < LogSearch.Length; i++)
                {
                    if (i == (LogSearch.Length - 1))
                    {
                        ini12.INIWrite(MainSettingPath, "LogSearch", LogSearch[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MainSettingPath, "LogSearch", LogSearch[i], "");
                    }
                    }
                }
            }
            #endregion
        /*
        #region -- 創建Mail.ini --
        public void CreateMailConfig()
        {
            string[] SendMail = { "value" };
            string[] DataInfo = { "TestCaseNumber", "Result", "NGfrequency", "CreateTime", "CloseTime", "ProjectNumber" };
            string[] TotalTestTime = { "value", "value1", "value2", "value3", "value4", "value5", "How Long" };
            string[] TestCase = { "TestCase1", "TestCase2", "TestCase3", "TestCase4", "TestCase5" };
            string[] MailInfo = { "From", "To", "ProjectName", "ModelName", "Version", "Tester", "TeamViewerID", "TeamViewerPassWord" };

            if (File.Exists(MailPath) == false)
            {
                for (int i = 0; i < SendMail.Length; i++)
                {
                    if (i == (SendMail.Length - 1))
                    {
                        ini12.INIWrite(MailPath, "Send Mail", SendMail[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MailPath, "Send Mail", SendMail[i], "");
                    }
                }

                for (int i = 0; i < DataInfo.Length; i++)
                {
                    if (i == (DataInfo.Length - 1))
                    {
                        ini12.INIWrite(MailPath, "Data Info", DataInfo[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MailPath, "Data Info", DataInfo[i], "");
                    }
                }

                for (int i = 0; i < TotalTestTime.Length; i++)
                {
                    if (i == (TotalTestTime.Length - 1))
                    {
                        ini12.INIWrite(MailPath, "Total Test Time", TotalTestTime[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MailPath, "Total Test Time", TotalTestTime[i], "");
                    }
                }

                for (int i = 0; i < TestCase.Length; i++)
                {
                    if (i == (TestCase.Length - 1))
                    {
                        ini12.INIWrite(MailPath, "Test Case", TestCase[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MailPath, "Test Case", TestCase[i], "");
                    }
                }

                for (int i = 0; i < MailInfo.Length; i++)
                {
                    if (i == (MailInfo.Length - 1))
                    {
                        ini12.INIWrite(MailPath, "Mail Info", MailInfo[i], "" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        ini12.INIWrite(MailPath, "Mail Info", MailInfo[i], "");
                    }
                }
            }
        }
        #endregion

        #region -- 創建RC.ini --
        public void CreateRcConfig()
        {
            string[] Setting = { "SelectRcLastTime", "SelectRcLastTimePath" };

            for (int i = 0; i < Setting.Length; i++)
            {
                if (i == (Setting.Length - 1))
                {
                    ini12.INIWrite(RcPath, "Setting", Setting[i], "" + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    ini12.INIWrite(RcPath, "Setting", Setting[i], "");
                }
            }
        }
        #endregion
    */
    }
}
