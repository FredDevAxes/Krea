using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Globalization;
using System.Diagnostics;

using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
using Krea;
using ScintillaNet;
using System.Text.RegularExpressions;
using Krea.CoronaClasses;
using Krea.GameEditor.CodeEditor;
using Krea.Debugger;
using System.Runtime.InteropServices;
using System.Xml;

namespace Krea
{
    public partial class CGEeditor : UserControl
    {
        #region Constants

        private const string NEW_DOCUMENT_TEXT = "Untitled";
        private const int LINE_NUMBERS_MARGIN_WIDTH = 35; // TODO Don't hardcode this

        #endregion Constants

        #region Fields

        Form1 mainForm;
        public int _newDocumentCount = 0;
        private string[] _args;
        private int _zoomLevel;
        private bool autoShowComplete = false;
        public Scene sceneSelected ;
        public bool IsFocused = false;
        public string Mode = "EDITOR";
        public AutoCompletionEngine AutoCompletionEngine;
        private bool isDebugTraceMarkerChanged = false;
        private List<Snippet> codeEditorSnippets;
        private DataTip dataTipForm;
        #endregion Fields
        
        #region Properties

        public DocumentForm ActiveDocument
        {
            get
            {
                return dockPanel.ActiveDocument as DocumentForm;
            }
        }

        #endregion Properties


        #region File Menu Handlers

        [DllImport("rdbglua51.dll", CharSet = CharSet.Ansi,CallingConvention = CallingConvention.Cdecl)]
        private static extern bool CheckLuaScriptSyntax(StringBuilder script, StringBuilder scriptName, StringBuilder errMsg, int errMsgLen);
        

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                 NewDocument();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                 OpenFile();
        }

        public StringBuilder checkLua(string code, string codeName)
        {
            try
            {
                StringBuilder script = new StringBuilder(code);
                StringBuilder scriptName = new StringBuilder(codeName);
                StringBuilder err = new StringBuilder(2048);

                if (CheckLuaScriptSyntax(script, scriptName, err, err.Capacity))
                {
                    return err;
                }
                return new StringBuilder("No Syntax error!");
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public void setDocumentsReadOnlyState(bool state)
        {
            foreach (DocumentForm doc in this.dockPanel.Documents)
            {
                doc.Scintilla.IsReadOnly = state;
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                {
                    if (this.ActiveDocument.Text.Equals("snippets.lua *"))
                    {
                        saveCurrentSnippetsFile();
                    }
               
                    ActiveDocument.Save();

                    this.refreshFunctionsNavigationContent(this.ActiveDocument.Scintilla);
                    if (this.luaSyntaxCheckerBt.Checked == true)
                        checkLuaSyntax();
                }
                
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
            {
                if (ActiveDocument != null)
                    ActiveDocument.SaveAs();

                if (this.luaSyntaxCheckerBt.Checked == true)
                    checkLuaSyntax();
            }
               
        }

        private void saveAllStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                foreach (DocumentForm doc in dockPanel.Documents)
                {
                    doc.Activate();



                    doc.Save();
                }

            if (this.luaSyntaxCheckerBt.Checked == true)
                checkLuaSyntax();

        }

        private void exportAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.ExportAsHtml();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.Close();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.Scintilla.Printing.Print();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.Scintilla.Printing.PrintPreview();
        }


        #endregion File Menu Handlers

        #region Edit Menu Handlers

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.Scintilla.UndoRedo.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.Scintilla.UndoRedo.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.Scintilla.Clipboard.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                    ActiveDocument.Scintilla.Clipboard.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                if (ActiveDocument != null)
                {
                    ActiveDocument.Scintilla.Clipboard.Paste();

                    this.refreshFunctionsNavigationContent(this.ActiveDocument.Scintilla);
                    if (this.luaSyntaxCheckerBt.Checked == true)
                        checkLuaSyntax();
                }
                   
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
            {
                if (ActiveDocument != null)
                    ActiveDocument.Scintilla.Selection.SelectAll();
            }
            else
               this.mainForm.sceneEditorView1.dispatchKeyEvent(new KeyEventArgs((Keys.Control |Keys.A)));
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                ActiveDocument.Scintilla.FindReplace.ShowFind();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                ActiveDocument.Scintilla.FindReplace.ShowReplace();
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
                ActiveDocument.Scintilla.GoTo.ShowGoToDialog();
        }

        private void toggleBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
            {
                Line currentLine = ActiveDocument.Scintilla.Lines.Current;
                if (ActiveDocument.Scintilla.Markers.GetMarkerMask(currentLine) == 1)
                {
                    currentLine.AddMarker(1);
                }
                else
                {
                    currentLine.DeleteMarker(1);
                }
            }
           
        }

        
        private void previosBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
            {

                //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
                Line l = ActiveDocument.Scintilla.Lines.Current.FindPreviousMarker(1);
                if (l != null)
                    l.Goto();
            }
        }

        private void nextBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
            {

                //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
                Line l = ActiveDocument.Scintilla.Lines.Current.FindNextMarker(1);
                if (l != null)
                    l.Goto();
            }
        }

        private void clearBookmarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFocused == true)
            {

                ActiveDocument.Scintilla.Markers.DeleteAll(0);
            }

        }

        private void dropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.DropMarkers.Drop();
        }

        private void collectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.DropMarkers.Collect();
        }

        private void makeUpperCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.UpperCase);
        }

        private void makeLowerCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.LowerCase);
        }

        private void commentStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.StreamComment);
        }

        private void commentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.LineComment);
        }

        private void uncommentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.LineUncomment);
        }

        private void autocompleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

         //   ActiveDocument.Scintilla.AutoComplete.Show();
        }

        private void insertSnippetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Snippets.ShowSnippetList();
        }

        private void surroundWithToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Snippets.ShowSurroundWithList();
        }

        #endregion Edit Menu Handlers


        #region View Menu Handlers


        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Increase the zoom for all open files
            _zoomLevel++;
            UpdateAllScintillaZoom();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _zoomLevel--;
            UpdateAllScintillaZoom();
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _zoomLevel = 0;
            UpdateAllScintillaZoom();
        }


        private void foldLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Lines.Current.FoldExpanded = true;
        }

        private void unfoldLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Lines.Current.FoldExpanded = false;
        }

        private void foldAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
            {
                foreach (Line l in ActiveDocument.Scintilla.Lines)
                {
                    l.FoldExpanded = true;
                }
            }
        }

        private void unfoldAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
            {
                foreach (Line l in ActiveDocument.Scintilla.Lines)
                {
                    l.FoldExpanded = true;
                }
            }
        }

        private void navigateForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.DocumentNavigation.NavigateForward();
        }

        private void navigateBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.DocumentNavigation.NavigateBackward();
        }

        #endregion View Menu Handlers

     
        public CGEeditor()
        {
            InitializeComponent();
            dataTipForm = new DataTip();

        }

        public void Init(Form1 MainForm)
        {
            if(MainForm!=null)
                this.mainForm = MainForm;

            codeEditorSnippets = new List<Snippet>();
            refreshAvailableCodeEditorSnippets();
            AutoCompletionEngine = new AutoCompletionEngine(this);
            AutoCompletionEngine.refreshStaticAPI();
            this.codeCheckerListView.Visible = false;
        }

        private void refreshAvailableCodeEditorSnippets()
        {
            string xmlFilePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\CodeEditorSnippets.xml";
            if (File.Exists(xmlFilePath))
            {
                //Clear all lists
                this.codeEditorSnippets.Clear();

                XmlTextReader reader = new XmlTextReader(xmlFilePath);

                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                reader.Close();

                for (int i = 0; i < doc.ChildNodes.Count; i++)
                {
                    XmlNode node = doc.ChildNodes[i];
                    for (int j = 0; j < node.ChildNodes.Count; j++)
                    {
                        XmlNode nodeSnippet = node.ChildNodes[j];

                        string snippetName = nodeSnippet.Name;
                        string shortCutStr = "";
                        string codeStr = "";
                        for (int k = 0; k < nodeSnippet.ChildNodes.Count; k++)
                        {
                            
                            XmlNode nodeAttribute = nodeSnippet.ChildNodes[k];
                            if (k == 0)
                                shortCutStr = nodeAttribute.InnerText;
                            else if (k == 1)
                                codeStr = nodeAttribute.InnerText;

                        }

                        Snippet snippet = new Snippet(shortCutStr, codeStr);
                        this.codeEditorSnippets.Add(snippet);
                    }
                }

                doc = null;
            }


            foreach (DocumentForm documentForm in dockPanel.Documents)
            {
                documentForm.Scintilla.Snippets.List.Clear();

                documentForm.Scintilla.Snippets.List.AddRange(this.codeEditorSnippets);
            }
        }

        public void saveCurrentSnippetsFile()
        {
            CoronaGameProject project = this.mainForm.CurrentProject;
            if (project != null)
            {
                if (this.ActiveDocument != null)
                {
                    if (this.ActiveDocument.Text.Equals("snippets.lua *"))
                    {
                        
                        string editorText = this.ActiveDocument.Scintilla.Text;

                        int snippetCount = 0;
                        int currentIndex = 0;
                        bool endOfFile = false;
                        while (endOfFile == false)
                        {
                            int indexDebutSnippet = editorText.IndexOf("--##### Snippet Begin #####", currentIndex);
                            if (indexDebutSnippet > -1)
                            {

                                int indexDebutFunction = editorText.IndexOf("--## Function Begin ##", indexDebutSnippet);
                                if (indexDebutFunction > -1)
                                {
                                    int indexFinFunction = editorText.IndexOf("--## Function End ##", indexDebutFunction);
                                    if (indexFinFunction > -1)
                                    {
                                        string function = editorText.Substring(indexDebutFunction + "--## Function Begin ##".Length +2,
                                                                (indexFinFunction-2) - (indexDebutFunction  + "--## Function Begin ##".Length +2));


                                        if (project.Snippets.Count > snippetCount)
                                        {
                                            Corona_Classes.Snippet snippet = project.Snippets[snippetCount];
                                            snippet.Function = function;
                                            snippetCount += 1;
                                        }
                                        else
                                            endOfFile = true;
                                    }
                                }

                                currentIndex = indexDebutSnippet + "--##### Snippet Begin #####".Length;

                                 
                            }
                            else
                                endOfFile = true;
                        }

                    }
                }
            }
           
        }

        public void closeAll(bool save)
        {
            List<DocumentForm> docs = new List<DocumentForm>();

            foreach (DocumentForm documentForm in dockPanel.Documents)
            {
                
                if(save == true)
                    documentForm.Save();

                docs.Add(documentForm);
            }

            for(int i = 0;i< docs.Count;i++)
            {
                docs[i].Close();
            }

            docs = null;

        }

        public void RefreshSnippetLuaCode(CoronaGameProject project)
        {
            try
            {

                if (!File.Exists(project.SourceFolderPath + "\\snippets.lua"))
                {
                    FileStream fs = File.Create(project.SourceFolderPath + "\\snippets.lua");
                    fs.Close();

                } 

                OpenFileInEditor(project.SourceFolderPath + "\\snippets.lua");

                this.saveCurrentSnippetsFile();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("local snippets = {}\n");
                for (int i = 0; i < project.Snippets.Count; i++)
                {
                    Corona_Classes.Snippet snippet = project.Snippets[i];
                    sb.AppendLine("--##### Snippet Begin #####");
                    sb.AppendLine("--## Name: " + snippet.Name);
                    sb.AppendLine("--## Category: " + snippet.Category);
                    sb.AppendLine("--## Author: " + snippet.Author);
                    sb.AppendLine("--## Description: " + snippet.Description);
                    sb.AppendLine("--## Function Begin ##");
                    sb.AppendLine(snippet.Function);
                    sb.AppendLine("--## Function End ##");
                    sb.AppendLine("snippets." + snippet.Name + " = " + snippet.Name);
                    sb.AppendLine("--##### Snippet End #####");
                    sb.Append("\n\n");
                }

                sb.AppendLine("return snippets");
                this.ActiveDocument.Scintilla.Text = sb.ToString();
                this.ActiveDocument.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void selectSceneFileTab(Scene scene)
        {
            foreach(DocumentForm doc in this.dockPanel.Documents)
            {
                if(doc.Text.Equals(scene.Name +".lua"))
                {
                    doc.Select();
                    doc.Show(dockPanel);
                    break;
                }
            }
        }

        //------------------------------ Change made to scene.lua files (initializeComponents become class method of "scene")
        public void checkIntegrityLuaSceneFileFromV1_14ToV1_15(DocumentForm doc)
        {
            if (doc != null)
            {
                string case1ToFind = "function initializeComponents(localGroup)";
                string case1ToReplace = "function scene:initializeComponents()\n\t local localGroup = self.view\n";

                doc.Scintilla.Text = doc.Scintilla.Text.Replace(case1ToFind, case1ToReplace);

                //-----

                string case2ToFind = "initializeComponents(group)";
                string case2ToReplace = "self:initializeComponents()";

                doc.Scintilla.Text = doc.Scintilla.Text.Replace(case2ToFind, case2ToReplace);
            }
        }
        
        public void RefreshSceneLuaCode(Scene scene,bool isCustomBuild,float XRatio, float YRatio)
        {
            this.sceneSelected = scene;
            try
            {
                OpenFileInEditor(scene.projectParent.SourceFolderPath + "\\" + scene.Name + ".lua");
                if (this.ActiveDocument != null && scene != null)
                {

                    this.ActiveDocument.Scintilla.IsReadOnly = false;
                    this.ActiveDocument.Scintilla.UndoRedo.IsUndoEnabled = false;

                    scene.prepareForBuild();
                    //Enregistrer la lingne courante
                    int currentLineNumber = this.ActiveDocument.Scintilla.CurrentPos;
                    
                    //--- Get all breakPoints
                    List<int> markedLines = new List<int>();
                    foreach (Line line in this.ActiveDocument.Scintilla.Lines)
                    {
                        int mask = this.ActiveDocument.Scintilla.Markers.GetMarkerMask(line);
                        if (mask != 0)
                        {
                            markedLines.Add(line.Number);
                        }

                    }


                    this.BuildRequires(scene.generateRequiresLua());
                    
                    //this.BuildGlobalVars(scene.generateGlobalVarsLua());

                    String codeDeclarationsVariables = scene.generateDeclarationsLua(XRatio, YRatio) + "\n" + scene.generateLocalVarsLua();
                    this.BuildLocalVars(codeDeclarationsVariables);

                    //Build the create scene (disp objects)
                    String codeInitializeComponents = scene.generateInitializeComponentLua(isCustomBuild,XRatio, YRatio);
                    this.BuildInitializeComponent(codeInitializeComponents);


                    //Build the  Will enter scene ( timers, listeners, generators ...)
                    String codeWillEnterScene = scene.buildWillEnterScene(XRatio,YRatio);
                    this.BuildWillEnterScene(codeWillEnterScene);

                    //Build the Did exit scene ( timers, listeners, generators ...)
                    String codeDidExitScene = scene.buildDidExitScene();
                    this.BuildDidExitScene(codeDidExitScene);

                    //Build the destroy scene (disp objects)
                    String codeCleanScene = scene.buildCleanScene();
                    this.BuildDestroyScene(codeCleanScene);

                    this.ActiveDocument.Scintilla.CurrentPos = currentLineNumber;

                    //Reinstaller les breakpoints
                    for (int i = 0; i < markedLines.Count; i++)
                    {
                        this.ActiveDocument.Scintilla.Lines[markedLines[i]].AddMarker(0);
                    }

                    this.ActiveDocument.Save();

                    this.ActiveDocument.Scintilla.UndoRedo.IsUndoEnabled = true;

                    refreshFunctionsNavigationContent(this.ActiveDocument.Scintilla);
                    this.AutoCompletionEngine.RefreshDynamicList();

                    if (this.luaSyntaxCheckerBt.Checked == true)
                        checkLuaSyntax();

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during lua code generation!\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
              
            }
        }

        private void CGEeditor_Load(object sender, EventArgs e)
        {
            if (_args != null && _args.Length != 0)
            {
                // Open the document specified on the command line
                FileInfo fi = new FileInfo(_args[0]);
                if (fi.Exists)
                    OpenFile(fi.FullName);
            }
           
        }

        private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
        {

        }
        #region Methods

        private DocumentForm NewDocument()
        {
            DocumentForm doc = new DocumentForm();

            doc.Text = String.Format(CultureInfo.CurrentCulture, "{0}{1}", NEW_DOCUMENT_TEXT, ++_newDocumentCount);

            doc.Scintilla.CharAdded += new EventHandler<CharAddedEventArgs>(this.sciDocument_CharAdded);
            doc.Scintilla.TextDeleted += new EventHandler<TextModifiedEventArgs>(Scintilla_TextDeleted);
            doc.Scintilla.AutoCompleteAccepted += new EventHandler<AutoCompleteAcceptedEventArgs>(Scintilla_AutoCompleteAccepted);

            doc.Scintilla.UndoRedo.EmptyUndoBuffer();
            doc.Scintilla.Modified = false;
            doc.Show(dockPanel);


          
            return doc;
        }

        private void OpenLuaFile(String FilePath)
        {
            if (File.Exists(FilePath))
            {

                    // Ensure this file isn't already open
                    bool isOpen = false;
                    foreach (DocumentForm documentForm in dockPanel.Documents)
                    {
                        if (FilePath.Equals(documentForm.FilePath, StringComparison.OrdinalIgnoreCase))
                        {
                            documentForm.Select();
                            isOpen = true;
                            break;
                        }
                    }

                    // Open the files
                    if (!isOpen)
                        OpenFile(FilePath);
                
            }
        }

        private void OpenFile()
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            foreach (string filePath in openFileDialog.FileNames)
            {
                // Ensure this file isn't already open
                bool isOpen = false;
                foreach (DocumentForm documentForm in dockPanel.Documents)
                {
                    if (filePath.Equals(documentForm.FilePath, StringComparison.OrdinalIgnoreCase))
                    {
                        documentForm.Select();
                        isOpen = true;
                        break;
                    }
                }

                // Open the files
                if (!isOpen)
                    OpenFile(filePath);
            }
        }

        public delegate void SetDebuggerAtLineDelegate(int Position);
        public void SetDebuggerAtLine(int Position)
        {
            if (this.InvokeRequired)
            {
                // this is worker thread     
                this.Invoke(new SetDebuggerAtLineDelegate(this.SetDebuggerAtLine), Position);
            }
            else
            {
                
                if (this.ActiveDocument != null)
                {
                    isDebugTraceMarkerChanged = true;
                    this.ActiveDocument.Scintilla.Markers.DeleteAll(2);
                    isDebugTraceMarkerChanged = false;
                    if (Position >= 0)
                    {
                        isDebugTraceMarkerChanged = true;
                        MarkerInstance marker = this.ActiveDocument.Scintilla.Lines[Position].AddMarker(2);
                        isDebugTraceMarkerChanged = false;
                        marker.Marker.Symbol = MarkerSymbol.Background;
                        marker.Marker.BackColor = Color.Yellow;
                        this.ActiveDocument.Scintilla.GoTo.Line(Position+1);


                        /*int pos = this.ActiveDocument.Scintilla.Lines[Position].EndPosition;
                        this.ActiveDocument.Scintilla.Caret.Goto(pos);
                        this.ActiveDocument.Scintilla.Caret.CurrentLineBackgroundColor = Color.Yellow;
                        this.ActiveDocument.Scintilla.Caret.BringIntoView();*/
                    }

                    this.ActiveDocument.Scintilla.Refresh();
                }

                
            }
        }

        public delegate void OpenFileInEditorDelegate(string filePath);
        public void OpenFileInEditor(string filePath)
        {
            if (this.InvokeRequired)
            {
                // this is worker thread     
                this.Invoke(new OpenFileInEditorDelegate(this.OpenFileInEditor), filePath);
            }
            else
            {
                // Ensure this file isn't already open
                bool isOpen = false;
                foreach (DocumentForm documentForm in dockPanel.Documents)
                {
                    if (filePath.Equals(documentForm.FilePath, StringComparison.OrdinalIgnoreCase))
                    {
                        documentForm.Select();
                        documentForm.Show(dockPanel);
                        isOpen = true;
                        if (this.Mode.Equals("DEBUG"))
                            documentForm.Scintilla.IsReadOnly = true;
                        else
                            documentForm.Scintilla.IsReadOnly = false;

                        break;
                    }
                }


                // Open the files
                if (!isOpen)
                    OpenFile(filePath);
            }
        }

        public void closeFile(string filePath)
        {
            // Ensure this file isn't already open
            foreach (DocumentForm documentForm in dockPanel.Documents)
            {
                if (filePath.Equals(documentForm.FilePath, StringComparison.OrdinalIgnoreCase))
                {
                    documentForm.Close();
                    break;
                }
            }
        }

        public void RefreshActiveDocumentsColors()
        {

            foreach (DocumentForm doc in dockPanel.Documents)
            {
                if (doc.Scintilla != null)
                {
                    doc.Scintilla.BackColor = Settings1.Default.CodeEditorBackColor;
                    doc.Scintilla.ForeColor = Settings1.Default.CodeEditorFontColor;
                    doc.Scintilla.FindReplace.Indicator.Color = Settings1.Default.CodeEditorSelectionColor;
                }
            }
        }
        public DocumentForm OpenFile(string filePath)
        {
            try
            {
                DocumentForm doc = new DocumentForm();
                //doc.Scintilla.BackColor = Settings1.Default.CodeEditorBackColor;
                //doc.Scintilla.ForeColor = Settings1.Default.CodeEditorFontColor;
                //doc.Scintilla.FindReplace.Indicator.Color = Settings1.Default.CodeEditorSelectionColor;

                if (filePath.Contains(".lua"))
                {
                    doc.Scintilla.CharAdded += new EventHandler<CharAddedEventArgs>(this.sciDocument_CharAdded);
                    doc.Scintilla.TextDeleted += new EventHandler<TextModifiedEventArgs>(Scintilla_TextDeleted);
                    doc.Scintilla.AutoCompleteAccepted += new EventHandler<AutoCompleteAcceptedEventArgs>(Scintilla_AutoCompleteAccepted);
                    doc.Scintilla.MarkerChanged += new EventHandler<MarkerChangedEventArgs>(Scintilla_MarkerChanged);
                    doc.Scintilla.SelectionChanged += new EventHandler(Scintilla_SelectionChanged);
                    //doc.Scintilla.NativeInterface.UpdateUI += new EventHandler<NativeScintillaEventArgs>(Scintilla_SUpdateUI);
                    doc.Scintilla.IsBraceMatching = true;

                    
                }
               
                doc.Scintilla.UndoRedo.EmptyUndoBuffer();


                List<Marker> markers = doc.Scintilla.Markers.GetMarkers(0);
                if (markers.Count > 0)
                    markers[0].BackColor = Color.OrangeRed;

                doc.Show(dockPanel);
                doc.Scintilla.Text = File.ReadAllText(filePath);
             
                doc.Scintilla.Modified = false;
                doc.Text = Path.GetFileName(filePath);
                doc.FilePath = filePath;

                if (filePath.Contains(".lua"))
                {
                    checkIntegrityLuaSceneFileFromV1_14ToV1_15(doc);
                }

                doc.Save();



                if (this.Mode.Equals("DEBUG"))
                    doc.Scintilla.IsReadOnly = true;
                else
                {
                    doc.Scintilla.IsReadOnly = false;

                   
                    foreach (DocumentForm documentForm in dockPanel.Documents)
                    {
                        documentForm.Scintilla.Snippets.List.Clear();

                        documentForm.Scintilla.Snippets.List.AddRange(this.codeEditorSnippets);
                    }
                    
                   
                }
                   

                return doc; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        private void Scintilla_SelectionChanged(object sender, System.EventArgs e)
        {
            if (this.ActiveDocument != null)
            {
                if (this.Mode.Equals("DEBUG"))
                {
                    string selection = this.ActiveDocument.Scintilla.Selection.Text;
                    if (!selection.Equals(""))
                    {



                        /*toolTip1.ToolTipIcon = ToolTipIcon.Info;
                        toolTip1.ToolTipTitle = "Details";
                        toolTip1.IsBalloon = false;
                        Point pos = Cursor.Position;
                        toolTip1.Show(selection, this, pos, 5000);

                        if (dataTipForm != null)
                        {
                            dataTipForm.Dispose();
                            dataTipForm = null;

                        }
                        dataTipForm = new DataTip();
                        dataTipForm.init();

                       
                        TreeListViewItem item = new TreeListViewItem(selection);
                        item.SubItems.Add("Description");
                        
                        item.Items.Add(item.Clone() as TreeListViewItem);

                        dataTipForm.treeListView1.Items.Add(item);
                        item.ImageIndex = 0;

                        

                        
                        dataTipForm.Show(this);
                        dataTipForm.Location = pos;
                        int height = dataTipForm.treeListView1.ItemsCount * 25;
                        dataTipForm.Size = new Size(dataTipForm.Width, height);
                        dataTipForm.treeListView1.Focus();*/
                    }
                }
                else
                {
                    this.ActiveDocument.Scintilla.FindReplace.ClearAllHighlights();
                    string selection = this.ActiveDocument.Scintilla.Selection.Text;
                    if (!selection.Equals(""))
                    {
                        this.ActiveDocument.Scintilla.FindReplace.Flags = SearchFlags.WholeWord | SearchFlags.MatchCase;
                        this.ActiveDocument.Scintilla.FindReplace.Indicator.Color = Color.DarkGreen;
                        this.ActiveDocument.Scintilla.FindReplace.HighlightAll(this.ActiveDocument.Scintilla.FindReplace.FindAll(selection));
                        this.ActiveDocument.Scintilla.FindReplace.Flags = SearchFlags.Empty;
                        
                    }
                }
            }
        }

        private void Scintilla_MarkerChanged(object sender, MarkerChangedEventArgs args)
        {
            if (this.Mode.Equals("DEBUG"))
            {
                if (args.Line > -1)
                {
                    if (isDebugTraceMarkerChanged == false)
                   {

                       int mask = this.ActiveDocument.Scintilla.Markers.GetMarkerMask(this.ActiveDocument.Scintilla.Lines[args.Line]);

                        if (mask == 0 || mask == 4)
                        {
                            this.mainForm.debuggerPanel1.sendDeleteBreakPointCommand(new GameEditor.Debugger.BreakPoint(this.ActiveDocument.Text, (args.Line + 1).ToString()));
                        }
                        else if (mask == 1 || mask == 5)
                        {
                            Line line = this.ActiveDocument.Scintilla.Lines[args.Line];
                        
                            if (line.Text.StartsWith("--") ||
                                line.Text.StartsWith("\t\r\n") ||
                                line.Text.StartsWith("\n") ||
                                line.Text.StartsWith("\r"))
                            {
                                line.DeleteMarker(0);
                                return;
                            }

                            this.mainForm.debuggerPanel1.sendAddBreakPointCommand(new GameEditor.Debugger.BreakPoint(this.ActiveDocument.Text, (args.Line + 1).ToString()));
                        }
                    }
                  
                }
              
            }
        }

        private void UpdateAllScintillaZoom()
        {
            // Update zoom level for all files
            foreach (DocumentForm doc in dockPanel.Documents)
                doc.Scintilla.Zoom = _zoomLevel;
        }





        private void SetLanguage(string language)
        {
            if ("ini".Equals(language, StringComparison.OrdinalIgnoreCase))
            {
                // Reset/set all styles and prepare scintilla for custom lexing
                ActiveDocument.IniLexer = true;
                IniLexer.Init(ActiveDocument.Scintilla);
            }
            else
            {
                // Use a built-in lexer and configuration
                ActiveDocument.IniLexer = false;
                ActiveDocument.Scintilla.ConfigurationManager.Language = language;

                // Smart indenting...
                if ("cs".Equals(language, StringComparison.OrdinalIgnoreCase))
                    ActiveDocument.Scintilla.Indentation.SmartIndentType = SmartIndent.CPP;
                else
                    ActiveDocument.Scintilla.Indentation.SmartIndentType = SmartIndent.None;
            }
        }

        #endregion Methods

        #region CodeBuilder
        public Boolean CheckAutoGenerateCodeIntegrity() {
            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:Requires:Begin]")) return false;
            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:Requires:End]")) return false;

            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:InitializeComponents:Begin]")) return false;
            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:InitializeComponents:End]")) return false;

            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:GlobalVars:Begin]")) return false;
            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:GlobalVars:End]")) return false;

            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:LocalVars:Begin]")) return false;
            if (!Regex.IsMatch(ActiveDocument.Scintilla.Text, "--[CGE:LocalVars:End]")) return false;


            return true;
        }
        public Boolean BuildInitializeComponent(String codeInput) {

            removeTextBetweenRegex("--##KREA:InitializeComponents:Begin##", "--##KREA:InitializeComponents:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:InitializeComponents:Begin##");
        }

        public Boolean BuildDestroyScene(String codeInput)
        {

            removeTextBetweenRegex("--##KREA:DestroyScene:Begin##", "--##KREA:DestroyScene:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:DestroyScene:Begin##");
        }

        public Boolean BuildEnterScene(String codeInput)
        {

            removeTextBetweenRegex("--##KREA:EnterScene:Begin##", "--##KREA:EnterScene:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:EnterScene:Begin##");
        }

        public Boolean BuildWillEnterScene(String codeInput)
        {

            removeTextBetweenRegex("--##KREA:WillEnterScene:Begin##", "--##KREA:WillEnterScene:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:WillEnterScene:Begin##");
        }

        public Boolean BuildExitScene(String codeInput)
        {

            removeTextBetweenRegex("--##KREA:ExitScene:Begin##", "--##KREA:ExitScene:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:ExitScene:Begin##");
        }
        public Boolean BuildDidExitScene(String codeInput)
        {

            removeTextBetweenRegex("--##KREA:DidExitScene:Begin##", "--##KREA:DidExitScene:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:DidExitScene:Begin##");
        }

        public Boolean BuildOverlayBeganScene(String codeInput)
        {

            removeTextBetweenRegex("--##KREA:OverlayBegan:Begin##", "--##KREA:OverlayBegan:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:OverlayBegan:Begin##");
        }

        public Boolean BuildOverlayEndedBeganScene(String codeInput)
        {

            removeTextBetweenRegex("--##KREA:OverlayEnded:Begin##", "--##KREA:OverlayEnded:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:OverlayEnded:Begin##");
        }

        public Boolean BuildRequires(String codeInput)
        {
            removeTextBetweenRegex("--##KREA:Requires:Begin##", "--##KREA:Requires:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:Requires:Begin##");
        }
        public Boolean BuildGlobalVars(String codeInput)
        {
            removeTextBetweenRegex("--##KREA:GlobalVars:Begin##", "--##KREA:GlobalVars:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:GlobalVars:Begin##");
        }

        public Boolean BuildLocalVars(String codeInput)
        {
            removeTextBetweenRegex("--##KREA:LocalVars:Begin##", "--##KREA:LocalVars:End##");
            return AppendTextWithRegex(codeInput, "--##KREA:LocalVars:Begin##");
        }

        public void removeTextBetweenRegex(String regEx1, String regEx2)
        {
            if (ActiveDocument != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(ActiveDocument.Scintilla.Text);
                ActiveDocument.Scintilla.ResetText();



                //Recuperer l'index de regEx 1
                int indexReg1 = sb.ToString().IndexOf(regEx1);

                //Recuperer l'index de regEx 2
                int indexReg2 = sb.ToString().IndexOf(regEx2);

                if (indexReg1 != -1 && indexReg2 != -1)
                {

                    //Supprimer le text entre les deux index
                    int startIndex = indexReg1 + regEx1.Length;
                    int count = indexReg2 - startIndex - 2;

                    sb.Remove(startIndex, count);

                    ActiveDocument.Scintilla.InsertText(0,sb.ToString());


                }
                else
                {
                    ActiveDocument.Scintilla.InsertText(0, sb.ToString());
                }
                    
                
            }
            
        }

        
        public Boolean AppendTextWithRegex(String codeInput, String pattern) {
            if (codeInput == null) return false;
            if (codeInput.Equals("")) return false;
           
            if (ActiveDocument != null)
            {
               
                foreach (Line l in ActiveDocument.Scintilla.Lines)
                {
                    
                    if (Regex.IsMatch(l.Text, pattern))
                    {
                        ActiveDocument.Scintilla.InsertText(l.EndPosition + 1, codeInput);

                        return true;
                    }
                }
            }
            return false;
        }
        #endregion CodeBuilder

        public void startEmulator()
        {
            //Try to build
            if (this.mainForm.CurrentProject != null)
            {
                if (Emulator.Instance.isRunning)
                {
                    Emulator.Instance.Dispose();
                }

                string coronaPath = Settings1.Default.CoronaSDKFolder;
                if (coronaPath == null || coronaPath.Equals(""))
                {
                    coronaPath = Emulator.Instance.getCoronaSDKPath();
                    Settings1.Default.CoronaSDKFolder = coronaPath;
                    Settings1.Default.Save();
                }

                if (!coronaPath.Equals("") && File.Exists(coronaPath))
                {
               
                    if (File.Exists(this.mainForm.CurrentProject.BuildFolderPath + "\\main.lua"))
                    {
                        Boolean debugMode = false;

                        if (this.mainForm.EmulatorModeCmbBx.SelectedIndex == 1)
                        {
                            debugMode = true;
                            this.setCurrentMode("DEBUG");
                            this.mainForm.setWorkSpace("DEBUG");
                            this.mainForm.debuggerPanel1.initServer();
                        }
                        else
                        {
                            this.setCurrentMode("EDITOR");
                        }


                        Emulator.Instance.Init(this.mainForm.CurrentProject, debugMode);
                        Emulator.Instance.Start();

                    }
                   
                }
                else
                {
                    DialogResult rs = MessageBox.Show("Corona SDK does not seem to be installed on this computer!\n Please install the simulator before trying to start it!\n\nDo you want to visit the Corona SDK website now?",
                        "Corona SDK is missing",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                    if (rs == DialogResult.Yes)
                        Process.Start("http://www.coronalabs.com/products/corona-sdk/");
                }
            }
        }
        private void playEmulatorBt_Click(object sender, EventArgs e)
        {
            
            if (this.mainForm == null) return;

            if (this.mainForm.mainBackWorker.IsBusy == true)
                this.mainForm.mainBackWorker.CancelAsync();

            this.mainForm.currentWorkerAction = "ACTION_BUILD_PLAY";
            this.mainForm.mainBackWorker.RunWorkerAsync(this.mainForm.currentWorkerAction);
        
        }

        private void stopEmulatorBt_Click(object sender, EventArgs e)
        {
            if (Emulator.Instance.isRunning) Emulator.Instance.Dispose();
        }

        private void CGEeditor_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void CGEeditor_MouseLeave(object sender, EventArgs e)
        {
            this.mainForm.Focus();
        }

        private void dockPanel_ActiveContentChanged_1(object sender, EventArgs e)
        {
            if (this.ActiveDocument != null)
            {
               
                this.ActiveDocument.Scintilla.Margins.Margin1.IsClickable = true;
                //this.ActiveDocument.Scintilla.Margins.Margin1.AutoToggleMarkerNumber = 1;
                this.ActiveDocument.Scintilla.Margins.Margin2.IsClickable = true;
                this.ActiveDocument.Scintilla.Margins.Margin2.Width = 10;
                this.ActiveDocument.Scintilla.Margins.Margin2.IsFoldMargin = true;
                this.ActiveDocument.Scintilla.Margins.Margin2.IsMarkerMargin = false;

            
                this.AutoCompletionEngine.RefreshDynamicList();

                this.refreshFunctionsNavigationContent(this.ActiveDocument.Scintilla);
            }
                
        }

        private void Scintilla_SUpdateUI(object sender, System.EventArgs e)
        {
            if (this.ActiveDocument != null)
            {

                Scintilla scintilla = this.ActiveDocument.Scintilla;
                INativeScintilla scintillaControl = scintilla.NativeInterface;
                int brace_position = scintilla.CurrentPos - 1;
                int character_before = scintilla.CharAt(brace_position);
                char ch = (char)character_before;
                if (ch == ']' || ch == '[' || ch == '{' || ch == '}' || ch == '(' ||
                    ch == ')' || ch == '<' || ch == '>')
                {
                    int has_match = scintillaControl.BraceMatch(brace_position,0);
                    if (has_match > -1)
                    {
                        scintillaControl.BraceHighlight(has_match, brace_position);
                    }
                    else
                    {
                        scintillaControl.BraceBadLight(brace_position);
                    }
                }
                else
                {
                    brace_position = scintilla.CurrentPos;
                    character_before = scintilla.CharAt(brace_position);
                    ch = (char)character_before;
                    if (ch == ']' || ch == '[' || ch == '{' || ch == '}' || ch ==
                        '(' || ch == ')' || ch == '<' || ch == '>')
                    {
                        int has_match = scintillaControl.BraceMatch(brace_position,0);
                        if (has_match > -1)
                        {
                            scintillaControl.BraceHighlight(has_match, brace_position);
                        }
                        else
                        {
                            scintillaControl.BraceBadLight(brace_position);
                        }
                    }
                    else
                    {
                        scintillaControl.BraceHighlight(-1, -1);
                    }
                }
            }
        }

        private void formatCodeCurrentDocument()
        {
            if (this.ActiveDocument != null)
            {
                Scintilla scintilla = this.ActiveDocument.Scintilla;
                this.SuspendLayout();
                scintilla.SuspendLayout();
                for (int i = 0; i < scintilla.Lines.Count; i++)
                {
                    Line currLine = scintilla.Lines[i];
                    string finalText = currLine.Text.Replace("\t", "");
                    int nbOfTab = getFoldLevelForLine(currLine);

                    if (finalText.StartsWith("end"))
                        nbOfTab = nbOfTab - 1;

                    for (int j = 0; j < nbOfTab; j++)
                        finalText = finalText.Insert(0, "\t");


                    currLine.Text = finalText.Replace("\r", "").Replace("\n", "");
                }

                scintilla.ResumeLayout();
                this.ResumeLayout();
            }
        }

        private int getFoldLevelForLine(Line line)
        {
            int res = 0;
            
            if (line.FoldParent != null)
            {
                res = 1;
                res = res + getFoldLevelForLine(line.FoldParent);
            }
            return res;
        }


        private void sciDocument_CharAdded(object sender, CharAddedEventArgs e)
        {
            
            Scintilla sci = (Scintilla)sender;

            string text = sci.Text;
            char lastChar = e.Ch;


            bool res = sci.Snippets.DoSnippetCheck();

            if (res == false)
            {

                AutoCompletionEngine.IsActive = true;
                AutoCompletionEngine.checkForAutoCompletion(lastChar);

            }
            else
            {
                //ActiveDocument.Scintilla.Indentation.SmartIndentType = 

            }
            
            
            if(this.luaSyntaxCheckerBt.Checked == true)
                checkLuaSyntax();


            //if (sci.GetWordFromPosition(sci.CurrentPos).Equals("end"))
            //    this.formatCodeCurrentDocument();
            
        }

        public void setCurrentMode(string mode)
        {
            if (mode.Equals("EDITOR"))
            {
                this.Mode = mode;
                this.setDocumentsReadOnlyState(false);
                   
            }
            else if (mode.Equals("DEBUG"))
            {
                this.Mode = mode;
                this.setDocumentsReadOnlyState(true);
            }
        }

        public bool CheckLuaSyntaxBeforeBuild()
        {
            try
            {
                foreach (DocumentForm doc in this.dockPanel.Documents)
                {
                    
                    if(doc.FilePath.EndsWith(".lua"))
                    {
                        string code = doc.Scintilla.Text;
                        //this.ActiveDocument.Scintilla.Markers.DeleteAll(0);

                        if (!code.Equals(""))
                        {
                            StringBuilder result = checkLua(code, doc.Text);
                            if (result == null)
                            {
                                return true;
                            }
                            else
                            {
                                string[] splittedResult = result.ToString().Split(':');
                                int index = -1;

                                for (int i = 0; i < splittedResult.Length; i++)
                                {
                                    if (int.TryParse(splittedResult[i], out index) == true)
                                        break;

                                }

                                if (index > 0)
                                {
                                    string file = splittedResult[0].Substring(splittedResult[0].IndexOf("\""), splittedResult[0].LastIndexOf("\"") - splittedResult[0].IndexOf("\""));
                                    file = file.Replace("\"", "").Replace("*", "").Replace(" ", "");
                                    Line line = doc.Scintilla.Lines[index];
                                    
                                    string description = splittedResult[splittedResult.Length - 1];

                                    string message = "Lua syntax errors exist in file " + doc.Text + " line " + line.Number.ToString() + ":\n" + description +"\n\nDo you still want to start the simulator?";

                                    if (MessageBox.Show(message, "Syntax Errors", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        return true;
                                    else
                                        return false;
                                }
                            }
                        }

                        
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void checkLuaSyntax()
        {
            try
            {
                string code = this.ActiveDocument.Scintilla.Text;
                //this.ActiveDocument.Scintilla.Markers.DeleteAll(0);

                if (!code.Equals(""))
                {
                    StringBuilder result = checkLua(code, this.ActiveDocument.Text);
                    if (result == null)
                    {
                        return;
                    }
                    else
                    {
                        string[] splittedResult = result.ToString().Split(':');
                        int index = -1;
                        
                        for (int i = 0; i < splittedResult.Length; i++)
                        {
                            if (int.TryParse(splittedResult[i], out index) == true)
                                break;

                        }

                        if (index > 0)
                        {
                            /*Marker marker = this.ActiveDocument.Scintilla.Markers[0];
                            marker.Symbol = MarkerSymbol.Background;
                            marker.BackColor = Color.Red;*/
                            string file = splittedResult[0].Substring(splittedResult[0].IndexOf("\""), splittedResult[0].LastIndexOf("\"") - splittedResult[0].IndexOf("\""));
                            file = file.Replace("\"", "").Replace("*","").Replace(" ","");
                            Line line = this.ActiveDocument.Scintilla.Lines[index];

                            //line.AddMarker(marker);
                            
                            //this.ActiveDocument.Scintilla.FindReplace.
                            
                            //this.ActiveDocument.Scintilla.CallTip.HighlightEnd = line.EndPosition;
                            //this.ActiveDocument.Scintilla.CallTip.BackColor = Color.FromArgb(10, Color.Salmon);
                            //this.ActiveDocument.Scintilla.CallTip.Message = "Line " + result.ToString().Substring(result.ToString().IndexOf(index.ToString()));
                            //this.ActiveDocument.Scintilla.CallTip.Show();
                            this.codeCheckerListView.Items.Clear();
                            ListViewItem item = new ListViewItem("Syntax Error");
                            string description = splittedResult[splittedResult.Length-1];
                            item.SubItems.Add(description);
                            item.SubItems.Add(file);
                            item.SubItems.Add(line.Number.ToString());
                            item.SubItems.Add(this.ActiveDocument.FilePath);
                            item.Tag = line;
                            item.ForeColor = Color.Red;
                            this.codeCheckerListView.FullRowSelect = true;
                            this.codeCheckerListView.Items.Add(item);
                            this.codeCheckerListView.Visible = true;
                         
                        }
                        else
                        {
                            

                            this.codeCheckerListView.Items.Clear();
                            this.codeCheckerListView.Visible = false;
                        }
                        this.ActiveDocument.Scintilla.Refresh();
                        
                     
                    }
                }
               

               // object[] res = lua.DoString(code, this.ActiveDocument.Text);
               
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          
          


        }
        private void Scintilla_TextDeleted(object sender,TextModifiedEventArgs e)
        {
            if (this.luaSyntaxCheckerBt.Checked == true)
                checkLuaSyntax();

            string changeWord = e.Text;
            AutoCompletionEngine.checkForAutoCompletion('!');
            

        }

        private void Scintilla_AutoCompleteAccepted(object sender, AutoCompleteAcceptedEventArgs e)
        {


            this.AutoCompletionEngine.IsActive = false;
            if (this.luaSyntaxCheckerBt.Checked == true)
                this.checkLuaSyntax();
        }

        private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (this.ActiveDocument != null)
            {


                if (this.ActiveDocument.Scintilla != null)
                {
                    this.refreshFunctionsNavigationContent(this.ActiveDocument.Scintilla);
                    this.AutoCompletionEngine.RefreshDynamicList();

                    if (this.luaSyntaxCheckerBt.Checked == true)
                        checkLuaSyntax();
                }
                   
            }

        }

        private void refreshFunctionsNavigationContent(Scintilla scintilla)
        {
            this.functionsCmbBx.Items.Clear();

            for (int i = 0; i < scintilla.Lines.Count; i++)
            {
                Line line = scintilla.Lines[i];
                if (line != null)
                {
                    if (!line.Text.StartsWith("--"))
                    {
                        if (line.Text.Contains("function"))
                        {
                            this.functionsCmbBx.Items.Add(line.Text);
                        }
                    }

                }
            }
            
        }

        private void functionsCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ActiveDocument != null)
            {
                if (this.ActiveDocument.Scintilla != null)
                {
                    Range range = this.ActiveDocument.Scintilla.FindReplace.Find(this.functionsCmbBx.SelectedItem.ToString());
                    if (range != null)
                    {
                       
                        range.GotoStart();
                        range.Select();
                    }
                        

                
                }
            }
        }

        private void luaSyntaxCheckerBt_Click(object sender, EventArgs e)
        {
            if (this.luaSyntaxCheckerBt.Checked == true)
            {
                this.luaSyntaxCheckerBt.Checked = false;
                this.codeCheckerListView.Visible = false;
            }
            else
            {
                this.luaSyntaxCheckerBt.Checked = true;
                checkLuaSyntax();
            }
        }

        private void codeCheckerListView_ItemActivate(object sender, EventArgs e)
        {
            if (this.codeCheckerListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = this.codeCheckerListView.SelectedItems[0];

                string file = selectedItem.SubItems[2].Text;
                string line = selectedItem.SubItems[3].Text;
                string filePath = selectedItem.SubItems[4].Text;

                this.OpenFileInEditor(filePath);

                if (this.ActiveDocument != null)
                {
                    int lineNum = -1;
                    int.TryParse(line, out lineNum);
                    if (lineNum > -1)
                    {
                        if (this.ActiveDocument.Scintilla.Lines.Count > lineNum)
                        {
                            Line finalLine = this.ActiveDocument.Scintilla.Lines[lineNum-1];
                            Range range = finalLine.Range;
                            if (range != null)
                            {

                                range.GotoStart();
                                range.Select();
                            }
                        }
                       
                    }
                   
                }
               
                
            }
        }
  

    }
}
