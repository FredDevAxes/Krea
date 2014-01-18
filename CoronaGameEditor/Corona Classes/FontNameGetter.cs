using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Media;
using Krea.CGE_Figures;

namespace Krea.Corona_Classes
{
    public class FontNameGetter
    {
        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        struct TT_OFFSET_TABLE
        {
            public ushort uMajorVersion;
            public ushort uMinorVersion;
            public ushort uNumOfTables;
            public ushort uSearchRange;
            public ushort uEntrySelector;
            public ushort uRangeShift;

        }
        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        struct TT_TABLE_DIRECTORY
        {
            public char szTag1;
            public char szTag2;
            public char szTag3;
            public char szTag4;
            public uint uCheckSum; //Check sum
            public uint uOffset; //Offset from beginning of file
            public uint uLength; //length of the table in bytes
        }
        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        struct TT_NAME_TABLE_HEADER
        {
            public ushort uFSelector;
            public ushort uNRCount;
            public ushort uStorageOffset;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        struct TT_NAME_RECORD
        {
            public ushort uPlatformID;
            public ushort uEncodingID;
            public ushort uLanguageID;
            public ushort uNameID;
            public ushort uStringLength;
            public ushort uStringOffset;
        }
        private TT_OFFSET_TABLE ttOffsetTable;
        private TT_TABLE_DIRECTORY tblDir;
        private TT_NAME_TABLE_HEADER ttNTHeader;
        private TT_NAME_RECORD ttNMRecord;


        public FontNameGetter()
        {
        }

        public string GetFontFileName(DirectoryInfo dir, Font2 fontNameToSearch)
        {
            string fileNameToReturn = "";

            FileInfo[] files = dir.GetFiles("*.ttf");

            foreach (FileInfo file in files)
            {
                GlyphTypeface gtf = new GlyphTypeface(new Uri(file.FullName));
                //Found Name Font for iPhone compatibility
                
                foreach (System.Globalization.CultureInfo keys in gtf.FamilyNames.Keys)
                {
                    string name = gtf.FamilyNames[keys].ToString();
                    if (name.Equals(fontNameToSearch.FamilyName))
                    {
                        string styleName = fontNameToSearch.Style.ToString();
                        if (gtf.FaceNames.Values.Contains(styleName))
                            return file.FullName;
                        
                        
                    }
                       
                }

            }
            /*foreach (FileInfo file in files)
            {
                FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);
                byte[] buff = r.ReadBytes(Marshal.SizeOf(ttOffsetTable));
                buff = BigEndian(buff);
                IntPtr ptr = Marshal.AllocHGlobal(buff.Length);
                Marshal.Copy(buff, 0x0, ptr, buff.Length);
                TT_OFFSET_TABLE ttResult = (TT_OFFSET_TABLE)Marshal.PtrToStructure(ptr, typeof(TT_OFFSET_TABLE));
                Marshal.FreeHGlobal(ptr);

                //Must be maj =1 minor = 0
                if (ttResult.uMajorVersion != 1 || ttResult.uMinorVersion != 0)
                {
                    r.Close();
                    r.Dispose();
                    fs.Close();
                    fs.Dispose();
                    continue;
                }
                   

                bool bFound = false;
                TT_TABLE_DIRECTORY tbName = new TT_TABLE_DIRECTORY();
                for (int i = 0; i < ttResult.uNumOfTables; i++)
                {
                    byte[] bNameTable = r.ReadBytes(Marshal.SizeOf(tblDir));
                    IntPtr ptrName = Marshal.AllocHGlobal(bNameTable.Length);
                    Marshal.Copy(bNameTable, 0x0, ptrName, bNameTable.Length);
                    tbName = (TT_TABLE_DIRECTORY)Marshal.PtrToStructure(ptrName, typeof(TT_TABLE_DIRECTORY));
                    Marshal.FreeHGlobal(ptrName);
                    string szName = tbName.szTag1.ToString() + tbName.szTag2.ToString() + tbName.szTag3.ToString() + tbName.szTag4.ToString();
                    if (szName != null)
                    {
                        if (szName.ToString() == "name")
                        {
                            bFound = true;
                            byte[] btLength = BitConverter.GetBytes(tbName.uLength);
                            byte[] btOffset = BitConverter.GetBytes(tbName.uOffset);
                            Array.Reverse(btLength);
                            Array.Reverse(btOffset);
                            tbName.uLength = BitConverter.ToUInt32(btLength, 0);
                            tbName.uOffset = BitConverter.ToUInt32(btOffset, 0);
                            break;
                        }
                    }
                }
                if (bFound)
                {
                    fs.Position = tbName.uOffset;
                    byte[] btNTHeader = r.ReadBytes(Marshal.SizeOf(ttNTHeader));
                    btNTHeader = BigEndian(btNTHeader);
                    IntPtr ptrNTHeader = Marshal.AllocHGlobal(btNTHeader.Length);
                    Marshal.Copy(btNTHeader, 0x0, ptrNTHeader, btNTHeader.Length);
                    TT_NAME_TABLE_HEADER ttNTResult = (TT_NAME_TABLE_HEADER)Marshal.PtrToStructure(ptrNTHeader, typeof(TT_NAME_TABLE_HEADER));
                    Marshal.FreeHGlobal(ptrNTHeader);
                    bFound = false;
                    for (int i = 0; i < ttNTResult.uNRCount; i++)
                    {
                        byte[] btNMRecord = r.ReadBytes(Marshal.SizeOf(ttNMRecord));
                        btNMRecord = BigEndian(btNMRecord);
                        IntPtr ptrNMRecord = Marshal.AllocHGlobal(btNMRecord.Length);
                        Marshal.Copy(btNMRecord, 0x0, ptrNMRecord, btNMRecord.Length);
                        TT_NAME_RECORD ttNMResult = (TT_NAME_RECORD)Marshal.PtrToStructure(ptrNMRecord, typeof(TT_NAME_RECORD));
                        Marshal.FreeHGlobal(ptrNMRecord);
                        if (ttNMResult.uNameID == 1)
                        {
                            long fPos = fs.Position;
                            fs.Position = tbName.uOffset + ttNMResult.uStringOffset + ttNTResult.uStorageOffset;
                            char[] szResult = r.ReadChars(ttNMResult.uStringLength);
                            if (szResult.Length != 0)
                            {
                                string name = new string(szResult);
                                if (name.Equals(fontNameToSearch))
                                {
                                    int y = 0;//szResult now contains the font name.

                                    r.Close();
                                    r.Dispose();
                                    fs.Close();
                                    fs.Dispose();

                                    return file.FullName;
                                }
                            }
                        }
                    }
                }

                r.Close();
                r.Dispose();
                fs.Close();
                fs.Dispose();

            }*/

            return fileNameToReturn;
        }
        private byte[] BigEndian(byte[] bLittle)
        {
            byte[] bBig = new byte[bLittle.Length];
            for (int y = 0; y < (bLittle.Length - 1); y += 2)
            {
                byte b1, b2;
                b1 = bLittle[y];
                b2 = bLittle[y + 1];
                bBig[y] = b2;
                bBig[y + 1] = b1;
            }
            return bBig;
        }
    }
}

