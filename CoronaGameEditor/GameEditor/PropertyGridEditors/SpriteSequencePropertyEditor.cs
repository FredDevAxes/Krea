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
using Krea.GameEditor.CollisionManager;

namespace Krea.GameEditor.PropertyGridEditors
{
    public partial class SpriteSequencePropertyEditor : ListBox
    {
        private static IWindowsFormsEditorService editorService = null;
        public SpriteSequencePropertyEditor(CoronaObject obj,
            IWindowsFormsEditorService editorServiceParam )
        {
            editorService = editorServiceParam;
            InitializeComponent();

            // Add all groups from the scene
            List<CoronaSpriteSetSequence> sequences = obj.DisplayObject.SpriteSet.Sequences;
            this.Items.Add("DEFAULT");
            for (int i = 0; i < sequences.Count; i++)
            {
                CoronaSpriteSetSequence sequence = sequences[i];
                this.Items.Add(sequence.Name);

            }

            this.Refresh();
        }

        private void CollisionGroupPropertyEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorService.CloseDropDown();


        }

    }

    public class SpriteSequenceEditor : UITypeEditor
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


            //use IWindowsFormsEditorService object to display a control in the dropdown area
            IWindowsFormsEditorService frmsvr = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (frmsvr == null)
                return null;



            object propertyConverter = null;
            if(context.Instance is object[])
                propertyConverter = ((object[])context.Instance)[0];
            else
                propertyConverter = context.Instance;

            PropertyGridConverters.SpritePropertyConverter prop = (PropertyGridConverters.SpritePropertyConverter)propertyConverter;

            CoronaObject obj = prop.GetObjectSelected();

            SpriteSequencePropertyEditor control = new SpriteSequencePropertyEditor(obj, frmsvr);
            frmsvr.DropDownControl(control);

            if (control.SelectedItem != null)
            {
                return  control.SelectedItem.ToString();
            } 
            else return "DEFAULT";
        }

       
    }
}
