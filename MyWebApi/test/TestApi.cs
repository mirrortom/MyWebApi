using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi
{
    class TestApi : ApiBase
    {
        [HTTPALL]
        public void Index()
        {
            string html = $@"
<h1>mywebapi测试页</h1>
<p>当前请求api /test/index</p>
<p>作为API的类要满足三点:</p>
<ol>
<li>继承ApiBase</li>
<li>类名以Api结尾</li>
<li>贴其中一个特性[HTTPALL][HTTPPOST][HTTPGET]</li>
</ol>
";
            this.Html(html);
        }
    }
}
