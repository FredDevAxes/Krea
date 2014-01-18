using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Krea.GameEditor;
using System.Security.Principal;
using System.Diagnostics;
using System.ComponentModel;


namespace Krea
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

   
            Form1 kreaForm = new Form1();

            Application.Run(kreaForm);
           
        }

       
    }
}
