using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Krea
{
    [Serializable()]
    public class KreaProjectVersion
    {
        public Int32 Major;
        public Int32 Minor;
        public Int32 Build;
        public Boolean isValid;

        public KreaProjectVersion() { }
        public KreaProjectVersion(string path)
        {
            StreamReader reader = new StreamReader(path);
            String result = reader.ReadToEnd();
            reader.Close();
            string[] result_cut = result.Split('.');
            if (result_cut.Length == 3)
            {
                if (Int32.TryParse(result_cut[0], out this.Major))
                {
                    if (Int32.TryParse(result_cut[1], out this.Minor))
                    {
                        if (Int32.TryParse(result_cut[2], out this.Build))
                        {
                            isValid = true;
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                this.Major = 0;
                this.Minor = 0;
                this.Build = 0;
                this.isValid = true;
            }
        }
        public KreaProjectVersion(String Major, String Minor, String Build)
        {

            if (Int32.TryParse(Major, out this.Major))
            {
                if (Int32.TryParse(Minor, out this.Minor))
                {
                    if (Int32.TryParse(Build, out this.Build))
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }
            }
            else {
                isValid = false;
            }
        }
        public KreaProjectVersion initFromString(String version)
        {
            string[] result_cut = version.Split('.');
            if (result_cut.Length == 3)
            {
                return new KreaProjectVersion(result_cut[0], result_cut[1], result_cut[2]);
            }
            else
            {
                return new KreaProjectVersion("0", "0", "0");
            }
        }
        public Boolean isNewer(KreaProjectVersion oldVersion)
        {


            if (this.Major > oldVersion.Major) return true;
            else if (this.Major < oldVersion.Major) return false;

            else if (this.Minor > oldVersion.Minor) return true;
            else if (this.Minor < oldVersion.Minor) return false;

            else if (this.Build > oldVersion.Build) return true;
            else if (this.Build < oldVersion.Build) return false;

            else
                return false;

            
        }

        public String ToString() {
            return Major.ToString() + "." + Minor.ToString() + "." + Build.ToString();
        }

    }
}
