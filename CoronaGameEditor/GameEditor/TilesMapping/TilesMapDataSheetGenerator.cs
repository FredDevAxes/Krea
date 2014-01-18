using System;
using System.Text;
using System.IO;
using System.Drawing;

namespace Krea.Corona_Classes
{
    class TilesMapDataSheetGenerator
    {
         //---------------------------------------------------
        //-------------------Constantes--------------------
        //---------------------------------------------------
        const String HEADER_LUA = @" module(...)
    -- This file is for use with Krea
    -- 
    -- The function getSpriteSheetData() returns a table suitable for importing using sprite.newSpriteSheetFromData()
   
    function getSpriteSheetData()
        local sheet = {
            frames = {

            ";


        const String FOOTER_LUA = @"
                }
            }
            return sheet
        end";


        //---------------------------------------------------
        //-------------------Attributs--------------------
        //---------------------------------------------------
        private string sheetName;
        private StringBuilder stringBuilder;
        public int textureCount = 0;
        //---------------------------------------------------
        //-------------------Constructeurs--------------------
        //---------------------------------------------------
        public TilesMapDataSheetGenerator(string name)
        {
            this.sheetName = name;
            this.stringBuilder = new StringBuilder();

            this.stringBuilder.Append(HEADER_LUA);

        }

        //---------------------------------------------------
        //-------------------Methodes--------------------
        //---------------------------------------------------
        public void addFrame(int index,Point position, Size size)
        {
            String frameLua = "";

            frameLua +="{ \n";
            frameLua += " name = \"frame"+index+"\", \n";

            frameLua += " spriteColorRect = { x = 0,y = 0, width = " + size.Width + ", height = " +size.Height + "},\n";

            frameLua += " textureRect = { x = " + position.X + ", y = " + position.Y +
                ", width = " + size.Width + ", height = " +size.Height + "},\n";

            frameLua += " spriteSourceSize = {width = " + size.Width + ", height = " +size.Height + "},\n";

            frameLua += " spriteTrimmed = true, \n";
            frameLua += " spriteRotated = false \n";

            frameLua += "}, \n";

            this.stringBuilder.Append(frameLua);
            textureCount = textureCount + 1;
        }

        public void createDataSheetFileLua(string destFolder,string suffix,int idSheet)
        {
            this.stringBuilder.Append(FOOTER_LUA);

            File.WriteAllText(destFolder + "\\" + this.sheetName +"data"+suffix+".lua",this.stringBuilder.ToString());
        }

        
    }
}
