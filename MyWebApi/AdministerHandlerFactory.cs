using System.Web;

namespace MyWebApi
{
    /*
     * asp.net管理事件中,第7个事件会调用此类方法.这里做个简单的跨域处理
     */
    class AdministerHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            /* ----------------------------------------------------------------------------*
             * 跨域处理
             * 跨域分简单跨域和复杂跨域,对于复杂跨域,浏览器会先发一个OPTIONS请求,
             * 用于试探服务器是否支持*跨域.此处受理OPTIONS请求时,均视为试探请求,
             * 将直接完成请求而不再执行后续逻辑.
             *-----------------------------------------------------------------------------*/
            SetCrosDomain(context, "*", "accessToken", "Content-Type");
            if (requestType == "OPTIONS")
            {
                context.Response.End();
                return null;
            }
            // 指定Ihttphandler
            return new ApiHandler();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
           
        }
        /// <summary>
        /// 1.设置允许跨域请求
        /// domain:设为*表示允许全部域名的请求.
        /// domain的组成形式是: 协议//域名:端口 (其中一个不同,就表示不同的源)
        /// 如果只希望某个域名,传一个网址如: http://localhost[:8080]
        /// 如果有多个域名,可以建一个列表保存.当请求来时,获取请求的Origin(就是源),如果在列表中,则调用此方法,并传入该Origin.
        /// 2.设置允许跨域的header自定义字段值
        /// 在跨域时,如果需要在header里带上自定义的字段时,需要在此添加.否则跨域出错.
        /// </summary>
        /// <param name="domain"></param>
        private void SetCrosDomain(HttpContext context, string domain = "*", params string[] allowHeader)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", domain);
            if (allowHeader.Length > 0)
            {
                string headers = string.Join(",", allowHeader);
                context.Response.AddHeader("Access-Control-Allow-Headers", headers);
                //context.Response.AddHeader("Access-Control-Allow-Methods", "POST");
                //context.Response.AddHeader("Access-Control-Allow-Methods", "GET");
            }
        }
    }
}
