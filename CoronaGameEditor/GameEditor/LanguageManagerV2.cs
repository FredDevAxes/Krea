using System;
using System.Linq;
using System.Windows.Forms;
using Krea.CoronaClasses;
using System.Text.RegularExpressions;



namespace Krea
{
    public partial class LanguageManagerV2 : UserControl
    {

        public LangueObject selectedLangue { get; set; }
        private Form1 mainForm;
        private CoronaGameProject project;

        public LanguageManagerV2()
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

        private void AddBt_Click(object sender, EventArgs e)
        {
            if (langueNameCb.SelectedItem != null)
            {
                if (CheckForDuplicateElement(langueNameCb.SelectedItem.ToString()))
                {
                    MessageBox.Show("You can't add the same language file.");
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
            listLangueLb.Items.Clear();
            GSelectedLanguageCb.Items.Clear();
            GSelectedLanguageCb.Text = "";

            for (int i = 0; i < this.project.Langues.Count; i++)
            {
                GSelectedLanguageCb.Items.Add(this.project.Langues[i]);
                defaultLangueCb.Items.Add(this.project.Langues[i]);
                listLangueLb.Items.Add(this.project.Langues[i]);
            }

            if (GSelectedLanguageCb.Items.Count > 0)
                GSelectedLanguageCb.SelectedIndex = 0;

        }
        private void reloadListTranslation()
        {
            if (selectedLangue == null) return;
            TranslationLb.Items.Clear();
            TranslationLb.Items.AddRange(selectedLangue.TranslationElement.ToArray());

        }
        private void removeBt_Click(object sender, EventArgs e)
        {
            if (listLangueLb.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listLangueLb.SelectedItems.Count; i++)
                {
                    this.project.Langues.Remove((LangueObject)listLangueLb.SelectedItems[i]);
                }

                this.reloadComboBox();
            }
            else
            {
                MessageBox.Show("Please select a language.");

            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (this.defaultLangueCb.SelectedItem != null)
                this.project.DefaultLanguage = this.defaultLangueCb.SelectedItem.ToString();

            this.mainForm.getGameEditorTabControl().TabPages.Remove(this.mainForm.getRosetaPage());
        }

        private void btAddTranslation_Click(object sender, EventArgs e)
        {
            if (selectedLangue == null)
            {
                MessageBox.Show("Please select a Language");
                return;
            }

            if (KeyTb.Text.Equals(""))
            {
                MessageBox.Show("Please Enter a Key");
                return;
            }
            if (TranslationTb.Text.Equals(""))
            {
                MessageBox.Show("Please Enter a Translation for the Key : " + KeyTb.Text);
                return;
            }
            if (selectedLangue.CheckForDuplicateElement(KeyTb.Text))
            {
                MessageBox.Show("The Key " + KeyTb.Text + " is already stored.");
                return;
            }

            LangueElement newLangueElement = new LangueElement(KeyTb.Text, TranslationTb.Text);
            selectedLangue.TranslationElement.Add(newLangueElement);
            this.reloadListTranslation();

        }

        private void btRemoveTranslation_Click(object sender, EventArgs e)
        {
            if (selectedLangue == null)
            {
                MessageBox.Show("Please select a Language");
                return;
            }

            if (TranslationLb.SelectedItem == null)
            {
                MessageBox.Show("Please select a Translation row");
                return;
            }

            selectedLangue.TranslationElement.Remove((LangueElement)TranslationLb.SelectedItem);
            TranslationLb.Items.Remove(TranslationLb.SelectedItem);

            this.reloadListTranslation();
        }

        private void listLangueLb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listLangueLb.SelectedItem != null)
            {
                this.selectedLangue = (LangueObject)listLangueLb.SelectedItem;
                this.reloadListTranslation();
            }
        }
        private void GTranslate(LangueObject From, String To)
        {
            if (From.Langue == To.ToString()) return;
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
                MessageBox.Show("It's seems like google translate can't perform the request.");
           
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
        private void GTranslateBt_Click(object sender, EventArgs e)
        {
            if (GSelectedLanguageCb.SelectedItem == null)
            {
                MessageBox.Show("Please Select a Language to translate !");
                return;
            }
            if (GTranslateLanguageCb.SelectedItem == null)
            {
                MessageBox.Show("Please Select a Language to use for the translation !");
                return;
            }
            this.translateBt.Enabled = false;
            LangueObject GSelectedLanguage = (LangueObject)GSelectedLanguageCb.SelectedItem;
            if (!GTranslateLanguageCb.SelectedItem.ToString().Equals("All"))
            {
                this.GTranslate(GSelectedLanguage, GTranslateLanguageCb.SelectedItem.ToString());
            }
            else
            {
                for (int i = 0; i < GTranslateLanguageCb.Items.Count; i++)
                {
                    if (!GTranslateLanguageCb.Items[i].ToString().Equals(GSelectedLanguage.Langue) && !GTranslateLanguageCb.Items[i].ToString().Equals("All"))
                    {
                        this.GTranslate(GSelectedLanguage, GTranslateLanguageCb.Items[i].ToString());
                    }
                }
            }
            this.reloadComboBox();
            translateBt.Enabled = true;
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
        private void GSelectedLanguageCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GSelectedLanguageCb.SelectedItem != null)
            {
                this.selectedLangue = (LangueObject)listLangueLb.SelectedItem;
                this.reloadListTranslation();
            }
        }

        private void TranslationLb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslationLb.SelectedItem != null)
            {
                String CurrentLine = TranslationLb.SelectedItem.ToString();
                String[] SplitValue = Regex.Split(CurrentLine, " => ");
                KeyTb.Text = SplitValue[0];
                TranslationTb.Text = SplitValue[1];
            }
        }

        private void defaultLangueCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.defaultLangueCb.SelectedItem != null)
                this.project.DefaultLanguage = this.defaultLangueCb.SelectedItem.ToString();

        }
    }
}
