using System;

namespace Krea.Corona_Classes
{
    [Serializable()]
    public class Snippet
    {
        //---------------------------------------------------
        //-------------------Attributs-----------------------
        //---------------------------------------------------
        public string Name;
        public string Author;
        public string Description;
        public float Version;
        public string Function;
        public string Category;
        public string Syntax;

        //---------------------------------------------------
        //-------------------Constructeurs-------------------
        //---------------------------------------------------
        public Snippet(string name,string syntax,string category, string author, string description, float version,string function)
        {
            this.Name = name;
            this.Description = description;
            this.Version = version;
            this.Author = author;
            this.Function = function;
            this.Category = category;
            this.Syntax = syntax;

        }
        //---------------------------------------------------
        //------------------- Methodes ----------------------
        //---------------------------------------------------
        public Snippet cloneInstance()
        {
            Snippet snippet = new Snippet(this.Name,this.Syntax, this.Category, this.Author, this.Description, this.Version, this.Function);
            return snippet;
        }
        //---------------------------------------------------
        //-------------------Accesseurs----------------------
        //--------------------------------------------------
    }
}
