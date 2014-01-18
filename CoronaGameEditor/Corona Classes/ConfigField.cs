using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.Corona_Classes
{
    [Serializable()]
    public class ConfigField
    {
       
        public string Name;
        public string Type;
        public string Value;
        public bool IsAutomaticField = false;
        public List<ConfigField> Children;
        public bool IsNamedField = true;

        public ConfigField(string name, string type, string value, bool isAutomaticField, bool isNamedField)
        {
            this.Name = name.Replace(" ", "");
            this.Value = value;
            this.Type = type;

            this.IsAutomaticField = isAutomaticField;
            this.IsNamedField = isNamedField;
        }



        public ConfigField(string name, bool isAutomaticField, bool isNamedField)
        {
            this.Name = name.Replace(" ", "");
            this.Children = new List<ConfigField>();
            this.Type = "TABLE";

            this.IsAutomaticField = isAutomaticField;
            this.IsNamedField = isNamedField;
        }

        public string ToLua(int indentCount)
        {
            string indentStr = "";
            for (int i = 0; i < indentCount; i++)
                indentStr += "\t";

            if (this.Type.Equals("TABLE"))
            {
                string finalSTR = indentStr + this.Name + " = \n" + indentStr +"{\n";
                for (int i = 0; i < this.Children.Count; i++)
                {
                    finalSTR += this.Children[i].ToLua(indentCount +1);
                }
                finalSTR += "\n" + indentStr + "},\n";

                return finalSTR;
            }
            else
            {
                if (this.IsNamedField == true)
                {
                    if (this.Type.Equals("STRING"))
                    {
                        string finalSTR = indentStr + this.Name + " = \"" + this.Value + "\",\n";
                        return finalSTR;
                    }
                    else
                    {
                        string finalSTR = indentStr + this.Name + " = " + this.Value + ",\n";
                        return finalSTR;
                    }
                }
                else
                {
                    if (this.Type.Equals("STRING"))
                    {
                        string finalSTR = indentStr + "\"" + this.Value + "\",\n";
                        return finalSTR;
                    }
                    else
                    {
                        string finalSTR = indentStr + this.Value + ",\n";
                        return finalSTR;
                    }
                }

            }
        }
    }
}
