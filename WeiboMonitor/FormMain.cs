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
        private WeiboLogin wbLogin;
        private bool isLogin = false;
        MonitorTimer mTimer;

        public FormMain()
        {
            InitializeComponent();
            rtbOutput.Text = "项目地址：https://github.com/huiyadanli/WeiboMonitor" + Environment.NewLine
                + "刷新时间间隔不宜太小，否则可能会出现账号异常的情况" + Environment.NewLine;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bgwLogin.RunWorkerAsync();
        }

        private void SwitchControl()
        {
            SetEnabled(txtUsername, !isLogin);
            SetEnabled(txtPassword, !isLogin);
            SetEnabled(txtUID, !isLogin);
            SetEnabled(txtInterval, !isLogin);
            SetEnabled(btnStart, !isLogin);
        }

        private void bgwLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            isLogin = false;
            SwitchControl();
            string result = "登陆失败，未知错误";

            try
            {
                // 模拟登陆
                wbLogin = new WeiboLogin(txtUsername.Text, txtPassword.Text);
                Image pinImage = wbLogin.Start();
                if (pinImage != null)
                {
                    Form formPIN = new FormPIN(wbLogin, pinImage);
                    if (formPIN.ShowDialog() == DialogResult.OK)
                    {
                        result = wbLogin.End((string)formPIN.Tag);
                    }
                    else
                    {
                        result = "用户没有输入验证码，请重新登陆";
                    }
                }
                else
                {
                    result = wbLogin.End(null);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            // 对登陆结果进行判断并处理
            if (result == "0")
            {
                // 开启timer监控页面
                isLogin = true;
                SetText(rtbOutput, "模拟登陆成功" + Environment.NewLine);
                try
                {
                    mTimer = new MonitorTimer(wbLogin, txtUID.Text.Trim());
                    mTimer.Interval = Convert.ToInt32(txtInterval.Text.Trim()) * 1000;
                    mTimer.Elapsed += new System.Timers.ElapsedEventHandler(mTimer_Elapsed);
                    mTimer.Start();
                    AppendText(rtbOutput, "开始监控，刷新间隔：" + txtInterval.Text.Trim() + " s" + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "提示");
                    SwitchControl();
                }
            }
            else if (result == "2070")
            {
                MessageBox.Show("验证码错误，请重新登陆", "提示");
            }
            else if (result == "101&")
            {
                MessageBox.Show("密码错误，请重新登陆", "提示");
            }
            else if (result == "4049")
            {
                MessageBox.Show("验证码为空，请重新登陆", "提示");
            }
            else
            {
                MessageBox.Show(result, "提示");
            }
            SwitchControl();
        }

        private void mTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                MonitorTimer t = (MonitorTimer)sender;
                string html = wbLogin.Get("http://weibo.com/" + t.Uid + "?is_all=1");
                WeiboPage newPage = new WeiboPage(html);
                List<WeiboFeed> newWbFeedList = newPage.Compare(t.OldPage.WbFeedList);
                if (newWbFeedList != null)
                {
                    for (int i = 0; i < newWbFeedList.Count; i++)
                    {
                        newWbFeedList[i].Like(wbLogin);
                    }
                    t.OldPage = newPage;

                    // 输出相关信息
                    if (newWbFeedList.Count > 0)
                    {
                        AppendText(rtbOutput, DateTime.Now.ToString("hh:mm:ss") + " 发现新微博：" + Environment.NewLine);
                        for (int i = 0; i < newWbFeedList.Count; i++)
                        {
                            AppendText(rtbOutput, "[" + i + "] [" + newWbFeedList[i].Content.Trim() + "]" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        AppendText(rtbOutput, DateTime.Now.ToString("hh:mm:ss") + " 当前页微博个数:" + newPage.WbFeedList.Count + Environment.NewLine);
                    }

                }
                else
                {
                    AppendText(rtbOutput, "本次微博页面获取失败" + Environment.NewLine);
                }
            }
        }

        delegate void AppendTextDelegate(Control ctrl, string text);

        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="ctrl">待设置的控件</param>
        /// <param name="text">Text</param>
        public static void AppendText(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired == true)
            {
                ctrl.Invoke(new AppendTextDelegate(AppendText), ctrl, text);
            }
            else
            {
                ctrl.Text += text;
            }
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

        private void rtbOutput_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void rtbOutput_TextChanged(object sender, EventArgs e)
        {
            rtbOutput.SelectionStart = rtbOutput.Text.Length;
            rtbOutput.ScrollToCaret();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注意判断关闭事件Reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                this.Hide();
                return;
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            //mTimer.Stop();
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Show();
                WindowState = FormWindowState.Normal;
                this.Focus();
            }
            else 
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }
    }
}
