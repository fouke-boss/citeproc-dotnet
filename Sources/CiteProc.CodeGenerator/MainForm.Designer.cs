namespace CiteProc.CodeGenerator
{
    partial class MainForm
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
            this.btnLocalesV10 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLocalesV10
            // 
            this.btnLocalesV10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocalesV10.Location = new System.Drawing.Point(12, 12);
            this.btnLocalesV10.Name = "btnLocalesV10";
            this.btnLocalesV10.Size = new System.Drawing.Size(260, 23);
            this.btnLocalesV10.TabIndex = 0;
            this.btnLocalesV10.Text = "Generate default locales v1.0";
            this.btnLocalesV10.UseVisualStyleBackColor = true;
            this.btnLocalesV10.Click += new System.EventHandler(this.btnLocalesV10_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 42);
            this.Controls.Add(this.btnLocalesV10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CiteProc - Code Generator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLocalesV10;
    }
}