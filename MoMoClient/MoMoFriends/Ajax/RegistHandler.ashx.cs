using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MoMoFriends.HttpServiceReference;

namespace MaoMaoFriendsWeb.Ajax
{
    /// <summary>
    /// Summary description for RegistHandler
    /// </summary>
    public class RegistHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            using (UserOperateClient insUserBLL = new UserOperateClient())
            {
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");
                UserInfo user = new UserInfo();
                user.LoginName = System.Web.HttpUtility.UrlDecode(context.Request["username"]);
                user.Password = System.Web.HttpUtility.UrlDecode(context.Request["password"]);
                try
                {
                    if (insUserBLL.UserRegist(user) == "注册成功")
                    {
                        //context.Session.Add("loginName", username); 
                        context.Application.Add("loginName", user.UserName);
                        //HttpApplication httpApp = new HttpApplication();
                        //httpApp.Application.Add("loginName", username);
                        context.Response.Write("success");
                    }
                    else
                    {
                        context.Response.Write("fail");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}