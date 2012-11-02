using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MoMoFriends.HttpServiceReference;


namespace MoMoFriends.Ajax
{
    /// <summary>
    /// GetInfoHandler 的摘要说明
    /// </summary>
    public class GetInfoHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            using (UserOperateClient insUserBLL = new UserOperateClient())
            {
                string strLoginName = "";
                UserInfo userInfo = new UserInfo();
                strLoginName = context.Session["loginname"].ToString();
                try
                {
                    userInfo = insUserBLL.GetUserInfo(strLoginName);
                    
                    context.Response.Write(userInfo.LoginName);
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