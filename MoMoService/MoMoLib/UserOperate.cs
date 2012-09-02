using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MoMoLib
{
    public class UserOperate:IUserOperate
    {
        DBCBase dbCon;
        public UserOperate()
        {
            Console.WriteLine("The MoMoWeb awaits your question...");
            dbCon = new DBCBase("strConnection");
        }

        public bool UserLogin(string username, string pwd)
        {
            DataSet sdr = null;
            sdr = dbCon.RunSqlString("select * from dbo.人员表", "人员表");
            DataRow[] dr = sdr.Tables[0].Select("账号=01");
            if (dr.Length > 0)
            {
                string strResult = dr[0]["用户名"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
