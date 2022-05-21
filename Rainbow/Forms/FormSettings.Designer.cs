
namespace Rainbow
{
    partial class FormSettings
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
            this.labelAntiAliasingText = new System.Windows.Forms.Label();
            this.buttonAntiAliasing = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelPixelOffset = new System.Windows.Forms.Label();
            this.labelTextRenderingHint = new System.Windows.Forms.Label();
            this.buttonPixelOffset = new System.Windows.Forms.Button();
            this.buttonTextRenderingHint = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelAntiAliasingText
            // 
            this.labelAntiAliasingText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAntiAliasingText.Location = new System.Drawing.Point(0, 0);
            this.labelAntiAliasingText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAntiAliasingText.Name = "labelAntiAliasingText";
            this.labelAntiAliasingText.Size = new System.Drawing.Size(160, 80);
            this.labelAntiAliasingText.TabIndex = 1;
            this.labelAntiAliasingText.Text = "Anti-Aliasing";
            this.labelAntiAliasingText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAntiAliasing
            // 
            this.buttonAntiAliasing.FlatAppearance.BorderSize = 0;
            this.buttonAntiAliasing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAntiAliasing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAntiAliasing.Location = new System.Drawing.Point(160, 0);
            this.buttonAntiAliasing.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAntiAliasing.Name = "buttonAntiAliasing";
            this.buttonAntiAliasing.Size = new System.Drawing.Size(160, 80);
            this.buttonAntiAliasing.TabIndex = 2;
            this.buttonAntiAliasing.Text = "Default";
            this.buttonAntiAliasing.UseVisualStyleBackColor = true;
            this.buttonAntiAliasing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonAntiAliasing_MouseDown);
            // 
            // buttonReset
            // 
            this.buttonReset.FlatAppearance.BorderSize = 0;
            this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReset.Location = new System.Drawing.Point(160, 320);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(80, 80);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.FlatAppearance.BorderSize = 0;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(240, 320);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(80, 80);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.FlatAppearance.BorderSize = 0;
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBack.Location = new System.Drawing.Point(0, 320);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(80, 80);
            this.buttonBack.TabIndex = 5;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // labelPixelOffset
            // 
            this.labelPixelOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPixelOffset.Location = new System.Drawing.Point(0, 80);
            this.labelPixelOffset.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPixelOffset.Name = "labelPixelOffset";
            this.labelPixelOffset.Size = new System.Drawing.Size(160, 80);
            this.labelPixelOffset.TabIndex = 6;
            this.labelPixelOffset.Text = "Pixel Offset";
            this.labelPixelOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTextRenderingHint
            // 
            this.labelTextRenderingHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTextRenderingHint.Location = new System.Drawing.Point(0, 160);
            this.labelTextRenderingHint.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTextRenderingHint.Name = "labelTextRenderingHint";
            this.labelTextRenderingHint.Size = new System.Drawing.Size(160, 80);
            this.labelTextRenderingHint.TabIndex = 7;
            this.labelTextRenderingHint.Text = "Text Rendering Hint";
            this.labelTextRenderingHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonPixelOffset
            // 
            this.buttonPixelOffset.FlatAppearance.BorderSize = 0;
            this.buttonPixelOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPixelOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPixelOffset.Location = new System.Drawing.Point(160, 80);
            this.buttonPixelOffset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPixelOffset.Name = "buttonPixelOffset";
            this.buttonPixelOffset.Size = new System.Drawing.Size(160, 80);
            this.buttonPixelOffset.TabIndex = 8;
            this.buttonPixelOffset.Text = "Default";
            this.buttonPixelOffset.UseVisualStyleBackColor = true;
            this.buttonPixelOffset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonPixelOffset_MouseDown);
            // 
            // buttonTextRenderingHint
            // 
            this.buttonTextRenderingHint.FlatAppearance.BorderSize = 0;
            this.buttonTextRenderingHint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTextRenderingHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTextRenderingHint.Location = new System.Drawing.Point(160, 160);
            this.buttonTextRenderingHint.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTextRenderingHint.Name = "buttonTextRenderingHint";
            this.buttonTextRenderingHint.Size = new System.Drawing.Size(160, 80);
            this.buttonTextRenderingHint.TabIndex = 9;
            this.buttonTextRenderingHint.Text = "Default";
            this.buttonTextRenderingHint.UseVisualStyleBackColor = true;
            this.buttonTextRenderingHint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonTextRenderingHint_MouseDown);
            // 
            // buttonDefault
            // 
            this.buttonDefault.FlatAppearance.BorderSize = 0;
            this.buttonDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefault.Location = new System.Drawing.Point(80, 320);
            this.buttonDefault.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(80, 80);
            this.buttonDefault.TabIndex = 10;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.ButtonDefault_Click);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(320, 400);
            this.ControlBox = false;
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.buttonTextRenderingHint);
            this.Controls.Add(this.buttonPixelOffset);
            this.Controls.Add(this.labelTextRenderingHint);
            this.Controls.Add(this.labelPixelOffset);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonAntiAliasing);
            this.Controls.Add(this.labelAntiAliasingText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormSettings";
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormSettings_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelAntiAliasingText;
        private System.Windows.Forms.Button buttonAntiAliasing;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelPixelOffset;
        private System.Windows.Forms.Label labelTextRenderingHint;
        private System.Windows.Forms.Button buttonPixelOffset;
        private System.Windows.Forms.Button buttonTextRenderingHint;
        private System.Windows.Forms.Button buttonDefault;
    }
}