using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Krea.GameEditor.FileExplorerControl
{
    public class FolderExplorerItem : TreeNode
    {

        public DirectoryInfo FolderInfo;
     
        public FolderExplorerItem(DirectoryInfo info)
        {
          
            this.FolderInfo = info;
            this.ImageKey = "folderIcon.png";
            this.SelectedImageKey = "folderIcon.png";
            this.Text = this.FolderInfo.Name;
            this.ForeColor = System.Drawing.Color.DarkBlue;
        }
    }
}
