using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using Krea.CoronaClasses;
using System.Windows;

namespace Krea.GameEditor.FontManager
{
    [Serializable()]
    public class FontItem
    {

        public string OriginalPath;
        public string NameForIphone;
        public string NameForAndroid;
        public string FileName;
        public Boolean isInstalled = false;
        public string FontFamilyName;
        public CoronaGameProject projectParent;
        public FontItem() { }


        public FontItem(string fontName, CoronaGameProject projectParent)
        {
            
            this.NameForAndroid = fontName;
            this.NameForIphone = fontName;
            this.projectParent = projectParent;
        }

        public bool InitFont(string fontName,string originalfilePath)
        {
            if (originalfilePath.Equals(""))
            {
                this.NameForAndroid = fontName;
            }
            else
            {
                OriginalPath = originalfilePath;
                this.isInstalled = LoadFont();
                

            }

            return this.isInstalled;

        }

      /*  [DllImport("gdi32", EntryPoint = "AddFontResource")]
        public static extern int AddFontResourceA(string lpFileName);*/

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, // handle to destination window
        uint Msg, // message
        int wParam, // first message parameter
        int lParam // second message parameter
        );

        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);

        [DllImport("gdi32")]
        public static extern int RemoveFontResource(string lpFileName);  

        public Boolean SanityzeFontAndMoveToProjectDirectory(string PathToFile, string projectPathDirectory) {
            if (File.Exists(PathToFile)) {
                if(Directory.Exists(projectPathDirectory)){

                    //Load FileName as Android compatibility
                    FileInfo fi = new FileInfo(this.OriginalPath);
                    this.FileName = fi.Name;
                    this.FileName = this.FileName.ToLower();
                    this.FileName = this.FileName.Replace(" ", "");
                    File.Copy(PathToFile, projectPathDirectory + "\\" + FileName, true);
                     return true;
                }
            }
            return false;
        }
        public Boolean LoadFont(){
            if (this.OriginalPath.Equals("")) return false;
            if (File.Exists(this.OriginalPath))
            {
               /* string fontsfolder = System.Environment.GetFolderPath(
                                            System.Environment.SpecialFolder.Fonts);

                string fileDest = fontsfolder+ "\\" + this.OriginalPath.Substring(this.OriginalPath.LastIndexOf("\\")+1);
                File.Copy(this.OriginalPath,fileDest, true);
                if (File.Exists(fileDest))
                    Console.Write("");*/
                //Load Font in memory
                Uri font = null;
                GlyphTypeface gtf = null;
                try
                {
                    FileInfo info = new FileInfo(this.OriginalPath);

                    font = new Uri(info.FullName);
                    gtf = new GlyphTypeface(font);

                    if (gtf != null)
                    {
                        //Found Name Font for iPhone compatibility
                        foreach (System.Globalization.CultureInfo keys in gtf.FamilyNames.Keys)
                        {
                            this.NameForIphone = gtf.FamilyNames[keys].ToString();

                        }

                        //Load FileName as Android compatibility
                        FileInfo fi = new FileInfo(this.OriginalPath);
                        this.NameForAndroid = fi.Name.Replace(".ttf", "");
                        fi = null;



                        gtf = null;
                        return true;
                    }

                    if (!this.IsFontInstalled())
                    {
                        MessageBox.Show("The True Type Font file \""+ this.NameForIphone+"\" does not seem to be installed on this computer!\n Please close Krea then install the font before using it!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }

                }
                catch
                {
                    MessageBox.Show("The True Type Font file \"" + this.NameForIphone + "\" does not seem to be installed on this computer!\n Please close Krea then install the font before using it!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
               
                
                gtf = null;
                return false;
            }
            return false;
        }

        public String GenerateIphoneProperties(List<FontItem> FontItemList) { 
        
            String resultCode = "\tiphone = {\n"+
                                "\t\tplist = {\n" +
                                "\t\t\tUIAppFonts =\n" +
                                "\t\t\t{\n";
            for(int i=0;i<FontItemList.Count;i++){
                resultCode += "\t\t\t\"" + FontItemList[i].NameForAndroid + "\",\n";
            }
                 resultCode += "\t\t\t}\n" +
                               "\t\t}\n" +
                               "\t},\n";

            return resultCode;
        }
        public String GenerateCode(){
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\tlocal font = \"\"");
            sb.AppendLine("\tif system.getInfo(\"platformName\") == \"Android\" then ");
            sb.AppendLine("\t\tfont=\""+this.NameForAndroid+"\"");
            sb.AppendLine("\telse");
            sb.AppendLine("\t\tfont=\""+this.NameForIphone+"\" end");


            return sb.ToString();
        }


        public bool IsFontInstalled()
        {
            InstalledFontCollection fontCollection = new InstalledFontCollection();


            // Add three font files to the private collection.
            for (int i = 0; i < fontCollection.Families.Length; i++)
            {
                if (fontCollection.Families[i].Name.Equals(this.NameForIphone))
                {
                    fontCollection.Dispose();
                    return true;
                }

            }

            fontCollection.Dispose();
            return false;
        }


        public bool InstallFont()
        {
            if (!this.IsFontInstalled())
            {
                //Install Font on windows machine (Simulator compatibility)
                //Int32 result = AddFontResourceA(this.OriginalPath);

                int Ret;
                int Res;
                const int WM_FONTCHANGE = 0x001D;
                const int HWND_BROADCAST = 0xffff;

                Ret = AddFontResource(this.OriginalPath);
                Res = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);
                Ret = WriteProfileString("fonts", this.NameForIphone + " (TrueType)", this.NameForAndroid);
            }
            return true;
        }

        public bool UninstallFont()
        {
            if (this.IsFontInstalled())
            {
                int Ret = RemoveFontResource(this.OriginalPath);
            }
            return true;
        }

        public override string ToString()
        {
            return this.NameForIphone;
        }

        public FontItem cloneInstance()
        {
            FontItem item = new FontItem();
            item.OriginalPath = this.OriginalPath;
            item.FileName = this.FileName;
            item.NameForAndroid = this.NameForAndroid;
            item.NameForIphone = this.NameForIphone;
            item.FontFamilyName = this.FontFamilyName;
            return item;
        }
    }
}
 