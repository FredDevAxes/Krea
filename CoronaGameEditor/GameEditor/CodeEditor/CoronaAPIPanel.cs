using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;
using System.Net;
using System.IO;
using mshtml;
using System.Diagnostics;
namespace Krea.GameEditor.CodeEditor
{
    public partial class CoronaAPIPanel : UserControl
    {
        private WebClient wc;
        private Form1 mainForm;

        private string mode = "CORONA_API";
        public CoronaAPIPanel()
        {
            InitializeComponent();

        }

        private void refreshAPIBt_Click(object sender, EventArgs e)
        {

            //string content = this.webBrowser1.DocumentText;
            

            //if(!backgroundWorker1.IsBusy)
            //     backgroundWorker1.RunWorkerAsync();
        }

        public void init(Form1 mainForm)
        {
            this.mainForm = mainForm;
            //initApiReference();
            initKreaModulesAPI();
            
        }

        private void initKreaModulesAPI()
        {

            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\LuaDoc\\index.html"))
            {
                this.webBrowser1.Url = new Uri(Path.GetDirectoryName(Application.ExecutablePath) + "\\LuaDoc\\index.html");
            }
            else
            {
                MessageBox.Show("Cannot load the Krea modules API because the file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void initApiReference()
        {

            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);


            if (!Directory.Exists(documentsDirectory + "\\api-reference"))
            {
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                //Afficher l'url du html
                if (File.Exists(documentsDirectory + "\\api-reference\\api-reference.html"))
                {
                    this.webBrowser1.Url = new Uri(documentsDirectory + "\\api-reference\\api-reference.html");


                }
                else
                {
                    Directory.Delete(documentsDirectory + "\\api-reference", true);
                    backgroundWorker1.RunWorkerAsync();
                }
            }

            //try
            //{
            //    string path = Path.GetDirectoryName(
            //        System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Corona API Docs\\api\\index.html";

            //    if (!File.Exists(path))
            //    {

            //        string zipDest = Path.GetDirectoryName(
            //             System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Corona API Docs\\CoronaApiDocs.zip";

            //        if (File.Exists(zipDest))
            //        {
            //            using (ZipFile zip = ZipFile.Read(zipDest))
            //            {
            //                foreach (ZipEntry entry in zip)
            //                {
            //                    entry.Extract(Path.GetDirectoryName(
            //                        System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Corona API Docs\\",
            //                        ExtractExistingFileAction.OverwriteSilently); // overwrite == true  
            //                }
            //            }
            //        }
            //    }


            //    if (File.Exists(path))
            //    {
            //        this.webBrowser1.Url = new Uri(path);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error during loading Corona API Docs:\n" + ex.Message);
            //}
        }

        private bool checkInternetConnection()
        {
            try
            {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient("developer.anscamobile.com", 80);
                clnt.Close();
                return true;

            }
            catch (Exception ex)
            {
                return false; // host not reachable.
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateApiReferenceFile();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);


            if (File.Exists(documentsDirectory + "\\api-reference\\api-reference.html"))
            {
                this.webBrowser1.Url = new Uri(documentsDirectory + "\\api-reference\\api-reference.html");
                wc.Dispose();
            }
        }

        private void UpdateApiReferenceFile()
        {
            wc = new WebClient();
            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
            if (!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);


            string fileNameDest = documentsDirectory + "\\api.zip";


            //Check if the directory of the api exists
            if (checkInternetConnection() == true)
            {
                try
                {
                    wc.DownloadFile(@"http://developer.anscamobile.com/sites/default/files/api-reference.2010.08.24.zip", fileNameDest);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during downloading api reference ! \n " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

             
                    return;
                }
            }
            else
            {
                MessageBox.Show("No internet connection !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            try
            {
                if (File.Exists(fileNameDest))
                {
                    using (ZipFile zip = ZipFile.Read(fileNameDest))
                    {
                        foreach (ZipEntry entry in zip)
                        {
                            entry.Extract(documentsDirectory, ExtractExistingFileAction.OverwriteSilently); // overwrite == true  
                        }
                    }
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during file extraction ! \n " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Process.Start("http://docs.coronalabs.com/api/");
        }

    }
}
