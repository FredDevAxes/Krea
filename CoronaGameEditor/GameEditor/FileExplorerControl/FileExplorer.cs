using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Krea.GameEditor.FileExplorerControl;
using System.Collections;

namespace Krea.GameEditor
{
    public partial class FileExplorer : UserControl
    {
       

        private Form1 mainForm;
        FolderExplorerItem rootNode;

        public List<TreeNode> SelectedNodes;
        protected TreeNode m_lastNode, m_firstNode;
        private bool isRemovingNode = false;
        public FileExplorer()
        {
            InitializeComponent();
        }

        public void Init(Form1 mainForm)
        {
            this.mainForm = mainForm;
            SelectedNodes = new List<TreeNode>();

             
        }

         
        public void PopulateTreeView()
        {
            try
            {
                this.treeView1.SuspendLayout();
                this.treeView1.Nodes.Clear();
                SelectedNodes.Clear();
                populateTree();
                this.treeView1.Sort();
                this.treeView1.ResumeLayout();
            }
            catch (Exception ex)
            {

            }
           
        }

        private void populateTree()
        {
            if (this.mainForm.CurrentProject != null)
            {
                

                DirectoryInfo info = new DirectoryInfo(this.mainForm.CurrentProject.ProjectPath);
                if (info.Exists)
                {

                    rootNode = new FolderExplorerItem(info);
                    GetDirectories(info.GetDirectories(), rootNode);
                    treeView1.Nodes.Add(rootNode);

                    // Create a new FileSystemWatcher and set its properties.
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Path = this.mainForm.CurrentProject.ProjectPath;
                    /* Watch for changes in LastAccess and LastWrite times, and 
                       the renaming of files or directories. */
                    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                       | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                    watcher.IncludeSubdirectories = true; 
 
                    // Add event handlers.
                    watcher.Created += new FileSystemEventHandler(watcher_Changed);
                    watcher.Deleted += new FileSystemEventHandler(watcher_Changed);
                    watcher.Renamed += new RenamedEventHandler(watcher_Renamed);

                    // Begin watching.
                    watcher.EnableRaisingEvents = true;
                }
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeNode nodeToAddTo)
        {
            FolderExplorerItem aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new FolderExplorerItem(subDir);
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);

                this.getFiles(subDir, aNode);
            }
        }

        private void getFiles(DirectoryInfo parentDir, FolderExplorerItem nodeToAddTo)
        {
            FileExplorerItem aNode;
            FileInfo[] files = parentDir.GetFiles();


            foreach (FileInfo file in files)
            {
                aNode = this.getFileNode(file.FullName);
                if (aNode == null)
                {
                    aNode = new FileExplorerItem(file);
                    nodeToAddTo.Nodes.Add(aNode);
                }
                
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Node is FileExplorerItem)
            {
                FileExplorerItem item = e.Node as FileExplorerItem;
                if (File.Exists(item.FileInfo.FullName))
                {
                    if (item.FileInfo.Extension.Equals(".lua") || item.FileInfo.Extension.Equals(".txt"))
                    {
                        this.mainForm.cgEeditor1.OpenFileInEditor(item.FileInfo.FullName);
                    }
                    else
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(item.FileInfo.FullName);
                        }
                        catch (Exception ex)
                        {
                            this.mainForm.cgEeditor1.OpenFileInEditor(item.FileInfo.FullName);
                        }
                    }


                }
                else
                {
                    e.Node.Remove();

                }
            }
            
            
        }

        private FolderExplorerItem getFolderNode(FolderExplorerItem parentFolder,string path)
        {
            if(this.rootNode != null)
            {
                
                if (parentFolder.FolderInfo.FullName.Equals(path))
                {
                    return parentFolder;
                }

                for (int i = 0; i < parentFolder.Nodes.Count; i++)
                {
                    if (parentFolder.Nodes[i] is FolderExplorerItem)
                    {
                        FolderExplorerItem item = parentFolder.Nodes[i] as FolderExplorerItem;
                        if (item.FolderInfo.FullName.Equals(path))
                        {
                            return item;
                        }
                        else
                        {
                            FolderExplorerItem sub = this.getFolderNode(item, path);
                            if (sub != null)
                                return sub;
                        }
                    }

                }
            }

           
            return null;
        }

        private FileExplorerItem getFileNode(string path)
        {
            if (this.rootNode != null)
            {
                return directoryHasNode(this.rootNode, path);
            }


            return null;
        }

        private FileExplorerItem directoryHasNode(FolderExplorerItem dir,string path)
        {
            if (dir != null)
            {
                for (int i = 0; i < dir.Nodes.Count; i++)
                {
                    if (dir.Nodes[i] is FileExplorerItem)
                    {
                        FileExplorerItem item = dir.Nodes[i] as FileExplorerItem;
                        if (item.FileInfo.FullName.Equals(path))
                        {
                            return item;
                        }
                    }
                    else if (dir.Nodes[i] is FolderExplorerItem)
                    {
                        FolderExplorerItem item = dir.Nodes[i] as FolderExplorerItem;
                        FileExplorerItem itemFound =  directoryHasNode(item,path);
                        if (itemFound != null)
                            return itemFound;
                    }

                }
            }


            return null;
        }
        private FileExplorerItem createFileNode(FileInfo info)
        {
            FileExplorerItem itemExisting = getFileNode(info.FullName);
            if (itemExisting != null)
            {
                itemExisting.FileInfo = info;
                itemExisting.Text = info.Name;
                this.treeView1.Sort();

                return itemExisting;
            }
            else
            {
                FolderExplorerItem folderParent = this.getFolderNode(this.rootNode, info.DirectoryName);
                if (folderParent != null)
                {
                    FileExplorerItem item = new FileExplorerItem(info);
                    folderParent.Nodes.Add(item);
                    this.treeView1.Sort();
                    return item;
                }

                return null;
            }
           
        }

        private FolderExplorerItem createFolderNode(DirectoryInfo info)
        {
            FolderExplorerItem itemExisting = this.getFolderNode(this.rootNode,info.FullName);
            if (itemExisting != null)
            {
                itemExisting.FolderInfo = info;
                itemExisting.Text = info.Name;
                this.treeView1.Sort();
                return itemExisting;
            }
            else
            {
                FolderExplorerItem folderParent = this.getFolderNode(this.rootNode,info.Parent.FullName);
                if (folderParent != null)
                {
                    FolderExplorerItem item = new FolderExplorerItem(info);
                    folderParent.Nodes.Add(item);
                    this.treeView1.Sort();

                    return item;
                }

                return null;
            }


        }

        private bool removeFileNode(string path)
        {
            FileExplorerItem itemExisting = getFileNode(path);
            if (itemExisting != null)
            {
                itemExisting.Remove();
                return true;
            }

            return false;
        }

        private bool removeFolderNode(string path)
        {
            FolderExplorerItem itemExisting = this.getFolderNode(this.rootNode,path);
            if (itemExisting != null)
            {
                itemExisting.Remove();
                return true;
            }
            return false;
        }

        private bool renameFileNode(string oldfullName, FileInfo info)
        {
            FileExplorerItem itemExisting = getFileNode(oldfullName);
            if (itemExisting != null)
            {
                itemExisting.FileInfo = info;
                itemExisting.Text = info.Name;
                this.treeView1.Sort();
                return true;
            }

            return false;
        }

        private bool renameFolderNode(string oldfullName, DirectoryInfo info)
        {
            FolderExplorerItem itemExisting = this.getFolderNode(this.rootNode,oldfullName);
            if (itemExisting != null)
            {

                itemExisting.FolderInfo = info;
                itemExisting.Text = info.Name;
                this.treeView1.Sort();
                return true;
            }

            return false;

        }

        delegate void dwatcher_Changed(object sender, FileSystemEventArgs e);
        public void watcher_Changed(object sender, FileSystemEventArgs args)
        {
            if (this.InvokeRequired)
            {
                dwatcher_Changed d = new dwatcher_Changed(watcher_Changed);
                object[] arguments = { sender, args };
                this.Invoke(d, arguments);
            }
            else
            {
                if (this.mainForm.Enabled == true)
                {
                    if (args.ChangeType == WatcherChangeTypes.Created)
                    {
                        if (Directory.Exists(args.FullPath))
                        {
                            DirectoryInfo folderInfo = new DirectoryInfo(args.FullPath);
                            FolderExplorerItem folderNode = createFolderNode(folderInfo);
                            if (folderNode != null)
                            {

                                GetDirectories(folderInfo.GetDirectories(), folderNode);
                                getFiles(folderInfo, folderNode);
                            }

                        }
                        else
                        {
                            createFileNode(new FileInfo(args.FullPath));
                        }

                    }
                    else if (args.ChangeType == WatcherChangeTypes.Deleted)
                    {
                        bool res = this.removeFileNode(args.FullPath);
                        if (res == false)
                            this.removeFolderNode(args.FullPath);

                    }
                }

               
            }
          
        }

       
     delegate void dwatcher_Renamed(object sender, RenamedEventArgs e);
        public void watcher_Renamed(object sender, RenamedEventArgs args)
        {
            if (this.InvokeRequired)
            {
                dwatcher_Renamed d = new dwatcher_Renamed(watcher_Renamed);
                object[] arguments = { sender, args };
                this.Invoke(d, arguments);
            }
            else
            {
                if (this.mainForm.Enabled == true)
                {
                    if (args.ChangeType == WatcherChangeTypes.Renamed)
                    {
                        string oldPath = args.OldFullPath;
                        string newPath = args.FullPath;
                        if (Directory.Exists(newPath))
                        {
                            renameFolderNode(oldPath, new DirectoryInfo(newPath));

                        }
                        else
                        {
                            renameFileNode(oldPath, new FileInfo(newPath));
                        }

                    }
                }
                
            }

        }


        private void removeFileBt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.SelectedNodes.Count; i++)
            {
                if (this.SelectedNodes[i] is FileExplorerItem)
                {
                    try
                    {
                        FileExplorerItem item = this.SelectedNodes[i] as FileExplorerItem;
                        if (File.Exists(item.FileInfo.FullName))
                            File.Delete(item.FileInfo.FullName);

                        item.Remove();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                   

                    
                }
            }
        }

        private void addFilesBt_Click_1(object sender, EventArgs e)
        {
            if (this.mainForm.CurrentProject != null && this.treeView1.SelectedNode != null && this.treeView1.SelectedNode is FolderExplorerItem)
            {
                try
                {
                    FolderExplorerItem currentFolder = this.treeView1.SelectedNode as FolderExplorerItem;
                    OpenFileDialog openF = new OpenFileDialog();
                    openF.Multiselect = true;
                    openF.Filter = "All files type (*.*)|*.*";
                    if (openF.ShowDialog() == DialogResult.OK)
                    {
                        int count = 0;
                        for (int i = 0; i < openF.FileNames.Length; i++)
                        {
                            string filename = openF.FileNames[i];
                            if (File.Exists(filename))
                            {
                                File.Copy(filename, currentFolder.FolderInfo.FullName + "\\" + openF.SafeFileNames[i], true);

                                FileExplorerItem newItem = new FileExplorerItem(new FileInfo(currentFolder.FolderInfo.FullName + "\\" + openF.SafeFileNames[i]));
                                currentFolder.Nodes.Add(newItem);

                                count++;
                            }
                            else
                            {
                                MessageBox.Show("The file " + filename + " does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        this.treeView1.Sort();
                        if (count > 1)
                            MessageBox.Show(count + " files have been successfully imported!", "Importation succeed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show(count + " file has been successfully imported!", "Importation succeed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during file importation!\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.isRemovingNode == false)
            {

                bool bControl = (ModifierKeys == Keys.Control);
                bool bShift = (ModifierKeys == Keys.Shift);



                m_lastNode = e.Node;
                if (!bShift) m_firstNode = e.Node; // store begin of shift sequence
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected bool isParent(TreeNode parentNode, TreeNode childNode)
        {
            if (parentNode == childNode)
                return true;

            TreeNode n = childNode;
            bool bFound = false;
            while (!bFound && n != null)
            {
                n = n.Parent;
                bFound = (n == parentNode);
            }
            return bFound;
        }


        protected void paintSelectedNodes()
        {
            foreach (TreeNode n in this.SelectedNodes)
            {
                n.BackColor = SystemColors.Highlight;
                n.ForeColor = SystemColors.HighlightText;
            }
        }

        protected void removePaintFromNodes()
        {
            if (SelectedNodes.Count == 0) return;


            Color back = this.treeView1.BackColor;
            Color fore = this.treeView1.ForeColor;

            foreach (TreeNode n in SelectedNodes)
            {
                if (n is GameElement)
                {
                    GameElement elem = n as GameElement;
                    fore = elem.getFontColor();
                }
                n.BackColor = back;
                n.ForeColor = fore;
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // e.Node is the current node exposed by the base TreeView control

            bool bControl = (ModifierKeys == Keys.Control);
            bool bShift = (ModifierKeys == Keys.Shift);

            removePaintFromNodes();

            if (e.Node is FileExplorerItem)
            {
                FileExplorerItem elem = e.Node as FileExplorerItem;
                bool isFirstFileItem = true;
                for (int i = 0; i < this.SelectedNodes.Count; i++)
                {
                    if (this.SelectedNodes[i] is FileExplorerItem)
                    {
                        
                    }
                    else
                    {
                        isFirstFileItem = false;
                        break;
                    }
                }

                if (isFirstFileItem == true)
                {
                    if (bControl)
                    {
                        if (!this.SelectedNodes.Contains(e.Node)) // new node ?
                        {
                            this.SelectedNodes.Add(e.Node);
                        }

                    }
                    else
                    {
                        if (bShift)
                        {
                            Queue myQueue = new Queue();

                            TreeNode uppernode = m_firstNode;
                            TreeNode bottomnode = e.Node;

                            // case 1 : begin and end nodes are parent
                            bool bParent = isParent(m_firstNode, e.Node);
                            if (!bParent)
                            {
                                bParent = isParent(bottomnode, uppernode);
                                if (bParent) // swap nodes
                                {
                                    TreeNode t = uppernode;
                                    uppernode = bottomnode;
                                    bottomnode = t;
                                }
                            }
                            if (bParent)
                            {
                                TreeNode n = bottomnode;
                                while (n != uppernode.Parent)
                                {
                                    if (!this.SelectedNodes.Contains(n)) // new node ?
                                        myQueue.Enqueue(n);

                                    n = n.Parent;
                                }
                            }
                            // case 2 : nor the begin nor the
                            // end node are descendant one another
                            else
                            {
                                // are they siblings ?                 

                                if ((uppernode.Parent == null && bottomnode.Parent == null)
                                      || (uppernode.Parent != null &&
                                      uppernode.Parent.Nodes.Contains(bottomnode)))
                                {
                                    int nIndexUpper = uppernode.Index;
                                    int nIndexBottom = bottomnode.Index;
                                    if (nIndexBottom < nIndexUpper) // reversed?
                                    {
                                        TreeNode t = uppernode;
                                        uppernode = bottomnode;
                                        bottomnode = t;
                                        nIndexUpper = uppernode.Index;
                                        nIndexBottom = bottomnode.Index;
                                    }

                                    TreeNode n = uppernode;
                                    while (nIndexUpper <= nIndexBottom)
                                    {
                                        if (!this.SelectedNodes.Contains(n)) // new node ?
                                            myQueue.Enqueue(n);

                                        n = n.NextNode;

                                        nIndexUpper++;
                                    } // end while

                                }
                                else
                                {
                                    if (!this.SelectedNodes.Contains(uppernode))
                                        myQueue.Enqueue(uppernode);
                                    if (!this.SelectedNodes.Contains(bottomnode))
                                        myQueue.Enqueue(bottomnode);
                                }

                            }

                            foreach (object objQueue in myQueue)
                            {
                                TreeNode node = objQueue as TreeNode;
                                this.SelectedNodes.Add(node);
                            }



                            // let us chain several SHIFTs if we like it
                            m_firstNode = e.Node;

                        } // end if m_bShift
                        else
                        {
                            // in the case of a simple click, just add this item
                            if (this.SelectedNodes != null && this.SelectedNodes.Count > 0)
                            {
                                removePaintFromNodes();
                                this.SelectedNodes.Clear();
                            }
                            this.SelectedNodes.Add(e.Node);
                        }
                    }

                }
                else
                {
                    this.SelectedNodes.Clear();
                    this.SelectedNodes.Add(e.Node);

                    
                }

            }
            else
            {
                this.SelectedNodes.Clear();
                this.SelectedNodes.Add(e.Node);
            }



            paintSelectedNodes();
           
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.Node is FileExplorerItem)
                {
                    this.treeView1.ContextMenuStrip = this.fileMenuStrip;
                    this.fileMenuStrip.Show();
                }
                else
                {
                    this.treeView1.ContextMenuStrip = this.folderMenuStrip;
                    this.folderMenuStrip.Show();
                }
            }

        }

        private void refreshBt_Click(object sender, EventArgs e)
        {
            this.PopulateTreeView();
        }
    }
}
