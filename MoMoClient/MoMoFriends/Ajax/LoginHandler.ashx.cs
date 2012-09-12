//<%@ WebHandler Language="C#" Class="Handler" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoMoFriends.HttpServiceReference;

namespace MaoMaoFriendsWeb.Ajax
{
    /// <summary>
    /// Summary description for LoginHandler
    /// </summary>
    public class LoginHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (UserOperateClient insUserBLL = new UserOperateClient())
            {
                UserInfo user = new UserInfo();
                user.LoginName = context.Request["username"].ToString();
                user.Password = context.Request["password"].ToString();                

                try
                {
                    if (insUserBLL.UserLogin(user) == user.Password)
                    {
                        context.Response.Write("success");
                        //存储session
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