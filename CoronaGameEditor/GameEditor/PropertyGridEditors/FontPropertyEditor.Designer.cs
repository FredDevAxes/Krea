namespace Krea.GameEditor.PropertyGridEditors
{
    partial class FontPropertyEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FontPropertyEditor
            // 
            this.FullRowSelect = true;
            this.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.Size = new System.Drawing.Size(178, 191);
            this.View = System.Windows.Forms.View.List;
            this.SelectedIndexChanged += new System.EventHandler(this.FontPropertyEditor_SelectedIndexChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
