using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.IO;
using Microsoft.Win32;

namespace OffLine.Installer
{
    // Taken from:http://msdn2.microsoft.com/en-us/library/
    // system.configuration.configurationmanager.aspx
    // Set 'RunInstaller' attribute to true.

    [RunInstaller(true)]
    public class InstallerClass : System.Configuration.Install.Installer
    {
        public InstallerClass()
            : base()
        {
            // Attach the 'Committed' event.
            this.Committed += new InstallEventHandler(MyInstaller_Committed);
            // Attach the 'Committing' event.
            this.Committing += new InstallEventHandler(MyInstaller_Committing);
        }

        // Event handler for 'Committing' event.
        private void MyInstaller_Committing(object sender, InstallEventArgs e)
        {
            //Console.WriteLine("");
            //Console.WriteLine("Committing Event occurred.");
            //Console.WriteLine("");
        }

        // Event handler for 'Committed' event.
        private void MyInstaller_Committed(object sender, InstallEventArgs e)
        {
            
        }

        // Override the 'Install' method.
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);

            try
            {

                string softw = Getinstalledsoftware();
                if (softw.ToLower().IndexOf("slimdx") < 0)
                {
                    string path = Path.GetDirectoryName(
                      Assembly.GetExecutingAssembly().Location) + "\\SlimDX-Runtime-.NET-2.0.msi";


                    Directory.SetCurrentDirectory(Path.GetDirectoryName
                    (Assembly.GetExecutingAssembly().Location));

                    Process process = new Process();

                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = " /quiet /qn /norestart";


                    process.Start();
                }

                if (softw.ToLower().IndexOf("sdl") < 0)
                {
                    string path = Path.GetDirectoryName(
                      Assembly.GetExecutingAssembly().Location) + "\\sdldotnet-6.1.1beta-sdk-setup.exe";


                    Directory.SetCurrentDirectory(Path.GetDirectoryName
                    (Assembly.GetExecutingAssembly().Location));

                    Process process = new Process();

                    process.StartInfo.FileName = path;
                    //process.StartInfo.Arguments = " /quiet /qn /norestart";


                    process.Start();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Override the 'Commit' method.
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        // Override the 'Rollback' method.
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        /// <summary>
        /// Gets a list of installed software and, if known, the software's install path.
        /// </summary>
        /// <returns></returns>
        private string Getinstalledsoftware()
        {
            //Declare the string to hold the list:
            string Software = null;

            //The registry key:
            string SoftwareKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(SoftwareKey))
            {
                //Let's go through the registry keys and get the info we need:
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            //If the key has value, continue, if not, skip it:
                            if (!(sk.GetValue("DisplayName") == null))
                            {
                                //Is the install location known?
                                if (sk.GetValue("InstallLocation") == null)
                                    Software += sk.GetValue("DisplayName") + " - Install path not known\n"; //Nope, not here.
                                else
                                    Software += sk.GetValue("DisplayName") + " - " + sk.GetValue("InstallLocation") + "\n"; //Yes, here it is...
                            }
                        }
                        catch (Exception ex)
                        {
                            //No, that exception is not getting away... :P
                        }
                    }
                }
            }

            return Software;
        }

    }
}