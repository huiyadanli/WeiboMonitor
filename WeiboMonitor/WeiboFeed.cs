using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WeiboMonitor
{
    /// <summary>
    /// 用于Json解析
    /// </summary>
    public class ResponseJson
    {
        public string code { get; set; }
        public string msg { get; set; }
        public DataJson data { get; set; }

        public class DataJson
        {
            public int isDel { get; set; }
            public string html { get; set; }
        }
    }

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
        public DateTime Time { get; set; }

        /// <summary>
        /// 微博内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 点赞状态
        /// </summary>
        public bool IsLike { get; set; }

        public WeiboPage WBPage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="content"></param>
        public WeiboFeed(WeiboPage fatherPage, string id, string username, string time, string content, bool isLike)
        {
            WBPage = fatherPage;
            ID = id;
            Username = username;
            Content = content;
            IsLike = isLike;

            Time = DateTime.Parse(time);
        }

        /// <summary>
        /// 点赞功能
        /// </summary>
        /// <param name="wbLogin"></param>
        /// <param name="yOrN">是点赞，还是取消赞</param>
        /// <returns></returns>
        public int Like(WeiboLogin wbLogin, bool yOrN)
        {
            if (IsLike == yOrN)
            {
                return -2;
            }

            try
            {
                string url = "http://weibo.com/aj/v6/like/add?ajwvr=6";
                string postStr = "location=" + WBPage.Location + "&version=mini&qid=heart&mid=" + ID + "&loc=profile";
                string responseStr = HttpHelper.Post(url, WBPage.Url, wbLogin.MyCookies, postStr);
                ResponseJson responseJson = JsonConvert.DeserializeObject<ResponseJson>(responseStr);
                return responseJson.data.isDel;
            }
            catch
            {
                return -1;
            }

        }

        public void Comment()
        {

        }
    }
}
