
namespace Rainbow
{
    partial class FormRecordsViewer
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
            this.listBoxRecords = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxRecords
            // 
            this.listBoxRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRecords.FormattingEnabled = true;
            this.listBoxRecords.Location = new System.Drawing.Point(0, 0);
            this.listBoxRecords.Name = "listBoxRecords";
            this.listBoxRecords.Size = new System.Drawing.Size(584, 561);
            this.listBoxRecords.TabIndex = 0;
            this.listBoxRecords.SelectedIndexChanged += new System.EventHandler(this.ListBoxRecords_SelectedIndexChanged);
            // 
            // FormRecordsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.Controls.Add(this.listBoxRecords);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRecordsViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Records Viewer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormRecordsViewer_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxRecords;
    }
}