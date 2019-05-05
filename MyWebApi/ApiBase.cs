using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace MyWebApi
{
    /*
     * 本类提供了请求上下文对象和一些便利方法处理参数和返回值
     * 用来提供数据的接口类(类似webapi的Controller),需要继承这个类.
     */
    /// <summary>
    /// webapi接口类基类
    /// </summary>
    class ApiBase
    {
        #region 请求上下文对象及其它工具属性

        /// <summary>
        /// http请求上下文对象传入,此方法由handler调用
        /// </summary>
        /// <param name="context"></param>
        internal void SetHttpContext(HttpContext context)
        {
            if (this.HttpContext == null)
                this.HttpContext = context;
        }
        /// <summary>
        /// 获取有关单个 HTTP 请求的 HTTP 特定的信息。
        /// </summary>
        protected HttpContext HttpContext { get; private set; }
        /// <summary>
        /// 为当前 HTTP 请求获取 HttpRequestBase 对象。
        /// </summary>
        protected HttpRequest Request
        {
            get { return this.HttpContext.Request; }
        }
        /// <summary>
        /// 为当前 HTTP 响应获取 HttpResponseBase 对象。
        /// </summary>
        protected HttpResponse Response
        {
            get { return this.HttpContext.Response; }
        }
        /// <summary>
        /// HttpContext.Server
        /// </summary>
        protected HttpServerUtility Server
        {
            get { return this.HttpContext.Server; }
        }
        /// <summary>
        /// 获取当前应用程序域的Cache
        /// HttpRuntime.Cache
        /// </summary>
        protected Cache Cache
        {
            get { return HttpRuntime.Cache; }
        }
        #endregion

        #region 便利方法,将请求参数转为对象

        /// <summary>
        /// 获取GET参数,并且转为动态类型
        /// 无参数时返回空对象
        /// </summary>
        /// <returns></returns>
        protected virtual dynamic ParaGET()
        {
            dynamic obj = new System.Dynamic.ExpandoObject();
            foreach (string key in this.Request.QueryString.Keys)
            {
                ((IDictionary<string, object>)obj).Add(key, this.Request.QueryString.Get(key));
            }
            return obj;
        }
        /// <summary>
        /// 获取form参数,并且转为动态类型
        /// 无参数时返回空对象
        /// </summary>
        /// <returns></returns>
        protected virtual dynamic ParaForm()
        {
            dynamic obj = new System.Dynamic.ExpandoObject();
            foreach (string key in this.Request.Form.Keys)
            {
                ((IDictionary<string, object>)obj).Add(key, this.Request.Form.Get(key));
            }
            return obj;
        }
        /// <summary>
        /// 从InputStream中获取参数.然后返回UTF8编码的字符串.如果是个JSON字符串,可再做转换
        /// 如果get,form都没参数,可尝试这个方法获取.例如Content-Type: application/json类型的参数
        /// 没有取到时返回null
        /// </summary>
        protected virtual string ParaStream()
        {
            byte[] byts = new byte[this.Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string json = Encoding.UTF8.GetString(byts);
            return json.Trim();
        }
        
        #endregion

        #region response返回各种结果形式

        /// <summary>
        /// 返回JSON格式数据
        /// </summary>
        /// <param name="obj"></param>
        protected void Json(object obj)
        {
            this.Response.ContentType = "application/json";
            this.Response.Charset = "UTF-8";
            string jsonstr = JsonConvert.SerializeObject(obj);
            this.Response.Write(jsonstr);
        }
        /// <summary>
        /// 返回一段HTML格式文本
        /// </summary>
        /// <param name="html"></param>
        protected void Html(string html)
        {
            this.Response.ContentType = "text/html";
            this.Response.Charset = "UTF-8";
            this.Response.Write(html);
        }
        /// <summary>
        /// 返回纯文本格式字符串
        /// </summary>
        /// <param name="text"></param>
        protected void Text(string text)
        {
            this.Response.ContentType = "text/plain";
            this.Response.Charset = "UTF-8";
            this.Response.Write(text);
        }
        /// <summary>
        /// 返回文件,需要指定文件内容头型
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        protected void File(string fileName, string contentType)
        {
            this.Response.Charset = "UTF-8";
            this.Response.ContentType = contentType;
            this.Response.WriteFile(fileName);
        }
        /// <summary>
        /// 返回文件,指定文件内容头型,下载文件显示名
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <param name="fileDownloadName"></param>
        protected void File(string fileName, string contentType, string fileDownloadName)
        {
            this.Response.Charset = "UTF-8";
            this.Response.Headers.Add("Content-disposition", $"attachment;filename={this.Server.UrlEncode(fileDownloadName)}");
            this.Response.ContentType = contentType;
            this.Response.WriteFile(fileName);
        }
        #endregion
    }
}
