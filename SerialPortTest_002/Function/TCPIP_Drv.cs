using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Driver_Layer
{
    public class Drv_TCPSocket_Server
    {
        private static bool _connectedFlag = false;
        private static Socket _socketConnection;
        private static AsyncCallback _dataTransferCallback;
        private Thread listenThread;
        //private byte[] _receiveBuffer = new byte[1024];
        private object _Lock = new object();
        
        private static string receivedData;

        /* code2study.blogspot.com/2011/12/c.html */
        public delegate void UpdateTBCallback(string showText);
        //private delegate void UpdateUICallback(string showText, Control ctrl);
        public UpdateTBCallback _updateTBRecvCallback;
        public UpdateTBCallback _updateTBSendCallback;
        /* ************************************** */

        //public string ipAddress;    //192.168.234.8
        //private int socketPort;
        private const short SocketPort = 1025;
        private const int MaxLengthOfPendingConnectionsQueue = 10;
        
        /* ludwigstuyck.wordpress.com/2012/09/21/communicating-through-sockets/ */

        public void Start()
        {
            listenThread = new Thread(new ThreadStart(CreateSocket));
            listenThread.IsBackground = true;
            listenThread.Start();
        }
        
        private void CreateSocket()
        {
            _socketConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Any, SocketPort);
            /*
             *  Using Bind() method we associate a network address to the Server Socket
             *  All client that will connect to this Server Socket must know this network Address
             */
            _socketConnection.Bind(endPoint);

            /* Using Listen() method we create the Client list that will want to connect to Server */
            _socketConnection.Listen(MaxLengthOfPendingConnectionsQueue);

            /* Begins an asynchronous operation to accept an incoming connection attempt. */
            _socketConnection.BeginAccept(OnConnectRequest, null);
                 
            lock (_Lock)
            {
                _connectedFlag = true;
            }
            //MessageBox.Show("Server starts listening...");
            _updateTBRecvCallback("Server starts listening...");
        }

        public void OnConnectRequest(IAsyncResult asyncRes)
        {
            try
            {
                // EndAccept completes a call to BeginAccept. 
                // It returns a new Socket that can be used to send data to and receive data from the remote host.
                var dataTransferSocket = _socketConnection.EndAccept(asyncRes);
                WaitForData(dataTransferSocket);
                _socketConnection.BeginAccept(OnConnectRequest, null);
            }
            catch (ObjectDisposedException)
            {
                //Console.WriteLine("OnConnectRequest: Socket has been closed.");
                _updateTBRecvCallback("OnConnectRequest: Socket has been closed.");
            }
            catch (SocketException sEx)
            {
                Console.WriteLine(string.Format("Something fishy happened: {0}", sEx.Message));
                throw;
            }
        }

        //In the OnDataReceived operation we check if there is data, if there is, we keep calling the WaitForData again to get the next data.
        //If there is no data anymore, we stop the transfer and close the socket:
        public void OnDataReceived(IAsyncResult asyncRes)
        {
            try
            {
                var socketPacket = (SocketPacket)asyncRes.AsyncState;
                int numberOfByteReceived = socketPacket.Socket.EndReceive(asyncRes);

                if (numberOfByteReceived <= 0)
                {
                    //MessageBox.Show("Disconnected with Client - " + socketPacket.Socket.RemoteEndPoint);
                    _updateTBRecvCallback("Disconnected with Client - " + socketPacket.Socket.RemoteEndPoint);
                    socketPacket.Socket.Close();
                    return;
                }

                receivedData = Encoding.ASCII.GetString(socketPacket.DataBuffer, 0, numberOfByteReceived);
                //MessageBox.Show("[From Client " + socketPacket.Socket.RemoteEndPoint + "] " + receivedData);
                _updateTBRecvCallback("[From Client " + socketPacket.Socket.RemoteEndPoint + "] " + receivedData);

                WaitForData(socketPacket.Socket);
            }
            catch (ObjectDisposedException)
            {
                //Console.WriteLine("OnConnectRequest: Socket has been closed.");
                _updateTBRecvCallback("OnConnectRequest: Socket has been closed.");
                throw;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(string.Format("Something fishy happened: {0}", ex.Message));
                throw;
            }
        }

        //We first call the WaitForData operation, and then call the BeginAccept operation again
        // to continue listening to incoming connection requests of other clients.
        //The WaitForData starts the actual data receiving: if the connected client sends data, the OnDataReceived callback operation is executed.
        public void WaitForData(Socket dataTransferSocket)
        {
            try
            {
                if (_dataTransferCallback == null)
                {
                    _dataTransferCallback = OnDataReceived;
                }

                var socketPacket = new SocketPacket(dataTransferSocket);

                dataTransferSocket.BeginReceive(socketPacket.DataBuffer, 0, socketPacket.DataBuffer.Length,
                    SocketFlags.None, _dataTransferCallback, socketPacket);

                string responseMsg = "[From Server] Ack response!";
                var msgBytes = Encoding.ASCII.GetBytes(responseMsg);
                dataTransferSocket.Send(msgBytes);
                Thread.Sleep(3000);
                responseMsg = "Robot, please go next step.";
                msgBytes = Encoding.ASCII.GetBytes(responseMsg);
                dataTransferSocket.Send(msgBytes);

                _updateTBSendCallback(responseMsg);
                //byte[] sendBuffer = new byte[1024];
                //sendBuffer = Encoding.ASCII.GetBytes(responseMsg);
                //dataTransferSocket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(string.Format("Something fishy happened: {0}", ex.Message));
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
            return _connectedFlag;
        }

        /* Clean up socket. */
        public void CloseSocket()
        {
            lock (_Lock)
            {
                _connectedFlag = false;
            }

            _socketConnection.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _updateTBRecvCallback("Server stops listening!!!");
        }
        

        public class SocketPacket
        {
            public Socket Socket { get; set; }
            public byte[] DataBuffer { get; set; }

            public SocketPacket(Socket socket)
            {
                Socket = socket;
                DataBuffer = new byte[1024];
            }
        }

    }   //End of class Drv_TCPSocket_Server

    public class Drv_TCPSocket_Client
    {
        private Socket _socket;
        private static bool _connectedFlag = false;
        private Thread clientReceiveThread;
        private Thread clientSendThread;
        private object _Lock = new object();
        private string remote_ipAddress;
        private int remote_portNumber;
        private byte[] _receiveBuffer = new byte[1024];
        private static string sendData;

        //private static AsyncCallback _dataTransferCallback;

        //static private string showMessage;

        /* code2study.blogspot.com/2011/12/c.html */
        public delegate void UpdateTBRecvCallback(string showText);
        //private delegate void UpdateUICallback(string showText, Control ctrl);
        public UpdateTBRecvCallback _updateTBRecvCallback;

        public delegate void UpdateTBSendCallback(string showText);
        public UpdateTBSendCallback _updateTBSendCallback;
        /* ************************************** */


        public void Start()
        {
            ConnectThread();
        }

        public void Send(string message)
        {
            SendThread(message);
        }

        public void Close()
        {
            CloseConnection();
        }

        private void ConnectThread()
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
                _updateTBRecvCallback("ClientEnd connect exception: " + ex.Message);
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
                    _updateTBRecvCallback("Connecting is successfully!");
                }

                while (_connectedFlag)
                {
                    int byteRecv = _socket.Receive(_receiveBuffer);
                    string msgRecv = Encoding.ASCII.GetString(_receiveBuffer, 0, byteRecv);
                    _updateTBRecvCallback("\r\n" + msgRecv);
                }
            }
            catch (SocketException sEx)
            {
                //MessageBox.Show("Socket listening exception: " + sEx);
                _updateTBSendCallback("Socket listening exception: " + sEx.Message);
                clientReceiveThread.Abort();
            }
        }

        private void SendThread(string message)
        {
            try
            {
                sendData = message;
                clientSendThread = new Thread(new ThreadStart(SendMessage));
                clientSendThread.IsBackground = true;
                _updateTBSendCallback("Client is ready to send message!");
                //Thread.Sleep(2000);
                clientSendThread.Start();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("On client connect exception " + e);
                _updateTBSendCallback("ClientEnd sending exception: " + ex.Message);
            }
        }

        private void SendMessage()
        {
            try
            {
                string clientMessage = sendData;
                byte[] clientMessageAsByte = Encoding.ASCII.GetBytes(clientMessage);
                //_socket.BeginSend(clientMessageAsByte, 0, clientMessageAsByte.Length, SocketFlags.None, _dataTransferCallback, null);
                _socket.Send(clientMessageAsByte, SocketFlags.None);
                //MessageBox.Show("Client sent his message - should be received by server");
                _updateTBSendCallback(clientMessage);
            }
            catch (SocketException sEx)
            {
                //MessageBox.Show("Socket sending exception: " + sEx);
                _updateTBSendCallback("Socket sending exception: " + sEx.Message);
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
