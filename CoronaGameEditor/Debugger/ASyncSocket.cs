#region Imports
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System;
#endregion
public class ASyncSocket
{
    #region Déclarations
    private Socket Socket;
    private byte[] Buffer = new byte[1024];
    private String SendStream = "";
    private List<SocketStates> States = new List<SocketStates>();
    #endregion
    #region Enumération
    enum SocketStates
    {
        IsConnecting = 0,
        IsAccepting = 1,
        IsSending = 2,
        IsReceving = 3,
        IsDisconnecting = 4
    }
    #endregion
    #region Evènements
    public event OnListenEventHandler OnListen;
    public delegate void OnListenEventHandler();
    public event OnListenFailedEventHandler OnListenFailed;
    public delegate void OnListenFailedEventHandler(Exception Exception);
    public event OnConnectEventHandler OnConnect;
    public delegate void OnConnectEventHandler();
    public event OnConnectFailedEventHandler OnConnectFailed;
    public delegate void OnConnectFailedEventHandler(Exception Exception);
    public event OnSendEventHandler OnSend;
    public delegate void OnSendEventHandler(string SendStream);
    public event OnSendFailedEventHandler OnSendFailed;
    public delegate void OnSendFailedEventHandler(Exception Exception);
    public event OnAcceptEventHandler OnAccept;
    public delegate void OnAcceptEventHandler(ASyncSocket AcceptedSocket);
    public event OnAcceptFailedEventHandler OnAcceptFailed;
    public delegate void OnAcceptFailedEventHandler(Exception Exception);
    public event OnReceiveEventHandler OnReceive;
    public delegate void OnReceiveEventHandler(string Stream,ASyncSocket sender);
    public event OnReceiveFailedEventHandler OnReceiveFailed;
    public delegate void OnReceiveFailedEventHandler(Exception Exception);
    public event OnDisconnectEventHandler OnDisconnect;
    public delegate void OnDisconnectEventHandler(ASyncSocket sender);
    public event OnDisconnectFailedEventHandler OnDisconnectFailed;
    public delegate void OnDisconnectFailedEventHandler(Exception Exception);
    #endregion
    #region Conctructeurs
    public ASyncSocket()
    {
        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
    public ASyncSocket(Socket Socket, [System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute(false)] // ERROR: Optional parameters aren't supported in C#
bool ListenData)
    {
        this.Socket = Socket;
        if (ListenData) this.ListenData();
    }
    public ASyncSocket(string Ip, int Port)
    {
        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Connect(Ip, Port);
    }

    #endregion
    #region Fonctions

    public bool isSimulator = true;
    public void Connect(string Ip, int Port)
    {
        if ((Socket == null)) return;
        try
        {
            States.Add(SocketStates.IsConnecting);
            Socket.BeginConnect(new IPEndPoint(System.Net.IPAddress.Parse(Ip), Port), ConnectCallBack, Socket);
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsConnecting);
            if (OnConnectFailed != null)
            {
                OnConnectFailed(Exception);
            }
        }
    }
    public void Send(string Stream)
    {

        if ((!IsConnected) || (Socket == null)) return;
        try
        {
            byte[] Buffer = Encoding.Default.GetBytes(Stream);
            this.States.Add(SocketStates.IsSending);
            this.Socket.Poll(1000, SelectMode.SelectRead);
            this.SendStream = Stream;
            IAsyncResult result = this.Socket.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None, OnSendCallBack, Stream);
            
        }
        catch (Exception Exception)
        {
            this.States.Remove(SocketStates.IsSending);
            if (OnSendFailed != null)
            {
                OnSendFailed(Exception);
            }
        }
    }
    public void Disconnect()
    {
        if ((!IsConnected) || (Socket == null)) return;
        try
        {
            

            while (States.Count != 0)
            {

                if (States.Contains(SocketStates.IsReceving) || States.Contains(SocketStates.IsSending))
                {
                    States.Clear();
                    break;
                }
                       

                Thread.Sleep(1);
            }
            States.Add(SocketStates.IsDisconnecting);
            Socket.BeginDisconnect(false, OnDisconnectCallBack, null);
            
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsDisconnecting);
            if (OnDisconnectFailed != null)
            {
                OnDisconnectFailed(Exception);
            }
        }
    }
    public void Listen(string ip,int Port)
    {
        if ((Socket == null)) return;
        try
        {
            Socket.Bind(new IPEndPoint(IPAddress.Parse(ip), Port));
            Socket.Listen(0);
            ListenSocket();
            if (OnListen != null)
            {
                OnListen();
            }
        }
        catch (Exception Exception)
        {
            if (OnListenFailed != null)
            {
                OnListenFailed(Exception);
            }
        }
    }
    public void ListenEvent()
    {
        if (OnListen != null)
        {
            OnListen();
        }
    }
    private void ListenSocket()
    {
        if ((Socket == null)) return;
        try
        {
            States.Add(SocketStates.IsAccepting);
            Socket.BeginAccept(OnAcceptCallBack, null);
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsAccepting);
            if (OnAcceptFailed != null)
            {
                OnAcceptFailed(Exception);
            }
        }
    }
    public void StopListenSocket()
    {
        if ((Socket == null)) return;
        try
        {
            Socket.Close(100);
        }
        catch (Exception Exception)
        {
        
        }
    }
    private void ListenData()
    {
        Thread.Sleep(100);
        if ((!IsConnected) || (Socket == null)) return;
        try
        {
            States.Add(SocketStates.IsReceving);
            Socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, OnReceiveCallBack, null);
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsReceving);
            if (OnReceiveFailed != null)
            {
                OnReceiveFailed(Exception);
            }
        }
    }
    private bool StateIs(SocketStates SocketState)
    {
        if (States.Contains(SocketState)) return true;
        return false;
    }
    #endregion
    #region Callbacks
    private void ConnectCallBack(IAsyncResult IAsyncResult)
    {
        if ((Socket == null) || (IAsyncResult == null) || (!StateIs(SocketStates.IsConnecting))) return;
        try
        {
            Socket.EndConnect(IAsyncResult);
            States.Remove(SocketStates.IsConnecting);
            ListenData();
            if (OnConnect != null)
            {
                OnConnect();
            }
        }
        catch (Exception Exception)
        {
            this.States.Remove(SocketStates.IsConnecting);
            if (OnConnectFailed != null)
            {
                OnConnectFailed(Exception);
            }
        }
    }
    private void OnReceiveCallBack(IAsyncResult IAsyncResult)
    {

        if ((!(bool)IsConnected) || (Socket == null) || (IAsyncResult == null) || (!StateIs(SocketStates.IsReceving))) return;
        try
        {
            int Bytes = Socket.EndReceive(IAsyncResult);
            States.Remove(SocketStates.IsReceving);
            if (Bytes > 0)
            {

                string Stream = Encoding.Default.GetString(Buffer);
                Array.Clear(Buffer, 0, Buffer.Length - 1);
                if (OnReceive != null)
                {
                    OnReceive(Stream, this);
                }
            }
            else
            {
                
                Socket sSocket = (Socket)IAsyncResult.AsyncState;
                if (sSocket != null)
                {
                    ASyncSocket AsyncSocket = new ASyncSocket(sSocket, true);
                    AsyncSocket.Disconnect();

                }
                    
               
            }
            ListenData();
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsReceving);
            if (OnReceiveFailed != null)
            {
                OnReceiveFailed(Exception);
            }
        }
    }
    private void OnSendCallBack(IAsyncResult IAsyncResult)
    {
        if ((!(bool)IsConnected) || (Socket == null) || (IAsyncResult == null) || (!StateIs(SocketStates.IsSending))) return;
        try
        {
            String MessageSend = (String)IAsyncResult.AsyncState;
            if (MessageSend != null)
            {
                SendStream = MessageSend;
            }
            Socket.EndSend(IAsyncResult);
            States.Remove(SocketStates.IsSending);
            if (OnSend != null)
            {
                OnSend(SendStream);
            }
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsSending);
            if (OnSendFailed != null)
            {
                OnSendFailed(Exception);
            }
        }
    }
    private void OnDisconnectCallBack(IAsyncResult IAsyncResult)
    {
        if ((Socket == null) || (IAsyncResult == null) || (!StateIs(SocketStates.IsDisconnecting))) return;
        try
        {
            Socket.EndDisconnect(IAsyncResult);
            States.Remove(SocketStates.IsDisconnecting);
            StopListenSocket();
            if (OnDisconnect != null)
            {
                OnDisconnect(this);
            }
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsDisconnecting);
            if (OnDisconnectFailed != null)
            {
                OnDisconnectFailed(Exception);
            }
        }
    }
    private void OnAcceptCallBack(IAsyncResult IAsyncResult)
    {
        if ((Socket == null) || (IAsyncResult == null) || (!StateIs(SocketStates.IsAccepting))) return;
        try
        {
            ASyncSocket AcceptedSocket = new ASyncSocket(Socket.EndAccept(IAsyncResult), true);
            States.Remove(SocketStates.IsAccepting);
            ListenSocket();
            if (OnAccept != null)
            {
                OnAccept(AcceptedSocket);
            }
        }
        catch (Exception Exception)
        {
            States.Remove(SocketStates.IsAccepting);
            if (OnAcceptFailed != null)
            {
                OnAcceptFailed(Exception);
            }
        }
    }
    #endregion
    #region Propriétés
    public bool IsConnected
    {
        get
        {
            if ((Socket == null)) return false;
            try
            {
                return Socket.Connected;
            }
            catch (Exception Exception)
            {
                return false;
            }
        }
    }
    public IPEndPoint NetworEndPoint
    {
        get
        {
            if ((Socket == null)) return null;
            try
            {
                return (IPEndPoint)Socket.RemoteEndPoint;
            }
            catch (Exception Exception)
            {
                return null;
            }
        }
    }
    public IPAddress NetworkAddress
    {
        get
        {
            if ((Socket == null)) return null;
            try
            {
                return ((IPEndPoint)Socket.RemoteEndPoint).Address;
            }
            catch (Exception Exception)
            {
                return null;
            }
        }
    }
    public Int32 NetworkPort
    {
        get
        {
            if ((Socket == null)) return new Int32();
            try
            {
                return ((IPEndPoint)Socket.RemoteEndPoint).Port;
            }
            catch (Exception Exception)
            {
                return new Int32();
            }
        }
    }
    #endregion
}
