using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MoMoFriends.HttpServiceReference;

namespace MaoMaoFriendsWeb.Ajax
{
    /// <summary>
    /// Summary description for IDInfoHandler
    /// </summary>
    public class IDInfoHandler : IHttpHandler, IRequiresSessionState
    {        
        public void ProcessRequest(HttpContext context)
        {
            using (UserOperateClient insUserBLL = new UserOperateClient())
            {
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");
                UserInfo strUserInfo = new UserInfo();
                //strUserInfo.LoginName = context.Application.Equals("loginName").ToString();
                strUserInfo.LoginName =context.Session["loginname"].ToString();
                strUserInfo.UserName = System.Web.HttpUtility.UrlDecode(context.Request["username"]);
                strUserInfo.Birthday = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(context.Request["birthday"]));
                strUserInfo.Sexy = System.Web.HttpUtility.UrlDecode(context.Request["sex"]);
                strUserInfo.GraduteSchool = System.Web.HttpUtility.UrlDecode(context.Request["school"]);
                strUserInfo.Job = System.Web.HttpUtility.UrlDecode(context.Request["job"]);
                strUserInfo.MobilPhone = System.Web.HttpUtility.UrlDecode(context.Request["mobilphone"]);
                strUserInfo.QQ = System.Web.HttpUtility.UrlDecode(context.Request["qq"]);
                strUserInfo.Msn = System.Web.HttpUtility.UrlDecode(context.Request["msn"]);
                strUserInfo.Weibo = System.Web.HttpUtility.UrlDecode(context.Request["weibo"]);
                strUserInfo.EnglishName = System.Web.HttpUtility.UrlDecode(context.Request["englishname"]);
                try
                {                    
                    if (insUserBLL.UpdateUserInfo(strUserInfo) == true)
                    {
                        context.Response.Write("success");
                    }
                    else
                    {
                        context.Response.Write("fail");
                    }
                }
                catch (Exception ex)
                { }
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