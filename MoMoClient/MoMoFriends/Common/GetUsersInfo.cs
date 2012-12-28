using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoMoFriends.Common
{
    public class GetUsersInfo : Controller
    {
        public JsonResult AjaxHandler(MoMoFriends.CommonFunction.DataTableParameter param)
        {
            //return Json(new
            //{
            //    sEcho = param.sEcho,
            //    iTotalRecords = 50,
            //    iTotalDisplayRecords = 50,
            //    aaData = new List<object> {
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"},
            //                             new string[] {"黎明","1989-04-23","男","北京大学","建筑师","15527088888","123456","whut_kaf@hotmail.com","fhng","LiMing"}
            //                               }
            //}, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = 50,
                iTotalDisplayRecords = 50,
                aaData = new List<object> {
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[] {"1","公司信息","地址信息","城市信息"},
                                         new string[]{"1","公司信息","地址信息","城市信息"}
                                           }
            }, JsonRequestBehavior.AllowGet);

        }

    }
}