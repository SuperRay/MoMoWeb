using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MoMoLib.Common
{
    public class JsonFunction
    {
        # region dataTable转换成Json格式
        /// <summary>      
        /// dataTable转换成Json格式      
        /// </summary>      
        /// <param name="dt"></param>      
        /// <returns></returns>      
        public static string DatatableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName.ToString());
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        # endregion dataTable转换成Json格式


        # region DataSet转换成Json格式
        /// <summary>      
        /// DataSet转换成Json格式      
        /// </summary>      
        /// <param name="ds">DataSet</param>      
        /// <returns></returns>      
        public static string ToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(DatatableToJson(dt));
                json.Append("}");
            }
            return json.ToString();
        }
        # endregion

        #region Json转换成DataTable
        /// <summary>
        /// Json转换成DataTable
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            //取出表名  
            Regex rg = new Regex(@"(?<={)[^:]+(?=:/[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名  
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据  
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');

                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].ToString();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }
        #endregion
    }
}
