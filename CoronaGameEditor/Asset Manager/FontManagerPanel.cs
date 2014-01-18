using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.CoronaClasses;
using System.IO;
using Krea.GameEditor.FontManager;
using System.Drawing.Text;
using System.Windows;

namespace Krea.Asset_Manager
{
    public partial class FontManagerPanel : UserControl
    {

        private FontItem customFont;
        private AssetManagerForm MainForm;
        ///////////////////////////////////
        //Default Constructor
        ///////////////////////////////////
        public FontManagerPanel(AssetManagerForm mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;
        }

        ///////////////////////////////////
        //Custom Constructor
        ///////////////////////////////////

        public FontManagerPanel(FontItem customFont, AssetManagerForm mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;

            this.customFont = customFont;

            refreshPanel();
        }


        public void refreshPanel()
        {
            if (this.customFont != null)
            {
                this.nameTb.Text = this.customFont.NameForAndroid;
                refreshFontPreview();
            }
         
        }
        ///////////////////////////////////
        //Event
        ///////////////////////////////////

        //Close the current editor and audio file
        //
        private void closeBt_Click(object sender, EventArgs e)
        {
            this.Parent.Dispose();
        }

        // Save the instance of Corona AudioObject File
        //
        private void saveBt_Click(object sender, EventArgs e)
        {

            this.Clean();
          
        }

        public void Clean()
        {

            this.MainForm.RemoveControlFromObjectsPanel(this);

            this.MainForm.RefreshAssetListView();
            this.Dispose(true);
        }

        private void importBt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "ttf files (*.ttf)|*.ttf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    string directoryDest = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software\\Asset Manager";
                    if (!Directory.Exists(directoryDest))
                        Directory.CreateDirectory(directoryDest);

                    string fileNameDest = directoryDest + "\\" + this.MainForm.CurrentAssetProject.ProjectName + "\\" + openFileDialog1.SafeFileName;
                    try
                    {
                        File.Copy(openFileDialog1.FileName, fileNameDest, true);
                    }
                    catch(Exception ex)
                    {
                        System.Windows.MessageBox.Show("Error during TTF File copy:\n"+ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    
                    }
                   

                    bool res = this.customFont.InitFont(openFileDialog1.SafeFileName, fileNameDest);
                    if(res == true)
                        refreshPanel();
                }
            }
            
        }

        private void refreshFontPreview()
        {
            this.richTextBox1.Clear();

            if (this.customFont != null)
            {
                if (this.customFont.isInstalled == true)
                {
                    string name = this.customFont.NameForIphone;

                    

                    this.richTextBox1.Font = new Font(name, 16);
                    this.richTextBox1.Text = "AaBbCcDdEeFfGgHhIiJj";
                }
            }
        }

       


    }
}
