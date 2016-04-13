using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WeiboMonitor
{
    public partial class FormPIN : Form
    {
        private WeiboLogin wb;

        public FormPIN(WeiboLogin wb, Image pinImage)
        {
            InitializeComponent();
            picPIN.Image = pinImage;
            this.wb = wb;
            //this.Owner = f;
            this.StartPosition = FormStartPosition.CenterScreen;
            //this.Location = new Point(this.Owner.Location.X + 10, this.Owner.Location.Y + 10);
        }

        private void picPIN_Click(object sender, EventArgs e)
        {
            picPIN.Image = wb.GetPIN();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPIN.Text.Trim() != "")
            {
                this.Tag = txtPIN.Text.Trim();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("请输入验证码");
            }
        }
    }
}
