namespace WeiboMonitor
{
    partial class FormPIN
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
            this.picPIN = new System.Windows.Forms.PictureBox();
            this.txtPIN = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPIN)).BeginInit();
            this.SuspendLayout();
            // 
            // picPIN
            // 
            this.picPIN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPIN.Location = new System.Drawing.Point(12, 12);
            this.picPIN.Name = "picPIN";
            this.picPIN.Size = new System.Drawing.Size(100, 40);
            this.picPIN.TabIndex = 4;
            this.picPIN.TabStop = false;
            this.picPIN.Click += new System.EventHandler(this.picPIN_Click);
            // 
            // txtPIN
            // 
            this.txtPIN.Location = new System.Drawing.Point(12, 61);
            this.txtPIN.MaxLength = 10;
            this.txtPIN.Name = "txtPIN";
            this.txtPIN.Size = new System.Drawing.Size(100, 21);
            this.txtPIN.TabIndex = 9;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 88);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormPIN
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 121);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtPIN);
            this.Controls.Add(this.picPIN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPIN";
            this.ShowIcon = false;
            this.Text = "请填写验证码";
            ((System.ComponentModel.ISupportInitialize)(this.picPIN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picPIN;
        private System.Windows.Forms.TextBox txtPIN;
        private System.Windows.Forms.Button btnOK;
    }
}