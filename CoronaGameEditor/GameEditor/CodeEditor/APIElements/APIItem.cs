using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krea.GameEditor.CodeEditor.APIElements
{
    public class APIItem
    {

         //----------------------------------------------------------
        //--------------------- ATTRIBUTS --------------------------
        //----------------------------------------------------------

        public string name;
        public bool isFunction;
        public bool isClassFunction;
        public string syntax;
        public string returns;
        public string remarks;
        public string description;
        public string parameters;
        //----------------------------------------------------------
        //--------------------- CONSTRUCTEURS ----------------------
        //----------------------------------------------------------

        public APIItem(string name, bool isFunction,bool isClassFunction, string syntax, string returns, string remarks, string description, string parameters)
        {
            this.name = name;
            this.isFunction = isFunction;
            this.isClassFunction = isClassFunction;
            this.syntax = syntax;
            this.description = description;
            this.remarks = remarks;
            this.returns = returns;
            this.parameters = parameters;

        }
        //----------------------------------------------------------
        //--------------------- METHODES ----------------------------
        //----------------------------------------------------------

    }
}
