using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace WeiboMonitor
{
    public class MonitorTimer : Timer
    {
        private WeiboLogin wbLogin;
        private string uid;
        private WeiboPage oldPage;

        public WeiboLogin WbLogin
        {
            get { return wbLogin; }
            set { wbLogin = value; }
        }

        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        public WeiboPage OldPage
        {
            get { return oldPage; }
            set { oldPage = value; }
        }

        public MonitorTimer(WeiboLogin wbLogin, string uid)
        {
            this.wbLogin = wbLogin;
            this.uid = uid;

            string html = wbLogin.Get("http://weibo.com/" + uid);
            oldPage = new WeiboPage(html);
        }
    }
}
