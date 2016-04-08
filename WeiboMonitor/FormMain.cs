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
        private WeiboLogin wb;
        private bool isLogin = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bgwLogin.RunWorkerAsync();
        }

        private void bgwLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            SetEnabled(txtUsername, false);
            SetEnabled(txtPassword, false);
            SetEnabled(btnStart, false);
            isLogin = false;
            string result = "登陆失败，未知错误";

            try
            {
                wb = new WeiboLogin(txtUsername.Text, txtPassword.Text);
                Image pinImage = wb.Start();
                if (pinImage != null)
                {
                    Form formPIN = new FormPIN(wb, pinImage);
                    if (formPIN.ShowDialog() == DialogResult.OK)
                    {
                        result = wb.End((string)formPIN.Tag);
                        string html = wb.Get("http://weibo.com/5237923337/");
                    }
                    else
                    {
                        result = "用户没有输入验证码，请重新登陆";
                    }
                }
                else
                {
                    result = wb.End(null);
                    string html = wb.Get("http://weibo.com/5237923337/");
                    SetText(txtOutput, html);
                }
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }

            //对登陆结果进行判断并处理

            if (result == "0")
            {
                isLogin = true;
            }
            else if (result == "2070")
            {
                MessageBox.Show("验证码错误，请重新登陆");
            }
            else if (result == "101&")
            {
                MessageBox.Show("密码错误，请重新登陆");
            }
            else if (result == "4049")
            {
                MessageBox.Show("验证码为空，请重新登陆");
            }
            else
            {
                MessageBox.Show(result);
            }
            SetEnabled(txtUsername, !isLogin);
            SetEnabled(txtPassword, !isLogin);
            SetEnabled(btnStart, !isLogin);
        }

        delegate void SetTextDelegate(Control ctrl, string text);

        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="ctrl">待设置的控件</param>
        /// <param name="text">Text</param>
        public static void SetText(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired == true)
            {
                ctrl.Invoke(new SetTextDelegate(SetText), ctrl, text);
            }
            else
            {
                ctrl.Text = text;
            }
        }

        delegate void SetEnabledDelegate(Control ctrl, bool enabled);

        /// <summary>
        /// 跨线程设置控件Enabled
        /// </summary>
        /// <param name="ctrl">待设置的控件</param>
        /// <param name="enabled">Enabled</param>
        public static void SetEnabled(Control ctrl, bool enabled)
        {
            if (ctrl.InvokeRequired == true)
            {
                ctrl.Invoke(new SetEnabledDelegate(SetEnabled), ctrl, enabled);
            }
            else
            {
                ctrl.Enabled = enabled;
            }
        }
    }
}
