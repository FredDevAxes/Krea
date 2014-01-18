using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using Microsoft.Win32;
using System.IO;
namespace Krea.GameEditor
{
    public partial class AppSettingsManager : UserControl
    {
        Form1 mainForm;
        public AppSettingsManager()
        { 
            InitializeComponent();
        }

        public void init(Form1 mainForm)
        {
            this.mainForm = mainForm;

            this.tabControl1.TabPages[0].Hide();

            initResolutionListBox();

            this.coronaPathTxtBx.Text = Settings1.Default.CoronaSDKFolder;
        }



        private void initResolutionListBox()
        {
            this.customResolutionListBx.Items.Clear();

            for (int i = 0; i < this.mainForm.resolutionsCmbBx.Items.Count; i++)
            {
                this.customResolutionListBx.Items.Add(this.mainForm.resolutionsCmbBx.Items[i]);
            }
        }

        private void addResolutionBt_Click(object sender, EventArgs e)
        {
            string deviceName = this.deviceNameTxtBx.Text;

            TargetResolution res = new TargetResolution(deviceName, new Size(Convert.ToInt32(this.widthNumUpDw.Value), Convert.ToInt32(this.heightNumUpDw.Value)));
            this.mainForm.resolutionsCmbBx.Items.Add(res);
            this.mainForm.saveResolutions();
            this.mainForm.initResolutions();
            initResolutionListBox();

        }

        private void removeResolutionBt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.customResolutionListBx.SelectedItems.Count; i++)
            {
                TargetResolution res = (TargetResolution)this.customResolutionListBx.SelectedItems[i];
                if (res != null)
                {
                    if (this.mainForm.resolutionsCmbBx.Items.Contains(res))
                        this.mainForm.resolutionsCmbBx.Items.Remove(res);

                }
            }

            initResolutionListBox();
        }

        private void autoFindCoronaSDKBt_Click(object sender, EventArgs e)
        {
            string coronaPath = this.getCoronaSDKPath();

            if (coronaPath.Equals(""))
            {
                MessageBox.Show("Corona Simulator has not been found! Please check Corona SDK is installed on this computer. If it is already installed, please use the \"Browse\" button to locate it manually!",
                    "Corona Simulator not found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Settings1.Default.CoronaSDKFolder = "";
               
            }
            else
            {
                if (File.Exists(coronaPath))
                {
                    Settings1.Default.CoronaSDKFolder = coronaPath;
                }
                else
                {
                    MessageBox.Show("Corona Simulator has not been found! Please check Corona SDK is installed on this computer. If it is already installed, please use the \"Browse\" button to locate it manually!",
                   "Corona Simulator not found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Settings1.Default.CoronaSDKFolder = "";
                }
            }

            this.coronaPathTxtBx.Text = Settings1.Default.CoronaSDKFolder;

            Settings1.Default.Save();
        }

        private void browseCoronaSDKFolder_Click(object sender, EventArgs e)
        {
            Settings1.Default.CoronaSDKFolder = "";
            FolderBrowserDialog folderDial = new FolderBrowserDialog();
            if (folderDial.ShowDialog() == DialogResult.OK)
            {
                string path = folderDial.SelectedPath;

                string[] filenames = Directory.GetFiles(path);
                foreach (string filename in filenames)
                {
                    if (filename.Contains("Corona Simulator.exe"))
                    {
                        Settings1.Default.CoronaSDKFolder = path+"\\Corona Simulator.exe";
                        Settings1.Default.Save();

                        break;
                    }

                }

                if (Settings1.Default.CoronaSDKFolder.Equals(""))
                {
                    MessageBox.Show("The selected path does not contain the Corona Simulator application!\nPlease select another folder!",
                        "Folder does not contain Corona SDK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            folderDial.Dispose();
            folderDial = null;

            this.coronaPathTxtBx.Text = Settings1.Default.CoronaSDKFolder;
        }

        public string getCoronaSDKPath()
        {

            string pathCoronaSDK_64 = @"C:\Program Files (x86)\Corona Labs\Corona SDK\Corona Simulator.exe";
            string pathCoronaSDK_32 = @"C:\Program Files\Corona Labs\Corona SDK\Corona Simulator.exe";
            string pathFoundInRegistry = "";


            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"Local Settings\Software\Microsoft\Windows\Shell\MuiCache");

            foreach (String keyName in key.GetValueNames())
            {
                if (keyName.Contains("Corona Simulator"))
                {
                    pathFoundInRegistry = keyName;
                    break;
                }
            }

            if (!pathFoundInRegistry.Equals("") && File.Exists(pathFoundInRegistry))
            {
                return pathFoundInRegistry;
            }
            else if (File.Exists(pathCoronaSDK_64))
            {
                return pathCoronaSDK_64;
            }
            else if (File.Exists(pathCoronaSDK_32))
            {
                return pathCoronaSDK_32;
            }
            else
            {
                //Try With OLD version of CORONA
                string pathCoronaSDK_64_OLD = @"C:\Program Files (x86)\Ansca\Corona SDK\Corona Simulator.exe";
                string pathCoronaSDK_32_OLD = @"C:\Program Files\Ansca\Corona SDK\Corona Simulator.exe";

                string pathFoundInRegistryOLD = "";

                RegistryKey key2 = Registry.ClassesRoot.OpenSubKey(@"Local Settings\Software\Microsoft\Windows\Shell\MuiCache");
                foreach (String keyName in key2.GetSubKeyNames())
                {
                    if (keyName.Contains("Corona Simulator"))
                    {
                        pathFoundInRegistryOLD = keyName;
                        break;
                    }
                }


                if (!pathFoundInRegistryOLD.Equals("") && File.Exists(pathFoundInRegistryOLD))
                {
                    return pathFoundInRegistryOLD;
                }
                else if (File.Exists(pathCoronaSDK_64_OLD))
                {
                    return pathCoronaSDK_64_OLD;
                }
                else if (File.Exists(pathCoronaSDK_32_OLD))
                {
                    return pathCoronaSDK_32_OLD;
                }
            }

            return "";
        }


      
    }
}
