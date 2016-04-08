using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WeiboMonitor
{
    /// <summary>
    /// 用于Json解析
    /// </summary>
    public class ViewJson
    {
        public string ns { get; set; }
        public string domid { get; set; }
        public string[] css { get; set; }
        public string js { get; set; }
        public string html { get; set; } //这个最重要
    }

    public class WeiboPage
    {
        private string originHTML;
        private string oid;
        private string onick;
        private string uid;
        private string nick;
        private string location;
        private List<WeiboFeed> wbFeedList = new List<WeiboFeed>();

        /// <summary>
        /// 页面url
        /// </summary>
        public string Url
        {
            get { return "http://weibo.com/" + Oid; }
        }

        /// <summary>
        /// GET得到的微博页面HTML
        /// </summary>
        public string OriginHTML
        {
            get { return originHTML; }
        }

        /// <summary>
        /// 该页面用户ID
        /// </summary>
        public string Oid
        {
            get { return oid; }
        }

        /// <summary>
        /// 该页面用户昵称
        /// </summary>
        public string Onick
        {
            get { return onick; }
        }

        /// <summary>
        /// 登陆用户的ID
        /// </summary>
        public string Uid
        {
            get { return uid; }
        }

        /// <summary>
        /// 登陆用户的昵称
        /// </summary>
        public string Nick
        {
            get { return nick; }
        }

        /// <summary>
        /// 页面中的location字段，POST点赞时的一个参数
        /// </summary>
        public string Location
        {
            get { return location; }
        }

        /// <summary>
        /// 各条微博内容
        /// </summary>
        public List<WeiboFeed> WbFeedList
        {
            get { return wbFeedList; }
        }

        public WeiboPage(string html)
        {
            originHTML = html;
            GetConfig();
            wbFeedList = GetAllFeeds();
        }

        /// <summary>
        /// 正则提取页面中的信息
        /// </summary>
        private void GetConfig()
        {
            oid = UseRegex(@"(?<=CONFIG\['oid'\]=').*?(?=';)");
            onick = UseRegex(@"(?<=CONFIG\['onick'\]=').*?(?=';)");
            uid = UseRegex(@"(?<=CONFIG\['uid'\]=').*?(?=';)");
            nick = UseRegex(@"(?<=CONFIG\['nick'\]=').*?(?=';)");
            location = UseRegex(@"(?<=CONFIG\['location'\]=').*?(?=';)");
        }

        /// <summary>
        /// 使用正则表达式
        /// </summary>
        /// <param name="regexStr"></param>
        /// <returns>返回第一个匹配项</returns>
        private string UseRegex(string regexStr)
        {
            Regex reg = new Regex(regexStr);
            Match match = reg.Match(OriginHTML);
            return match.Groups[0].Value;
        }

        /// <summary>
        /// 获取该页面各条微博
        /// </summary>
        public List<WeiboFeed> GetAllFeeds()
        {
            //微博正文内容 在下面这个字符串所在的那一行
            string searchStr = "<script>FM.view({\"ns\":\"pl.content.homeFeed.index\",\"domid\":\"Pl_Official_MyProfileFeed";
            //按换行符分割HTML
            string[] line = OriginHTML.Split('\n');
            int i;
            for (i = 0; i < line.Length; i++)
            {
                if (line[i].Length > searchStr.Length && line[i].Substring(0, searchStr.Length) == searchStr)
                {
                    break;
                }
            }
            //取出<script>标签内的Json数据
            string jsonStr = line[i].Replace("<script>FM.view(", "").Replace(")</script>", "");
            //使用 Newtonsoft.Json 反序列化
            ViewJson viewJson = JsonConvert.DeserializeObject<ViewJson>(jsonStr);

            //使用 HtmlAgilityPack 解析HTML
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(viewJson.html);
            //HtmlNodeCollection nodes = doc.DocumentNode.ChildNodes; //.SelectSingleNode("/div/div[2]/div[1]/div[3]/div[3]");

            HtmlNode topNode = doc.DocumentNode.ChildNodes[1];
            List<WeiboFeed> wbFeedList = new List<WeiboFeed>();
            foreach (HtmlNode feedListItem in topNode.ChildNodes)
            {
                if (feedListItem.Attributes.Contains("action-type") && feedListItem.Attributes["action-type"].Value == "feed_list_item")
                {
                    string mid = feedListItem.Attributes["mid"].Value;
                    string username = feedListItem.SelectSingleNode("div[1]/div[@class='WB_detail']/div[1]/a[1]").InnerHtml;
                    string time = feedListItem.SelectSingleNode("div[1]/div[@class='WB_detail']/div[2]/a[1]").Attributes["title"].Value;
                    string content = feedListItem.SelectSingleNode("div[1]/div[@class='WB_detail']/div[3]").InnerHtml;

                    WeiboFeed wbFeedTmp = new WeiboFeed(this, mid, username, time, content);
                    wbFeedList.Add(wbFeedTmp);
                }
            }

            return wbFeedList;
        }

        /// <summary>
        /// 比较输入的List（较旧），输出新增的WeiboFeed
        /// </summary>
        /// <param name="oldFeedList">旧的List</param>
        /// <returns></returns>
        public List<WeiboFeed> Compare(List<WeiboFeed> oldFeedList)
        {
            List<WeiboFeed> newWbFeedList = new List<WeiboFeed>();
            foreach (WeiboFeed newFeed in WbFeedList)
            {
                bool isNewly = true;
                foreach (WeiboFeed oldFeed in oldFeedList)
                {
                    if (oldFeed.Time >= newFeed.Time)
                    {
                        isNewly = false;
                        break;
                    }
                }
                if (isNewly)
                {
                    newWbFeedList.Add(newFeed);
                }
            }
            return newWbFeedList;
        }
    }
}
