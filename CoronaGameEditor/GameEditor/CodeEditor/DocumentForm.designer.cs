namespace Krea
{
	partial class DocumentForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentForm));
            this.scintilla = new ScintillaNet.Scintilla();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla)).BeginInit();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.AutoComplete.CancelAtShowPoint = false;
            this.scintilla.AutoComplete.ListString = "";
            this.scintilla.AutoComplete.TriggerChars = ((System.Collections.Generic.List<string>)(resources.GetObject("scintilla.AutoComplete.TriggerChars")));
            this.scintilla.Caret.CurrentLineBackgroundColor = System.Drawing.Color.PaleTurquoise;
            this.scintilla.Caret.HighlightCurrentLine = true;
            this.scintilla.ConfigurationManager.Language = "lua";
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.Folding.MarkerScheme = ScintillaNet.FoldMarkerScheme.Custom;
            this.scintilla.LineWrap.VisualFlags = ScintillaNet.WrapVisualFlag.End;
            this.scintilla.Location = new System.Drawing.Point(0, 0);
            this.scintilla.Margins.Margin0.Width = 40;
            this.scintilla.Margins.Margin1.AutoToggleMarkerNumber = 0;
            this.scintilla.Margins.Margin1.IsClickable = true;
            this.scintilla.Markers.Folder.BackColor = System.Drawing.Color.Gray;
            this.scintilla.Markers.Folder.ForeColor = System.Drawing.Color.White;
            this.scintilla.Markers.Folder.Number = 30;
            this.scintilla.Markers.Folder.Symbol = ScintillaNet.MarkerSymbol.BoxPlus;
            this.scintilla.Markers.FolderEnd.BackColor = System.Drawing.Color.Gray;
            this.scintilla.Markers.FolderEnd.ForeColor = System.Drawing.Color.White;
            this.scintilla.Markers.FolderEnd.Number = 25;
            this.scintilla.Markers.FolderEnd.Symbol = ScintillaNet.MarkerSymbol.BoxPlusConnected;
            this.scintilla.Markers.FolderOpen.BackColor = System.Drawing.Color.Gray;
            this.scintilla.Markers.FolderOpen.ForeColor = System.Drawing.Color.White;
            this.scintilla.Markers.FolderOpen.Number = 31;
            this.scintilla.Markers.FolderOpen.Symbol = ScintillaNet.MarkerSymbol.BoxMinus;
            this.scintilla.Markers.FolderOpenMid.BackColor = System.Drawing.Color.Gray;
            this.scintilla.Markers.FolderOpenMid.ForeColor = System.Drawing.Color.White;
            this.scintilla.Markers.FolderOpenMid.Number = 26;
            this.scintilla.Markers.FolderOpenMid.Symbol = ScintillaNet.MarkerSymbol.BoxMinusConnected;
            this.scintilla.Markers.FolderOpenMidTail.BackColor = System.Drawing.Color.Gray;
            this.scintilla.Markers.FolderOpenMidTail.ForeColor = System.Drawing.Color.White;
            this.scintilla.Markers.FolderOpenMidTail.Number = 27;
            this.scintilla.Markers.FolderOpenMidTail.Symbol = ScintillaNet.MarkerSymbol.TCorner;
            this.scintilla.Markers.FolderSub.BackColor = System.Drawing.Color.Gray;
            this.scintilla.Markers.FolderSub.ForeColor = System.Drawing.Color.White;
            this.scintilla.Markers.FolderSub.Number = 29;
            this.scintilla.Markers.FolderSub.Symbol = ScintillaNet.MarkerSymbol.VLine;
            this.scintilla.Markers.FolderTail.BackColor = System.Drawing.Color.Gray;
            this.scintilla.Markers.FolderTail.ForeColor = System.Drawing.Color.White;
            this.scintilla.Markers.FolderTail.Number = 28;
            this.scintilla.Markers.FolderTail.Symbol = ScintillaNet.MarkerSymbol.LCorner;
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(292, 266);
            this.scintilla.Styles.BraceBad.BackColor = System.Drawing.Color.White;
            this.scintilla.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.scintilla.Styles.LastPredefined.BackColor = System.Drawing.SystemColors.Info;
            this.scintilla.TabIndex = 0;
            this.scintilla.StyleNeeded += new System.EventHandler<ScintillaNet.StyleNeededEventArgs>(this.scintilla_StyleNeeded);
            this.scintilla.ModifiedChanged += new System.EventHandler(this.scintilla_ModifiedChanged);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "All Files (*.*)|*.*";
            // 
            // DocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.scintilla);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DocumentForm";
            this.Activated += new System.EventHandler(this.DocumentForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocumentForm_FormClosing);
            this.Load += new System.EventHandler(this.DocumentForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.scintilla)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private ScintillaNet.Scintilla scintilla;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
	}
}