
namespace Rainbow
{
    partial class FormPause
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
            this.buttonResume = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonRecord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonResume
            // 
            this.buttonResume.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonResume.Location = new System.Drawing.Point(40, 20);
            this.buttonResume.Margin = new System.Windows.Forms.Padding(2);
            this.buttonResume.Name = "buttonResume";
            this.buttonResume.Size = new System.Drawing.Size(120, 40);
            this.buttonResume.TabIndex = 0;
            this.buttonResume.Text = "Resume";
            this.buttonResume.UseVisualStyleBackColor = false;
            this.buttonResume.Click += new System.EventHandler(this.ButtonResume_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSettings.Location = new System.Drawing.Point(40, 140);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(120, 40);
            this.buttonSettings.TabIndex = 1;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = false;
            this.buttonSettings.Click += new System.EventHandler(this.ButtonSettings_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuit.Location = new System.Drawing.Point(40, 200);
            this.buttonQuit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(120, 40);
            this.buttonQuit.TabIndex = 2;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = false;
            this.buttonQuit.Click += new System.EventHandler(this.ButtonQuit_Click);
            // 
            // buttonRecord
            // 
            this.buttonRecord.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRecord.Location = new System.Drawing.Point(40, 80);
            this.buttonRecord.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRecord.Name = "buttonRecord";
            this.buttonRecord.Size = new System.Drawing.Size(120, 40);
            this.buttonRecord.TabIndex = 3;
            this.buttonRecord.Text = "Record";
            this.buttonRecord.UseVisualStyleBackColor = false;
            this.buttonRecord.Click += new System.EventHandler(this.ButtonRecord_Click);
            // 
            // FormPause
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(200, 260);
            this.Controls.Add(this.buttonRecord);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.buttonResume);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPause";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Settings";
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPause_FormClosed);
            this.Load += new System.EventHandler(this.FormPause_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPause_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonResume;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonRecord;
    }
}