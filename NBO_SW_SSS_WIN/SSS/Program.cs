using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ModuleLayer;
using log4net.Config;

namespace Cheese
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("./log4net.config"));      //log4net configure file
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Thread to load time-consuming resources.
            Thread th = new Thread(new ThreadStart(LoadResources))
            {
                Name = "Resource Loader",
                Priority = ThreadPriority.Normal
            };
            th.Start();
            th.Join();

            Application.Run(new Main());
        }

        private static void LoadResources()
        {
            Add_ons Add_ons = new Add_ons();
            Add_ons.CreateConfig();	//Create Config.ini if it is not present in root directory
            Add_ons.USB_Read(); //read Pid and Vid of USB device
            Add_ons.ScheduleCSV_InitialFlag();
        }
    }

    public static class GlobalData
    {   //global variables and classes//
        public static string MainSettingPath = Application.StartupPath + "\\Config.ini";
        public static string MailSettingPath = Application.StartupPath + "\\Mail.ini";
        public static string RcSettingPath = Application.StartupPath + "\\RC.ini";
        public static string StartupPath = Application.StartupPath;

        public static Mod_RS232 m_SerialPort = new Mod_RS232();
        public static Mod_RS232 sp_Arduino = new Mod_RS232();

        //public static byte[] Cmdbuf = new byte[128];
        public static byte[] returnBytes = new byte[1024];

        public static string Arduino_Comport = "";
        public static string IO_INPUT = "";
        public static string Arduino_IO_INPUT = "";
        public static ushort Arduino_IO_INPUT_value = 0x00;
        public static ushort Arduino_IO_INPUT_status = 0x00;
        public static string Arduino_Read_String = "";
        public static bool Arduino_relay_status = false;
        public static bool Arduino_openFlag;
        public static bool Arduino_recFlag;
        public static int IO_PA10_0_COUNT = 0;
        public static int IO_PA10_1_COUNT = 0;
        public static int IO_PA11_0_COUNT = 0;
        public static int IO_PA11_1_COUNT = 0;
        public static int IO_PA14_0_COUNT = 0;
        public static int IO_PA14_1_COUNT = 0;
        public static int IO_PA15_0_COUNT = 0;
        public static int IO_PA15_1_COUNT = 0;
        public static int IO_PB1_0_COUNT = 0;
        public static int IO_PB1_1_COUNT = 0;
        public static int IO_PB7_0_COUNT = 0;
        public static int IO_PB7_1_COUNT = 0;
        public static int IO_Arduino2_0_COUNT = 0;
        public static int IO_Arduino2_1_COUNT = 0;
        public static int IO_Arduino3_0_COUNT = 0;
        public static int IO_Arduino3_1_COUNT = 0;
        public static int IO_Arduino4_0_COUNT = 0;
        public static int IO_Arduino4_1_COUNT = 0;
        public static int IO_Arduino5_0_COUNT = 0;
        public static int IO_Arduino5_1_COUNT = 0;
        public static int IO_Arduino6_0_COUNT = 0;
        public static int IO_Arduino6_1_COUNT = 0;
        public static int IO_Arduino7_0_COUNT = 0;
        public static int IO_Arduino7_1_COUNT = 0;
        public static int IO_Arduino8_0_COUNT = 0;
        public static int IO_Arduino8_1_COUNT = 0;
        public static int IO_Arduino9_0_COUNT = 0;
        public static int IO_Arduino9_1_COUNT = 0;
    }
}
