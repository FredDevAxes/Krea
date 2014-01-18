using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Krea.GameEditor.FileExplorerControl
{
    public class FileExplorerItem : TreeNode
    {

        public FileInfo FileInfo;
        public FileExplorerItem(FileInfo info)
        {

            this.FileInfo = info;
            string ext = this.FileInfo.Extension;

            if (ext.Equals(".png"))
            {
                this.ImageKey = "png.png";
                this.SelectedImageKey = "png.png";
            }
            else if (ext.Equals(".jpg"))
            {
                this.ImageKey = "jpg.png";
                this.SelectedImageKey = "jpg.png";
            }
            else if (ext.Equals(".gif"))
            {
                this.ImageKey = "gif.png";
                this.SelectedImageKey = "gif.png";
            }
            else if (ext.Equals(".wav") || ext.Equals(".ogg")||ext.Equals(".mp3"))
            {
                this.ImageKey = "audioIcon.png";
                this.SelectedImageKey = "audioIcon.png";
            }
            else if (ext.Equals(".bmp"))
            {
                this.ImageKey = "bmpFileIcon.png";
                this.SelectedImageKey = "bmpFileIcon.png";
            }
            else if (ext.Equals(".xls"))
            {
                this.ImageKey = "excel.png";
                this.SelectedImageKey = "excel.png";
            }
            else if (ext.Equals(".exe"))
            {
                this.ImageKey = "exe.png";
                this.SelectedImageKey = "exe.png";
            }
            else if (ext.Equals(".fla"))
            {
                this.ImageKey = "flash.png";
                this.SelectedImageKey = "flash.png";
            }
            else if (ext.Equals(".pdf"))
            {
                this.ImageKey = "pdf.png";
                this.SelectedImageKey = "pdf.png";
            }
            else if (ext.Equals(".ppt"))
            {
                this.ImageKey = "powerpoint.png";
                this.SelectedImageKey = "powerpoint.png";
            }
            else if (ext.Equals(".psd"))
            {
                this.ImageKey = "psd.png";
                this.SelectedImageKey = "psd.png";
            }
            else if (ext.Equals(".txt") || ext.Equals(".lua"))
            {
                this.ImageKey = "textFileIcon.png";
                this.SelectedImageKey = "textFileIcon.png";
            }
            else if (ext.Equals(".ttf"))
            {
                this.ImageKey = "ttf.png";
                this.SelectedImageKey = "ttf.png";
            }
            else if (ext.Equals(".mov") || ext.Equals(".mp4") || ext.Equals(".m4v") || ext.Equals(".3gp"))
            {
                this.ImageKey = "videoFileIcon.png";
                this.SelectedImageKey = "videoFileIcon.png";
            }
            else if (ext.Equals(".doc") || ext.Equals(".docx"))
            {
                this.ImageKey = "word.png";
                this.SelectedImageKey = "word.png";
            }
            else if (ext.Equals(".zip"))
            {
                this.ImageKey = "zip.png";
                this.SelectedImageKey = "zip.png";
            }
            else if (ext.Equals(".dll"))
            {
                this.ImageKey = "settings.png";
                this.SelectedImageKey = "settings.png";
            }
            else
            {
                this.ImageKey = "plainFileIcon.png";
                this.SelectedImageKey = "plainFileIcon.png";
            }

            this.Text = this.FileInfo.Name;
            this.ForeColor = System.Drawing.Color.Black;
        }
    }
}
