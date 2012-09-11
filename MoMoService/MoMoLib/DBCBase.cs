using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MoMoLib
{
    public class DBCBase : System.IDisposable
    {
        /// <summary>
        /// 数据连接
        /// </summary>
        protected SqlConnection Connection;
        private string connectionString;


        public DBCBase()
        {

        }
        /// <summary>
        /// 该构造函数使用新的newConnectionString代表的数据库（该设置在Web.Config）
        /// </summary>
        /// <param name="newConnectionString">Connection String to the associated database</param>
        public DBCBase(string newConnectionString)
        {
            //connectionString = ConfigurationManager.ConnectionStrings[newConnectionString].ConnectionString;
            //connectionString = ConfigurationSettings.AppSettings[newConnectionString];
            connectionString = "Data Source=127.0.0.1\\MYSQLSERVER;Initial Catalog=IBMS;Persist Security Info=True;User ID=sa;Password=123456";
            Connection = new SqlConnection(connectionString);
        }


        /// <summary>
        /// 连接字符串.
        /// </summary>
        protected string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        /// <summary>
        /// Private routine allowed only by this base class, it automates the task
        /// of building a SqlCommand object designed to obtain a return value from
        /// the stored procedure.
        /// </summary>
        /// <param name="storedProcName">Name of the stored procedure in the DB, eg. sp_DoTask</param>
        /// <param name="parameters">Array of IDataParameter objects containing parameters to the stored proc</param>
        /// <returns>Newly instantiated SqlCommand instance</returns>
        private SqlCommand BuildIntCommand(string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(storedProcName, parameters);

            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int,
                4, /* Size */
                ParameterDirection.ReturnValue,
                false, /* is nullable */
                0, /* byte precision */
                0, /* byte scale */
                string.Empty,
                DataRowVersion.Default,
                null));

            return command;
        }


        /// <summary>
        /// Builds a SqlCommand designed to return a SqlDataReader, and not
        /// an actual integer value.
        /// </summary>
        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="parameters">Array of IDataParameter objects</param>
        /// <returns></returns>

        private SqlCommand BuildQueryCommand(string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, Connection);
            command.CommandTimeout = 10800;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            return command;

        }


        /// <param name="storedProcName">Name of the stored procedure</param>
        /// <param name="parameters">存储过程参数数组</param>
        /// <param name="rowsAffected">Number of rows affected by the stored procedure.</param>
        /// <returns>An integer indicating return value of the stored procedure</returns>
        public int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            int result;

            Connection.Open();
            SqlCommand command = BuildIntCommand(storedProcName, parameters);
            using (this)
            {
                rowsAffected = command.ExecuteNonQuery();
            }
            result = (int)command.Parameters["ReturnValue"].Value;
            Connection.Close();
            return result;
        }

        /// <summary>
        /// Will run a stored procedure, can only be called by those classes deriving
        /// from this base. It returns a Object value the result of the stored
        /// procedure.
        /// </summary>
        /// <param name="storedProcName">存储过程名字</param>
        /// <param name="parameters">存储过程参数数组</param>
        /// <returns>返回记录第一行第一列的值</returns>
        public Object RunProcedureScalar(string storedProcName, IDataParameter[] parameters)
        {
            Object result;

            Connection.Open();
            try
            {
                SqlCommand command = BuildQueryCommand(storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;

                result = command.ExecuteScalar();
                Connection.Close();
            }
            catch (Exception ex)
            {
                Connection.Close();
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Will run a stored procedure, can only be called by those classes deriving
        /// from this base. It returns a SqlDataReader containing the result of the stored
        /// procedure.
        /// </summary>
        /// <param name="storedProcName">存储过程名字</param>
        /// <param name="parameters">存储过程参数数组</param>
        /// <returns>A newly instantiated SqlDataReader object</returns>
        public SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlDataReader returnReader;



            Connection.Open();
            try
            {
                SqlCommand command = BuildQueryCommand(storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;

                returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Connection.Close();
                throw ex;
            }
            return returnReader;
        }

        /// <summary>
        /// Creates a DataSet by running the stored procedure and placing the results
        /// of the query/proc into the given tablename.
        /// </summary>
        /// <param name="storedProcName">存储过程名字</param>
        /// <param name="parameters">存储过程参数数组</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            DataSet dataSet = new DataSet();
            Connection.Open();
            using (this)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                Connection.Close();
            }

            return dataSet;
        }

        /// <summary>
        /// Takes an -existing- dataset and fills the given table name with the results
        /// of the stored procedure.
        /// </summary>
        /// <param name="storedProcName">存储过程名字</param>
        /// <param name="parameters">存储过程参数数组</param>
        /// <param name="dataSet">被添加的DataSet</param>
        /// <param name="tableName">运行结果的TableName</param>
        /// <returns></returns>
        public void RunProcedure(string storedProcName, IDataParameter[] parameters, DataSet dataSet, string tableName)
        {
            Connection.Open();
            using (this)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildIntCommand(storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                Connection.Close();
            }
        }

        /// <summary>
        /// 运行一个SQL语句，没有返回值。可以有一个输出参数，返回影响的行数。
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="rowsAffected">输出参数，返回影响的行数</param>
        /// <returns></returns>
        public void RunSqlString(string sql, out int rowsAffected)
        {
            Connection.Open();
            using (this)
            {
                SqlCommand command = new SqlCommand(sql, Connection);
                command.CommandTimeout = 10800;
                rowsAffected = command.ExecuteNonQuery();
                Connection.Close();
                return;
            }
        }

        /// <summary>
        /// 运行一个SQL语句，返回SqlDataReader。输入参数：SQL语句。
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns>返回SqlDataReader，关闭SqlDataReader时候，数据连接关闭</returns>
        public SqlDataReader RunSqlString(string sql)
        {
            SqlDataReader returnReader;

            Connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(sql, Connection);
                command.CommandTimeout = 10800;
                returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Connection.Close();
                throw ex;
            }
            return returnReader;
        }

        /// <summary>
        /// 运行一个SQL语句，返回DataSet。输入参数：SQL语句，DataSet中Table名字。
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="sql">DataSet中Table名字</param>
        /// <returns>返回DataSet</returns>
        public DataSet RunSqlString(string sql, string tableName)
        {
            DataSet dataSet = new DataSet();
            Connection.Open();
            using (this)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter();

                sqlDA.SelectCommand = new SqlCommand(sql, Connection);
                sqlDA.SelectCommand.CommandTimeout = 10800;

                sqlDA.Fill(dataSet, tableName);
                Connection.Close();

                return dataSet;
            }
        }

        /// <summary>
        /// 已有DataSet,将查询结构的DataTable加入到DataSet.
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <param name="dataSet">被添加的DataSet</param>
        /// <param name="tableName">运行结果的TableName</param>
        /// <returns></returns>
        public void RunSqlString(string sql, DataSet dataSet, string tableName)
        {
            Connection.Open();
            using (this)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = new SqlCommand(sql, Connection);
                sqlDA.SelectCommand.CommandTimeout = 10800;
                sqlDA.Fill(dataSet, tableName);
                Connection.Close();
            }
        }
        /// <summary>
        /// 参数化查询，返回DataSet
        /// </summary>
        /// <param name="paraSql">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="tableName">返回DataSet的Table名字</param>
        /// <returns></returns>
        public DataSet RunParaString(string paraSql, IDataParameter[] parameters, string tableName)
        {
            DataSet dataSet = new DataSet();
            Connection.Open();
            using (this)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(paraSql, parameters);
                sqlDA.SelectCommand.CommandType = CommandType.Text;
                sqlDA.Fill(dataSet, tableName);
                Connection.Close();

                return dataSet;
            }
        }

        /// <summary>
        /// 已有DataSet,将查询结构的DataTable加入到DataSet.
        /// </summary>
        /// <param name="paraSql">存储过程名字</param>
        /// <param name="parameters">存储过程参数数组</param>
        /// <param name="dataSet">被添加的DataSet</param>
        /// <param name="tableName">运行结果的TableName</param>
        /// <returns></returns>
        public void RunParaString(string paraSql, IDataParameter[] parameters, DataSet dataSet, string tableName)
        {
            Connection.Open();
            using (this)
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(paraSql, parameters);
                sqlDA.SelectCommand.CommandType = CommandType.Text;
                sqlDA.Fill(dataSet, tableName);
                Connection.Close();
            }
        }

        /// <summary>
        /// 取得一个SqlDataReader
        /// </summary>
        /// <param name="paraSql">参数化查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader RunParaString(string paraSql, IDataParameter[] parameters)
        {
            SqlDataReader returnReader;

            Connection.Open();

            try
            {
                SqlCommand command = BuildQueryCommand(paraSql, parameters);
                command.CommandType = CommandType.Text;

                returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Connection.Close();
                throw ex;
            }
            return returnReader;
        }

        /// <summary>
        /// 运行一个参数化SQL语句，没有返回值。可以有一个输出参数，返回影响的行数。
        /// </summary>
        /// <param name="paraSql">要执行的SQL语句</param>
        /// <param name="parameters">参数</param> 
        /// <param name="rowsAffected">输出参数，返回影响的行数</param>
        /// <returns></returns>
        public void RunParaString(string paraSql, IDataParameter[] parameters, out int rowsAffected)
        {
            Connection.Open();
            using (this)
            {
                SqlCommand command = BuildQueryCommand(paraSql, parameters);
                command.CommandType = CommandType.Text;

                rowsAffected = command.ExecuteNonQuery();
                Connection.Close();
                return;
            }
        }

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.

            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
    }
}
