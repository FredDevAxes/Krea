using System;
using System.Linq;
using System.Windows.Forms;
using Krea.CoronaClasses;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Collections.Generic;



namespace Krea.GameEditor
{
    public partial class LanguageManager : UserControl
    {
        public LangueObject selectedLangue { get; set; }
        private Form1 mainForm;
        private CoronaGameProject project;

        private string selectedDestinationTranslationLanguage;
        public LanguageManager()
        {
            InitializeComponent();
        }

        public void Init(Form1 mainForm, CoronaGameProject project)
        {
            this.mainForm = mainForm;

            this.project = project;

            this.reloadComboBox();
            this.reloadListTranslation();
        }

        public Boolean CheckForDuplicateElement(String _Key)
        {
            for (int i = 0; i < this.project.Langues.Count; i++)
            {
                if (this.project.Langues[i].Langue == _Key) return true;
            }
            return false;
        }
        private void reloadComboBox()
        {
            defaultLangueCb.Items.Clear();
            this.languagesListBx.Items.Clear();
            this.translateFromLanguageCmbBx.Items.Clear();
            this.translateFromLanguageCmbBx.Text = "";

            for (int i = 0; i < this.project.Langues.Count; i++)
            {
                this.translateFromLanguageCmbBx.Items.Add(this.project.Langues[i]);
                defaultLangueCb.Items.Add(this.project.Langues[i]);
                ListViewItem newLanguageItem = new ListViewItem();
                newLanguageItem.Text = this.project.Langues[i].Langue;
                newLanguageItem.Tag = this.project.Langues[i];
                languagesListBx.Items.Add(newLanguageItem);
            }

            if (translateFromLanguageCmbBx.Items.Count > 0)
                translateFromLanguageCmbBx.SelectedIndex = 0;

        }
        private void reloadListTranslation()
        {

            if (selectedLangue == null) return;


            this.tranlationGridView.Rows.Clear();


            for (int i = 0; i < selectedLangue.TranslationElement.Count; i++)
            {
                int index = this.tranlationGridView.Rows.Add();
                this.tranlationGridView.Rows[index].Tag = selectedLangue.TranslationElement[i];
                this.tranlationGridView.Rows[index].Cells[0].Value = selectedLangue.TranslationElement[i].Key.ToString().Replace("\n","!NL!");
                this.tranlationGridView.Rows[index].Cells[1].Value = selectedLangue.TranslationElement[i].Translation.ToString().Replace("\n", "!NL!");
              
             
            }
           // TranslationLb.Items.AddRange(selectedLangue.TranslationElement.ToArray());

        }
        private void tranlationGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < this.tranlationGridView.Rows.Count)
            {
                DataGridViewRow rowSelected = this.tranlationGridView.Rows[e.RowIndex];
                LangueElement elem = rowSelected.Tag as LangueElement;
                elem.Key = rowSelected.Cells[0].Value.ToString().Replace("!NL!", "\n");
                elem.Translation = rowSelected.Cells[1].Value.ToString().Replace("!NL!", "\n");
            }
         

        }

        private void addFieldBt_Click(object sender, EventArgs e)
        {
            if (selectedLangue == null)
            {
                MessageBox.Show("Please select a Language");
                return;
            }

            if (this.newKeyTxtBx.Text.Equals(""))
            {
                MessageBox.Show("Please enter a translation key");
                return;
            }
            if (this.newValueTxtBx.Text.Equals(""))
            {
                MessageBox.Show("Please enter a translation for the Key : " + newKeyTxtBx.Text);
                return;
            }
            if (selectedLangue.CheckForDuplicateElement(newKeyTxtBx.Text))
            {
                MessageBox.Show("The Key " + newKeyTxtBx.Text + " is already stored.");
                return;
            }

            LangueElement newLangueElement = new LangueElement(newKeyTxtBx.Text, newValueTxtBx.Text);
            selectedLangue.TranslationElement.Add(newLangueElement);
            this.reloadListTranslation();
        }

        private void removeFieldBt_Click(object sender, EventArgs e)
        {
            if (selectedLangue == null)
            {
                MessageBox.Show("Please select a language");
                return;
            }

            if (this.tranlationGridView.SelectedRows.Count<1)
            {
                MessageBox.Show("Please select a translation row");
                return;
            }

            selectedLangue.TranslationElement.Remove((LangueElement)this.tranlationGridView.SelectedRows[0].Tag);
            this.tranlationGridView.Rows.Remove(this.tranlationGridView.SelectedRows[0]);

           // this.reloadListTranslation();
        }

        private void AddBt_Click(object sender, EventArgs e)
        {
            if (langueNameCb.SelectedItem != null)
            {
                if (CheckForDuplicateElement(langueNameCb.SelectedItem.ToString()))
                {
                    MessageBox.Show("The language already exists");
                    return;
                }
                LangueObject newLangueObject = new LangueObject(langueNameCb.SelectedItem.ToString());
                this.project.Langues.Add(newLangueObject);
                this.reloadComboBox();
            }
            else
            {
                MessageBox.Show("Please select a language !");
            }
        }

        private void removeBt_Click(object sender, EventArgs e)
        {
            if (languagesListBx.SelectedItems.Count > 0)
            {
                for (int i = 0; i < languagesListBx.SelectedItems.Count; i++)
                {
                    this.project.Langues.Remove((LangueObject)languagesListBx.SelectedItems[i].Tag);
                }

                this.reloadComboBox();
            }
            else
            {
                MessageBox.Show("Please select a language.");

            }
        }

        private void defaultLangueCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.defaultLangueCb.SelectedItem != null)
                this.project.DefaultLanguage = this.defaultLangueCb.SelectedItem.ToString();
        }

        private void languagesListBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (languagesListBx.SelectedItems.Count >0)
            {
                this.selectedLangue = (LangueObject)languagesListBx.SelectedItems[0].Tag;
                this.reloadListTranslation();
            }
        }

        private void translateFromLanguageCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (translateFromLanguageCmbBx.SelectedItem != null)
            {
                if (this.languagesListBx.SelectedItems.Count > 0)
                {
                    this.selectedLangue = (LangueObject)this.languagesListBx.SelectedItems[0].Tag;
                    this.reloadListTranslation();
                }
              
            }
        }

        private string GetGoogleTranslateLanguageName(string language)
        {

            if (language.Equals("Arabic")) return "ar";
            if (language.Equals("Bulgarian")) return "bg";
            if (language.Equals("Catalan")) return "ca";
            if (language.Equals("Chinese (Simplified)")) return "zh-CHS";
            if (language.Equals("Chinese (Traditional)")) return "zh-CHT";
            if (language.Equals("Czech")) return "cs";
            if (language.Equals("Danish")) return "da";
            if (language.Equals("Dutch")) return "nl";
            if (language.Equals("English")) return "en";
            if (language.Equals("Estonian")) return "et";
            if (language.Equals("Filipino")) return "tl";
            if (language.Equals("Finnish")) return "fi";
            if (language.Equals("French")) return "fr";
            if (language.Equals("German")) return "de";
            if (language.Equals("Greek")) return "el";
            if (language.Equals("Haitian Creole")) return "ht";
            if (language.Equals("Hebrew")) return "he";
            if (language.Equals("Hindi")) return "hi";
            if (language.Equals("Hungarian")) return "hu";
            if (language.Equals("Indonesian")) return "id";
            if (language.Equals("Italian")) return "it";
            if (language.Equals("Japanese")) return "ja";
            if (language.Equals("Korean")) return "ko";
            if (language.Equals("Latvian")) return "lv";
            if (language.Equals("Lithuanian")) return "lt";
            if (language.Equals("Norwegian")) return "no";
            if (language.Equals("Polish")) return "pl";
            if (language.Equals("Portuguese")) return "pt";
            if (language.Equals("Romanian")) return "ro";
            if (language.Equals("Russian")) return "ru";
            if (language.Equals("Slovak")) return "sk";
            if (language.Equals("Slovenian")) return "sl";
            if (language.Equals("Spanish")) return "es";
            if (language.Equals("Swedish")) return "sv";
            if (language.Equals("Thai")) return "th";
            if (language.Equals("Turkish")) return "tr";
            if (language.Equals("Ukrainian")) return "uk";
            if (language.Equals("Vietnamese")) return "vi";

            return "";
        }

        private void GTranslate(LangueObject From, String To)
        {
            if (From.Langue == To.ToString()) return;
            if (toolStrip1.Equals("")) return;
            LangueObject newLangue = new LangueObject(To.ToString());
            String CFrom = this.GetGoogleTranslateLanguageName(From.Langue);
            String CTo = GetGoogleTranslateLanguageName(To.ToString());
            try
            {
                for (int i = 0; i < From.TranslationElement.Count; i++)
                {
                    string key = From.TranslationElement[i].Translation;
                    MicrosoftTranslator.LanguageServiceClient TranslateC = new MicrosoftTranslator.LanguageServiceClient();
                    string[] l = TranslateC.GetLanguagesForTranslate("A1720512EE086AC9060D14F925EE3D0543CEDF90");
                    var availableLanguages = String.Join(",", l.Select(x => x.ToString()).ToArray());
                    MicrosoftTranslator.TranslateOptions options = new MicrosoftTranslator.TranslateOptions(); // Use the default options
                    string translation = TranslateC.GetTranslations("A1720512EE086AC9060D14F925EE3D0543CEDF90", key.ToString(), CFrom, CTo, 1, options).Translations[0].TranslatedText;
                    LangueElement newLangueElement = new LangueElement(key, translation);
                    newLangue.TranslationElement.Add(newLangueElement);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("It seems that Microsoft Bing translator can't perform the request.");
             
            }

            //Add new Language
            if (newLangue.TranslationElement.Count > 0)
            {
                // Check if the new language exist
                for (int i = 0; i < this.project.Langues.Count; i++)
                {
                    if (this.project.Langues[i].Langue.Equals(To.ToString()))
                    {
                        // Removes the previous Langues files
                        this.project.Langues.Remove(this.project.Langues[i]);
                    }
                }
                this.project.Langues.Add(newLangue);

            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.translateFromLanguageCmbBx.SelectedItem == null)
            {
                MessageBox.Show("Please select a language to translate!");
                return;
            }
            if (this.translateToLanguageCmbBx.SelectedItem == null)
            {
                MessageBox.Show("Please select the destination language for the translation!");
                return;
            }
            this.translateNowBt.Enabled = false;

            this.translationBackWorker.RunWorkerAsync();

            
           
        }

        private void translationBackWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            if (this.selectedLangue != null)
            {
                if (!selectedDestinationTranslationLanguage.Equals("All"))
                {
                    this.translationBackWorker.ReportProgress(10);
                    this.GTranslate(this.selectedLangue, selectedDestinationTranslationLanguage);
                }
                //else
                //{
                //    this.translationBackWorker.ReportProgress(0);
                //    for (int i = 0; i < translateToLanguageCmbBx.Items.Count; i++)
                //    {
                //        this.translationBackWorker.ReportProgress(100 * (i / translateToLanguageCmbBx.Items.Count));
                //        if (!translateToLanguageCmbBx.Items[i].ToString().Equals(this.selectedLangue.Langue) && !translateToLanguageCmbBx.Items[i].ToString().Equals("All"))
                //        {
                //            this.GTranslate(this.selectedLangue, selectedDestinationTranslationLanguage);
                //        }
                //    }
                //}
            }
           
        }

        private void translationBackWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.translationStatusProgressBar.Value = e.ProgressPercentage;
        }

        private void translationBackWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.translationStatusProgressBar.Value = 100;
            translateNowBt.Enabled = true;

            this.reloadComboBox();
        }

        private void translateToLanguageCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.translateToLanguageCmbBx.SelectedItem != null)
            {
                this.selectedDestinationTranslationLanguage = this.translateToLanguageCmbBx.SelectedItem.ToString();
            }
            else
            {
                this.selectedDestinationTranslationLanguage = "";
            }
        }

        private void LoadtBt_Click(object sender, EventArgs e)
        {

            if (languagesListBx.SelectedItems.Count > 0)
            {
                String langue = languagesListBx.SelectedItems[0].Text;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "CSV file format (UTF-8) |*.csv";
                openFileDialog1.Title = "Open as CSV File (semicolon separator)";
                openFileDialog1.ShowDialog();

               

                try
                {
                    if (openFileDialog1.FileName != "" && File.Exists(openFileDialog1.FileName))
                    {
                         StringBuilder buffer = new StringBuilder();
                        Stream stream = openFileDialog1.OpenFile();
                        TextReader tr = new StreamReader(stream, Encoding.UTF8);
                        buffer.Append(tr.ReadToEnd());
                        tr.Close();
                        stream.Close();

                        String[] splittedbuff = buffer.ToString().Split('\n');

                        List<String> dicoLangue = new List<string>();
                        for (int i = 0; i < splittedbuff.Length; i++)
                        {
                            if (splittedbuff[i].StartsWith("#"))
                            {
                                //Is a comment, Do Nothing
                                continue;
                            }
                            String[] elements = splittedbuff[i].Split(';');
                            for (int j = 0; j < elements.Length; j++)
                            {
                                dicoLangue.Add(elements[j].ToString().Replace("\n", "!NL!"));
                            }

                        }
                        //Reload DicoLangue
                        this.selectedLangue = (LangueObject)this.languagesListBx.SelectedItems[0].Tag;
                        this.selectedLangue.TranslationElement.Clear();
                        for (int i = 0; i < dicoLangue.Count - 1; i++)
                        {
                            LangueElement newLangueElement = new LangueElement(dicoLangue[i], dicoLangue[i + 1]);
                            this.selectedLangue.TranslationElement.Add(newLangueElement);
                            i++;
                        }
                        this.reloadListTranslation();

                        MessageBox.Show("Import sucess!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("You need to select a CSV file to import it.", "Warning !", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }



         
        
        }

        private void SavetBt_Click(object sender, EventArgs e)
        {
            try
            {
                if (languagesListBx.SelectedItems.Count > 0)
                {
                    // Displays a SaveFileDialog so the user can save the Image
                    // assigned to Button2.
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "CSV file format (UTF-8) |*.csv";
                    saveFileDialog1.Title = "Save as CSV File";
                    saveFileDialog1.ShowDialog();

                    // If the file name is not an empty string open it for saving.
                    if (saveFileDialog1.FileName != "")
                    {

                        List<String> CSVContent = new List<String>();

                        CSVContent.Add("#Project_Name : " + this.project.ProjectName);
                        CSVContent.Add("\n");
                        CSVContent.Add("#Date : " + DateTime.Now.ToString());
                        CSVContent.Add("\n");
                        CSVContent.Add("#Langue : " + this.selectedLangue);
                        CSVContent.Add("\n");
                        // storing header part in Excel
                        //for (int i = 1; i < tranlationGridView.Columns.Count + 1; i++)
                        //{
                        //    CSVContent.Add(tranlationGridView.Columns[i - 1].HeaderText);
                        //}

                        String buff = "";
                        // storing Each row and column value to excel sheet
                        for (int i = 0; i < tranlationGridView.Rows.Count; i++)
                        {
                            for (int j = 0; j < tranlationGridView.Columns.Count; j++)
                            {
                                buff = tranlationGridView.Rows[i].Cells[j].Value.ToString().Replace("\n", "!NL!");
                                CSVContent.Add(buff);
                                //CSVContent.Add("\"" + buff + "\"");
                            }
                            CSVContent.Add("\n");
                        }
                        CSVContent.Add("\n");
                        // save the application
                        StringBuilder s = new StringBuilder();
                        for (int i = 0; i < CSVContent.Count; i++)
                        {
                            if (i < CSVContent.Count - 1)
                            {
                                if (CSVContent[i + 1] != "\n" && CSVContent[i] != "\n")
                                {
                                    s.Append(CSVContent[i] + ";");
                                }
                                else
                                {
                                    s.Append(CSVContent[i]);
                                }
                            }
                            else
                            {
                                s.Append(CSVContent[i]);
                            }
                        }

                        Stream stream = saveFileDialog1.OpenFile();

                        // create a writer and open the file
                        TextWriter tw = new StreamWriter(stream, Encoding.UTF8);

                        // write a line of text to the file
                        tw.WriteLine(s.ToString());

                        // close the stream
                        tw.Close();
                        stream.Close();
                        s.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("You need to select a language first to export it.", "Warning !", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error during language export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
