using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.Debugger;
using GMap.NET;
using System.Web.Script.Serialization;

namespace Krea.RemoteDebugger
{
    public partial class AppRemoteController : UserControl
    {
        //---------------------------------------------------
        //-------------------Attributs------------------------
        //---------------------------------------------------
        private static ASyncSocket server;
        public List<ASyncSocket> AcceptedClients;
        public String LastCommandReceive;
        public String CurrentCommandReceive;
        public String CurrentCommandSend;
        private RemoteControllerForm remoteControllerForm;

        private string currentBuffer = "";
        private bool isMouseDown = false;
        private bool valueChanged = false;
        private int scrollCount = 0;
        DateTime origine = new DateTime(1970, 1, 1);
        private bool accelerometerIsShake = false;
        private JavaScriptSerializer deserializer;

        private int currentGPSMarkerIndex;
        //---------------------------------------------------
        //-------------------Constructor------------------------
        //---------------------------------------------------
        public AppRemoteController()
        {
            InitializeComponent();

            this.AcceptedClients = new List<ASyncSocket>();

            deserializer = new JavaScriptSerializer();
            deserializer.MaxJsonLength = 999999999;

            this.geographicCompassView.init(this);
            this.magneticCompassView.init(this);
            this.keyView1.init(this);

            this.googleMapControl1.initGoogleMap();
        }

        

        //---------------------------------------------------
        //-------------------Methods------------------------
        //---------------------------------------------------
        public void initServer(string ip, int port, RemoteControllerForm remoteControllerForm)
        {
            this.remoteControllerForm = remoteControllerForm;
            if (server != null)
            {
                server.Disconnect();
                server.StopListenSocket();
                server = null;

            }

            if (server == null)
            {
                LastCommandReceive = "";
                CurrentCommandSend = "";

                server = new ASyncSocket();
                server.OnAccept += new ASyncSocket.OnAcceptEventHandler(server_OnAccept);
                server.OnReceive += new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                server.OnSend += new ASyncSocket.OnSendEventHandler(server_OnSend);
                server.OnListen += new ASyncSocket.OnListenEventHandler(server_OnListen);
                server.OnAcceptFailed += new ASyncSocket.OnAcceptFailedEventHandler(server_OnAcceptFailed);
                server.OnListenFailed += new ASyncSocket.OnListenFailedEventHandler(server_OnListenFailed);
                server.OnReceiveFailed += new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                server.OnSendFailed += new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);
               
                server.Listen(ip, port);
            }

            currentBuffer = "";
           

            // config map 
            //this.gMapControl1.MapProvider = GMapProviders.GoogleMap;
            //this.gMapControl1.Position = new PointLatLng(54.6961334816182, 25.2985095977783);
            //this.gMapControl1.MinZoom = 0;
            //this.gMapControl1.MaxZoom = 24;
            //this.gMapControl1.Zoom = 9;

            //this.gMapControl1.Overlays.Add(routes);
            //this.gMapControl1.Overlays.Add(markers);

        }

        //////////////////////////////////////////
        /// ASyncSocket SERVER EVENT
        //////////////////////////////////////////
        delegate void dserver_OnSendFailed(Exception Exception);
        void server_OnSendFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnSendFailed d = new dserver_OnSendFailed(server_OnSendFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\n> Send Failed: " + Exception.Message + "\n";
                
            }
        }

        delegate void dserver_OnReceiveFailed(Exception Exception);
        void server_OnReceiveFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnReceiveFailed d = new dserver_OnReceiveFailed(server_OnReceiveFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {


                String command = "\n> Receive Failed: " + Exception.Message + "\n";

          
            }
        }

        delegate void dserver_OnListenFailed(Exception Exception);
        void server_OnListenFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnListenFailed d = new dserver_OnListenFailed(server_OnListenFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\n> Listen Failed: " + Exception.Message + "\n";

               
            }
        }

        delegate void dserver_OnAcceptFailed(Exception Exception);
        void server_OnAcceptFailed(Exception Exception)
        {
            if (this.InvokeRequired)
            {
                dserver_OnAcceptFailed d = new dserver_OnAcceptFailed(server_OnAcceptFailed);
                object[] args = { Exception };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\n> Accept Failed : " + Exception.Message + "\n";
             
            }
        }

        delegate void dserver_OnListen();
        void server_OnListen()
        {
            if (this.InvokeRequired)
            {
                dserver_OnListen d = new dserver_OnListen(server_OnListen);
                this.Invoke(d);
            }
            else
            {
                String command = "\n> Debugger ready.\n";
                command += "\n> Waiting for the emulator.\n";
              
            }
        }

        delegate void dserver_OnSend(String SendStream);
        void server_OnSend(String SendStream)
        {
            if (this.InvokeRequired)
            {
                dserver_OnSend d = new dserver_OnSend(server_OnSend);
                object[] args = { SendStream };
                this.Invoke(d, args);
            }
            else
            {
                String command = "\n> " + SendStream + "\n";
                this.CurrentCommandSend = SendStream;
               
            }
        }

        delegate void dserver_OnReceive(string Stream, ASyncSocket AcceptedSocket);
        void server_OnReceive(string Stream,ASyncSocket AcceptedSocket)
        {
            if (this.InvokeRequired)
            {
                dserver_OnReceive d = new dserver_OnReceive(server_OnReceive);
                object[] args = { Stream, AcceptedSocket };
                this.Invoke(d, args);
            }
            else
            {
                
                    //Receive All waiting message from the sockets.
                    this.CurrentCommandReceive = new String(Stream.ToCharArray());
                    this.analyseReceivedMessage(this.CurrentCommandReceive, AcceptedSocket);
                    
            }

        }

        
        private void analyseReceivedMessage(string stream, ASyncSocket AcceptedSocket)
        {
            if (this.CurrentCommandReceive.StartsWith("EXIT"))
            {
                this.AcceptedClients.Remove(AcceptedSocket);
                AcceptedSocket.OnReceive -= new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                AcceptedSocket.OnSend -= new ASyncSocket.OnSendEventHandler(server_OnSend);

                AcceptedSocket.OnDisconnect -= new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);

                AcceptedSocket.OnReceiveFailed -= new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                AcceptedSocket.OnSendFailed -= new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);
                AcceptedSocket.Disconnect();
                AcceptedSocket = null;

                return;
            }


            this.currentBuffer += stream;

            //SPlit the message
            string[] messages = currentBuffer.Split('|');
            for (int i = 0; i < messages.Length; i++)
            {
                string message = messages[i].Replace("\0", "");

                if (message.StartsWith("[EXIT]"))
                {
                    this.AcceptedClients.Remove(AcceptedSocket);
                    AcceptedSocket.OnReceive -= new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                    AcceptedSocket.OnSend -= new ASyncSocket.OnSendEventHandler(server_OnSend);

                    AcceptedSocket.OnDisconnect -= new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);

                    AcceptedSocket.OnReceiveFailed -= new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                    AcceptedSocket.OnSendFailed -= new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);
                    AcceptedSocket.Disconnect();
                    AcceptedSocket = null;

                    deserializer = null;
                    return;
                }

                else if(message.StartsWith("[DEVICE]") && message.EndsWith("[END]"))
                {
                    AcceptedSocket.isSimulator = false;

                    int indexMessage = this.currentBuffer.IndexOf(message);
                    if (indexMessage > -1)
                    {
                        this.currentBuffer = this.currentBuffer.Remove(indexMessage, message.Length);
                    }

                    this.currentBuffer.Replace("||", "|");
                    message = message.Replace("[DEVICE]", "").Replace("[END]", "");
                    
                }
                else if (message.StartsWith("[SIMULATOR]") && message.EndsWith("[END]"))
                {
                    AcceptedSocket.isSimulator = true;
                    int indexMessage = this.currentBuffer.IndexOf(message);
                    if (indexMessage > -1)
                    {
                        this.currentBuffer = this.currentBuffer.Remove(indexMessage, message.Length);
                    }

                    this.currentBuffer.Replace("||", "|");
                    message = message.Replace("[SIMULATOR]", "").Replace("[END]", "");
                    
                }
                else
                {
                    continue;
                }


                
                this.sendEvent(message);
                if (!message.Equals(""))
                {
                    try
                    {
                        Dictionary<string, object> dico = deserializer.Deserialize<Dictionary<string, object>>(message);

                        object eventNameOBJ = null;
                        if (dico.TryGetValue("name", out eventNameOBJ) == true)
                        {
                            string eventNameSTR = eventNameOBJ.ToString();
                            //------------------------------------------------------------------------------------------------------------------------
                            //---------------------------------------------ACCELEROMETER -------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            if (eventNameSTR.Equals("accelerometer"))
                            {
                                object xGravityOBJ = null;
                                if(dico.TryGetValue("xGravity", out xGravityOBJ) ==true)
                                {
                                    double value = Convert.ToDouble(xGravityOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.acceleromterXTrackBar.Maximum) intValue = this.acceleromterXTrackBar.Maximum;
                                    if (intValue < this.acceleromterXTrackBar.Minimum) intValue = this.acceleromterXTrackBar.Minimum;
                                    this.acceleromterXTrackBar.Value = intValue;
   
                                }

                                object yGravityOBJ = null;
                                if (dico.TryGetValue("yGravity", out yGravityOBJ) == true)
                                {
                                    double value = Convert.ToDouble(yGravityOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.acceleromterYTrackBar.Maximum) intValue = this.acceleromterYTrackBar.Maximum;
                                    if (intValue < this.acceleromterYTrackBar.Minimum) intValue = this.acceleromterYTrackBar.Minimum;
                                    this.acceleromterYTrackBar.Value = intValue;

                                }

                                object zGravityOBJ = null;
                                if (dico.TryGetValue("zGravity", out zGravityOBJ) == true)
                                {
                                    double value = Convert.ToDouble(zGravityOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.acceleromterZTrackBar.Maximum) intValue = this.acceleromterZTrackBar.Maximum;
                                    if (intValue < this.acceleromterZTrackBar.Minimum) intValue = this.acceleromterZTrackBar.Minimum;
                                    this.acceleromterZTrackBar.Value = intValue;

                                }

                                object xInstantOBJ = null;
                                if (dico.TryGetValue("xInstant", out xInstantOBJ) == true)
                                {
                                    double value = Convert.ToDouble(xInstantOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.accelerometerXInstantTrackBar.Maximum) intValue = this.accelerometerXInstantTrackBar.Maximum;
                                    if (intValue < this.accelerometerXInstantTrackBar.Minimum) intValue = this.accelerometerXInstantTrackBar.Minimum;
                                    this.accelerometerXInstantTrackBar.Value = intValue;

                                }

                                object yInstantOBJ = null;
                                if (dico.TryGetValue("yInstant", out yInstantOBJ) == true)
                                {
                                    double value = Convert.ToDouble(yInstantOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.accelerometerYInstantTrackBar.Maximum) intValue = this.accelerometerYInstantTrackBar.Maximum;
                                    if (intValue < this.accelerometerYInstantTrackBar.Minimum) intValue = this.accelerometerYInstantTrackBar.Minimum;
                                    this.accelerometerYInstantTrackBar.Value = intValue;

                                }

                                object zInstantOBJ = null;
                                if (dico.TryGetValue("zInstant", out zInstantOBJ) == true)
                                {
                                    double value = Convert.ToDouble(zInstantOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.accelerometerZInstantTrackBar.Maximum) intValue = this.accelerometerZInstantTrackBar.Maximum;
                                    if (intValue < this.accelerometerZInstantTrackBar.Minimum) intValue = this.accelerometerZInstantTrackBar.Minimum;
                                    this.accelerometerZInstantTrackBar.Value = intValue;

                                }
                              
                            }

                            //------------------------------------------------------------------------------------------------------------------------
                            //---------------------------------------------COMPASS -------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            else if (eventNameSTR.Equals("heading"))
                            {
                                object geographicOBJ = null;
                                if (dico.TryGetValue("geographic", out geographicOBJ) == true)
                                {
                                    float value = (float)Convert.ToDouble(geographicOBJ);
                                    this.geographicCompassView.applyAngleFromCompassValues(value);
                                    this.geographicCompassView.refreshGUI();
                                }

                                object magneticOBJ = null;
                                if (dico.TryGetValue("magnetic", out magneticOBJ) == true)
                                {
                                    float value = (float)Convert.ToDouble(magneticOBJ);
                                    this.magneticCompassView.applyAngleFromCompassValues(value);
                                    this.magneticCompassView.refreshGUI();
                                    
                                }
                                
                            }

                            //------------------------------------------------------------------------------------------------------------------------
                            //---------------------------------------------GYROSCOPE -------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            else if (eventNameSTR.Equals("gyroscope"))
                            {
                                object xRotationOBJ = null;
                                if (dico.TryGetValue("xRotation", out xRotationOBJ) == true)
                                {
                                    double value = Convert.ToDouble(xRotationOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.gyroscopeXAxisTrackBar.Maximum) intValue = this.gyroscopeXAxisTrackBar.Maximum;
                                    if (intValue < this.gyroscopeXAxisTrackBar.Minimum) intValue = this.gyroscopeXAxisTrackBar.Minimum;
                                    this.gyroscopeXAxisTrackBar.Value = intValue;

                                }

                                object yRotationOBJ = null;
                                if (dico.TryGetValue("yRotation", out yRotationOBJ) == true)
                                {
                                    double value = Convert.ToDouble(yRotationOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.gyroscopeYAxisTrackBar.Maximum) intValue = this.gyroscopeYAxisTrackBar.Maximum;
                                    if (intValue < this.gyroscopeYAxisTrackBar.Minimum) intValue = this.gyroscopeYAxisTrackBar.Minimum;
                                    this.gyroscopeYAxisTrackBar.Value = intValue;

                                }

                                object zRotationOBJ = null;
                                if (dico.TryGetValue("zRotation", out zRotationOBJ) == true)
                                {
                                    double value = Convert.ToDouble(zRotationOBJ);
                                    int intValue = Convert.ToInt32(value * (float)1000);
                                    if (intValue > this.gyroscopeZAxisTrackBar.Maximum) intValue = this.gyroscopeZAxisTrackBar.Maximum;
                                    if (intValue < this.gyroscopeZAxisTrackBar.Minimum) intValue = this.gyroscopeZAxisTrackBar.Minimum;
                                    this.gyroscopeZAxisTrackBar.Value = intValue;

                                }
                            }

                            //------------------------------------------------------------------------------------------------------------------------
                            //---------------------------------------------KEY -------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            else if (eventNameSTR.Equals("key"))
                            {
                                //Recuperer le nom du bouton
                                object keyNameOBJ = null;
                                if(dico.TryGetValue("keyName",out keyNameOBJ) == true) 
                                {
                                    string keyNameSTR = keyNameOBJ.ToString();

                                    object phaseOBJ = null;
                                    if(dico.TryGetValue("phase",out phaseOBJ) == true) 
                                    {
                                        string phaseSTR = phaseOBJ.ToString();
                                        this.keyView1.refreshButtonStateFromRemote(keyNameSTR, phaseSTR);
                                    }
                                  
                                }
                            }

                            //------------------------------------------------------------------------------------------------------------------------
                            //---------------------------------------------GPS -------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            else if (eventNameSTR.Equals("location"))
                            {
                                PointLatLng location = new PointLatLng();
                                 object latitudeOBJ = null;
                                 
                                 if (dico.TryGetValue("latitude", out latitudeOBJ) == true)
                                 {
                                     location.Lat= Convert.ToDouble(latitudeOBJ);
                                 }

                                 object longitudeOBJ = null;

                                 if (dico.TryGetValue("longitude", out longitudeOBJ) == true)
                                 {
                                     location.Lng = Convert.ToDouble(longitudeOBJ);
                                 }
                                 if (this.gpsTimer.Enabled == true)
                                     this.stopRouteBt_Click(null, null);

                                 this.googleMapControl1.setRemoteGGPSMarkerLocation(location);

                                 object speedOBJ = null;
                                 if (dico.TryGetValue("speed", out speedOBJ) == true)
                                 {
                                     this.speedTxtBx.Text = Convert.ToDouble(speedOBJ).ToString();
                                 }

                                 object accuracyOBJ = null;
                                 if (dico.TryGetValue("accuracy", out accuracyOBJ) == true)
                                 {
                                     this.accuracyTxtBx.Text = Convert.ToDouble(accuracyOBJ).ToString();
                                 }

                                 object altitudeOBJ = null;
                                 if (dico.TryGetValue("altitude", out altitudeOBJ) == true)
                                 {
                                     this.altitudeTxtBx.Text = Convert.ToDouble(altitudeOBJ).ToString();
                                 }

                                 object timeOBJ = null;
                                 if (dico.TryGetValue("time", out timeOBJ) == true)
                                 {
                                     this.timeTxtBx.Text = Convert.ToDouble(timeOBJ).ToString();
                                 }

                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error MESSAGE : " + ex.Message);
                    }
                  

                }
                   
                
            }

        }

        private void analyseMessage(string stream, ASyncSocket AcceptedSocket)
        {
            if (this.CurrentCommandReceive.Contains("EXIT"))
            {
                this.AcceptedClients.Remove(AcceptedSocket);
                AcceptedSocket.OnReceive -= new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                AcceptedSocket.OnSend -= new ASyncSocket.OnSendEventHandler(server_OnSend);

                AcceptedSocket.OnDisconnect -= new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);

                AcceptedSocket.OnReceiveFailed -= new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                AcceptedSocket.OnSendFailed -= new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);
                AcceptedSocket.Disconnect();
                AcceptedSocket = null;
               
                return;
            }

            bool hasFoundAccelerometerValues = false;
            bool hasFoundCompassValues = false;
            bool hasFoundGyroscopeValues = false;
            bool hasFoundGPSValues = false;

            //SPlit the message
            string[] messages = stream.Split('|');
            for (int i = 0; i < messages.Length; i++)
            {
                string message = messages[i];
                string[] commands = message.Split(',');
                for (int j = 0; j < commands.Length; j++)
                {
                    string currentCommand = commands[j];
                    if (currentCommand.Equals("DEVICE"))
                        AcceptedSocket.isSimulator = false;
                    else if (currentCommand.Equals("SIMULATOR"))
                        AcceptedSocket.isSimulator = true;
                    else if (currentCommand.Contains("ACCELEROMETER_XGRAVITY"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        if (keyValue.Length < 2) continue;

                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * (float)1000);
                            if (intValue > this.acceleromterXTrackBar.Maximum) intValue = this.acceleromterXTrackBar.Maximum;
                            if (intValue < this.acceleromterXTrackBar.Minimum) intValue = this.acceleromterXTrackBar.Minimum;
                            this.acceleromterXTrackBar.Value = intValue;
                            hasFoundAccelerometerValues = true;
                        }
                    }
                    else if (currentCommand.Contains("ACCELEROMETER_YGRAVITY"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * (float)1000);
                            if (intValue > this.acceleromterYTrackBar.Maximum) intValue = this.acceleromterYTrackBar.Maximum;
                            if (intValue < this.acceleromterYTrackBar.Minimum) intValue = this.acceleromterYTrackBar.Minimum;
                            this.acceleromterYTrackBar.Value = intValue;
                            hasFoundAccelerometerValues = true;

                        }
                    }
                    else if (currentCommand.Contains("ACCELEROMETER_ZGRAVITY"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * (float)1000);
                            if (intValue > this.acceleromterZTrackBar.Maximum) intValue = this.acceleromterZTrackBar.Maximum;
                            if (intValue < this.acceleromterZTrackBar.Minimum) intValue = this.acceleromterZTrackBar.Minimum;
                            this.acceleromterZTrackBar.Value = intValue;
                            hasFoundAccelerometerValues = true;

                        }
                    }
                    else if (currentCommand.Contains("ACCELEROMETER_XINSTANT"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        if (keyValue.Length < 2) continue;

                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * (float)1000);
                            if (intValue > this.accelerometerXInstantTrackBar.Maximum) intValue = this.accelerometerXInstantTrackBar.Maximum;
                            if (intValue < this.accelerometerXInstantTrackBar.Minimum) intValue = this.accelerometerXInstantTrackBar.Minimum;
                            this.accelerometerXInstantTrackBar.Value = intValue;
                            hasFoundAccelerometerValues = true;
                        }
                    }
                    else if (currentCommand.Contains("ACCELEROMETER_YINSTANT"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * (float)1000);
                            if (intValue > this.accelerometerYInstantTrackBar.Maximum) intValue = this.accelerometerYInstantTrackBar.Maximum;
                            if (intValue < this.accelerometerYInstantTrackBar.Minimum) intValue = this.accelerometerYInstantTrackBar.Minimum;
                            this.accelerometerYInstantTrackBar.Value = intValue;
                            hasFoundAccelerometerValues = true;
                        }
                    }
                    else if (currentCommand.Contains("ACCELEROMETER_ZINSTANT"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * (float)1000);
                            if (intValue > this.accelerometerZInstantTrackBar.Maximum) intValue = this.accelerometerZInstantTrackBar.Maximum;
                            if (intValue < this.accelerometerZInstantTrackBar.Minimum) intValue = this.accelerometerZInstantTrackBar.Minimum;
                            this.accelerometerZInstantTrackBar.Value = intValue;
                            hasFoundAccelerometerValues = true;
                        }
                    }
                    //------------------------COMPASS------------------------------------------------------------------------------------------------------------
                    else if (currentCommand.Contains("COMPASS_GEOGRAPHIC"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            this.geographicCompassView.applyAngleFromCompassValues(value);
                            hasFoundCompassValues = true;
                        }
                    }
                    else if (currentCommand.Contains("COMPASS_MAGNETIC"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            this.magneticCompassView.applyAngleFromCompassValues(value);
                            hasFoundCompassValues = true;
                        }
                    }
                    //------------------------GYROSCOPE------------------------------------------------------------------------------------------------------------
                    else if (currentCommand.Contains("GYROSCOPE_DELTATIME"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            hasFoundGyroscopeValues = true;
                        }
                    }
                    
                    else if (currentCommand.Contains("GYROSCOPE_XROTATION"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value *10);
                            if (intValue > this.gyroscopeXAxisTrackBar.Maximum) intValue = this.gyroscopeXAxisTrackBar.Maximum;
                            if (intValue < this.gyroscopeXAxisTrackBar.Minimum) intValue = this.gyroscopeXAxisTrackBar.Minimum;
                            this.gyroscopeXAxisTrackBar.Value = intValue;
                            hasFoundGyroscopeValues = true;
                        }
                    }

                    else if (currentCommand.Contains("GYROSCOPE_YROTATION"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * 10);
                            if (intValue > this.gyroscopeYAxisTrackBar.Maximum) intValue = this.gyroscopeYAxisTrackBar.Maximum;
                            if (intValue < this.gyroscopeYAxisTrackBar.Minimum) intValue = this.gyroscopeYAxisTrackBar.Minimum;
                            this.gyroscopeYAxisTrackBar.Value = intValue;
                            hasFoundGyroscopeValues = true;
                        }
                    }

                    else if (currentCommand.Contains("GYROSCOPE_ZROTATION"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * 10);
                            if (intValue > this.gyroscopeZAxisTrackBar.Maximum) intValue = this.gyroscopeZAxisTrackBar.Maximum;
                            if (intValue < this.gyroscopeZAxisTrackBar.Minimum) intValue = this.gyroscopeZAxisTrackBar.Minimum;
                            this.gyroscopeZAxisTrackBar.Value = intValue;
                            hasFoundGyroscopeValues = true;
                        }
                    }
                    //------------------------GPS------------------------------------------------------------------------------------------------------------
                    else if (currentCommand.Contains("GYROSCOPE_ZROTATION"))
                    {
                        string[] keyValue = currentCommand.Split('=');
                        float value = -1;
                        if (keyValue.Length < 2) continue;
                        bool res = float.TryParse(keyValue[1].Replace(".", ","), out value);
                        if (res == true)
                        {
                            int intValue = Convert.ToInt32(value * 10);
                            if (intValue > this.gyroscopeZAxisTrackBar.Maximum) intValue = this.gyroscopeZAxisTrackBar.Maximum;
                            if (intValue < this.gyroscopeZAxisTrackBar.Minimum) intValue = this.gyroscopeZAxisTrackBar.Minimum;
                            this.gyroscopeZAxisTrackBar.Value = intValue;
                            hasFoundGyroscopeValues = true;
                        }
                    }
                }

                if (hasFoundAccelerometerValues == true)
                {
                    this.sendAccelerometerEvent();
                    hasFoundAccelerometerValues = false;
                }
                    
                if (hasFoundCompassValues == true)
                {
                    this.sendCompassEvent();
                    hasFoundCompassValues = false;
                    this.magneticCompassView.refreshGUI();
                    this.geographicCompassView.refreshGUI();
                }

                if (hasFoundGyroscopeValues == true)
                {
                    this.sendGyroscopeEvent();
                    hasFoundGyroscopeValues = false;
                   
                }
                    
            }

           
        }


        delegate void dserver_OnDisconnect(ASyncSocket AcceptedSocket);
        void server_OnDisconnect(ASyncSocket AcceptedSocket)
        {
            if (this.InvokeRequired)
            {
                dserver_OnDisconnect d = new dserver_OnDisconnect(server_OnDisconnect);
                object[] args = { AcceptedSocket };
                this.Invoke(d);
            }
            else
            {
                String command = "Remote Disconnected";
              
                
                //              Console.WriteLine(Stream);
            }

        }

        delegate void dserver_OnAccept(ASyncSocket AcceptedSocket);
        void server_OnAccept(ASyncSocket AcceptedSocket)
        {
            if (this.InvokeRequired)
            {
                dserver_OnAccept d = new dserver_OnAccept(server_OnAccept);
                object[] args = { AcceptedSocket };
                this.Invoke(d, args);
            }
            else
            {
                ASyncSocket newClient = AcceptedSocket;
                
                this.AcceptedClients.Add(newClient);

                AcceptedSocket.OnReceive += new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                AcceptedSocket.OnSend += new ASyncSocket.OnSendEventHandler(server_OnSend);
             
              
                AcceptedSocket.OnDisconnect += new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);
           
                AcceptedSocket.OnReceiveFailed += new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                AcceptedSocket.OnSendFailed += new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);

                String command = "\n> Emulator Ready to send order.\n";

           

            }
        }
        public void closeDebuggerSession()
        {
            if (this.gpsTimer.Enabled == true) this.gpsTimer.Stop();

            for (int i = 0; i < this.AcceptedClients.Count; i++)
            {
                ASyncSocket client = this.AcceptedClients[i];
                if (client != null)
                {

                    client.OnReceive -= new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                    client.OnSend -= new ASyncSocket.OnSendEventHandler(server_OnSend);

                    client.OnDisconnect -= new ASyncSocket.OnDisconnectEventHandler(server_OnDisconnect);

                    client.OnReceiveFailed -= new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                    client.OnSendFailed -= new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);

                    client.Send("EXIT");
                    client.Disconnect();
                }
            }

            this.AcceptedClients.Clear();

            if (server != null)
            {
                server.OnAccept -= new ASyncSocket.OnAcceptEventHandler(server_OnAccept);
                server.OnReceive -= new ASyncSocket.OnReceiveEventHandler(server_OnReceive);
                server.OnSend -= new ASyncSocket.OnSendEventHandler(server_OnSend);
                server.OnListen -= new ASyncSocket.OnListenEventHandler(server_OnListen);
                server.OnAcceptFailed -= new ASyncSocket.OnAcceptFailedEventHandler(server_OnAcceptFailed);
                server.OnListenFailed -= new ASyncSocket.OnListenFailedEventHandler(server_OnListenFailed);
                server.OnReceiveFailed -= new ASyncSocket.OnReceiveFailedEventHandler(server_OnReceiveFailed);
                server.OnSendFailed -= new ASyncSocket.OnSendFailedEventHandler(server_OnSendFailed);
              
                server.StopListenSocket();

            }
                 
            server = null;
        }


        //----------------------------------- CORONA REMOTE EVENTS --------------------------------------------------------
        public void sendEvent(string jsonEvent)
        {

            for (int i = 0; i < this.AcceptedClients.Count; i++)
            {
                ASyncSocket client = this.AcceptedClients[i];
                if (client != null)
                {
                    if (client.isSimulator == true)
                    {
                        client.Send(jsonEvent+"\n");
                    }
                }

            }
        }
        public void sendAccelerometerEvent()
        {
            float accelerometerXGravityValue = (float)this.acceleromterXTrackBar.Value / (float)1000;
            float accelerometerYGravityValue = (float)this.acceleromterYTrackBar.Value / (float)1000;
            float accelerometerZGravityValue = (float)this.acceleromterZTrackBar.Value / (float)1000;

            float accelerometerXInstantValue = (float)this.accelerometerXInstantTrackBar.Value / (float)1000;
            float accelerometerYInstantValue = (float)this.accelerometerYInstantTrackBar.Value / (float)1000;
            float accelerometerZInstantValue = (float)this.accelerometerZInstantTrackBar.Value / (float)1000;

            RemoteEvent eventJson = new RemoteEvent("accelerometer", accelerometerXGravityValue, accelerometerYGravityValue, accelerometerZGravityValue,
                accelerometerXInstantValue, accelerometerYInstantValue, accelerometerZInstantValue, this.accelerometerIsShake);

            string serializedJson = eventJson.serialize();
            if (serializedJson != null)
            {
                this.sendEvent(serializedJson);
            }


            serializedJson = null;
        }

        public void sendGyroscopeEvent()
        {
            float XRotationValue = (float)this.gyroscopeXAxisTrackBar.Value / (float)1000;
            float YRotationValue = (float)this.gyroscopeYAxisTrackBar.Value / (float)1000;
            float ZRotationValue = (float)this.gyroscopeZAxisTrackBar.Value / (float)1000;

            RemoteEvent eventJson = new RemoteEvent("gyroscope", XRotationValue, YRotationValue, ZRotationValue);

            string serializedJson = eventJson.serialize();
            if (serializedJson != null)
            {
                this.sendEvent(serializedJson);
            }


            serializedJson = null;

        }

        public void sendGPSEvent()
        {
            PointLatLng gpsPos = this.googleMapControl1.getMarkerPos();
            
        }

        public void sendCompassEvent()
        {
            float geographicCompass = this.geographicCompassView.getLastValue();
            float magneticCompass = this.magneticCompassView.getLastValue();

            RemoteEvent eventJson = new RemoteEvent("heading", geographicCompass, magneticCompass);

            string serializedJson = eventJson.serialize();
            if (serializedJson != null)
            {
                this.sendEvent(serializedJson);
            }


            serializedJson = null;

        }
        private void accelerometerXInstantTrackBar_ValueChanged(object sender, EventArgs e)
        {
            float accelerometerXGravityValue = (float)this.acceleromterXTrackBar.Value / (float)1000;
            float accelerometerYGravityValue = (float)this.acceleromterYTrackBar.Value / (float)1000;
            float accelerometerZGravityValue = (float)this.acceleromterZTrackBar.Value / (float)1000;

            float accelerometerXInstantValue = (float)this.accelerometerXInstantTrackBar.Value / (float)1000;
            float accelerometerYInstantValue = (float)this.accelerometerYInstantTrackBar.Value / (float)1000;
            float accelerometerZInstantValue = (float)this.accelerometerZInstantTrackBar.Value / (float)1000;

            this.accelerometerXAxisValueLabel.Text = accelerometerXGravityValue.ToString();
            this.accelerometerYAxisValueLabel.Text = accelerometerYGravityValue.ToString();
            this.accelerometerZAxisValueLabel.Text = accelerometerZGravityValue.ToString();

            this.accelerometerXInstantValueLabel.Text = accelerometerXInstantValue.ToString();
            this.accelerometerYInstantValueLabel.Text = accelerometerYInstantValue.ToString();
            this.accelerometerZInstantValueLabel.Text = accelerometerZInstantValue.ToString();
        }

        private void acceleromterXTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            this.isMouseDown = true;
        }

        private void acceleromterXTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            this.isMouseDown = false;

            if (this.valueChanged == true)
            {
                this.valueChanged = false;
                this.sendAccelerometerEvent();
            }
        }

        private void acceleromterXTrackBar_Scroll(object sender, EventArgs e)
        {
            this.valueChanged = true;

            scrollCount = scrollCount + 1;
            if (scrollCount >= 5)
            {
                scrollCount = 0;
                this.sendAccelerometerEvent();

            }
        }



        //-------------------------------------- GPS--------------------------------------------------------
        private void addMarkerBt_Click(object sender, EventArgs e)
        {
            
        }

      

        private void xAxisTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            this.isMouseDown = true;
        }

        private void xAxisTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            this.isMouseDown = false;

            if (this.valueChanged == true)
            {
                this.valueChanged = false;
                this.sendGyroscopeEvent();
            }

        }

        private void xAxisTrackBar_Scroll(object sender, EventArgs e)
        {
            this.valueChanged = true;

            scrollCount = scrollCount + 1;
            if (scrollCount >= 5)
            {
                scrollCount = 0;
                this.sendGyroscopeEvent();

            }
        }

        private void shakeBt_Click(object sender, EventArgs e)
        {
            accelerometerIsShake = true;
            this.sendAccelerometerEvent();
            accelerometerIsShake = false;
        }

        private void gyroscopeXAxisTrackBar_ValueChanged(object sender, EventArgs e)
        {

            this.gyroscopeXAxisValueLabel.Text =  ((double)this.gyroscopeXAxisTrackBar.Value /1000.0f).ToString();
            this.gyroscopeYAxisValueLabel.Text = ((double)this.gyroscopeYAxisTrackBar.Value / 1000.0f).ToString();
            this.gyroscopeZAxisValueLabel.Text = ((double)this.gyroscopeZAxisTrackBar.Value / 1000.0f).ToString();


        }

        private void playRouteBt_Click(object sender, EventArgs e)
        {
            stopRouteBt_Click(null,null);
            if (this.googleMapControl1.currentRoute != null)
            {
                int timeTransit = -1;
                if (int.TryParse(this.transitTimeTxtBx.Text, out timeTransit) && this.googleMapControl1.currentRoute.Points.Count > 1)
                {

                    this.googleMapControl1.currentMarkerRoute.IsVisible = true;
                    this.googleMapControl1.currentRoute.IsVisible = true;
                    this.googleMapControl1.currentMarkerFrom.IsVisible = true;
                    this.googleMapControl1.currentMarkerTo.IsVisible = true;
                    this.googleMapControl1.currentMarkerRemote.IsVisible = false;

                    int interval = (int)(timeTransit / this.googleMapControl1.currentRoute.Points.Count);
                    if (interval < 1) interval = 1;
                    this.gpsTimer.Interval = interval;
                    this.gpsTimer.Start();
                }
                else
                {
                    if(this.googleMapControl1.currentRoute.Points.Count < 1)
                        MessageBox.Show("Please create a route path before playing it!", "Route not set", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Please fill the route transition time in milliseconds before playing the route path!", "Transition time not set", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                
                
            }
        }

        private void stopRouteBt_Click(object sender, EventArgs e)
        {
            if (this.googleMapControl1.currentRoute != null)
            {
                this.gpsTimer.Stop();

            }
        }

        private void resetRouteBt_Click(object sender, EventArgs e)
        {
            currentGPSMarkerIndex = 0;
            playRouteBt_Click(null, null);
        }

        private void gpsTimer_Tick(object sender, EventArgs e)
        {
            if (this.googleMapControl1.currentRoute != null)
            {
                currentGPSMarkerIndex = currentGPSMarkerIndex + 1;

                if (currentGPSMarkerIndex < this.googleMapControl1.currentRoute.Points.Count)
                {
                    this.googleMapControl1.currentMarkerRoute.Position = this.googleMapControl1.currentRoute.Points[currentGPSMarkerIndex];
                    this.googleMapControl1.currentMarkerRoute.IsVisible = true;

                    double altitude = 1;
                    double longitude = this.googleMapControl1.currentMarkerRoute.Position.Lng;
                    double latitude =this.googleMapControl1.currentMarkerRoute.Position.Lat;
                    double accuracy = 1;
                    double speed = -1;
                    this.latitudeTxtBx.Text = latitude.ToString();
                    this.longitudeTxtBx.Text = longitude.ToString();
                    this.accuracyTxtBx.Text = accuracy.ToString();
                    this.altitudeTxtBx.Text = altitude.ToString();

                    if( this.googleMapControl1.currentRoute.Points.Count>1)
                    {
                        if(currentGPSMarkerIndex>=0 && currentGPSMarkerIndex<this.googleMapControl1.currentRoute.Points.Count-1)
                        {
                            PointLatLng p1 = this.googleMapControl1.currentRoute.Points[currentGPSMarkerIndex];
                            PointLatLng p2 = this.googleMapControl1.currentRoute.Points[currentGPSMarkerIndex +1];
                            double distance = this.googleMapControl1.getDistance(p1, p2);
                            speed = ((distance*1000) / ((double)gpsTimer.Interval/1000));
                            this.speedTxtBx.Text = speed.ToString();
                        }
                        
                    }
                    
                    TimeSpan span = DateTime.Now - origine;
                    int nbSecondes = (int)span.TotalSeconds;
                    this.timeTxtBx.Text = nbSecondes.ToString();



                    RemoteEvent eventGPS = new RemoteEvent("location", altitude, longitude, latitude, accuracy, 0, speed, nbSecondes);
                    string eventStr = eventGPS.serialize();
                    this.sendEvent(eventStr);

                    this.googleMapControl1.updateMarkers();
                }
                else
                {
                    stopRouteBt_Click(null, null);
                }

            }
        }

        
    }
}
