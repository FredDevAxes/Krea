using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Krea.RemoteDebugger
{
    class ComInterProcess
    {
        private byte[] buffer;
        private System.Net.IPEndPoint targetAdress;
        Thread threadListenAndReceive;
        Thread threadSend;
        Socket localSocketListening;
        Socket localSocketSending;
        public ObjectsToSerialize obj;
        public bool receivedDatas;
        
        //constructeur
        public ComInterProcess(System.Net.IPEndPoint adressToConnect)
        {

            //Recuperer l'adresse distante
            this.targetAdress = adressToConnect;
            this.receivedDatas = false;
                    
        }

        public void startListenAndReceive()
        {
            //Creer un thread de traitement parallèle
            this.threadListenAndReceive = new Thread(new ThreadStart(actionListenAndReceive));

            //Lancer le thread
            this.threadListenAndReceive.Start();
                                  

        }

        public void stopListenAndReceive()
        {
            this.threadListenAndReceive.Abort();
        }

        public bool isConnected()
        {
            if (localSocketListening.Connected == true)
                return true;
            else 
                return false;
        }

        private void actionListenAndReceive()
        {

            try
            {
                while (true)
                {
                    //Creation de la socket du serveur
                    localSocketListening = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    //Lier la socket au point de communication voulue
                    localSocketListening.Bind(targetAdress);


                    //Mettre en ecoute le serveur
                    localSocketListening.Listen(1);


                    //Mettre le serveur en attente de connexion
                    Socket clientSocket = localSocketListening.Accept();

                    //if (clientSocket.Connected == true)
                      //MessageBox.Show("Nouvelle connexion reussite avec " + clientSocket.AddressFamily.ToString());

                    int index = 0;
                    int bytes = 0;

                    //Verifier si le dossier temp existe
                    DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + @"\Temp");
                    if (!dir.Exists)
                        dir.Create();

                    //Verifier si le fichier TXT temp existe
                    string fileTempPath = Application.StartupPath + @"\Temp\datasTemp.xml";
                  //  File.Create(fileTempPath);
                    
                    FileStream fs = new FileStream(fileTempPath,FileMode.Create);
                    StreamWriter writer = new StreamWriter(fs);
                    
                    byte[] buffer;

                    buffer = new byte[5000];
                    int tailleALire;
               
                    clientSocket.Receive(buffer);
                    tailleALire = Convert.ToInt32(ASCIIEncoding.ASCII.GetString(buffer));
                    buffer = new byte[tailleALire*8];
                    clientSocket.Receive(buffer);

                    writer.Write((ASCIIEncoding.ASCII.GetString(buffer)).ToCharArray());
                    
                    /*do
                    {
                        try
                        {
                            bytes = 0;

                            //Recevoir une reponse ...
                            buffer = new byte[512 * 1024];

                            bytes = clientSocket.Receive(buffer, buffer.Length, 0);
                            clientSocket.re
                            //Ecrire les bytes dans un fichier texte temp      
                            //writer.Write(buffer, index, bytes);
                            writer.
                            index += bytes;
                            if (bytes == 0)
                                break;
                        }
                        catch { }
                        
                   
                       
                    } while (bytes > 0);*/

                    writer.Close();
                    fs.Close();
                    writer = null;
                    fs = null;

                    //Recuperer les objets sérialises du codage XML 
                    this.obj = new ObjectsToSerialize();
                    this.obj = obj.Deserialise(fileTempPath);


                    //Marquer la presence de données
                    this.receivedDatas = true;

                    //Fermer la socket d'ecoute et de reception
                    this.localSocketListening.Close();

                    //Supprimer le fichier temp
                    File.Delete(Application.StartupPath + @"/Temp/datasTemp.xml");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR ListenAndReceive : " + ex.Message);
                localSocketListening.Close();
                threadListenAndReceive.Abort();
                this.receivedDatas = false;
            }
        }


        
        public void SendDatas(ObjectsToSerialize datas)
        {
            this.receivedDatas = false;
            this.obj = datas;


            //Creer un thread de traitement parallèle
            this.threadSend = new Thread(new ThreadStart(actionSend));

            if (localSocketListening != null)
            {
                //Fermer la socket d'ecoute 
                localSocketListening.Close();
            }

            if (this.threadListenAndReceive != null)
            {
                //Arreter le thread d'ecoute en attente
                this.threadListenAndReceive.Abort();
            }

            //Lancer le thread
            this.threadSend.Start();
        }

        private void actionSend()
        {
            try
            {
                //Creation de la socket locale
                localSocketSending = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           
                buffer = new byte[1024*512];

                buffer = ASCIIEncoding.ASCII.GetBytes(obj.Serialise());

                localSocketSending.Connect(targetAdress);

                if (localSocketSending.Connected)
                {
                    //Envoi du message
                    localSocketSending.Send(buffer);
                }

                threadListenAndReceive.Start();
                localSocketSending.Close();
                threadSend.Abort();
             
                
            }
            catch (Exception ex)
            {

                if (this.threadListenAndReceive != null)
                {
                    //Arreter le thread d'ecoute en attente
                    this.threadListenAndReceive.Abort();
                }
                localSocketSending.Close();
                threadSend.Abort();
                MessageBox.Show("Methode Action send : " + ex.Message);
            }

        }

        public byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        public string ByteArrayToString(Byte[] bArray)
        {
            string str;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            str = enc.GetString(bArray);
            return str;

        }

        // Convert an object to a byte array
        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
        // Convert a byte array to an Object
        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }

    }
}
