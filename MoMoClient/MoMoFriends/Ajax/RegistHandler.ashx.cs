using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MoMoFriends;
using MoMoFriends.HttpServiceReference;

namespace MaoMaoFriendsWeb.Ajax
{
    /// <summary>
    /// Summary description for RegistHandler
    /// </summary>
    public class RegistHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            using (UserOperateClient insUserBLL = new UserOperateClient())
            {
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");
                UserInfo user = new UserInfo();
                
                user.LoginName = System.Web.HttpUtility.UrlDecode(context.Request["username"]);
                user.Password = CommonFunction.StringToMD5(System.Web.HttpUtility.UrlDecode(context.Request["password"]), 16);
                user.Mail = System.Web.HttpUtility.UrlDecode(context.Request["email"]);
                user.QuestionID = Convert.ToInt32(System.Web.HttpUtility.UrlDecode(context.Request["question"]));
                user.QuestionAnswer = System.Web.HttpUtility.UrlDecode(context.Request["answer"]);
                try
                {
                    if (insUserBLL.UserRegist(user) == "注册成功")
                    {
                        context.Session.Add("loginname", user.LoginName);
                        context.Application.Add("loginName", user.LoginName);
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