# 微博秒赞器
微博自动点赞，监控微博页面，新发微博秒赞。

貌似用在女神身上不错（暴露我内心的想法了）

## 界面
![界面](https://raw.githubusercontent.com/huiyadanli/WeiboMonitor/master/image/screenshot0.png)

## 使用方法
* [账号] [密码] 处输入你的微博账号密码；

* [页面UID] 里面输入微博主页地址 `weibo.com/u/` 或者 `weibo.com/` 后面跟随的那一串字符
        比如
        `http://weibo.com/booknsong?refer_flag=1028035010_&is_hot=1`
        中，`booknsong` 就是UID。请确保`http://weibo.com/[UID]`是能够打开的
* [刷新间隔] GET 该微博页面地址的时间间隔（单位：秒），推荐 20s~60s 就能达到秒赞的效果。注意不要太小，时间久了可能会出现账号异常。

## 相关说明
模拟登录这一块就是在我原来写的[C# 实现新浪微博模拟登录](https://github.com/huiyadanli/SinaLogin)上修改了一下。
实际测试时，不影响网页端微博的使用。
这个软件还未经过详细测试，还存在许多bug。

## TODO
* 加入同时监控多个微博的功能（感觉意义不是很大，同时监控太多会影响电脑的正常使用）
* 增加新微博提示，并增加相关设置（可选开关）
* 界面修改（现在还是一幅测试功能的样子）