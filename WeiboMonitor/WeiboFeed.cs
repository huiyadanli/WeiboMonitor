using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiboMonitor
{
    public class WeiboFeed
    {
        /// <summary>
        /// 微博ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 微博内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="content"></param>
        public WeiboFeed(string id,string username, string time, string content)
        {
            ID = id;
            Username = username;
            Time = time;
            Content = content;
        }

        public void Like()
        {

        }

        public void Comment()
        {

        }
    }
}
