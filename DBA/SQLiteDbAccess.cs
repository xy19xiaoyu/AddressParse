//类名：SqlDbAccess 
//功能：SQLiteDataReader sqlReader; 专用的连接数据库的操作类
//作者：陈晓雨
//编写时间：2007-6-30

using System.Collections;
using System.Data.Sql;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using System;
namespace DBA
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public static class SQLiteDbAccess
    {

        #region "字段"
        private static string _Connstr;

        static SQLiteDbAccess()
        {
            _Connstr = "Data Source=address.db;Version=3";
        }

        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
        #endregion
        #region 属性
        public static string Connstr
        {
            get { return SQLiteDbAccess._Connstr; }
            set { SQLiteDbAccess._Connstr = value; }
        }
        #endregion
        #region "方法"

        /// <summary>
        /// 得到默认的数据库连接对象
        /// </summary>
        /// <return s></return s>
        /// <remarks>
        /// e.g.:SQLiteConnection conn = SqlDbAccess.GetSQLiteConnection();
        /// 得到一个SQL SERVER数据库连接对象 连接字符串为Web.Config文件中的ConnectionStrings("SqlServerConnString")节点
        /// </remarks>
        public static SQLiteConnection GetSQLiteConnection()
        {
            return new SQLiteConnection(_Connstr);
        }
        /// <summary>
        /// 得到连接字符串的数据库连接对象没啥用
        /// </summary>
        /// <remarks>
        /// e.g.: dim conn as new  SQLiteConnection = SqlDbAccess.GetSQLiteConnection("连接字符串...")
        ///       dim conn as new  SQLiteConnection("连接字符串...")
        /// 两者一样的
        /// </remarks>      
        /// <param name="ConnString">一个合法的连接字符串</param>
        /// <return s></return s>
        public static SQLiteConnection GetSQLiteConnection(string ConnString)
        {
            return new SQLiteConnection(ConnString);
        }

        public static int ExecNoQuery(string cmdText, params SQLiteParameter[] param)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = new SQLiteConnection(Connstr))
            {
                PreparativeCommand(cmd, conn, null, CommandType.Text, cmdText, param);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 使用默认的数据库连接 执行没有返回结果集的查询  
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数数组</param>
        /// <return s>返回所影响的行数</return s>
        /// <remarks></remarks>
        public static int ExecNoQuery(CommandType cmdType, string cmdText, params SQLiteParameter[] param)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = new SQLiteConnection(Connstr))
            {
                PreparativeCommand(cmd, conn, null, cmdType, cmdText, param);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        /// 使用指定的数据库连接字符串 执行没有返回结果集的查询 
        /// </summary>
        /// <param name="ConnString">数据库连接字符串</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数数组</param>
        /// <return s>返回所影响的行数</return s>
        /// <remarks></remarks>
        public static int ExecNoQuery(string ConnString, CommandType cmdType, string cmdText, params SQLiteParameter[] param)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(ConnString))
            {
                PreparativeCommand(cmd, con, null, cmdType, cmdText, param);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        /// 使用指定的数据库连接 执行没有返回结果集的查询 
        /// </summary>
        /// <param name="Conn">已存在的数据库连接对象</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数数组</param>
        /// <return s>返回所影响的行数</return s>
        /// <remarks></remarks>
        public static int ExecNoQuery(SQLiteConnection conn, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            int val = 0;
            PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms); ;
            val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 使用指定的数据库连接事务 执行没有返回结果集的查询  
        /// </summary>
        /// <param name="trans">已经存在的数据库事务</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数数组</param>
        /// <return s>返回所影响的行数</return s>
        /// <remarks></remarks>
        public static int ExecNoQuery(SQLiteTransaction trans, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PreparativeCommand(cmd, trans.Connection, trans, cmdType, cmdText, parms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 查询 使用默认的数据库连接 返回数据流
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>数据流</return s>
        /// <remarks></remarks>
        public static SQLiteDataReader ExecuteReader(CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader sqlReader;
            SQLiteConnection conn = new SQLiteConnection(Connstr);
            PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
            sqlReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return sqlReader;
        }
        /// <summary>
        /// 查询 使用指定的数据库连接字符串 返回数据流
        /// </summary>
        /// <param name="ConnString">一个合法的连接字符串</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>数据流</return s>
        /// <remarks></remarks>
        public static SQLiteDataReader ExecuteReader(string ConnString, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader sqlReader;
            SQLiteConnection conn = new SQLiteConnection(ConnString);
            PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
            sqlReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return sqlReader;
        }
        /// <summary>
        /// 查询 使用指定的数据库连接 返回数据流
        /// </summary>
        /// <param name="conn">已存在的数据库连接对象</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>数据流</return s>
        /// <remarks></remarks>
        public static SQLiteDataReader ExecuteReader(SQLiteConnection conn, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader sqlReader;
            PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
            sqlReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return sqlReader;
        }
        /// <summary>
        /// 查询 使用指定的数据库连接事务 返回数据流
        /// </summary>
        /// <param name="trans">已经存在的数据库事务</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>数据流</return s>
        /// <remarks></remarks>
        public static SQLiteDataReader ExecuteReader(SQLiteTransaction trans, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader sqlReader;
            PreparativeCommand(cmd, null, trans, cmdType, cmdText, parms);
            sqlReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return sqlReader;
        }

        /// <summary>
        /// 查询数据 使用默认的数据库连接 返回第一行的第一列
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s></return s>
        /// <remarks></remarks>
        public static int ExecuteScalarInt(string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = new SQLiteConnection(Connstr))
            {
                PreparativeCommand(cmd, conn, null, CommandType.Text, cmdText, parms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                int tmp = -1;
                if (val != null)
                {
                    int.TryParse(val.ToString(), out tmp);
                }
                return tmp;
            }

        }


        /// <summary>
        /// 查询数据 使用默认的数据库连接 返回第一行的第一列
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s></return s>
        /// <remarks></remarks>
        public static object ExecuteScalar(string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = new SQLiteConnection(Connstr))
            {
                PreparativeCommand(cmd, conn, null, CommandType.Text, cmdText, parms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }

        }

        /// <summary>
        /// 查询数据 使用默认的数据库连接 返回第一行的第一列
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s></return s>
        /// <remarks></remarks>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = new SQLiteConnection(Connstr))
            {
                PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }

        }
        /// <summary>
        /// 查询数据 使用指定的连接字符串 返回第一行的第一列
        /// </summary>
        /// <param name="ConnString">使用指定的连接字符串</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s></return s>
        /// <remarks></remarks>
        public static object ExecuteScalar(string ConnString, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = new SQLiteConnection(ConnString))
            {
                PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }

        }
        /// <summary>
        /// 查询数据 使用指定的数据库连接 返回第一行的第一列
        /// </summary>
        /// <param name="conn">一个已经开启的数据库连接</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s></return s>
        /// <remarks></remarks>
        public static object ExecuteScalar(SQLiteConnection conn, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();

            PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;

        }
        /// <summary>
        /// 查询数据 使用指定的数据库连接事务 返回第一行的第一列
        /// </summary>
        /// <param name="trans">一个已经开启的事务</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s></return s>
        /// <remarks></remarks>
        public static object ExecuteScalar(SQLiteTransaction trans, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PreparativeCommand(cmd, trans.Connection, trans, cmdType, cmdText, parms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;

        }


        /// <summary>
        /// 查询数据 使用默认的数据库连接   返回查询结果集
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataSet</return s>
        /// <remarks></remarks>
        public static DataSet GetDataSet(CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataSet ds = new DataSet();
            using (SQLiteConnection conn = new SQLiteConnection(Connstr))
            {
                PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }
        /// <summary>
        /// 查询数据 使用指定的连接字符串   返回查询结果集
        /// </summary>
        /// <param name="ConnString">一个合法的连接字符串</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataSet</return s>
        /// <remarks></remarks>
        public static DataSet GetDataSet(string ConnString, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataSet ds = new DataSet();
            using (SQLiteConnection conn = new SQLiteConnection(ConnString))
            {
                PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }
        /// <summary>
        /// 查询数据 使用指定的数据库连接   返回查询结果集
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataSet</return s>
        /// <remarks></remarks>
        public static DataSet GetDataSet(SQLiteConnection conn, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataSet ds = new DataSet();
            PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        /// <summary>
        /// 查询数据 使用的指定数据库连接事务 返回查询结果集
        /// </summary>
        /// <param name="trans">指定数据库连接事务</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataSet</return s>
        /// <remarks></remarks>
        public static DataSet GetDataSet(SQLiteTransaction trans, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataSet ds = new DataSet();
            PreparativeCommand(cmd, trans.Connection, trans, cmdType, cmdText, parms);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        public static DataTable getDB(String sql)
        {
            return GetDataTable(CommandType.Text, sql);
        }
        public static DataTable getDB(String sql, params SQLiteParameter[] parms)
        {
            return GetDataTable(CommandType.Text, sql, parms);
        }

        public static DataTable GetDataTable(string sql, params SQLiteParameter[] parms)
        {
            return GetDataTable(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 查询数据 使用默认的数据库连接 返回查询结果集
        /// </summary>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataTable</return s>
        /// <remarks></remarks>
        public static DataTable GetDataTable(CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataTable table = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(Connstr))
            {
                PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(table);
                cmd.Parameters.Clear();
                return table;
            }
        }
        /// <summary>
        /// 查询数据 使用指定的连接字符串   返回查询结果集
        /// </summary>
        /// <param name="ConnString">一个合法的连接字符串</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataTable</return s>
        /// <remarks></remarks>
        public static DataTable GetDataTable(string ConnString, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataTable table = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(ConnString))
            {
                PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(table);
                cmd.Parameters.Clear();
                return table;
            }
        }
        /// <summary>
        /// 查询数据 使用指定的数据库连接   返回查询结果集
        /// </summary>
        /// <param name="conn">一个打开的数据库连接</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataTable</return s>
        /// <remarks></remarks>
        public static DataTable GetDataTable(SQLiteConnection conn, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataTable table = new DataTable();
            PreparativeCommand(cmd, conn, null, cmdType, cmdText, parms);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(table);
            cmd.Parameters.Clear();
            return table;
        }
        /// <summary>
        /// 查询数据 使用的指定数据库连接事务 返回查询结果集
        /// </summary>
        /// <param name="trans">一个已经开启的事务</param>
        /// <param name="cmdType">查询类型T-SQL语句\存储过程</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="params">查询参数</param>
        /// <return s>DataTable</return s>
        /// <remarks></remarks>
        public static DataTable GetDataTable(SQLiteTransaction trans, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            DataTable table = new DataTable();
            PreparativeCommand(cmd, trans.Connection, trans, cmdType, cmdText, parms);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(table);
            cmd.Parameters.Clear();
            return table;
        }


        /// <summary>
        /// 为执行T-SQL语句做准备
        /// </summary>
        /// <param name="cmd">SQLiteCommand 对象</param>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="trans">数据库操作事务</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="cmdText">查询语句</param>
        /// <param name="parms">查询需要的参数数组</param>
        /// <remarks></remarks>
        private static void PreparativeCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, CommandType cmdType, string cmdText, params SQLiteParameter[] parms)
        {

            //如果连接没有打开 打开连接
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            //设置数据库连接
            cmd.Connection = conn;
            //查询语句
            cmd.CommandText = cmdText;
            //查询类型  T-SQL \ 存储过程
            cmd.CommandType = cmdType;

            //是使用事务提交还是不使用
            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            //如果查询参数不为空(给参数赋值)
            if (parms != null)
            {
                foreach (SQLiteParameter par in parms)
                {
                    cmd.Parameters.Add(par);
                }
            }
        }
        /// <summary>
        /// 缓存查询参数
        /// </summary>
        /// <param name="cacheKey">名字</param>
        /// <param name="parms">参数数组</param>
        /// <remarks>
        /// 保存需要缓存的参数数组
        /// </remarks>
        public static void staticCacheParameters(string cacheKey, SQLiteParameter[] parms)
        {
            parmCache[cacheKey] = parms;
        }
        /// <summary>
        /// 得到缓存的参数
        /// </summary>
        /// <param name="cacheKey">名字</param>
        /// <return s></return s>
        /// <remarks></remarks>
        public static SQLiteParameter[] GetCachedParameters(string cacheKey)
        {

            //判断是否保存了需要的参数
            SQLiteParameter[] parms = parmCache[cacheKey] as SQLiteParameter[];
            //如果没有就返回null
            if (parms == null)
            {
                return null;
            }
            SQLiteParameter[] clonedParms = new SQLiteParameter[parms.Length];
            //赋值
            for (int i = 0; i < parms.Length; i++)
            {
                clonedParms[1] = ((ICloneable)parms[i]).Clone() as SQLiteParameter;
            }
            return clonedParms;
        }
        #endregion

    }
}