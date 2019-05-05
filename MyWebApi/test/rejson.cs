using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApi
{
    class rejsonApi : ApiBase
    {
        [HTTPGET]
        public void index()
        {
            this.Json(new
            {
                name = "mywebapi",
                info = "测试返回json数据"
            });
        }

        [HTTPGET]
        public void paraget()
        {
            dynamic para = this.ParaGET();
            this.Json(para);
        }

        [HTTPPOST]
        public void paraform()
        {
            dynamic para = this.ParaForm();
            this.Json(para);
        }

        [HTTPPOST]
        public void parajson()
        {
            dynamic para = this.ParaStream();
            this.Json(para);
        }
    }
}
