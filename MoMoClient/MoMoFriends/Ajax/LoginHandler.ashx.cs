//<%@ WebHandler Language="C#" Class="Handler" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoMoFriends.HttpServiceReference;
using MoMoFriends;
using System.Web.SessionState;

namespace MaoMaoFriendsWeb.Ajax
{
    /// <summary>
    /// Summary description for LoginHandler
    /// </summary>
    public class LoginHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            using (UserOperateClient insUserBLL = new UserOperateClient())
            {
                UserInfo user = new UserInfo();
                user.LoginName = context.Request["username"].ToString();
                user.Password = CommonFunction.StringToMD5(context.Request["password"].ToString(), 16);

                try
                {
                    string pwd = insUserBLL.UserLogin(user);
                    if (pwd == user.Password)
                    {
                        context.Response.Write("success");
                        //存储session
                        context.Session["loginname"] = user.LoginName;
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