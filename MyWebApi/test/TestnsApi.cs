using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi.api
{
    class TestnsApi : ApiBase
    {
        [HTTPGET]
        public void index()
        {
            string html = $@"
<h1>mywebapi测试页</h1>
<p>当前请求api /api/test/index</p>
<p>作为API的类要满足三点:</p>
<ol>
<li>继承ApiBase</li>
<li>类名以Api结尾</li>
<li>贴其中一个特性[HTTPALL][HTTPPOST][HTTPGET]</li>
</ol>
<h2>请求路径有两种:</h2>
<ol>
<li>类名/方法 例如:/user/info</li>
<li>命名空间/类名/方法 例如:/api/user/index .这种可以给API分文件夹</li>
</ol>
";
            this.Html(html);
        }
    }
}
