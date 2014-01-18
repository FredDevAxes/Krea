using System;
using System.Diagnostics;
using System.IO;
using Krea.CoronaClasses;
using Utility.ModifyRegistry;
using Microsoft.Win32;
using System.Windows.Forms;
namespace Krea
{
    public class Emulator
    {
        private static Emulator instance;
        public Process ProcessCorona { get; set; }
        public CoronaGameProject cProject { get; set; }
        public Boolean isRunning { get; set; }
        public Boolean isDebugMode { get; set; }
        public Boolean isInit { get; set;}
        StreamWriter sw;
        StreamReader sr;
        StreamReader err;
        private Emulator() { }

        public static Emulator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Emulator();
                    instance.isInit = false;
                    instance.isRunning = false;
                }
                return instance;
            }
        }


        public void Init(CoronaGameProject _project, Boolean _isDebugMode)
        {
            this.cProject = _project;
            this.isDebugMode = _isDebugMode;
            this.isRunning = false;
            this.isInit = true;
        }

        public Process getCoronaSimulatorOutputProcess()
        {
             Process[] processlist = Process.GetProcesses();

             foreach (Process theprocess in processlist)
             {
                 if (theprocess.ProcessName.Contains("Corona Simulator"))
                 {
                     return theprocess;
                 }

             }

            return null;
        }

        private static void SortOutputHandler(object sendingProcess,
           DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }

        public Boolean Start(){
            if (this.isInit)
            {
                Dispose();

                string coronaPath = Settings1.Default.CoronaSDKFolder;
                if (coronaPath == null || coronaPath.Equals(""))
                {
                    coronaPath = this.getCoronaSDKPath();
                    Settings1.Default.CoronaSDKFolder = coronaPath;
                    Settings1.Default.Save();
                }

                if (!coronaPath.Equals("") && File.Exists(coronaPath))
                {
                    ProcessCorona = new Process();
                    if (isDebugMode)
                    {
                        ProcessCorona.StartInfo.FileName = coronaPath;
                        ProcessCorona.StartInfo.Arguments = " -debug";
                       
                    }
                        
                    else
                    {
                        ProcessCorona.StartInfo.FileName = coronaPath;

                        // Set UseShellExecute to false for redirection.
                        ProcessCorona.StartInfo.UseShellExecute = false;
                        
                        ProcessCorona.StartInfo.CreateNoWindow = true;
                        ProcessCorona.StartInfo.RedirectStandardError = true;
                        //ProcessCorona.StartInfo.RedirectStandardOutput = true;

                        // Set our event handler to asynchronously read the sort output.
                       // ProcessCorona.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                        ProcessCorona.ErrorDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                        ProcessCorona.StartInfo.Arguments = "\"" + cProject.BuildFolderPath + "\\main.lua\"";
                    }
                       

                    

                    this.isRunning = ProcessCorona.Start();

                   
                    return this.isRunning;
                }
                else
                {

                    DialogResult result = MessageBox.Show("Corona SDK has not been found on your computer!\nIf it is already installed and Krea does not detect it automatically, you can manually set its folder path in the Krea preferences, in the Corona tab.\n If you do not have installed Corona SDK yet, do you want to download it now?",
                        "Corona SDK not found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string target = "https://developer.coronalabs.com/user/register?destination=downloads/coronasdk";

                        try
                        {
                            System.Diagnostics.Process.Start(target);
                        }

                        catch (System.ComponentModel.Win32Exception noBrowser)
                        {
                            if (noBrowser.ErrorCode == -2147467259)
                                MessageBox.Show(noBrowser.Message);

                        }
                        catch (System.Exception other)
                        {
                            MessageBox.Show(other.Message);

                        }
                    }
                }
                
            }
            return false;
        }

        void ProcessCorona_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            MessageBox.Show(sr.ReadToEnd());
        }


        public void checkCoronaDebuggerRunning()
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName.Contains("Corona.Debugger"))
                    theprocess.Kill();
                else if (theprocess.ProcessName.Contains("Corona Simulator"))
                    theprocess.Kill();
            }
        }

        public Boolean Dispose() {
            if (this.isInit)
            {
                if (this.isRunning)
                {
                    try
                    {
                        checkCoronaDebuggerRunning();
                        if (!this.ProcessCorona.HasExited)
                        {
                            try
                            {
                                this.ProcessCorona.Kill();
                                this.ProcessCorona.WaitForExit();
                                return true;
                            }
                            catch
                            {
                                MessageBox.Show("Error Emulator can't kill application.");
                                return false;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public Boolean isCoronaSDKInstalled()
        {
            if (this.getCoronaSDKPath().Equals(""))
            {
                return false;
            }
            return true;
        }


        public string getCoronaSDKPath()
        {
           
            string pathCoronaSDK_64 = @"C:\Program Files (x86)\Corona Labs\Corona SDK\Corona Simulator.exe";
            string pathCoronaSDK_32 = @"C:\Program Files\Corona Labs\Corona SDK\Corona Simulator.exe";
            string pathFoundInRegistry = "";

            
            RegistryKey key = Registry.ClassesRoot.OpenSubKey( @"Local Settings\Software\Microsoft\Windows\Shell\MuiCache");
            
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
