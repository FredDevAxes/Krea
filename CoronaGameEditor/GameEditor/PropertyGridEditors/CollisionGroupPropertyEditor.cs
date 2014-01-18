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
    public partial class CollisionGroupPropertyEditor : ListBox
    {
        private static IWindowsFormsEditorService editorService = null;
        public CollisionGroupPropertyEditor(CoronaObject obj,
            IWindowsFormsEditorService editorServiceParam )
        {
            editorService = editorServiceParam;
            InitializeComponent();

            // Add all groups from the scene
            List<CollisionFilterGroup> groups = obj.LayerParent.SceneParent.CollisionFilterGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                CollisionFilterGroup group = groups[i];
                this.Items.Add(group.GroupName);

            }

            this.Refresh();
        }

        private void CollisionGroupPropertyEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorService.CloseDropDown();


        }

    }

    public class CollisionGroupEditor : UITypeEditor
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

            CoronaObject obj = null;
            if (propertyConverter is PropertyGridConverters.ObjectPropertyConverter)
            {
                PropertyGridConverters.ObjectPropertyConverter prop = (PropertyGridConverters.ObjectPropertyConverter)propertyConverter;

                obj = prop.GetObjectSelected();

            }
            else if (propertyConverter is PropertyGridConverters.SpritePropertyConverter)
            {
                PropertyGridConverters.SpritePropertyConverter prop = (PropertyGridConverters.SpritePropertyConverter)propertyConverter;

                obj = prop.GetObjectSelected();

            }
            else if (propertyConverter is PropertyGridConverters.BodyElementPropertyConverter)
            {
                PropertyGridConverters.BodyElementPropertyConverter prop = (PropertyGridConverters.BodyElementPropertyConverter)propertyConverter;

                obj = prop.GetObjectSelected();

            }

            if (obj != null)
            {
                CollisionGroupPropertyEditor control = new CollisionGroupPropertyEditor(obj, frmsvr);
                frmsvr.DropDownControl(control);

                if (control.SelectedItem != null)
                {
                    return control.SelectedItem.ToString();
                }
                else return "Default";
            }

            return "Default";
        }

       
    }
}
