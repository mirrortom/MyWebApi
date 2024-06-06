# MyWebApi
基于asp.net自定义的webapi处理工具
## 原理
基于asp.net的IHttpHandler对象的webapi实现.用dotnet类库项目改装的webapi项目.  
## 目的
主要是为了方便使用和减少逻辑,实现添加一个普通的类就能作为webapi  
## 特点
基于asp.net管道事件的逻辑.在IIS中运行.程序池是.net framework4.8  
项目代码精减,主要是ApiHandler类分析URL,然后反射到类和方法上完成请求处理.  
[Doc](https://mirrortom.github.io/wz/jizizuo/mywebapi.html)  
2024/06/06:以后都使用.netcore了,这个项目不再更新.