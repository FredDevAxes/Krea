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

namespace Krea
{
    public partial class SnippetEditor : UserControl
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDocument();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAllStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Printing.Print();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Printing.PrintPreview();
        }


        #endregion File Menu Handlers

        #region Edit Menu Handlers

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Selection.SelectAll();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.FindReplace.ShowFind();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.FindReplace.ShowReplace();
        }

        private void findInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	Coming someday...
        }

        private void replaceInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	Coming someday...
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.GoTo.ShowGoToDialog();
        }

        private void toggleBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Line currentLine = ActiveDocument.Scintilla.Lines.Current;
            if (ActiveDocument.Scintilla.Markers.GetMarkerMask(currentLine) == 0)
            {
                currentLine.AddMarker(0);
            }
            else
            {
                currentLine.DeleteMarker(0);
            }
        }

        private void previosBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
            Line l = ActiveDocument.Scintilla.Lines.Current.FindPreviousMarker(1);
            if (l != null)
                l.Goto();
        }

        private void nextBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
            Line l = ActiveDocument.Scintilla.Lines.Current.FindNextMarker(1);
            if (l != null)
                l.Goto();
        }

        private void clearBookmarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Markers.DeleteAll(0);

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
            ActiveDocument.Scintilla.AutoComplete.Show();
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


        public SnippetEditor()
        {
            InitializeComponent();

        }

        public void Init(Form1 MainForm)
        {

            if(MainForm!=null)
                this.mainForm = MainForm;

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
            // Update the main form text to show the current document
            if (ActiveDocument != null)
                this.Text = String.Format(CultureInfo.CurrentCulture, "{0} - {1}", ActiveDocument.Text, "Corona Game Editor");
            else
                this.Text = "Corona Game Editor";	
        }
        #region Methods

        public DocumentForm NewDocument()
        {
            DocumentForm doc = new DocumentForm();
            
            doc.Text = String.Format(CultureInfo.CurrentCulture, "{0}{1}", NEW_DOCUMENT_TEXT, ++_newDocumentCount);
            doc.Show(dockPanel);
            toolIncremental.Searcher.Scintilla = doc.Scintilla;
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

        public void OpenFileInEditor(string filePath)
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
                    break;
                }
            }

            
            // Open the files
            if (!isOpen)
                OpenFile(filePath);
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
        public DocumentForm OpenFile(string filePath)
        {
            try
            {
                DocumentForm doc = new DocumentForm();


                doc.Scintilla.Text = File.ReadAllText(filePath);
                doc.Scintilla.UndoRedo.EmptyUndoBuffer();
                doc.Scintilla.Modified = false;
                doc.Text = Path.GetFileName(filePath);
                doc.FilePath = filePath;
                doc.Show(dockPanel);
                toolIncremental.Searcher.Scintilla = doc.Scintilla;

                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
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

            removeTextBetweenRegex("--##CGE:InitializeComponents:Begin##", "--##CGE:InitializeComponents:End##");
            return AppendTextWithRegex(codeInput, "--##CGE:InitializeComponents:Begin##");
        }

        public Boolean BuildDestroyScene(String codeInput)
        {

            removeTextBetweenRegex("--##CGE:DestroyScene:Begin##", "--##CGE:DestroyScene:End##");
            return AppendTextWithRegex(codeInput, "--##CGE:DestroyScene:Begin##");
        }

        public Boolean BuildEnterScene(String codeInput)
        {

            removeTextBetweenRegex("--##CGE:EnterScene:Begin##", "--##CGE:EnterScene:End##");
            return AppendTextWithRegex(codeInput, "--##CGE:EnterScene:Begin##");
        }

        public Boolean BuildExitScene(String codeInput)
        {

            removeTextBetweenRegex("--##CGE:ExitScene:Begin##", "--##CGE:ExitScene:End##");
            return AppendTextWithRegex(codeInput, "--##CGE:ExitScene:Begin##");
        }

        public Boolean BuildRequires(String codeInput)
        {
            removeTextBetweenRegex("--##CGE:Requires:Begin##", "--##CGE:Requires:End##");
            return AppendTextWithRegex(codeInput, "--##CGE:Requires:Begin##");
        }
        public Boolean BuildGlobalVars(String codeInput)
        {
            removeTextBetweenRegex("--##CGE:GlobalVars:Begin##", "--##CGE:GlobalVars:End##");
            return AppendTextWithRegex(codeInput, "--##CGE:GlobalVars:Begin##");
        }

        public Boolean BuildLocalVars(String codeInput)
        {
            removeTextBetweenRegex("--##CGE:LocalVars:Begin##", "--##CGE:LocalVars:End##");
            return AppendTextWithRegex(codeInput, "--##CGE:LocalVars:Begin##");
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

                    ActiveDocument.Scintilla.AppendText(sb.ToString());
                    ActiveDocument.Save();
                }
                else
                {
                    ActiveDocument.Scintilla.AppendText(sb.ToString());
                    ActiveDocument.Save();
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
                        ActiveDocument.Save();
                       
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion CodeBuilder

        
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
            this.Select();
        }

        private void CGEeditor_MouseLeave(object sender, EventArgs e)
        {
            this.mainForm.Select();
        }





    }
}
