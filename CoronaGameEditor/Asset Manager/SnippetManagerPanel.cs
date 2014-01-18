using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krea.Corona_Classes;
using Krea.Asset_Manager.Assets_Property_Converters;

namespace Krea.Asset_Manager
{
    public partial class SnippetManagerPanel : UserControl
    {
        public SnippetManagerPanel()
        {
            InitializeComponent();
        }

        private AssetManagerForm mainForm;
        private Snippet snippet;
        private DocumentForm form;

        public void init(AssetManagerForm mainForm, Snippet snippet)
        {
            form = this.snippetEditor1.NewDocument();

            this.mainForm = mainForm;
            this.snippet = snippet;

            this.form.Scintilla.Text = snippet.Function;
            this.DisplayObjectProperties();
        }


        public void DisplayObjectProperties()
        {
            if (this.snippet != null)
            {
                SnippetPropertyConverter spriteProp = new SnippetPropertyConverter(this.snippet, this.mainForm);
                this.mainForm.propertyGrid1.SelectedObject = spriteProp;

            }
        }

        private void validBt_Click(object sender, EventArgs e)
        {
            this.Clean();
        }


        public void Clean()
        {
            if (this.snippet != null)
            {
                string function = this.form.Scintilla.Text;
                this.snippet.Function = function;

                int indexOfFirstAcc = function.IndexOf("(");
                if(indexOfFirstAcc>-1)
                {
                     int indexOfSecondAcc = function.IndexOf(")");
                    if(indexOfSecondAcc>-1)
                    {
                        int indexOfFunction = function.IndexOf("function");
                        if(indexOfFunction>-1)
                        {
                            this.snippet.Syntax = function.Substring(indexOfFunction + "function".Length, indexOfSecondAcc + 1 - indexOfFunction- "function".Length).Replace(" ", "");
                        }
                        
                    }
                }
               
            }

            this.snippetEditor1.Dispose();
            this.mainForm.RemoveControlFromObjectsPanel(this);
            this.mainForm.RefreshAssetListView();
            this.Dispose();
        }

    }
}
