using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyWebApi
{
    /*
     * 统一处理类,
     * 根据URL段匹配出API类名和方法并执行之
     */
    class ApiHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            // 程序集名(当前运行此代码的程序集)
            string assbName = Assembly.GetExecutingAssembly().GetName().Name;

            // 分解URL路径用于找类名和方法名
            string[] urlparts = context.Request.Path.Split('/');

            // 如果路径不合规则,引发异常.合法路径 /api/user/info , /user/name
            if (urlparts.Length < 3)
                throw new HttpException("无效的API请求地址");

            // 类名和方法名:类名全称是- 程序集名.[命名空间].类名WebApi(约定后缀)
            // 命名空间: url拆分后,0位是命名空间,如果只有两段,则没有命名空间(只有当前程序集的默认命名空间)
            // 后缀约定: 为了统一书写,一个成为api的类,结尾为WebApi.例如: UserWebApi

            string apiClass = "", apiMethod = "";
            if (urlparts.Length < 4)
            {
                apiClass = $"{assbName}.{urlparts[1]}Api";
                apiMethod = urlparts[2];
            }
            else
            {
                apiClass = $"{assbName}.{urlparts[1]}.{urlparts[2]}Api";
                apiMethod = urlparts[3];
            }

            // 到当前程序集中寻找这个类
            Type webapiT = Assembly.GetExecutingAssembly().GetType(apiClass, false, true);

            // 未找到该类,引发异常
            if (webapiT == null)
                throw new HttpException($"无法找到名为{apiClass}的定义");

            // 建立实例,再传入HttpContext对象
            // 继承约定: 需要继承ApiBase这个基类
            ApiBase workapi = (ApiBase)Activator.CreateInstance(webapiT, true);
            workapi.SetHttpContext(context);

            // 到类中寻找方法
            MethodInfo webapiMethod = webapiT.GetMethods().FirstOrDefault(o => string.Compare(o.Name, apiMethod, true) == 0);

            // 未找到方法,引发异常
            if (webapiMethod == null)
                throw new HttpException($"无法找到名为{apiClass}.{apiMethod}的方法定义");

            // 检查方法是否贴有特性HTTPPOST/HTTPGET/HTTPALL. 例如[HTTPPOST],只响应POST请求.
            // 特性约定:做为API的方法需要贴上三个特性中的一个
            if (!this.AttributeCheck(context, webapiMethod, workapi))
                throw new HttpException($"无法响应请求.方法无必要特性!方法定义{apiClass}.{apiMethod}");

            // 如果不需要权限,注释掉.
            if (!this.PowerCheck(context, apiClass, apiMethod))
                throw new HttpException($"Access Denied!方法定义{apiClass}.{apiMethod}");

            // 执行方法
            webapiMethod.Invoke(workapi, null);
        }
        /// <summary>
        /// 接口权限判断
        /// </summary>
        /// <param name="context"></param>
        /// <param name="clsName"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private bool PowerCheck(HttpContext context, string clsName, string method)
        {
            return true;
        }
        /// <summary>
        /// 方法特性检查.是否贴有作为接口的三个特性
        /// 并非必要,只是为了加一个功能,让贴了特性的方法才能被访问.没贴的当内部方法
        /// </summary>
        /// <param name="webapiMethod">要检查的接口方法</param>
        /// <param name="workapi">所在接口实例</param>
        /// <returns></returns>
        private bool AttributeCheck(HttpContext context, MethodInfo webapiMethod, ApiBase workapi)
        {
            string httpMethod = context.Request.HttpMethod.ToUpper();
            if (httpMethod == "POST" && Attribute.IsDefined(webapiMethod, typeof(HTTPPOSTAttribute)))
            {
                return true;
            }
            else if (httpMethod == "GET" && Attribute.IsDefined(webapiMethod, typeof(HTTPGETAttribute)))
            {
                return true;
            }
            else if (Attribute.IsDefined(webapiMethod, typeof(HTTPALLAttribute)))
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
    class HTTPPOSTAttribute : Attribute
    {
        /// <summary>
        /// 接口功能描述
        /// </summary>
        public string Desc { get; set; }
    }
    class HTTPGETAttribute : Attribute
    {
        /// <summary>
        /// 接口功能描述
        /// </summary>
        public string Desc { get; set; }
    }
    class HTTPALLAttribute : Attribute
    {
        /// <summary>
        /// 接口功能描述
        /// </summary>
        public string Desc { get; set; }
    }
}
