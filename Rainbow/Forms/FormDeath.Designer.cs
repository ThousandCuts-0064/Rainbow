
namespace Rainbow
{
    partial class FormDeath
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
            this.labelColorModelText = new System.Windows.Forms.Label();
            this.labelColorModelValue = new System.Windows.Forms.Label();
            this.labelLevelValue = new System.Windows.Forms.Label();
            this.labelLevelText = new System.Windows.Forms.Label();
            this.panelGameModifiers = new System.Windows.Forms.Panel();
            this.buttonRestart = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.labelTimeSpanText = new System.Windows.Forms.Label();
            this.labelTimeSpanValue = new System.Windows.Forms.Label();
            this.labelGameModifiers = new System.Windows.Forms.Label();
            this.labelTilesPassedValue = new System.Windows.Forms.Label();
            this.labelTilesPassedText = new System.Windows.Forms.Label();
            this.labelTilesClickedValue = new System.Windows.Forms.Label();
            this.labelTilesClickedText = new System.Windows.Forms.Label();
            this.labelTilesPoppedValue = new System.Windows.Forms.Label();
            this.labelTilesPoppedText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelColorModelText
            // 
            this.labelColorModelText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelColorModelText.Location = new System.Drawing.Point(200, 0);
            this.labelColorModelText.Name = "labelColorModelText";
            this.labelColorModelText.Size = new System.Drawing.Size(100, 100);
            this.labelColorModelText.TabIndex = 0;
            this.labelColorModelText.Text = "Color Model";
            this.labelColorModelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelColorModelValue
            // 
            this.labelColorModelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelColorModelValue.Location = new System.Drawing.Point(300, 0);
            this.labelColorModelValue.Name = "labelColorModelValue";
            this.labelColorModelValue.Size = new System.Drawing.Size(100, 100);
            this.labelColorModelValue.TabIndex = 1;
            this.labelColorModelValue.Text = "|||||";
            this.labelColorModelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLevelValue
            // 
            this.labelLevelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLevelValue.Location = new System.Drawing.Point(100, 0);
            this.labelLevelValue.Name = "labelLevelValue";
            this.labelLevelValue.Size = new System.Drawing.Size(100, 100);
            this.labelLevelValue.TabIndex = 3;
            this.labelLevelValue.Text = "|||||";
            this.labelLevelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLevelText
            // 
            this.labelLevelText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLevelText.Location = new System.Drawing.Point(0, 0);
            this.labelLevelText.Name = "labelLevelText";
            this.labelLevelText.Size = new System.Drawing.Size(100, 100);
            this.labelLevelText.TabIndex = 2;
            this.labelLevelText.Text = "Level";
            this.labelLevelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGameModifiers
            // 
            this.panelGameModifiers.Location = new System.Drawing.Point(100, 100);
            this.panelGameModifiers.Name = "panelGameModifiers";
            this.panelGameModifiers.Size = new System.Drawing.Size(500, 200);
            this.panelGameModifiers.TabIndex = 4;
            // 
            // buttonRestart
            // 
            this.buttonRestart.AutoSize = true;
            this.buttonRestart.FlatAppearance.BorderSize = 0;
            this.buttonRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRestart.Location = new System.Drawing.Point(500, 500);
            this.buttonRestart.Name = "buttonRestart";
            this.buttonRestart.Size = new System.Drawing.Size(100, 100);
            this.buttonRestart.TabIndex = 5;
            this.buttonRestart.Text = "Restart";
            this.buttonRestart.UseVisualStyleBackColor = true;
            this.buttonRestart.Click += new System.EventHandler(this.ButtonRestart_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.AutoSize = true;
            this.buttonQuit.FlatAppearance.BorderSize = 0;
            this.buttonQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuit.Location = new System.Drawing.Point(0, 500);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(100, 100);
            this.buttonQuit.TabIndex = 6;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.ButtonQuit_Click);
            // 
            // labelTimeSpanText
            // 
            this.labelTimeSpanText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeSpanText.Location = new System.Drawing.Point(400, 0);
            this.labelTimeSpanText.Name = "labelTimeSpanText";
            this.labelTimeSpanText.Size = new System.Drawing.Size(100, 100);
            this.labelTimeSpanText.TabIndex = 7;
            this.labelTimeSpanText.Text = "Time Span";
            this.labelTimeSpanText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTimeSpanValue
            // 
            this.labelTimeSpanValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeSpanValue.Location = new System.Drawing.Point(500, 0);
            this.labelTimeSpanValue.Name = "labelTimeSpanValue";
            this.labelTimeSpanValue.Size = new System.Drawing.Size(100, 100);
            this.labelTimeSpanValue.TabIndex = 8;
            this.labelTimeSpanValue.Text = "|||||";
            this.labelTimeSpanValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGameModifiers
            // 
            this.labelGameModifiers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameModifiers.Location = new System.Drawing.Point(0, 100);
            this.labelGameModifiers.Name = "labelGameModifiers";
            this.labelGameModifiers.Size = new System.Drawing.Size(100, 200);
            this.labelGameModifiers.TabIndex = 9;
            this.labelGameModifiers.Text = "Game Modifiers";
            this.labelGameModifiers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTilesPassedValue
            // 
            this.labelTilesPassedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTilesPassedValue.Location = new System.Drawing.Point(500, 300);
            this.labelTilesPassedValue.Name = "labelTilesPassedValue";
            this.labelTilesPassedValue.Size = new System.Drawing.Size(100, 100);
            this.labelTilesPassedValue.TabIndex = 15;
            this.labelTilesPassedValue.Text = "|||||";
            this.labelTilesPassedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTilesPassedText
            // 
            this.labelTilesPassedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTilesPassedText.Location = new System.Drawing.Point(400, 300);
            this.labelTilesPassedText.Name = "labelTilesPassedText";
            this.labelTilesPassedText.Size = new System.Drawing.Size(100, 100);
            this.labelTilesPassedText.TabIndex = 14;
            this.labelTilesPassedText.Text = "Tiles Passed";
            this.labelTilesPassedText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTilesClickedValue
            // 
            this.labelTilesClickedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTilesClickedValue.Location = new System.Drawing.Point(100, 300);
            this.labelTilesClickedValue.Name = "labelTilesClickedValue";
            this.labelTilesClickedValue.Size = new System.Drawing.Size(100, 100);
            this.labelTilesClickedValue.TabIndex = 13;
            this.labelTilesClickedValue.Text = "|||||";
            this.labelTilesClickedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTilesClickedText
            // 
            this.labelTilesClickedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTilesClickedText.Location = new System.Drawing.Point(0, 300);
            this.labelTilesClickedText.Name = "labelTilesClickedText";
            this.labelTilesClickedText.Size = new System.Drawing.Size(100, 100);
            this.labelTilesClickedText.TabIndex = 12;
            this.labelTilesClickedText.Text = "Tiles Clicked";
            this.labelTilesClickedText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTilesPoppedValue
            // 
            this.labelTilesPoppedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTilesPoppedValue.Location = new System.Drawing.Point(300, 300);
            this.labelTilesPoppedValue.Name = "labelTilesPoppedValue";
            this.labelTilesPoppedValue.Size = new System.Drawing.Size(100, 100);
            this.labelTilesPoppedValue.TabIndex = 11;
            this.labelTilesPoppedValue.Text = "|||||";
            this.labelTilesPoppedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTilesPoppedText
            // 
            this.labelTilesPoppedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTilesPoppedText.Location = new System.Drawing.Point(200, 300);
            this.labelTilesPoppedText.Name = "labelTilesPoppedText";
            this.labelTilesPoppedText.Size = new System.Drawing.Size(100, 100);
            this.labelTilesPoppedText.TabIndex = 10;
            this.labelTilesPoppedText.Text = "Tiles Popped";
            this.labelTilesPoppedText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormDeath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.ControlBox = false;
            this.Controls.Add(this.labelTilesPassedValue);
            this.Controls.Add(this.labelTilesPassedText);
            this.Controls.Add(this.labelTilesClickedValue);
            this.Controls.Add(this.labelTilesClickedText);
            this.Controls.Add(this.labelTilesPoppedValue);
            this.Controls.Add(this.labelTilesPoppedText);
            this.Controls.Add(this.labelGameModifiers);
            this.Controls.Add(this.labelTimeSpanValue);
            this.Controls.Add(this.labelTimeSpanText);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonRestart);
            this.Controls.Add(this.panelGameModifiers);
            this.Controls.Add(this.labelLevelValue);
            this.Controls.Add(this.labelLevelText);
            this.Controls.Add(this.labelColorModelValue);
            this.Controls.Add(this.labelColorModelText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDeath";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormDeath";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDeath_FormClosed);
            this.Load += new System.EventHandler(this.FormDeath_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormDeath_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelColorModelText;
        private System.Windows.Forms.Label labelColorModelValue;
        private System.Windows.Forms.Label labelLevelValue;
        private System.Windows.Forms.Label labelLevelText;
        private System.Windows.Forms.Panel panelGameModifiers;
        private System.Windows.Forms.Button buttonRestart;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Label labelTimeSpanText;
        private System.Windows.Forms.Label labelTimeSpanValue;
        private System.Windows.Forms.Label labelGameModifiers;
        private System.Windows.Forms.Label labelTilesPassedValue;
        private System.Windows.Forms.Label labelTilesPassedText;
        private System.Windows.Forms.Label labelTilesClickedValue;
        private System.Windows.Forms.Label labelTilesClickedText;
        private System.Windows.Forms.Label labelTilesPoppedValue;
        private System.Windows.Forms.Label labelTilesPoppedText;
    }
}