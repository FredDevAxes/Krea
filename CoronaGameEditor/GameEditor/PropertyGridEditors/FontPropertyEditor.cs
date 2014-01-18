using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Drawing.Text;
using Krea.CoronaClasses;
using Krea.GameEditor.FontManager;

namespace Krea.GameEditor.PropertyGridEditors
{
    public partial class FontPropertyEditor : ListView
    {
        private static IWindowsFormsEditorService editorService = null;
        public FontPropertyEditor(FontItem  font,
            IWindowsFormsEditorService editorServiceParam )
        {
            editorService = editorServiceParam;
            InitializeComponent();

            this.View = System.Windows.Forms.View.Tile;


            //Default Font 
            ListViewItem itemDefault = new ListViewItem();
            itemDefault.Tag = new FontItem("DEFAULT",font.projectParent);
            itemDefault.Text = "Default Font";
            this.Items.Add(itemDefault);

            // Add three font files to the private collection.
            for (int i = 0; i < font.projectParent.AvailableFont.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = font.projectParent.AvailableFont[i];
                item.Font = new Font(font.projectParent.AvailableFont[i].NameForIphone, 14);
                item.Text = font.projectParent.AvailableFont[i].NameForIphone;
                this.Items.Add(item);

            }


            //this.Invalidate();
        }

        private void FontPropertyEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorService.CloseDropDown();

        }



    

    }

    public class FontEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(
            System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }


        public override object EditValue(
                ITypeDescriptorContext context,
                IServiceProvider provider,
                object value)
        {
            if(context.Instance is object[])
                if (((object[])context.Instance).Length > 1)
                    return null;

            //use IWindowsFormsEditorService object to display a control in the dropdown area
            IWindowsFormsEditorService frmsvr = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (frmsvr == null)
                return null;

            FontItem font = null;
            if (value != null)
            {
                font  = value as FontItem;
            }
            else
            {
                try
                {
                    object obj = ((object[])context.Instance)[0];
                    PropertyGridConverters.TextPropertyConverter prop = (PropertyGridConverters.TextPropertyConverter)obj;

                    CoronaObject objectSelected = prop.GetObjectSelected();
                    font = (objectSelected.DisplayObject.Figure as CGE_Figures.Texte).font2.FontItem;
                }
                catch (Exception ex)
                {
                    return null;
                }
                    

            
            }

           

            FontPropertyEditor control = new FontPropertyEditor(font, frmsvr);
            frmsvr.DropDownControl(control);
            
            if (control.SelectedItems.Count > 0)
                return control.SelectedItems[0].Tag;
            else return font;
        }

       
    }
}
