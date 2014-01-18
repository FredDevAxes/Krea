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
using System.Windows.Forms.PropertyGridInternal;

namespace Krea.GameEditor.PropertyGridEditors
{
    public partial class SliderPropertyEditor : UserControl
    {
        public IWindowsFormsEditorService editorService = null;
        public ITypeDescriptorContext contextDescriptor = null;
        public bool isDecimalValue = false;
        public bool isFloatValue = false;
        public Point trackLimits = new Point(0, 100);

        public SliderPropertyEditor()
        {
            InitializeComponent();

        }

        private void previewValueBt_Click(object sender, EventArgs e)
        {
            if (editorService != null)
                editorService.CloseDropDown();

            
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            if (this.isDecimalValue == false)
                this.previewValueBt.Text = this.trackBar.Value.ToString();
            else
            {
                this.previewValueBt.Text = ((float)this.trackBar.Value/100.0f).ToString();
            }
        }
     


    }


    public class SliderEditor : UITypeEditor
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

            
            
            SliderPropertyEditor slider = new SliderPropertyEditor();
            slider.editorService = frmsvr;
            slider.contextDescriptor = context;

            slider.isDecimalValue = false;
            slider.trackLimits = new Point(0, 100);


            PropertyDescriptor propertydesc = null;
            if (context.PropertyDescriptor.Attributes.Count == 0)
            {
                System.Type propertyType = context.PropertyDescriptor.GetType();
                System.Reflection.FieldInfo fieldInfo = propertyType.GetField(
                    "descriptors",
                    System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance
                );
                PropertyDescriptor[] descriptors =
                    (PropertyDescriptor[])(fieldInfo.GetValue(context.PropertyDescriptor));

                propertydesc = descriptors[0];
            }
            else
                propertydesc = context.PropertyDescriptor;

            foreach (Attribute att in propertydesc.Attributes)
            {
                if (att is AmbientValueAttribute)
                {
                    AmbientValueAttribute tempAmbiantAttribute = att as AmbientValueAttribute;
                    if (tempAmbiantAttribute.Value is string)
                    {
                        string paramStr = tempAmbiantAttribute.Value as string;
                        string[] splittedParams = paramStr.Split('/');
                        //Treat Type
                        if (splittedParams[0].Equals("float"))
                            slider.isFloatValue = true;
                        else if (splittedParams[0].Equals("decimal"))
                            slider.isDecimalValue = true;

                        //Treat Limits
                        string strMin = splittedParams[1].Split(',')[0];
                        int min = 0;
                        int.TryParse(strMin, out min);

                        string strMax = splittedParams[1].Split(',')[1];
                        int max = 0;
                        int.TryParse(strMax, out max);

                        slider.trackLimits = new Point(min, max);

                        break;
                    }
                }
            }


       
            

            slider.trackBar.Minimum = slider.trackLimits.X;
            slider.trackBar.Maximum = slider.trackLimits.Y;

            if (context.PropertyDescriptor.Attributes.Count == 0)
            {
                if (slider.isDecimalValue == true || slider.isFloatValue == true)
                {
                    int defaultValue = (slider.trackBar.Minimum + slider.trackBar.Maximum) / 2;
                    defaultValue = Convert.ToInt32(Convert.ToDouble(defaultValue) * 100);
                    slider.trackBar.Value = defaultValue;
                    slider.trackBar.TickFrequency = 10;
                }
                else
                {
                    int defaultValue = (slider.trackBar.Minimum + slider.trackBar.Maximum) / 2;
                    slider.trackBar.Value = defaultValue;
                }
            }
            else
            {
                if (slider.isDecimalValue == true || slider.isFloatValue == true)
                {
                    int currentValue = Convert.ToInt32(Convert.ToDouble(value) * 100);
                    slider.trackBar.Value = currentValue;
                    slider.trackBar.TickFrequency = 10;
                }
                else
                {
                    int currentValue = Convert.ToInt32(value);
                    slider.trackBar.Value = currentValue;
                }

            }

           
          
            frmsvr.DropDownControl(slider);


            if (slider.isDecimalValue == true)
            {
                decimal finalValue = (decimal)((float)slider.trackBar.Value / 100.0f);
                slider.Dispose();
                return finalValue;
            }
            else if (slider.isFloatValue == true)
            {
                float finalValue = (float)slider.trackBar.Value / 100.0f;
                slider.Dispose();
                return finalValue;
            }
            else
            {
                int finalValue = slider.trackBar.Value;
                slider.Dispose();
                return finalValue;
            }
           
        }


    }
}
