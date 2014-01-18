using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Krea.CoronaClasses;
using Krea.Corona_Classes;
using System.Collections.Generic;

namespace Krea.GameEditor
{
    public partial class ProjectSettings : UserControl
    {
        private Form1 parent;
        private CoronaGameProject NewCoronaGameProject;
        private CoronaGameProject.OrientationScreen orientation = CoronaGameProject.OrientationScreen.Portrait;
        private string mode;
        private Image Icone;

        public ProjectSettings()
        {
            InitializeComponent();
        }

        public void init(Form1 p)
        {
            this.parent = p;
            NewCoronaGameProject = p.CurrentProject;
            if (NewCoronaGameProject == null)
            {
                mode = "NEW";
                this.langueNameCb.SelectedItem = "English";
                NewCoronaGameProject = new CoronaGameProject();

                tbProjectName.Text = "";

                tbProjectPath.Text = "";

                this.lbListPermissionAndroid.Items.Clear();
                this.lbSupportedOrientation.Items.Clear();

                this.updateConfigTreeView();
                this.updateBuildTreeView();
            }
            else
            {
                this.reloadProject(NewCoronaGameProject);
            }


        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.parent.sceneEditorView1.init(this.parent);

            this.lbListPermissionAndroid.Items.Clear();
            this.parent.closeProjectPage();
        }

        private void btCreate_Click(object sender, EventArgs e)
        {

            if (this.landscapeRBt.Checked == true)
                orientation = CoronaGameProject.OrientationScreen.Landscape;
            else
                orientation = CoronaGameProject.OrientationScreen.Portrait;

            if (Icone == null) Icone = Properties.Resources.iconkrea;

            if (mode.Equals("MODIFY"))
            {
                //Si le nom du repertoire a changer, fermer tous les fichiers de code lua
                if (!(this.NewCoronaGameProject.ProjectPath + "\\" + this.NewCoronaGameProject.ProjectName + ".krp")
                    .Equals(this.NewCoronaGameProject.ProjectPath + "\\" + tbProjectName.Text.Replace(" ", "_") + ".krp"))
                {
                    MessageBox.Show("The Lua files need to be closed before continuing! \n You can reload them from the new project path after changes have been applied!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.parent.cgEeditor1.closeAll(true);
                }
            }

            int fps = 30;
            if(cbFPS.SelectedItem != null)
            {
                fps = Convert.ToInt32(cbFPS.SelectedItem.ToString());
            }

            string xAlign = "center";
            if(cbXAlign.SelectedItem != null)
            {
                xAlign = cbXAlign.SelectedItem.ToString();
            }

            string yAlign = "center";
            if(cbYAlign.SelectedItem != null)
            {
                yAlign = cbYAlign.SelectedItem.ToString();
            }

            string scale = "letterbox";
            if(cbScale.SelectedItem != null)
            {
                scale = cbScale.SelectedItem.ToString();
            }

            if (this.NewCoronaGameProject.Init(tbProjectName.Text, tbProjectPath.Text,
                 orientation, Convert.ToInt32(this.screenWidthNumUpDw.Value), Convert.ToInt32(this.screenHeightNumUpDw.Value),
               scale, xAlign, yAlign, lbDynamicResizing.Items, fps,
                cbAntiAliasing.Checked, tbVersionCodeAndroid.Text, lbSupportedOrientation.Items, lbListPermissionAndroid.Items, tbCustomCoronaBuildName.Text, Icone))
            {

                this.parent.CurrentProject = NewCoronaGameProject;



                if (mode.Equals("NEW"))
                {

                    this.NewCoronaGameProject.createProjectFiles();

                    //Creer le repertoire dans l'asset manager
                    string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Native-Software";
                    if (!Directory.Exists(documentsDirectory))
                        Directory.CreateDirectory(documentsDirectory);

                    DirectoryInfo dir = new DirectoryInfo(documentsDirectory + "\\Asset Manager");
                    if (!Directory.Exists(dir.FullName + "\\" + NewCoronaGameProject.ProjectName))
                        Directory.CreateDirectory(dir.FullName + "\\" + NewCoronaGameProject.ProjectName);

                    //Set the language of the project
                    if (!this.langueNameCb.SelectedItem.ToString().Equals(""))
                    {
                        LangueObject langue = new LangueObject(this.langueNameCb.SelectedItem.ToString());
                        this.parent.CurrentProject.DefaultLanguage = this.langueNameCb.SelectedItem.ToString();

                        bool langExists = false;
                        for (int i = 0; i < this.parent.CurrentProject.Langues.Count; i++)
                        {
                            if (this.parent.CurrentProject.Langues[i].Langue.Equals(langue.Langue))
                                langExists = true;
                        }

                        if (langExists == false)
                        {
                            this.parent.CurrentProject.Langues.Add(langue);
                        }

                    }

                    this.parent.sceneEditorView1.GraphicsContentManager.SetCurrentProject(NewCoronaGameProject, this.parent.sceneEditorView1.CurrentScale, this.parent.sceneEditorView1.getOffsetPoint());
                    this.parent.getElementTreeView().loadProject(this.parent.CurrentProject);


                    

                }
                else
                {
                    //Renomer le repertoire du projet
                    this.parent.getElementTreeView().ProjectRootNodeSelected.Text = this.NewCoronaGameProject.ProjectName;

                    //Set the language of the project
                    string defaultLang = "english";
                    if(this.langueNameCb.SelectedItem != null)
                    {
                        defaultLang = this.langueNameCb.SelectedItem.ToString();
                    }

                    if (!defaultLang.Equals(""))
                    {
                        LangueObject langue = new LangueObject(defaultLang);
                        this.parent.CurrentProject.DefaultLanguage = defaultLang;

                        bool langExists = false;
                        for (int i = 0; i < this.parent.CurrentProject.Langues.Count; i++)
                        {
                            if (this.parent.CurrentProject.Langues[i].Langue.Equals(langue.Langue))
                                langExists = true;
                        }

                        if (langExists == false)
                        {
                            this.parent.CurrentProject.Langues.Add(langue);
                        }

                    }
                }
                this.parent.saveProject(false);
              
            }
            else
            {
                MessageBox.Show("Path name incorect or project name invalid.");
                return;
            }

            this.lbListPermissionAndroid.Items.Clear();
        }


        private void reloadProject(CoronaGameProject cgp)
        {
           
            this.updateConfigTreeView();
            this.updateBuildTreeView();

            this.mode = "MODIFY";
            this.Icone = cgp.Icone;
            if (Icone != null)
            {
                try
                {
                    Image i = new Bitmap(this.Icone);
                    Image resize = new Bitmap(i, 72, 72);

                    IconePathPb.BackgroundImage = resize;
                }
                catch
                {
                    MessageBox.Show("Can't reload IconePath, File not found!  " + this.Icone);
                }
            }
            tbProjectName.Text = cgp.ProjectName;

            tbProjectPath.Text = cgp.ProjectPath;


            if (cgp.Orientation == CoronaGameProject.OrientationScreen.Landscape)
            {

                this.landscapeRBt.Checked = true;
                this.portraitRBt.Checked = false;

            }

            else
            {

                this.portraitRBt.Checked = true;
                this.landscapeRBt.Checked = false;

            }

            this.screenWidthNumUpDw.Value = cgp.width;

            this.screenHeightNumUpDw.Value = cgp.height;

            cbScale.SelectedItem = cgp.scale;


            lbDynamicResizing.Items.Clear();

            if (cgp.ImageSuffix != null)
                lbDynamicResizing.Items.AddRange(cgp.ImageSuffix.ToArray());

            cbFPS.SelectedItem = cgp.fps.ToString();

            cbAntiAliasing.Checked = cgp.antialias;

            tbVersionCodeAndroid.Text = cgp.AndroidVersionCode;

            lbSupportedOrientation.Items.Clear();
            if (cgp.SupportedOrientation != null)
                lbSupportedOrientation.Items.AddRange(cgp.SupportedOrientation.ToArray());

            lbListPermissionAndroid.Items.Clear();
            if (cgp.AndroidPermissions != null)
                lbListPermissionAndroid.Items.AddRange(cgp.AndroidPermissions.ToArray());

            tbCustomCoronaBuildName.Text = cgp.CustomBuildName;

            if (langueNameCb.Items.Contains(cgp.DefaultLanguage))
                this.langueNameCb.SelectedItem = cgp.DefaultLanguage;
            else
                this.langueNameCb.SelectedItem = "English";


         
        }


        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Directory.CreateDirectory(sourceFolder);
            }
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }



        private void tbProjectName_TextChanged(object sender, EventArgs e)
        {
            if (this.mode.Equals("NEW"))
            {

                if (folderBrowserDialog1.SelectedPath != "")
                {
                    tbProjectPath.Text = folderBrowserDialog1.SelectedPath + "\\" + tbProjectName.Text.Replace(" ", "_") + "_Project";
                }
            }
            else
            {
                int lastIndex = this.NewCoronaGameProject.ProjectPath.LastIndexOf("\\");
                string pathWithoutNameProject = this.NewCoronaGameProject.ProjectPath.Substring(0, lastIndex);
                tbProjectPath.Text = pathWithoutNameProject + "\\" + tbProjectName.Text.Replace(" ", "_") + "_Project";
            }
        }

        private void portraitRBt_CheckedChanged(object sender, EventArgs e)
        {
            if (this.portraitRBt.Checked == true)
                this.landscapeRBt.Checked = false;
        }

        private void landscapeRBt_CheckedChanged(object sender, EventArgs e)
        {
            if (this.landscapeRBt.Checked == true)
                this.portraitRBt.Checked = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (cbSupportedOrientation.SelectedItem != null)
            {
                lbSupportedOrientation.Items.Add(cbSupportedOrientation.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Please select an orientation !");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lbSupportedOrientation.SelectedItem != null)
            {
                lbSupportedOrientation.Items.Remove(lbSupportedOrientation.SelectedItem);
            }
            else
            {
                MessageBox.Show("Nothing to delete!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tbSuffixResizing.Text != null && tbValueResizing.Text != null)
            {
                lbDynamicResizing.Items.Add("[\"" + tbSuffixResizing.Text + "\"] = " + tbValueResizing.Text.Replace(",", ".").Replace(" ", ""));
            }
            else
            {
                MessageBox.Show("Suffix and Value needed!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lbDynamicResizing.SelectedItem != null)
            {
                lbDynamicResizing.Items.Remove(lbDynamicResizing.SelectedItem);
            }
            else
            {
                MessageBox.Show("Nothing to delete!");
            }
        }

        private void btAddPermissionAndroid_Click(object sender, EventArgs e)
        {
            if (cbPermissionAndroid.SelectedItem != null)
            {
                this.lbListPermissionAndroid.Items.Add(cbPermissionAndroid.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Please select an Android permission !");
            }
        }

        private void btRemovePermissionAndroid_Click(object sender, EventArgs e)
        {
            if (lbListPermissionAndroid.SelectedItem != null)
            {
                lbListPermissionAndroid.Items.Remove(lbListPermissionAndroid.SelectedItem);
            }
            else
            {
                MessageBox.Show("Nothing to delete!");
            }
        }

        private void IconePathPb_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                // ofd.Filter = "PNG|.png";
                ofd.DefaultExt = ".png";
                DialogResult dr = ofd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Stream s = ofd.OpenFile();
                    Image i = new Bitmap(s);
                    Image resize = new Bitmap(i, 72, 72);
                    IconePathPb.BackgroundImage = resize;
                    Icone = resize;
                }
            }
            catch
            {
                MessageBox.Show("Incorect File Format (only .png are supported) !");
            }
        }

        private void browseFolderBt_Click(object sender, EventArgs e)
        {

            DialogResult Result = folderBrowserDialog1.ShowDialog();
            switch (Result)
            {
                case DialogResult.OK:
                    {
                        tbProjectPath.Text = folderBrowserDialog1.SelectedPath + "\\" + this.tbProjectName.Text.Replace(" ", "_") + "_Project";
                        break;
                    }
                case DialogResult.Cancel:
                    {
                        this.Text = "[Cancel]";
                        break;
                    }
            }
        }

        private void addFieldBt_Click(object sender, EventArgs e)
        {
            string fieldName = this.fieldNameTxtBx.Text.Replace(" ","");
            string fieldType = this.fieldTypeCmbBx.Text;
            string value = this.fieldValueTxtBx.Text;

            TreeListViewItem itemSelected = null;
            if (this.appConfigFieldsTreeView.SelectedItems.Count >0)
            {
                itemSelected = this.appConfigFieldsTreeView.SelectedItems[0];
            }

            if (!fieldName.Equals(""))
            {

                if (fieldType.Equals("Number") || fieldType.Equals("String") || fieldType.Equals("Boolean"))
                {
                    if (fieldType.Equals("Boolean"))
                    {
                        if (value.ToLower().Equals("true") || value.ToLower().Equals("false"))
                        {
                            ConfigField newField = new ConfigField(fieldName, fieldType.ToUpper(), value.ToLower(), false, true);

                            if (itemSelected != null)
                            {
                                ConfigField fieldSelected = itemSelected.Tag as ConfigField;
                                if (fieldSelected != null)
                                {
                                    if (fieldSelected.Type.Equals("TABLE"))
                                    {
                                        bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldSelected.Children, false);
                                        if (alreayExist == false)
                                        {
                                            fieldSelected.Children.Add(newField);
                                            this.newConfigFieldTreeItem(newField, itemSelected);
                                        }
                                        else
                                        {
                                            MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        //GetParent
                                        if (itemSelected.Parent != null)
                                        {
                                            ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;
                                            bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldParent.Children, false);
                                            if (alreayExist == false)
                                            {
                                                fieldParent.Children.Add(newField);
                                                this.newConfigFieldTreeItem(newField, itemSelected.Parent);
                                            }
                                            else
                                            {
                                                MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                                
                        }
                        else
                        {
                            MessageBox.Show("A Boolean type must have a true or false value!", "Value is not a boolean!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        if (fieldType.ToUpper().Equals("NUMBER"))
                        {
                            int valueInt = -1;
                            bool res = int.TryParse(value, out valueInt);
                            if (res == false)
                            {
                                MessageBox.Show("The given value is not a Number!", "Value does not match type!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        ConfigField newField = new ConfigField(fieldName, fieldType.ToUpper(), value, false, true);

                        if (itemSelected != null)
                        {
                            ConfigField fieldSelected = itemSelected.Tag as ConfigField;
                            if (fieldSelected != null)
                            {
                                if (fieldSelected.Type.Equals("TABLE"))
                                {
                                    bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldSelected.Children, false);
                                     if (alreayExist == false)
                                     {
                                         fieldSelected.Children.Add(newField);
                                         this.newConfigFieldTreeItem(newField, itemSelected);
                                     }
                                     else
                                     {
                                         MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                     }
                                }
                                else
                                {
                                    //GetParent
                                    if (itemSelected.Parent != null)
                                    {
                                        ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;
                                        bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldParent.Children, false);
                                         if (alreayExist == false)
                                         {
                                             fieldParent.Children.Add(newField);
                                             this.newConfigFieldTreeItem(newField, itemSelected.Parent);
                                         }
                                         else
                                         {
                                             MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                         }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }



                }

                else if (fieldType.Equals("Table"))
                {
                    ConfigField newField = new ConfigField(fieldName, false, true);

                    if (itemSelected != null)
                    {
                        ConfigField fieldSelected = itemSelected.Tag as ConfigField;
                        if (fieldSelected != null)
                        {
                            if (fieldSelected.Type.Equals("TABLE"))
                            {
                                bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldSelected.Children, false);
                                 if (alreayExist == false)
                                 {
                                     fieldSelected.Children.Add(newField);
                                     this.newConfigFieldTreeItem(newField, itemSelected);
                                 }
                                 else
                                 {
                                     MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 }
                            }
                            else
                            {
                                //GetParent
                                if (itemSelected.Parent != null)
                                {
                                    ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;

                                    bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldParent.Children, false);
                                    if (alreayExist == false)
                                    {
                                        fieldParent.Children.Add(newField);
                                        this.newConfigFieldTreeItem(newField, itemSelected.Parent);
                                    }
                                    else
                                    {
                                        MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    MessageBox.Show("Please provide field information before adding it!", "Field Information Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
        }

        private void fieldTypeCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.fieldTypeCmbBx.Text;
            if (text.Equals("Number") || text.Equals("String") || text.Equals("Boolean"))
            {
                this.addFieldBt.Enabled = true;
                this.fieldValueTxtBx.Enabled = true;

            }
            else if(text.Equals("Table"))
            {
                this.addFieldBt.Enabled = true;
                this.fieldValueTxtBx.Enabled = false;
            }
            else
            {
                this.fieldValueTxtBx.Enabled = false;
                this.addFieldBt.Enabled = false;
            }
        }

        public void newConfigFieldTreeItem(ConfigField field,TreeListViewItem itemParent)
        {
          

            TreeListViewItem item = new TreeListViewItem();
          
            item.Text = field.Name;
            if (field.Type.Equals("TABLE"))
                item.ImageIndex = 0;
            else
                item.ImageIndex = 1;

            item.SubItems.Add(field.Type);
            
            if (!field.Type.Equals("TABLE"))
            {
                if(field.Type.Equals("STRING"))
                    item.SubItems.Add("\"" + field.Value + "\"");
                else
                    item.SubItems.Add(field.Value);
            }
            else
            {
                for (int i = 0; i < field.Children.Count; i++)
                {
                    this.newConfigFieldTreeItem(field.Children[i], item);
                }
            }

            if(itemParent!= null)
                itemParent.Items.Add(item);
            else
                this.appConfigFieldsTreeView.Items.Add(item);


            item.SetIndentation();
            item.Tag = field;
        }

        public void updateConfigTreeView()
        {
            this.appConfigFieldsTreeView.BeginUpdate();
            this.appConfigFieldsTreeView.Items.Clear();

            if (this.NewCoronaGameProject != null)
            {
                this.NewCoronaGameProject.updateConfigFields(1,1);

                for (int i = 0; i < this.NewCoronaGameProject.CustomConfigFields.Count; i++)
                {
                    this.newConfigFieldTreeItem(this.NewCoronaGameProject.CustomConfigFields[i], null);
                }
            }

            this.appConfigFieldsTreeView.EndUpdate();

            this.appConfigFieldsTreeView.ExpandAll();
        }

        private void removeFieldBt_Click(object sender, EventArgs e)
        {
            if (this.appConfigFieldsTreeView.SelectedItems.Count > 0)
            {
                TreeListViewItem itemSelected = this.appConfigFieldsTreeView.SelectedItems[0];
                ConfigField selectedField = itemSelected.Tag as ConfigField;

                if (selectedField.IsAutomaticField == true)
                {
                    MessageBox.Show("Cannot remove the " + selectedField.Name + " field because Krea uses it!", "Field cannot be removed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (itemSelected.Parent == null)
                {
                    MessageBox.Show("Cannot remove this field because Krea uses it!", "Field cannot be removed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;

                    fieldParent.Children.Remove(selectedField);
                    itemSelected.Remove();
                }
            }
           
        }


        public void newBuildFieldTreeItem(ConfigField field, TreeListViewItem itemParent)
        {


            TreeListViewItem item = new TreeListViewItem();

            item.Text = field.Name;
            if (field.Type.Equals("TABLE"))
                item.ImageIndex = 0;
            else
                item.ImageIndex = 1;

            item.SubItems.Add(field.Type);

            if (!field.Type.Equals("TABLE"))
            {
                if (field.Type.Equals("STRING"))
                    item.SubItems.Add("\"" + field.Value + "\"");
                else
                    item.SubItems.Add(field.Value);
            }
            else
            {
                for (int i = 0; i < field.Children.Count; i++)
                {
                    this.newBuildFieldTreeItem(field.Children[i], item);
                }
            }

            if (itemParent != null)
                itemParent.Items.Add(item);
            else
                this.buildSettingsTreeListView.Items.Add(item);


            item.SetIndentation();
            item.Tag = field;
        }

        public void updateBuildTreeView()
        {
            this.buildSettingsTreeListView.BeginUpdate();
            this.buildSettingsTreeListView.Items.Clear();

            if (this.NewCoronaGameProject != null)
            {
                this.NewCoronaGameProject.updateBuildFields();

                for (int i = 0; i < this.NewCoronaGameProject.CustomBuildFields.Count; i++)
                {
                    this.newBuildFieldTreeItem(this.NewCoronaGameProject.CustomBuildFields[i], null);
                }
            }

            this.buildSettingsTreeListView.EndUpdate();

            this.buildSettingsTreeListView.ExpandAll();
        }

    


        private void addBuildFieldBt_Click(object sender, EventArgs e)
        {
            string fieldName = this.buildFieldNameTxtBx.Text.Replace(" ", "");
            string fieldType = this.buildFieldTypeCmbBx.Text;
            string value = this.buildFieldValueTxtBx.Text;

            TreeListViewItem itemSelected = null;
            if (this.buildSettingsTreeListView.SelectedItems.Count > 0)
            {
                itemSelected = this.buildSettingsTreeListView.SelectedItems[0];
            }

            if (!fieldName.Equals(""))
            {

                if (fieldType.Equals("Number") || fieldType.Equals("String") || fieldType.Equals("Boolean"))
                {
                    if (fieldType.Equals("Boolean"))
                    {
                        if (value.ToLower().Equals("true") || value.ToLower().Equals("false"))
                        {
                          
                            ConfigField newField = new ConfigField(fieldName, fieldType.ToUpper(), value.ToLower(), false, true);

                            if (itemSelected != null)
                            {
                                ConfigField fieldSelected = itemSelected.Tag as ConfigField;
                                if (fieldSelected != null)
                                {
                                    if (fieldSelected.Type.Equals("TABLE"))
                                    {
                                        bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldSelected.Children, false);
                                        if (alreayExist == false)
                                        {
                                            fieldSelected.Children.Add(newField);
                                            this.newBuildFieldTreeItem(newField, itemSelected);
                                        }
                                        else
                                        {
                                            MessageBox.Show("The field \""+newField.Name+"\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        //GetParent
                                        if (itemSelected.Parent != null)
                                        {
                                            ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;

                                            bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldParent.Children, false);
                                             if (alreayExist == false)
                                             {
                                                 fieldParent.Children.Add(newField);
                                                 this.newBuildFieldTreeItem(newField, itemSelected.Parent);
                                             }
                                             else
                                             {
                                                 MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                             }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                        else
                        {
                            MessageBox.Show("A Boolean type must have a true or false value!", "Value is not a boolean!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        if (fieldType.ToUpper().Equals("NUMBER"))
                        {
                            int valueInt = -1;
                            bool res = int.TryParse(value, out valueInt);
                            if (res == false)
                            {
                                MessageBox.Show("The given value is not a Number!", "Value does not match type!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        ConfigField newField = new ConfigField(fieldName, fieldType.ToUpper(), value, false, true);

                        if (itemSelected != null)
                        {
                            ConfigField fieldSelected = itemSelected.Tag as ConfigField;
                            if (fieldSelected != null)
                            {
                                if (fieldSelected.Type.Equals("TABLE"))
                                {
                                    bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldSelected.Children, false);
                                    if (alreayExist == false)
                                    {
                                        fieldSelected.Children.Add(newField);
                                        this.newBuildFieldTreeItem(newField, itemSelected);
                                    }
                                    else
                                    {
                                        MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    //GetParent
                                    if (itemSelected.Parent != null)
                                    {
                                        ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;

                                        bool alreayExist = this.NewCoronaGameProject.doesFieldAlreadyExist(newField.Name, fieldParent.Children, false);
                                        if (alreayExist == false)
                                        {

                                            fieldParent.Children.Add(newField);
                                            this.newBuildFieldTreeItem(newField, itemSelected.Parent);
                                        }
                                        else
                                        {
                                            MessageBox.Show("The field \"" + newField.Name + "\" already exists!", "Integrity information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }



                }

                else if (fieldType.Equals("Table"))
                {
                    ConfigField newField = new ConfigField(fieldName, false, true);

                    if (itemSelected != null)
                    {
                        ConfigField fieldSelected = itemSelected.Tag as ConfigField;
                        if (fieldSelected != null)
                        {
                            if (fieldSelected.Type.Equals("TABLE"))
                            {
                                fieldSelected.Children.Add(newField);
                                this.newBuildFieldTreeItem(newField, itemSelected);
                            }
                            else
                            {
                                //GetParent
                                if (itemSelected.Parent != null)
                                {
                                    ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;
                                    fieldParent.Children.Add(newField);
                                    this.newBuildFieldTreeItem(newField, itemSelected.Parent);
                                }
                                else
                                {
                                    MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a table field to insert the new field! ", "Select parent item!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    MessageBox.Show("Please provide field information before adding it!", "Field Information Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void removeBuildFieldBt_Click(object sender, EventArgs e)
        {
            if (this.buildSettingsTreeListView.SelectedItems.Count > 0)
            {
                TreeListViewItem itemSelected = this.buildSettingsTreeListView.SelectedItems[0];
                ConfigField selectedField = itemSelected.Tag as ConfigField;

                if (selectedField.IsAutomaticField == true)
                {
                    MessageBox.Show("Cannot remove the " + selectedField.Name + " field because Krea uses it!", "Field cannot be removed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (itemSelected.Parent == null)
                {
                    MessageBox.Show("Cannot remove this field because Krea uses it!", "Field cannot be removed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ConfigField fieldParent = itemSelected.Parent.Tag as ConfigField;

                    fieldParent.Children.Remove(selectedField);
                    itemSelected.Remove();
                }
            }
        }

        private void buildFieldTypeCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.buildFieldTypeCmbBx.Text;
            if (text.Equals("Number") || text.Equals("String") || text.Equals("Boolean"))
            {
                this.addBuildFieldBt.Enabled = true;
                this.buildFieldValueTxtBx.Enabled = true;

            }
            else if (text.Equals("Table"))
            {
                this.addBuildFieldBt.Enabled = true;
                this.buildFieldValueTxtBx.Enabled = false;
            }
            else
            {
                this.buildFieldValueTxtBx.Enabled = false;
                this.addBuildFieldBt.Enabled = false;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.updateBuildTreeView();
            this.updateConfigTreeView();
        }

     

        private void setConfigValuesFromItem(ConfigField field)
        {
            if (field.IsAutomaticField == true)
            {
                this.toolStrip7.Enabled = false;
            }
            else
            {
                this.toolStrip7.Enabled = true;
                this.editConfigFieldNameTxtBx.Text = field.Name;
                if (!field.Type.Equals("TABLE"))
                {
                    this.editConfigFieldValueTxtBx.Enabled = true;
                    this.editConfigFieldValueTxtBx.Text = field.Value;
                }
                else
                {
                    this.editConfigFieldValueTxtBx.Enabled = false;
                }
            }

        }


        private void setBuildValuesFromItem(ConfigField field)
        {
            if (field.IsAutomaticField == true)
            {
                this.toolStrip8.Enabled = false;
            }
            else
            {
                this.toolStrip8.Enabled = true;
                this.editBuildFieldNameTxtBx.Text = field.Name;
                if (!field.Type.Equals("TABLE"))
                {
                    this.editBuildFieldValueTxtBx.Enabled = true;
                    this.editBuildFieldValueTxtBx.Text = field.Value;
                }
                else
                {
                    this.editBuildFieldValueTxtBx.Enabled = false;
                }
            }

        }

        private void appConfigFieldsTreeView_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeListViewItem itemSelected = this.appConfigFieldsTreeView.FocusedItem;
            if (itemSelected != null)
            {
                ConfigField field = itemSelected.Tag as ConfigField;
                this.setConfigValuesFromItem(field);
            }
        }

        private void saveConfigFieldItemEditionBt_Click(object sender, EventArgs e)
        {
            TreeListViewItem itemSelected = this.appConfigFieldsTreeView.FocusedItem;
            if (itemSelected != null)
            {
                ConfigField field = itemSelected.Tag as ConfigField;

                if (field.IsAutomaticField == true)
                {
                    MessageBox.Show("Cannot edit a read-only field!", "Field locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                field.Name = this.editConfigFieldNameTxtBx.Text.Replace(" ","").Replace(".","_");

                if (field.Type.ToUpper().Equals("NUMBER"))
                {
                    int valueInt = -1;
                    bool res = int.TryParse(this.editConfigFieldValueTxtBx.Text, out valueInt);
                    if (res == false)
                    {
                        MessageBox.Show("The given value is not a Number!", "Value does not match type!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    field.Value = valueInt.ToString() ;
                }
                else if (!field.Type.Equals("TABLE"))
                {
                    string value = this.editConfigFieldValueTxtBx.Text;
                    if (field.Type.Equals("BOOLEAN"))
                    {
                        if (value.ToLower().Equals("true") || value.ToLower().Equals("false"))
                        {
                            field.Value = value.ToLower();
                        }
                        else
                        {
                            MessageBox.Show("A Boolean type must have a true or false value!", "Value is not a boolean!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        field.Value = value;
                    }
                 
                }

                this.updateConfigTreeView();
            }
        }

        private void editBuildFieldSaveBt_Click(object sender, EventArgs e)
        {
            TreeListViewItem itemSelected = this.buildSettingsTreeListView.FocusedItem;
            if (itemSelected != null)
            {
                ConfigField field = itemSelected.Tag as ConfigField;

                if (field.IsAutomaticField == true)
                {
                    MessageBox.Show("Cannot edit a read-only field!", "Field locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                field.Name = this.editBuildFieldNameTxtBx.Text.Replace(" ", "").Replace(".", "_");

                if (field.Type.ToUpper().Equals("NUMBER"))
                {
                    int valueInt = -1;
                    bool res = int.TryParse(this.editBuildFieldValueTxtBx.Text, out valueInt);
                    if (res == false)
                    {
                        MessageBox.Show("The given value is not a Number!", "Value does not match type!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    field.Value = valueInt.ToString();
                }
                else if (!field.Type.Equals("TABLE"))
                {
                    string value = this.editBuildFieldValueTxtBx.Text;
                    if (field.Type.Equals("BOOLEAN"))
                    {
                        if (value.ToLower().Equals("true") || value.ToLower().Equals("false"))
                        {
                            field.Value = value.ToLower();
                        }
                        else
                        {
                            MessageBox.Show("A Boolean type must have a true or false value!", "Value is not a boolean!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        field.Value = value;
                    }

                }

                this.updateBuildTreeView();
            }
        }

        private void buildSettingsTreeListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeListViewItem itemSelected = this.buildSettingsTreeListView.FocusedItem;
            if (itemSelected != null)
            {
                ConfigField field = itemSelected.Tag as ConfigField;
                this.setBuildValuesFromItem(field);
            }
        }

    }
}
