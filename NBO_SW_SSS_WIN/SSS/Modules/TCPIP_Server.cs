using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ModuleLayer
{
    public class Mod_TCPIP_SocketListener
    {
        private const int MAX_CLIENTS = 2, BUFFERSIZE = 512, BACKLOG = 10;   //Max Length Of Pending Connections Queue;
        private const short LISTENPORT = 8025;
        /* ************************************** */
        private static bool connectedFlag = false;
        private static Socket listener = null;
        public Socket[] m_clientHandler = new Socket[MAX_CLIENTS];
        private static AsyncCallback dataTransferCallback;
        private object oLock = new object();
        public delegate void dUpdateUI(int status);
        public dUpdateUI m_UpdateRobot, m_UpdateTPsw;

        private static string receivedMessage;
        private enum eClientID { idx_Robot, idx_TPsw };
        private enum eSourceID { SSS, ROBOT, TPSW };
        public int m_headerID = 0;

        //public static ManualResetEvent allDone = new ManualResetEvent(false);
        //private Dictionary<long, Mod_TCPIP_ClientHandler> m_clients = new Dictionary<long, Mod_TCPIP_ClientHandler>();

        public void CreateSocket()
        {
            /* Establish the local endpoint for the socket.
             * The DNS name of the computer running the listener is "xxx.com". */
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            //IPEndPoint endPoint = new IPEndPoint(ipAddress, SocketPort);
            var localEndPoint = new IPEndPoint(IPAddress.Any, LISTENPORT);

            /* Create a TCP/IP socket. */
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            /*
             *  Using Bind() method we associate a network address to the Server Socket
             *  All client that will connect to this Server Socket must know this network Address */
            listener.Bind(localEndPoint);

            /* Using Listen() method we create the Client list that will want to connect to Server */
            listener.Listen(BACKLOG);

            /* Begins an asynchronous operation to accept an incoming connection attempt. */
            listener.BeginAccept(OnConnectionRequested, null);

            lock (oLock)
            {
                connectedFlag = true;
            }

            MessageBox.Show("Server starts listening...");
        }

        private void OnConnectionRequested(IAsyncResult asyncRes)
        {
            try
            {
                if (!connectedFlag)
                    return;

                /* EndAccept completes a call to BeginAccept.
                  * It returns a new Socket that can be used to send data to and receive data from the remote host. */
                var clientSocket = listener.EndAccept(asyncRes);
                IPEndPoint clientEp = clientSocket.RemoteEndPoint as IPEndPoint;
                string clientAddr = clientEp.Address.ToString();
                int clientCount = -1;
                
                if (clientAddr != "127.0.0.1")                          //Robot End
                    clientCount = (int)eClientID.idx_Robot;
                else if (clientAddr == "127.0.0.1")                 //TPsw End
                    clientCount = (int)eClientID.idx_TPsw;

                WaitForData(clientSocket);
                m_clientHandler[clientCount] = clientSocket;
                if (clientCount == 0)
                    m_UpdateRobot(1);
                else if (clientCount == 1)
                    m_UpdateTPsw(1);

                listener.BeginAccept(OnConnectionRequested, null);
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("OnConnectRequest: Socket has been closed.");
            }
            catch (SocketException sEx)
            {
                Console.WriteLine(string.Format("[OnConnectRequested][SocketException] {0}", sEx.Message));
                throw;
            }
        }

        /* In the OnDataReceived operation we check if there is data, if there is, we keep calling the WaitForData again to get the next data.
         *  If there is no data anymore, we stop the transfer and close the socket: */
        private void OnDataReceived(IAsyncResult asyncRes)
        {
            try
            {
                cSocketPacket sp = asyncRes.AsyncState as cSocketPacket;
                int numberOfByteReceived = sp.WorkSocket.EndReceive(asyncRes);

                if (numberOfByteReceived <= 0)
                {
                    sp.WorkSocket.Close();
                    //MessageBox.Show("Disconnected with Client - " + sp.WorkSocket.RemoteEndPoint);
                    if (sp.WorkSocket.Equals(m_clientHandler[0]))
                        m_UpdateRobot(0);
                    else if (sp.WorkSocket.Equals(m_clientHandler[1]))
                        m_UpdateTPsw(0);

                    return;
                }

                receivedMessage = Encoding.ASCII.GetString(sp.DataBuffer, 0, numberOfByteReceived);
                m_headerID = StringExtraction(ref receivedMessage, numberOfByteReceived);

                WaitForData(sp.WorkSocket);
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("OnConnectRequest: Socket has been closed.");
                throw;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(string.Format("Something happened: {0}", ex.Message));
                throw;
            }
        }

        /* We first call the WaitForData operation, and then call the BeginAccept operation again
         *  to continue listening to incoming connection requests of other clients.
         *  The WaitForData starts the actual data receiving: if the connected client sends data, the OnDataReceived callback operation is executed. */
        private void WaitForData(Socket dataTransferSocket)
        {
            try
            {
                if (dataTransferCallback == null)
                {
                    dataTransferCallback = OnDataReceived;
                }

                var socketPacket = new cSocketPacket(dataTransferSocket);
                /* ===== Clean up data stored in DataBuffer ===== */
                for (int i = 0; i < BUFFERSIZE; i++)
                    socketPacket.DataBuffer[i] = 0;

                Thread.Sleep(100);
                dataTransferSocket.BeginReceive(socketPacket.DataBuffer, 0, socketPacket.DataBuffer.Length, SocketFlags.None, dataTransferCallback, socketPacket);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(string.Format("Something happened: {0}", ex.Message));
                throw;
            }
        }

        public string ReceiveData(int headerID)
        {
            if (headerID == 1)              //from ROBOT
                WaitForData(m_clientHandler[0]);
            else if (headerID == 2)     //from TPSW
                WaitForData(m_clientHandler[1]);

            return receivedMessage;
        }

        public void SendData(int headerID, string dataToBeSent)
        {
            try
            {
                byte[] messageAsByte = Encoding.ASCII.GetBytes(dataToBeSent);
                if (headerID == 1)              //To ROBOT
                    m_clientHandler[0].Send(messageAsByte, messageAsByte.Length, SocketFlags.None);
                else if (headerID == 2)     //To TPSW
                    m_clientHandler[1].Send(messageAsByte, messageAsByte.Length, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(string.Format("[SendData]: {0}", ex.Message));
                throw;
            }
        }

        public bool IsConnected()
        {
            //bool connectedFlag;

            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //lock (_Lock)
            {
                //connectedFlag = socket.Connected;
            }
            return connectedFlag;
        }

        /* Clean up socket. */
        public void CloseSocket()
        {
            lock (oLock)
            {
                connectedFlag = false;
            }

            listener.Close();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            //_updateTBRecvCallback("Server stops listening!!!");
            MessageBox.Show("Server stops listening!!!");
        }

        public int StringExtraction(ref string str, int str_length)
        {
            if (str.Length != str_length)
            {
                str = "FAIL";
            }

            int hid = 0;
            string str_id = str;
            //string payload = str;
            bool sid_robot = str_id.Contains("_RobotDone");
            bool sid_tpsw = str_id.Contains("_TPswDone");
            if (sid_robot && !sid_tpsw)
                hid = (int)eSourceID.ROBOT;
            else if (sid_robot && sid_tpsw)
                hid = (int)eSourceID.TPSW;

            /*
            sid = sid.Remove(7, str_length - 7);
            if (str_header == "[SSS__]")
                hid = (int)eSID.SSS;
            else if (str_header == "[ROBOT]")
                hid = (int)eSID.ROBOT;
            else if (str_header == "[TPSW_]")
                hid = (int)eSID.TPSW;

            str = str_data.Remove(0, 7);
            */
            return hid;
        }

        public class cSocketPacket
        {
            public Socket WorkSocket { get; set; }
            public byte[] DataBuffer { get; set; }

            public cSocketPacket(Socket socket)
            {
                WorkSocket = socket;
                DataBuffer = new byte[BUFFERSIZE];
            }
        }

    }   //End of class Mod_TCPIP_SocketListener


    /* ============ Reserved For Future Modification  ============ */
    public class Mod_TCPIP_ClientHandler
    {
        private Socket m_clientSocket;
        Mod_TCPIP_SocketListener listener;
        public Mod_TCPIP_ClientHandler(Socket clientSocket)
        {
            m_clientSocket = clientSocket;
            listener = new Mod_TCPIP_SocketListener();
        }
        /*
        public event dMessageReceived evnMsgRcv
        {
            add
            {
                listener.evnMsgRcv += value;
            }
            remove
            {
                listener.evnMsgRcv -= value;
            }
        }
        public event dDisconnection evnDisconnect
        {
            add
            {
                listener.evnDisconnect += value;
            }
            remove
            {
                listener.evnDisconnect -= value;
            }
        }
        */
        public void Receive()
        {
            //listener.DataReceiving(m_clientSocket);
        }

        public void Send(byte[] buffer)
        {
            if (m_clientSocket == null)
            {
                throw new Exception("Can't send data. ConnectedClient is Closed!");
            }
            m_clientSocket.Send(buffer);

        }

        public void Stop()
        {
            //listener.StopListening();
            m_clientSocket = null;
        }
    }   //End of class Mod_TCPIP_ClientHandler


    /* ================ reserved for reference ================ 
    //#define OPTION_TPSW
    //using System;
    // ...
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
    */
}
