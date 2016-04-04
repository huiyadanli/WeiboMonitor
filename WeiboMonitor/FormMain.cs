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
    public partial class FormMain : Form
    {
        //下一步
        //把登录过程放在后台线程中
        bool needPIN = false;
        WeiboLogin wb;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Text = "开始登陆";
            txtPIN.Enabled = false;
            if (!needPIN)
            {
                wb = new WeiboLogin(txtUsername.Text, txtPassword.Text);
                Image pinImage = wb.Start();
                if (pinImage != null)
                {
                    picPIN.Image = pinImage;
                    needPIN = true;
                    labelState.Text = "请填写验证码";
                    txtPIN.Enabled = true;
                    btnStart.Text = "继续登陆";
                }
                else
                {
                    labelState.Text = "登录结果：" + wb.End(null);
                    btnStart.Text = "重新登陆";
                    txtRet.Text = wb.Get("http://weibo.com/5237923337/");
                    WeiboPage wbPage = new WeiboPage(txtRet.Text);
                }
            }
            else
            {
                if (txtPIN.Text.Trim() != "")
                {
                    needPIN = false;
                    labelState.Text = "登录结果：" + wb.End(txtPIN.Text.Trim());
                    btnStart.Text = "重新登陆";
                    txtRet.Text = wb.Get("http://weibo.com/5237923337/");
                }
                else
                {
                    MessageBox.Show("请填写验证码");
                    txtPIN.Enabled = true;
                }
            }
        }
    }
}
