using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoMoFriends.HttpServiceReference;

namespace MoMoFriends.Ajax
{
    /// <summary>
    /// ListCodeQ 的摘要说明
    /// </summary>
    public class ListCodeQ : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            using (UserOperateClient insUserBLL = new UserOperateClient())
            {
                string questions = "";
                questions = insUserBLL.ListCodeQuestion();
                context.Response.Write(questions);
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