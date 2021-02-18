//#define OPTION_TPSW
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Module_Layer
{
    public class Mod_TCPIP_Client
    {
        const int BUFFERSIZE = 1024;
        private Socket _socket;
        private static bool _connectedFlag = false;
        private Thread clientReceiveThread;
        private Thread clientSendThread;
        private object _Lock = new object();
        private string remote_ipAddress;
        private int remote_portNumber;
        private static int recCount = 0;
        private byte[] _receiveBuffer = new byte[BUFFERSIZE];
        private string clientMessage="", serverMessage, recMsg_first;
        public enum eHID { SSS, ROBOT, TPSW };
        string msgRecv;
        public int hid;

        /* code2study.blogspot.com/2011/12/c.html */
        public delegate void UpdateTBRecvCallback(string showText);
        //private delegate void UpdateUICallback(string showText, Control ctrl);
        public UpdateTBRecvCallback _updateTBRecvCallback;

        public delegate void UpdateTBSendCallback(string showText);
        public UpdateTBSendCallback _updateTBSendCallback;
        /* ************************************** */


        public void Start()
        {
            ConnectToRemote();
        }

        //public void Send(string message)
		public void Send()
        {
            //SendThread(message);
			SendData();
        }

        public void Close()
        {
            CloseConnection();
        }
        
        private void ConnectToRemote()
        {
            try
            {
                clientReceiveThread = new Thread(new ThreadStart(ListenForData));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Start();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("On client connect exception " + ex);
                _updateTBRecvCallback("On client connect exception " + ex.ToString());
            }
        }

        private void ListenForData()
        {
            try
            {
                //string _remoteAddress = "192.168.234.7";
                //int _remotePortNumber = 1025;

                //var remoteIpAddress = IPAddress.Parse(_remoteAddress);
                //var remoteEndPoint = new IPEndPoint(remoteIpAddress, _remotePortNumber);
                var remoteIpAddress = IPAddress.Parse(remote_ipAddress);
                var remoteEndPoint = new IPEndPoint(remoteIpAddress, remote_portNumber);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(remoteEndPoint);
                
                if (_socket.Connected)
                {
                    _connectedFlag = true;
                    //MessageBox.Show("Connecting is successfully!");
                    //_updateTBRecvCallback("Connecting is successfully!");
                }

                while (_connectedFlag)
                {
                    int byteRecv = _socket.Receive(_receiveBuffer);
                    msgRecv = Encoding.ASCII.GetString(_receiveBuffer, 0, byteRecv);
                    hid = StringExtraction(ref msgRecv, byteRecv);
                    /* =============== ROBOT 10.83.102.149 || 10.62.32.96 ===============
                    if (!msgRecv.Contains("_RobotDone"))
                    {
                        serverMessage = msgRecv + "_RobotDone";
                        _updateTBRecvCallback("[RecFromSSS] " + msgRecv + ";    [hid] " + hid);

                        Thread.Sleep(2000);
                        SendMessage();
                    }
                    else if (msgRecv == "All schedules finished")
                    {
                        serverMessage = msgRecv;
                        _updateTBRecvCallback("[RecFromSSS] " + msgRecv);
                    }*/

                    /* =============== TPSW 127.0.0.1 ===============*/
                    if (msgRecv != recMsg_first + "_RobotDone")
                    {
                        //serverMessage = msgRecv;
                        recMsg_first = msgRecv;
                        _updateTBRecvCallback("[RecFromSSS-1] " + msgRecv + ";\r\n[recMsg_first] " + recMsg_first);
                        serverMessage = "";
                        //Thread.Sleep(100);
                        //SendMessage();
                    }
                    //if (msgRecv == "Path_10_RobotDone" || msgRecv == "Path_20_RobotDone")
                    else if (msgRecv == recMsg_first + "_RobotDone")
                    {
                        serverMessage = msgRecv + "_TPswDone";
                        _updateTBRecvCallback("[RecFromSSS-2] " + msgRecv + ";\r\n[recMsg_first] " + recMsg_first);
                        recMsg_first = "";

                        Thread.Sleep(3000);
                        SendMessage();
                    }

                    /* ===== Clean up data stored in DataBuffer ===== */
                    
                }
            }
            catch (SocketException sEx)
            {
                //MessageBox.Show("Socket listening exception: " + sEx);
                //_updateTBSendCallback("Socket listening exception: " + sEx.Message);
                clientReceiveThread.Abort();
            }
        }

        //private void SendThread(string message)
		private void SendData()
        {
            try
            {
                clientSendThread = new Thread(new ThreadStart(SendMessage));
                clientSendThread.IsBackground = true;
                //_updateTBSendCallback("Client is ready to send message!");
                //Thread.Sleep(2000);
                clientSendThread.Start();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("On client connect exception " + e);
                //_updateTBSendCallback("ClientEnd sending exception: " + ex.Message);
            }
        }

        private void SendMessage()
        {
            try
            {
                //string sendingStr = StringEncoder(serverMessage);
                byte[] clientMessageAsByte = Encoding.ASCII.GetBytes(serverMessage);
                //_socket.BeginSend(clientMessageAsByte, 0, clientMessageAsByte.Length, SocketFlags.None, _dataTransferCallback, null);
                _socket.Send(clientMessageAsByte, SocketFlags.None);
                //MessageBox.Show("Client sent his message - should be received by server");
                //_updateTBSendCallback("[SentToSSS] " + serverMessage);
            }
            catch (SocketException sEx)
            {
                //MessageBox.Show("Socket sending exception: " + sEx);
                //_updateTBSendCallback("Socket sending exception: " + sEx.Message);
            }
        }

        private void CloseConnection()
        {
            if (_socket.Connected == true)
            {
                _connectedFlag = false;
                _socket.Shutdown(SocketShutdown.Both);
                _updateTBRecvCallback("Socket is shut down right now!!!");
                //_socket.Close();

                //clientReceiveThread.Abort();
                //clientSendThread.Abort();   //Abort before creating SendThread

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /* FUNCTIONS TO SET AND CHECK */
        public bool IsConnected()
        {
            return _connectedFlag;
        }

        public void SetIpAddr(string ipAddr)
        {
            lock (_Lock)
            {
                remote_ipAddress = ipAddr;
            }
        }
        
        public void SetPortNumber(int portNumber)
        {
            lock (_Lock)
            {
                remote_portNumber = portNumber;
            }
        }
		
#if OPTION_TPSW
        public string StringEncoder(string str)
        {
            string sendingStr = "[TPSW_]" + str;

            return sendingStr;
        }
//#else
        public string StringEncoder(string str)
        {
            string sendingStr = "[ROBOT]" + str;

            return sendingStr;
        }
#endif

        public int StringExtraction(ref string str, int str_length)
        {
            if (str.Length != str_length)
            {
                str = "FAIL";
            }

            string str_id = str;
            string payload = str;
            //sid = sid.Remove(7, str_length - 7);
            bool sid_robot = str_id.Contains("_RobotDone");
            bool sid_tpsw = str_id.Contains("_TPswDone");
            if (sid_robot && !sid_tpsw)
            {
                hid = (int)eHID.ROBOT;
                payload = "_RobotDone";
            }
            else if (sid_robot && sid_tpsw)
            {
                hid = (int)eHID.TPSW;
                payload = "_TPswDone";
            }
            /*
            if (str_header == "[SSS__]")
                hid = (int)eHID.SSS;
            else if (str_header == "[ROBOT]")
                hid = (int)eHID.ROBOT;
            else if (str_header == "[TPSW_]")
                hid = (int)eHID.TPSW;*/

            //str = str_data.Remove(0, 7);

            return hid;
        }
		
		
        #region 采用Socket方式，测试服务器连接 
        /// <summary> 
        /// 采用Socket方式，测试服务器连接 
        /// </summary> 
        /// <param name="host">服务器主机名或IP</param> 
        /// <param name="port">端口号</param> 
        /// <param name="millisecondsTimeout">等待时间：毫秒</param> 
        /// <returns></returns> 
        public bool TestConnection(string host, int port, int millisecondsTimeout)
        {
            //int millisecondsTimeout = 5;//等待时间
            TcpClient client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(host, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                client.Close();
            }
        }
        #endregion

    }   //End of class Drv_TCPSocket_Client




    #region --TcpClient/NetworkStream method--
#if userDefined
    /* *********************************************** */
    /* home.gamer.com.tw/creationDetail.php?sn=3466685 */
    /* *********************************************** */
    public class Drv_TCPSocket_Client
    {
        private TcpClient socketConnection;
        private Thread clientReceiveThread;
        private Thread clientSendThread;
        private NetworkStream stream;
        static private string showMessage;

        public void Start()
        {
            ConnectToTcpServer();
        }

        public void Send()
        {
            SendData();
        }

        public void Close()
        {
            CloseConnection();
        }

        private void ConnectToTcpServer()
        {
            try
            {
                clientReceiveThread = new Thread(new ThreadStart(ListenForData));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Start();
                
            }
            catch (Exception e)
            {
                MessageBox.Show("On client connect exception " + e);
            }
        }


        private void ListenForData()
        {
            string _remoteAddress = "192.168.234.9";
            int _remotePortNumber = 1025;

            socketConnection = new TcpClient(remoteIpAddress, _remotePortNumber);
            MessageBox.Show("Connecting is successfully!");
            
            Byte[] bytes = new Byte[128];
            while (true)
            {
                // Get a stream object for reading 				
                NetworkStream stream = GetStream();
                using (stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        MessageBox.Show("server message received as: " + serverMessage);
                    }
                }
            }
        }
        private void SendData()
        {
            try
            {
                clientSendThread = new Thread(new ThreadStart(SendMessage));
                clientSendThread.IsBackground = true;
                clientSendThread.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("On client connect exception " + e);
            }
        }

        private void SendMessage()
        {
            if (socketConnection == null)
            {
                return;
            }
            try
            {
                // Get a stream object for writing. 			
                NetworkStream stream = socketConnection.GetStream();
                MessageBox.Show("Client is ready to send message!");

                if (stream.CanWrite)
                {
                    string clientMessage = "This is a message from one of your clients.";
                    // Convert string message to byte array.                 
                    byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                    // Write byte array to socketConnection stream.                 
                    stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    MessageBox.Show("Client sent his message - should be received by server");
                }
            }
            catch (SocketException socketException)
            {
                MessageBox.Show("Socket exception: " + socketException);
            }
        }

        private void CloseConnection()
        {
            if (socketConnection.Connected == true)
            {
                clientReceiveThread.Abort();
                clientSendThread.Abort();   //Abort before creating SendThread

                Byte[] data=System.Text.Encoding.Unicode.GetBytes("disconnect");
                // 取得client stream.
                stream =client.GetStream();
                // 送 disconnect 資料給 TcpServer.
                stream.Write(data, 0,data.Length);
                // 關閉串流與連線
                stream.Close();
                testClient.Close();
            }
        }
    }
#endif
    #endregion
}
