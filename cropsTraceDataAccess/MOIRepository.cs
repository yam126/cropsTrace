using ePioneer.Data.Kernel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cropsTraceDataAccess.Model;
using cropsTraceDataAccess.Entities;
using Common;

namespace cropsTraceDataAccess.Data
{
    public class MOIRepository : BaseDataProvider, IMOIRepository
    {
        #region Fields

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string m_connectionString;

        /// <summary>
        /// 数据库帮助类
        /// </summary>
        private static DbHelper m_dbHelper = null;
        #endregion

        public MOIRepository() : base(MOIRepository.m_connectionString)
        {
            CreateDBHelper();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public MOIRepository(String connectionString) : base(connectionString)
        {
            if (Database != null)
                m_dbHelper = Database;
        }

        #region Common

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="where">查询条件</param>
        /// <param name="oderBy">排序字符串</param>
        /// <param name="fields">字段列表</param>
        /// <returns>返回值</returns>
        public Task<PagerSet> GetPagerSetAsync(String tableName, Int32 pageIndex, Int32 pageSize, String where, String oderBy, String[] fields)
        {
            return GetPagerSet2Async(new PagerParameters(tableName, "ORDER BY " + oderBy, "WHERE " + where, pageIndex, pageSize, fields)
            {
                CacherSize = 2
            });
        }

        /// <summary>
        /// 分页方法重载
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="where">查询条件</param>
        /// <param name="oderBy">排序字符串</param>
        /// <returns>返回值</returns>
        public Task<PagerSet> GetPagerSetAsync(String tableName, Int32 pageIndex, Int32 pageSize, String where, String orderBy)
        {
            return GetPagerSet2Async(new PagerParameters(tableName, "ORDER BY " + orderBy, "WHERE " + where, pageIndex, pageSize));
        }

        /// <summary>
        /// 异步查询方法(有返回结果)
        /// </summary>
        /// <param name="commandText">SQL字符串</param>
        /// <returns>查询结果</returns>
        public Task<DataSet> ExecuteDataSetAsync(String commandText)
        {
            return m_dbHelper.ExecuteDataSetAsync(CommandType.Text, commandText);
        }

        /// <summary>
        /// 查询方法(有返回结果)
        /// </summary>
        /// <param name="commandText">SQL字符串</param>
        /// <returns>查询结果</returns>
        public DataSet ExecuteDataSet(String commandText)
        {
            return m_dbHelper.ExecuteDataSet(CommandType.Text, commandText);
        }

        /// <summary>
        /// 带参数查询方法(有返回结果)
        /// </summary>
        /// <param name="commandText">SQL字符串</param>
        /// <param name="commandType">SQL语句类型</param>
        /// <param name="sqlparms">参数集合</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, DbParameter[] sqlparms, out string message)
        {
            DataSet result = null;
            message = string.Empty;
            try
            {
                result = m_dbHelper.ExecuteDataSet(commandType, commandText, sqlparms);
            }
            catch (Exception exp)
            {
                message = exp.Message;
            }
            return result;
        }

        /// <summary>
        /// 异步查询方法(增删改语句)
        /// </summary>
        /// <param name="commandText">SQL字符串</param>
        /// <returns>查询结果</returns>
        public Task<Int32> ExecuteNonQueryAsync(String commandText)
        {
            return m_dbHelper.ExecuteNonQueryAsync(commandText);
        }

        /// <summary>
        /// 异步查询方法(增删改语句)
        /// </summary>
        /// <param name="commandText">SQL字符串</param>
        /// <returns>查询结果</returns>
        public Task<Int32> ExecuteNonQueryAsync(CommandType commandType,String commandText, params DbParameter[] commandParameters)
        {
            return m_dbHelper.ExecuteNonQueryAsync(commandType,commandText, commandParameters);
        }

        /// <summary>
        /// 查询方法(增删改查语句)
        /// </summary>
        /// <param name="commandText">SQL字符串</param>
        /// <returns>查询结果</returns>
        public Int32 ExecuteNonQuery(String commandText)
        {
            return m_dbHelper.ExecuteNonQuery(commandText);
        }

        public Int32 ExecuteNonQuery(CommandType commandType, String commandText, params DbParameter[] commandParameters)
        {
            return m_dbHelper.ExecuteNonQuery(commandType, commandText, commandParameters);
        }

        public List<T> QueryPage<T>(
             string SqlWhere,
             string SortField,
             string SortMethod,
             string FieldStr,
             string TableName,
             int PageSize,
             int CurPage,
             Func<DataRow, T, T> ReadDataRow,
             out int TotalNumber,
             out int PageCount,
             out string message)
        {
            message = string.Empty;
            PageCount = 0;
            TotalNumber = 0;
            DataSet ds = null;
            DataTable dt = null;
            List<T> result = new List<T>();
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("CurPage",CurPage),
                m_dbHelper.MakeInParam("PageSize",PageSize),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeOutParam("PageCount",typeof(System.Int32)),
                m_dbHelper.MakeInParam("FieldStr",FieldStr),
                m_dbHelper.MakeInParam("TableName",TableName),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "QueryPage", sqlparm);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            TotalNumber = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                T t = default(T);
                result.Add(ReadDataRow(dr, t));
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.FileInfo 增删改

        #region 增加数据
        /// <summary>
        /// 单条增加
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="message">消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertFileInfo(cropsTraceDataAccess.Model.FileInfo model)
        {
            #region 声明和初始化

            //返回值
            Message result = new Message(true,string.Empty);

            //存储过程参数
            List<DbParameter> parameters = null;
            #endregion

            if (model != null)
            {
                try
                {
                    parameters = SetDBParameter(model).ToList();
                    result = MessageHelper.GetMessage(m_dbHelper, "Create_FileInfo", parameters);
                }
                catch (Exception ex)
                {
                    result =  new Message(false, ex.Message);
                }
            }
            else
            {
                result = new Message(false, "参数为空不能添加");
            }

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertFileInfo(List<cropsTraceDataAccess.Model.FileInfo> lists)
        {
            int DbState = -1;
            Message result = new Message(true,string.Empty);
            if(lists==null)
                return new Message(false, "参数为空不能添加");
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkInsertSql(lists);
            try
            {
                DbState = ExecuteNonQuery(string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex) 
            {
                result = new Message(false, ex.Message);
            }
            return result;
        }
        #endregion

        #region 修改方法
        /// <summary>
        /// 单条修改
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">消息</param>
        /// <returns>修改条数</returns>
        public int UpdateFileInfo(cropsTraceDataAccess.Model.FileInfo model, string SqlWhere, out string message)
        {
            message = string.Empty;
            List<DbParameter> sqlparms = this.SetDBParameter(model).ToList<DbParameter>();
            sqlparms.Add(Database.MakeInParam("SqlWhere", SqlWhere));
            int result = -1;
            try
            {
                result = ExecuteNonQuery(CommandType.StoredProcedure, "Update_FileInfo", sqlparms.ToArray());
            }
            catch(Exception ex) 
            {
                message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public int UpdateFileInfo(List<cropsTraceDataAccess.Model.FileInfo> lists, string SqlWhere, out string message)
        {
            int result = -1;
            message = string.Empty;
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkUpdateSql(lists, SqlWhere);
            try
            {
                result = ExecuteNonQuery(CommandType.Text, string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 删除方法

        /// <summary>
        /// 删除文件信息方法
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public async Task<Message> DeleteFileInfo(string where) 
        {
            #region 声明变量

            string sql = string.Empty;

            //返回值
            Message result = new Message(true,string.Empty);

            //数据库返回状态
            int DbState = -1;
            #endregion

            if (string.IsNullOrEmpty(where)) 
            {
                result = new Message(false,"删除条件不能为空");
                return result;
            }

            sql = $" delete from FileInfo where {where} ";

            try 
            {
                DbState = ExecuteNonQuery(CommandType.Text, sql);
            } 
            catch (Exception ex) 
            {
                result = new Message(false,ex.Message);
            }

            return result;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.FileInfo> QueryFileInfo(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<cropsTraceDataAccess.Model.FileInfo> result = new List<cropsTraceDataAccess.Model.FileInfo>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere))
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_FileInfo", sqlparms);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.FileInfo model = new cropsTraceDataAccess.Model.FileInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.FileInfo> QueryFileInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<cropsTraceDataAccess.Model.FileInfo> result = new List<cropsTraceDataAccess.Model.FileInfo>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_FileInfo_Page", sqlparm);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.FileInfo model = new cropsTraceDataAccess.Model.FileInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #endregion

        #region cropsTraceDataAccess.Model.FileInfo 文件信息基础方法

        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref cropsTraceDataAccess.Model.FileInfo model, DataRow dr)
        {
            //文件编号[自增列因为目前只是本表查询]
            model.FileId = Convert.IsDBNull(dr["FileId"]) ? 0 : Convert.ToInt64(dr["FileId"]);
            //农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
            model.CropsId = Convert.IsDBNull(dr["CropsId"]) ? 0 : Convert.ToInt64(dr["CropsId"]);
            //周期记录编号[外键对应生长周期表GrowthInfo字段RecordId关系多对1]
            model.GrowthRecordId = Convert.IsDBNull(dr["GrowthRecordId"]) ? 0 : Convert.ToInt64(dr["GrowthRecordId"]);
            //文件名
            model.FileName = Convert.IsDBNull(dr["FileName"]) ? string.Empty : Convert.ToString(dr["FileName"]).Trim();
            //文件URL路径
            model.FileUrl = Convert.IsDBNull(dr["FileUrl"]) ? string.Empty : Convert.ToString(dr["FileUrl"]).Trim();
            //文件大小
            model.FileLength = Convert.IsDBNull(dr["FileLength"]) ? 0 : Convert.ToInt64(dr["FileLength"]);
            //创建时间
            model.CreatedDateTime = Convert.IsDBNull(dr["CreatedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["CreatedDateTime"]);
            //显示参数JSON字符串
            model.ShowParamJson = Convert.IsDBNull(dr["ShowParamJson"]) ? string.Empty : Convert.ToString(dr["ShowParamJson"]).Trim();
        }

        ///<summary>
        ///检查是否空值
        ///</summary>
        private void CheckEmpty(ref List<cropsTraceDataAccess.Model.FileInfo> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                //文件编号[自增列因为目前只是本表查询]
                lists[i].FileId = lists[i].FileId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].FileId);
                //农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
                lists[i].CropsId = lists[i].CropsId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].CropsId);
                //周期记录编号[外键对应生长周期表GrowthInfo字段RecordId关系多对1]
                lists[i].GrowthRecordId = lists[i].GrowthRecordId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].GrowthRecordId);
                //文件名
                lists[i].FileName = string.IsNullOrEmpty(lists[i].FileName) ? string.Empty : Convert.ToString(lists[i].FileName).Trim();
                //文件URL路径
                lists[i].FileUrl = string.IsNullOrEmpty(lists[i].FileUrl) ? string.Empty : Convert.ToString(lists[i].FileUrl).Trim();
                //文件大小
                lists[i].FileLength = lists[i].FileLength == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].FileLength);
                //创建时间
                lists[i].CreatedDateTime = lists[i].CreatedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].CreatedDateTime.GetValueOrDefault());
                //显示参数JSON字符串
                lists[i].ShowParamJson = string.IsNullOrEmpty(lists[i].ShowParamJson) ? string.Empty : Convert.ToString(lists[i].ShowParamJson).Trim();
            }
        }

        ///<summary>
        ///检查是否超过长度
        ///</summary>
        ///<param name="lists">数据集</param>
        ///<param name="message">错误消息</param>
        private void CheckMaxLength(ref List<cropsTraceDataAccess.Model.FileInfo> lists, out string message)
        {
            #region 声明变量

            //错误消息
            message = string.Empty;

            //超过的长度
            int OutLength = 0;
            #endregion

            #region 循环验证长度
            for (int i = 0; i < lists.Count; i++)
            {
                if (!string.IsNullOrEmpty(lists[i].FileName))
                {
                    if (lists[i].FileName.Length > 150)
                    {
                        OutLength = lists[i].FileName.Length - 150;
                        message += "字段名[FileName]描述[文件名]超长、字段最长[150]实际" + lists[i].FileName.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].FileUrl))
                {
                    if (lists[i].FileUrl.Length > 150)
                    {
                        OutLength = lists[i].FileUrl.Length - 150;
                        message += "字段名[FileUrl]描述[文件URL路径]超长、字段最长[150]实际" + lists[i].FileUrl.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].ShowParamJson))
                {
                    if (lists[i].ShowParamJson.Length > 8000)
                    {
                        OutLength = lists[i].ShowParamJson.Length-8000;
                        message += "字段名[ShowParamJson]描述[显示参数JSON字符串]超长、字段最长[8000]实际" + lists[i].ShowParamJson.Length + "超过长度" + OutLength + ",";
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(message)) message = message.Substring(0, message.Length - 1);
        }

        ///<summary>
        ///赋值数据行
        ///</summary>
        ///<param name="model">数据行model</param>
        public DbParameter[] SetDBParameter(cropsTraceDataAccess.Model.FileInfo model)
        {
            #region 赋值Sql参数
            DbParameter[] result = new DbParameter[]
            {
                 Database.MakeInParam("FileId",model.FileId),
                 Database.MakeInParam("CropsId",model.CropsId),
                 Database.MakeInParam("GrowthRecordId",model.GrowthRecordId),
                 Database.MakeInParam("FileName",model.FileName),
                 Database.MakeInParam("FileUrl",model.FileUrl),
                 Database.MakeInParam("FileLength",model.FileLength),
                 Database.MakeInParam("CreatedDateTime",model.CreatedDateTime),
                 Database.MakeInParam("ShowParamJson",model.ShowParamJson),
            };
            #endregion

            return result;
        }

        ///<summary>
        ///生成插入Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<returns>插入Sql语句字符串数组</returns>
        private List<string> MarkInsertSql(List<cropsTraceDataAccess.Model.FileInfo> lists)
        {
            List<string> result = new List<string>();
            foreach (cropsTraceDataAccess.Model.FileInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "insert into FileInfo(";
                Sql += "FileId,";
                Sql += "CropsId,";
                Sql += "GrowthRecordId,";
                Sql += "FileName,";
                Sql += "FileUrl,";
                Sql += "FileLength,";
                Sql += "CreatedDateTime,";
                Sql += "ShowParamJson";
                Sql += ") values(";
                Sql += "'" + FilteSQLStr(model.FileId) + "',";
                Sql += "'" + FilteSQLStr(model.CropsId) + "',";
                Sql += "'" + FilteSQLStr(model.GrowthRecordId) + "',";
                Sql += "'" + FilteSQLStr(model.FileName) + "',";
                Sql += "'" + FilteSQLStr(model.FileUrl) + "',";
                Sql += "'" + FilteSQLStr(model.FileLength) + "',";
                Sql += "CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "'" + model.ShowParamJson + "'";
                Sql += ")";
                #endregion
                result.Add(Sql);
            }
            return result;
        }

        ///<summary>
        ///生成更新Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<param name="SqlWhere">更新条件</param>
        ///<returns>更新Sql语句字符串数组</returns>
        private List<string> MarkUpdateSql(List<cropsTraceDataAccess.Model.FileInfo> lists, string SqlWhere)
        {
            List<string> result = new List<string>();
            foreach (cropsTraceDataAccess.Model.FileInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "update FileInfo set ";
                Sql += "FileId='" + FilteSQLStr(model.FileId) + "',";
                Sql += "CropsId='" + FilteSQLStr(model.CropsId) + "',";
                Sql += "GrowthRecordId='" + FilteSQLStr(model.GrowthRecordId) + "',";
                Sql += "FileName='" + FilteSQLStr(model.FileName) + "',";
                Sql += "FileUrl='" + FilteSQLStr(model.FileUrl) + "',";
                Sql += "FileLength='" + FilteSQLStr(model.FileLength) + "',";
                Sql += "CreatedDateTime=CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "ShowParamJson='" + FilteSQLStr(model.ShowParamJson) + "'";
                if (!string.IsNullOrEmpty(SqlWhere))
                    Sql += " Where " + SqlWhere;
                #endregion
                result.Add(Sql);
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.GrowthInfo 增删改

        #region 增加数据
        /// <summary>
        /// 单条增加
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="message">消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertGrowthInfo(cropsTraceDataAccess.Model.GrowthInfo model)
        {
            #region 声明和初始化

            //返回值
            Message result = new Message(true,string.Empty);

            //存储过程参数
            List<DbParameter> parameters = null;
            #endregion

            if (model != null)
            {
                try
                {
                    parameters = SetDBParameter(model).ToList();
                    result = await MessageHelper.GetMessageAsync(m_dbHelper, "Create_GrowthInfo", parameters);
                }
                catch (Exception ex)
                {
                    result = new Message(false, ex.Message);
                }
            }
            else
            {
                result = new Message(false, "参数为空不能添加");
            }

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertGrowthInfo(List<cropsTraceDataAccess.Model.GrowthInfo> lists)
        {
            int DbState = -1;
            Message result = new Message(true,string.Empty);
            if (lists == null)
                return new Message(false, "参数为空不能添加");
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkInsertSql(lists);
            try
            {
                DbState = ExecuteNonQuery(string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            return result;
        }
        #endregion

        #region 修改方法
        /// <summary>
        /// 单条修改
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">消息</param>
        /// <returns>修改条数</returns>
        public async Task<Message> UpdateGrowthInfo(cropsTraceDataAccess.Model.GrowthInfo model, string SqlWhere)
        {
            List<DbParameter> sqlparms = this.SetDBParameter(model).ToList<DbParameter>();
            sqlparms.Add(Database.MakeInParam("SqlWhere", SqlWhere));
            Message result = new Message(true, string.Empty);
            int DbState = -1;
            try
            {
                DbState = ExecuteNonQuery(CommandType.StoredProcedure, "Update_GrowthInfo", sqlparms.ToArray());
            }
            catch (Exception ex)
            {
                result=new Message(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public async Task<Message> UpdateGrowthInfo(List<cropsTraceDataAccess.Model.GrowthInfo> lists, string SqlWhere)
        {
            int DbState = -1;
            Message result = new Message(true, string.Empty);
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkUpdateSql(lists, SqlWhere);
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            return result;
        }
        #endregion

        #region 删除方法

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="IDStr">编号字符串</param>
        /// <returns>返回消息</returns>
        public async Task<Message> DeleteGrowthInfo(string IDStr)
        {
            #region 声明和初始化

            //错误消息
            string message = string.Empty;

            //返回值
            Message result = null;

            //SQL语句
            string sql = string.Empty;

            //数据库状态
            int DbState = -1;
            #endregion

            //初始化SQL语句
            sql = $" delete from GrowthInfo where RecordId in ({IDStr}) ";

            #region 执行SQL语句
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = new Message(false, ex.Message);
            }
            #endregion

            if (result == null)
                result = new Message(true, string.Empty);

            return result;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.GrowthInfo> QueryGrowthInfo(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<cropsTraceDataAccess.Model.GrowthInfo> result = new List<cropsTraceDataAccess.Model.GrowthInfo>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere)) 
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_GrowthInfo", sqlparms);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.GrowthInfo model = new cropsTraceDataAccess.Model.GrowthInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.GrowthInfo> QueryGrowthInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<cropsTraceDataAccess.Model.GrowthInfo> result = new List<cropsTraceDataAccess.Model.GrowthInfo>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_GrowthInfo_Page", sqlparm);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.GrowthInfo model = new cropsTraceDataAccess.Model.GrowthInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 返回生长周期所有年份
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllGrowthInfoYear(int CompanyId,out string message) 
        {
            #region 声明变量

            //SQL语句
            string sql = string.Empty;

            //错误消息
            message = string.Empty;

            //年份列表
            List<int> result = new List<int>();

            DataSet ds = null;

            DataTable dt = null;
            #endregion

            sql = $"select Year(CreatedDateTime) as Year from vw_GrowthInfo where CompanyId='{CompanyId}'group by Year(CreatedDateTime) order by Year desc";
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion

            foreach (DataRow dr in dt.Rows)
            {
                object itemYear = Utils.GetDataRow(dr, "Year");
                result.Add(itemYear==null?0:Convert.ToInt32(itemYear));
            }
            return result;
        }
        #endregion

        #endregion

        #region cropsTraceDataAccess.Model.GrowthInfo 生长信息基础方法

        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref GrowthInfo model, DataRow dr)
        {
            //记录编号[用雪花ID生成]
            model.RecordId = Convert.IsDBNull(dr["RecordId"]) ? 0 : Convert.ToInt64(dr["RecordId"]);
            //泵房编号[外键值对应PumpHouseInfo表字段PumpId多对1]
            model.PumpId = Convert.IsDBNull(dr["PumpId"]) ? 0 : Convert.ToInt64(dr["PumpId"]);
            //农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
            model.CropsId = Convert.IsDBNull(dr["CropsId"]) ? 0 : Convert.ToInt64(dr["CropsId"]);
            //周期名称
            model.GrowthName = Convert.IsDBNull(dr["GrowthName"]) ? string.Empty : Convert.ToString(dr["GrowthName"]).Trim();
            //位置名称
            model.LandName = Convert.IsDBNull(dr["LandName"]) ? string.Empty : Convert.ToString(dr["LandName"]).Trim();
            //株高
            model.PlantHeight = Convert.IsDBNull(dr["PlantHeight"]) ? 0 : Convert.ToDecimal(dr["PlantHeight"]);
            //胸径
            model.DBH = Convert.IsDBNull(dr["DBH"]) ? 0 : Convert.ToDecimal(dr["DBH"]);
            //叶片数
            model.NumberOfBlades = Convert.IsDBNull(dr["NumberOfBlades"]) ? 0 : Convert.ToInt32(dr["NumberOfBlades"]);
            //出苗率
            model.EmergenceRate = Convert.IsDBNull(dr["EmergenceRate"]) ? 0 : Convert.ToDecimal(dr["EmergenceRate"]);
            //创建时间
            model.CreatedDateTime = Convert.IsDBNull(dr["CreatedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["CreatedDateTime"]);
            //修改时间
            model.ModifiedDateTime = Convert.IsDBNull(dr["ModifiedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["ModifiedDateTime"]);
        }

        ///<summary>
        ///检查是否空值
        ///</summary>
        private void CheckEmpty(ref List<GrowthInfo> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                //记录编号[用雪花ID生成]
                lists[i].RecordId = lists[i].RecordId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].RecordId);
                //泵房编号[外键值对应PumpHouseInfo表字段PumpId多对1]
                lists[i].PumpId = lists[i].PumpId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].PumpId);
                //农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
                lists[i].CropsId = lists[i].CropsId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].CropsId);
                //周期名称
                lists[i].GrowthName = string.IsNullOrEmpty(lists[i].GrowthName) ? string.Empty : Convert.ToString(lists[i].GrowthName).Trim();
                //位置名称
                lists[i].LandName = string.IsNullOrEmpty(lists[i].LandName) ? string.Empty : Convert.ToString(lists[i].LandName).Trim();
                //株高
                lists[i].PlantHeight = lists[i].PlantHeight == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].PlantHeight);
                //胸径
                lists[i].DBH = lists[i].DBH == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].DBH);
                //叶片数
                lists[i].NumberOfBlades = lists[i].NumberOfBlades == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].NumberOfBlades);
                //出苗率
                lists[i].EmergenceRate = lists[i].EmergenceRate == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].EmergenceRate);
                //创建时间
                lists[i].CreatedDateTime = lists[i].CreatedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].CreatedDateTime.GetValueOrDefault());
                //修改时间
                lists[i].ModifiedDateTime = lists[i].ModifiedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].ModifiedDateTime.GetValueOrDefault());
            }
        }

        ///<summary>
        ///赋值数据行
        ///</summary>
        ///<param name="model">GSK货品移动表C21A</param>
        public DbParameter[] SetDBParameter(GrowthInfo model)
        {
            #region 赋值Sql参数
            DbParameter[] result = new DbParameter[]
            {
                 Database.MakeInParam("RecordId",model.RecordId),
                 Database.MakeInParam("PumpId",model.PumpId),
                 Database.MakeInParam("CropsId",model.CropsId),
                 Database.MakeInParam("GrowthName",model.GrowthName),
                 Database.MakeInParam("LandName",model.LandName),
                 Database.MakeInParam("PlantHeight",model.PlantHeight),
                 Database.MakeInParam("DBH",model.DBH),
                 Database.MakeInParam("NumberOfBlades",model.NumberOfBlades),
                 Database.MakeInParam("EmergenceRate",model.EmergenceRate),
                 Database.MakeInParam("CreatedDateTime",model.CreatedDateTime),
                 Database.MakeInParam("ModifiedDateTime",model.ModifiedDateTime),
            };
            #endregion

            return result;
        }

        ///<summary>
        ///生成插入Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<returns>插入Sql语句字符串数组</returns>
        private List<string> MarkInsertSql(List<GrowthInfo> lists)
        {
            List<string> result = new List<string>();
            foreach (GrowthInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "insert into GrowthInfo(";
                Sql += "RecordId,";
                Sql += "PumpId,";
                Sql += "CropsId,";
                Sql += "GrowthName,";
                Sql += "LandName,";
                Sql += "PlantHeight,";
                Sql += "DBH,";
                Sql += "NumberOfBlades,";
                Sql += "EmergenceRate,";
                Sql += "CreatedDateTime,";
                Sql += "ModifiedDateTime";
                Sql += ") values(";
                Sql += "'" + FilteSQLStr(model.RecordId) + "',";
                Sql += "'" + FilteSQLStr(model.PumpId) + "',";
                Sql += "'" + FilteSQLStr(model.CropsId) + "',";
                Sql += "'" + FilteSQLStr(model.GrowthName) + "',";
                Sql += "'" + FilteSQLStr(model.LandName) + "',";
                Sql += "'" + FilteSQLStr(model.PlantHeight) + "',";
                Sql += "'" + FilteSQLStr(model.DBH) + "',";
                Sql += "'" + FilteSQLStr(model.NumberOfBlades) + "',";
                Sql += "'" + FilteSQLStr(model.EmergenceRate) + "',";
                Sql += "CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "CAST('" + model.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME)";
                Sql += ")";
                #endregion
                result.Add(Sql);
            }
            return result;
        }

        ///<summary>
        ///生成更新Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<param name="SqlWhere">更新条件</param>
        ///<returns>更新Sql语句字符串数组</returns>
        private List<string> MarkUpdateSql(List<GrowthInfo> lists, string SqlWhere)
        {
            List<string> result = new List<string>();
            foreach (GrowthInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "update GrowthInfo set ";
                Sql += "RecordId='" + FilteSQLStr(model.RecordId) + "',";
                Sql += "PumpId='" + FilteSQLStr(model.PumpId) + "',";
                Sql += "CropsId='" + FilteSQLStr(model.CropsId) + "',";
                Sql += "GrowthName='" + FilteSQLStr(model.GrowthName) + "',";
                Sql += "LandName='" + FilteSQLStr(model.LandName) + "',";
                Sql += "PlantHeight='" + FilteSQLStr(model.PlantHeight) + "',";
                Sql += "DBH='" + FilteSQLStr(model.DBH) + "',";
                Sql += "NumberOfBlades='" + FilteSQLStr(model.NumberOfBlades) + "',";
                Sql += "EmergenceRate='" + FilteSQLStr(model.EmergenceRate) + "',";
                Sql += "CreatedDateTime=CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "ModifiedDateTime=CAST('" + model.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME)";
                if (!string.IsNullOrEmpty(SqlWhere))
                    Sql += " Where " + SqlWhere;
                #endregion
                result.Add(Sql);
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.PumpHouseInfo 增删改

        #region 增加数据
        /// <summary>
        /// 单条增加
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="message">消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertPumpHouseInfo(cropsTraceDataAccess.Model.PumpHouseInfo model)
        {
            #region 声明和初始化

            //返回值
            Message result = new Message(true, string.Empty);

            //存储过程参数
            List<DbParameter> parameters = null;
            #endregion

            if (model != null)
            {
                try
                {
                    parameters = SetDbParameter(model).ToList();
                    result = await MessageHelper.GetMessageForObjectAsync<Id32>(m_dbHelper, "Create_PumpHouseInfo", parameters);
                }
                catch (Exception ex)
                {
                    result = new Message(false, ex.Message);
                }
            }
            else
            {
                result = new Message(false, "参数为空不能添加");
            }

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertPumpHouseInfo(List<cropsTraceDataAccess.Model.PumpHouseInfo> lists)
        {
            int DbState = -1;
            Message result = new Message(true,string.Empty);
            if (lists == null)
                return new Message(false, "参数为空不能添加");
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkInsertSql(lists);
            try
            {
                DbState = ExecuteNonQuery(string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            return result;
        }
        #endregion

        #region 修改方法

        /// <summary>
        /// 单条修改
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">消息</param>
        /// <returns>修改条数</returns>
        public async Task<Message> UpdatePumpHouseInfo(cropsTraceDataAccess.Model.PumpHouseInfo model, string SqlWhere)
        {
            List<DbParameter> sqlparms = this.SetDbParameter(model).ToList<DbParameter>();
            sqlparms.Add(Database.MakeInParam("SqlWhere", SqlWhere));
            Message result = new Message(true, string.Empty);
            int DbState = -1;
            try
            {
                DbState = ExecuteNonQuery(CommandType.StoredProcedure, "Update_PumpHouseInfo", sqlparms.ToArray());
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public async Task<Message> UpdatePumpHouseInfo(List<cropsTraceDataAccess.Model.PumpHouseInfo> lists, string SqlWhere)
        {
            int DbState = -1;
            Message result=new Message(true, string.Empty);
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkUpdateSql(lists, SqlWhere);
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result=new Message(false, ex.Message);
            }
            return result;
        }
        #endregion

        #region 批量删除泵房信息

        /// <summary>
        /// 批量删除泵房信息
        /// </summary>
        /// <param name="IDStr">泵房编号字符串</param>
        /// <returns></returns>
        public async Task<Message> BatchDeletePumpHouse(string IDStr) 
        {
            #region 声明变量

            //数据库状态返回值
            int DbState = -1;

            //文件删除SQL语句
            string deleteFileSql = string.Empty;

            //生长数据删除SQL语句
            string deleteGrowthInfoSql = string.Empty;

            //删除泵房数据SQL语句
            string deletePumpHouseInfoSql = string.Empty;
            
            //返回值
            Message result=new Message(true, string.Empty);
            #endregion

            #region 文件删除
            deleteFileSql = $@"delete from FileInfo where GrowthRecordId in (
                                    select gw.RecordId from GrowthInfo gw where gw.PumpId in (
                                         select phi.PumpId from PumpHouseInfo phi where phi.PumpId in ({IDStr})
                                    )
                               )";
            #endregion

            #region 生长数据删除
            deleteGrowthInfoSql = $@"delete from GrowthInfo where PumpId in (
                                         select phi.PumpId from PumpHouseInfo phi where phi.PumpId in ({IDStr})
                                   )";
            #endregion

            //泵房删除SQL语句
            deletePumpHouseInfoSql = $"delete from PumpHouseInfo where PumpId in ({IDStr})";

            #region 删除文件
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, deleteFileSql);
                result = new Message(true, string.Empty);
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            if (!result.Successful) 
            {
                string errorMessage = result.Content;
                result = new Message(false, $"文件删除出错，原因[{errorMessage}]");
                return result;
            }
            #endregion


            #region 删除生长数据
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, deleteGrowthInfoSql);
                result = new Message(true, string.Empty);
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            if (!result.Successful)
            {
                string errorMessage = result.Content;
                result = new Message(false, $"生长数据删除出错，原因[{errorMessage}]");
                return result;
            }
            #endregion

            #region 删除泵房数据
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, deletePumpHouseInfoSql);
                result = new Message(true, string.Empty);
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            if (!result.Successful)
            {
                string errorMessage = result.Content;
                result = new Message(false, $"泵房数据删除出错，原因[{errorMessage}]");
                return result;
            }
            #endregion


            return result;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.PumpHouseInfo> QueryPumpHouseInfo(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<cropsTraceDataAccess.Model.PumpHouseInfo> result = new List<cropsTraceDataAccess.Model.PumpHouseInfo>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere)) 
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_PumpHouseInfo", sqlparms);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.PumpHouseInfo model = new cropsTraceDataAccess.Model.PumpHouseInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.PumpHouseInfo> QueryPumpHouseInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<cropsTraceDataAccess.Model.PumpHouseInfo> result = new List<cropsTraceDataAccess.Model.PumpHouseInfo>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_PumpHouseInfo_Page", sqlparm);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.PumpHouseInfo model = new cropsTraceDataAccess.Model.PumpHouseInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #endregion

        #region cropsTraceDataAccess.Model.PumpHouseInfo 泵房信息基础方法

        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref PumpHouseInfo model, DataRow dr)
        {
            //泵房编号
            model.PumpId = Convert.IsDBNull(dr["PumpId"]) ? 0 : Convert.ToInt64(dr["PumpId"]);
            //公司编号
            model.CompanyId = Convert.IsDBNull(dr["CompanyId"]) ? 0 : Convert.ToInt32(dr["CompanyId"]);
            //泵房名称
            model.PumpHouseName = Convert.IsDBNull(dr["PumpHouseName"]) ? string.Empty : Convert.ToString(dr["PumpHouseName"]).Trim();
            //泵房种类
            model.PumpHouseClass = Convert.IsDBNull(dr["PumpHouseClass"]) ? string.Empty : Convert.ToString(dr["PumpHouseClass"]).Trim();
            //负责人
            model.PersonIinCharge = Convert.IsDBNull(dr["PersonIinCharge"]) ? string.Empty : Convert.ToString(dr["PersonIinCharge"]).Trim();
            //创建时间
            model.CreatedDateTime = Convert.IsDBNull(dr["CreatedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["CreatedDateTime"]);
            //修改时间
            model.ModifiedDateTime = Convert.IsDBNull(dr["ModifiedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["ModifiedDateTime"]);
            //监控地址
            model.MonitoringAddress= Convert.IsDBNull(dr["MonitoringAddress"]) ? string.Empty : Convert.ToString(dr["MonitoringAddress"]).Trim();
        }

        ///<summary>
        ///检查是否空值
        ///</summary>
        private void CheckEmpty(ref List<PumpHouseInfo> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                //泵房编号
                lists[i].PumpId = lists[i].PumpId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].PumpId);
                //公司编号
                lists[i].CompanyId = lists[i].CompanyId == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].CompanyId);
                //泵房名称
                lists[i].PumpHouseName = string.IsNullOrEmpty(lists[i].PumpHouseName) ? string.Empty : Convert.ToString(lists[i].PumpHouseName).Trim();
                //泵房种类
                lists[i].PumpHouseClass = string.IsNullOrEmpty(lists[i].PumpHouseClass) ? string.Empty : Convert.ToString(lists[i].PumpHouseClass).Trim();
                //负责人
                lists[i].PersonIinCharge = string.IsNullOrEmpty(lists[i].PersonIinCharge) ? string.Empty : Convert.ToString(lists[i].PersonIinCharge).Trim();
                //创建时间
                lists[i].CreatedDateTime = lists[i].CreatedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].CreatedDateTime.GetValueOrDefault());
                //修改时间
                lists[i].ModifiedDateTime = lists[i].ModifiedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].ModifiedDateTime.GetValueOrDefault());
                //监控地址
                lists[i].MonitoringAddress = string.IsNullOrEmpty(lists[i].MonitoringAddress) ? string.Empty : Convert.ToString(lists[i].MonitoringAddress).Trim();
            }
        }

        ///<summary>
        ///赋值数据行
        ///</summary>
        ///<param name="model">GSK货品移动表C21A</param>
        public DbParameter[] SetDbParameter(PumpHouseInfo model)
        {
            #region 赋值Sql参数
            DbParameter[] result = new DbParameter[]
            {
                 Database.MakeInParam("PumpId",model.PumpId),
                 Database.MakeInParam("CompanyId",model.CompanyId),
                 Database.MakeInParam("PumpHouseName",model.PumpHouseName),
                 Database.MakeInParam("PumpHouseClass",model.PumpHouseClass),
                 Database.MakeInParam("PersonIinCharge",model.PersonIinCharge),
                 Database.MakeInParam("CreatedDateTime",model.CreatedDateTime),
                 Database.MakeInParam("ModifiedDateTime",model.ModifiedDateTime),
                 Database.MakeInParam("MonitoringAddress",model.MonitoringAddress)
            };
            #endregion

            return result;
        }

        ///<summary>
        ///生成插入Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<returns>插入Sql语句字符串数组</returns>
        private List<string> MarkInsertSql(List<PumpHouseInfo> lists)
        {
            List<string> result = new List<string>();
            foreach (PumpHouseInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "insert into PumpHouseInfo(";
                Sql += "PumpId,";
                Sql += "CompanyId,";
                Sql += "PumpHouseName,";
                Sql += "PumpHouseClass,";
                Sql += "PersonIinCharge,";
                Sql += "CreatedDateTime,";
                Sql += "ModifiedDateTime,";
                Sql += "MonitoringAddress";
                Sql += ") values(";
                Sql += "'" + FilteSQLStr(model.PumpId) + "',";
                Sql += "'" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "'" + FilteSQLStr(model.PumpHouseName) + "',";
                Sql += "'" + FilteSQLStr(model.PumpHouseClass) + "',";
                Sql += "'" + FilteSQLStr(model.PersonIinCharge) + "',";
                Sql += "CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "CAST('" + model.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "'" + model.MonitoringAddress + "'";
                Sql += ")";
                #endregion
                result.Add(Sql);
            }
            return result;
        }

        ///<summary>
        ///生成更新Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<param name="SqlWhere">更新条件</param>
        ///<returns>更新Sql语句字符串数组</returns>
        private List<string> MarkUpdateSql(List<PumpHouseInfo> lists, string SqlWhere)
        {
            List<string> result = new List<string>();
            foreach (PumpHouseInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "update PumpHouseInfo set ";
                Sql += "PumpId='" + FilteSQLStr(model.PumpId) + "',";
                Sql += "CompanyId='" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "PumpHouseName='" + FilteSQLStr(model.PumpHouseName) + "',";
                Sql += "PumpHouseClass='" + FilteSQLStr(model.PumpHouseClass) + "',";
                Sql += "PersonIinCharge='" + FilteSQLStr(model.PersonIinCharge) + "',";
                Sql += "CreatedDateTime=CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "ModifiedDateTime=CAST('" + model.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "MonitoringAddress='" + model.MonitoringAddress + "'";
                if (!string.IsNullOrEmpty(SqlWhere))
                    Sql += " Where " + SqlWhere;
                #endregion
                result.Add(Sql);
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.SeedInfo 增删改

        #region 增加数据
        /// <summary>
        /// 单条增加
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="message">消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertSeedInfo(cropsTraceDataAccess.Model.SeedInfo model)
        {
            #region 声明和初始化

            //错误消息
            string message = string.Empty;

            //返回值
            Message result = null;

            //存储过程参数
            List<DbParameter> parameters = null;
            #endregion

            if (model != null)
            {
                try
                {
                    parameters = SetDbParameter(model).ToList();
                    result = await MessageHelper.GetMessageAsync(m_dbHelper, "Create_SeedInfo", parameters);
                }
                catch (Exception ex)
                {
                    result = new Message(false, ex.Message);
                }
            }
            else
            {
                result = new Message(false, "参数为空不能添加");
            }

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public Task<Message> InsertSeedInfo(List<cropsTraceDataAccess.Model.SeedInfo> lists)
        {
            int DbState = -1;
            Task<Message> result = null;
            if (lists == null)
                return new Task<Message>(() => new Message(false, "参数为空不能添加"));
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkInsertSql(lists);
            try
            {
                DbState = ExecuteNonQuery(string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Task<Message>(() => new Message(false, ex.Message));
            }
            return result;
        }
        #endregion

        #region 修改方法
        /// <summary>
        /// 单条修改
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">消息</param>
        /// <returns>修改条数</returns>
        public Task<Message> UpdateSeedInfo(cropsTraceDataAccess.Model.SeedInfo model, string SqlWhere, out string message)
        {
            message = string.Empty;
            List<DbParameter> sqlparms = this.SetDbParameter(model).ToList<DbParameter>();
            sqlparms.Add(Database.MakeInParam("SqlWhere", SqlWhere));
            Task<Message> result = null;
            int DbState = -1;
            try
            {
                DbState = ExecuteNonQuery(CommandType.StoredProcedure, "Update_SeedInfo", sqlparms.ToArray());
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = new Task<Message>(() => new Message(false, ex.Message));
            }
            if (result == null) 
            {
                result = new Task<Message>(() => new Message(true,string.Empty));
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public async Task<Message> UpdateSeedInfo(List<cropsTraceDataAccess.Model.SeedInfo> lists, string SqlWhere)
        {
            Message result = null;
            int DbState = -1;
            string message = string.Empty;
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkUpdateSql(lists, SqlWhere);
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = new Message(false, ex.Message);
            }
            if (result == null)
            {
                result =  new Message(true, string.Empty);
            }
            return result;
        }
        #endregion

        #region 删除方法

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="IDStr">编号字符串</param>
        /// <returns>返回消息</returns>
        public async Task<Message> DeleteSeedInfo(string IDStr) 
        {
            #region 声明和初始化

            //错误消息
            string message = string.Empty;
            
            //返回值
            Message result = null;

            //SQL语句
            string sql = string.Empty;

            //数据库状态
            int DbState = -1;
            #endregion

            //初始化SQL语句
            sql = $" delete from SeedInfo where CropsId in ({IDStr}) ";

            #region 执行SQL语句
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = new Message(false, ex.Message);
            }
            #endregion

            if (result == null)
                result = new Message(true, string.Empty);

            return result;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.SeedInfo> QuerySeedInfo(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<cropsTraceDataAccess.Model.SeedInfo> result = new List<cropsTraceDataAccess.Model.SeedInfo>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere)) 
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_SeedInfo", sqlparms);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.SeedInfo model = new cropsTraceDataAccess.Model.SeedInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.SeedInfo> QuerySeedInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<cropsTraceDataAccess.Model.SeedInfo> result = new List<cropsTraceDataAccess.Model.SeedInfo>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_SeedInfo_Page", sqlparm);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.SeedInfo model = new cropsTraceDataAccess.Model.SeedInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #endregion

        #region cropsTraceDataAccess.Model.SeedInfo 农作物信息基础方法

        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref SeedInfo model, DataRow dr)
        {
            //农作物编号[雪花ID生成]
            model.CropsId = Convert.IsDBNull(dr["CropsId"]) ? 0 : Convert.ToInt64(dr["CropsId"]);
            //公司编号[对应登录者编号]
            model.CompanyId = Convert.IsDBNull(dr["CompanyId"]) ? 0 : Convert.ToInt32(dr["CompanyId"]);
            //种子名称
            model.SeedName = Convert.IsDBNull(dr["SeedName"]) ? string.Empty : Convert.ToString(dr["SeedName"]).Trim();
            //种子品种
            model.SeedVariety = Convert.IsDBNull(dr["SeedVariety"]) ? string.Empty : Convert.ToString(dr["SeedVariety"]).Trim();
            //种子类型
            model.SeedClass = Convert.IsDBNull(dr["SeedClass"]) ? string.Empty : Convert.ToString(dr["SeedClass"]).Trim();
            //种植面积
            model.PlantArea = Convert.IsDBNull(dr["PlantArea"]) ? 0 : Convert.ToDecimal(dr["PlantArea"]);
            //创建时间
            model.CreatedDateTime = Convert.IsDBNull(dr["CreatedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["CreatedDateTime"]);
            //修改时间
            model.ModifiedDateTime = Convert.IsDBNull(dr["ModifiedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["ModifiedDateTime"]);
            //农作物介绍
            model.Introduce = Convert.IsDBNull(dr["Introduce"]) ? string.Empty : Convert.ToString(dr["Introduce"]).Trim();
            //土壤类型
            model.SoilType= Convert.IsDBNull(dr["SoilType"]) ? string.Empty : Convert.ToString(dr["SoilType"]).Trim();
        }

        ///<summary>
        ///检查是否空值
        ///</summary>
        private void CheckEmpty(ref List<SeedInfo> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                //农作物编号[雪花ID生成]
                lists[i].CropsId = lists[i].CropsId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].CropsId);
                //公司编号[对应登录者编号]
                lists[i].CompanyId = lists[i].CompanyId == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].CompanyId);
                //种子名称
                lists[i].SeedName = string.IsNullOrEmpty(lists[i].SeedName) ? string.Empty : Convert.ToString(lists[i].SeedName).Trim();
                //种子品种
                lists[i].SeedVariety = string.IsNullOrEmpty(lists[i].SeedVariety) ? string.Empty : Convert.ToString(lists[i].SeedVariety).Trim();
                //种子类型
                lists[i].SeedClass = string.IsNullOrEmpty(lists[i].SeedClass) ? string.Empty : Convert.ToString(lists[i].SeedClass).Trim();
                //种植面积
                lists[i].PlantArea = lists[i].PlantArea == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].PlantArea);
                //创建时间
                lists[i].CreatedDateTime = lists[i].CreatedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].CreatedDateTime.GetValueOrDefault());
                //修改时间
                lists[i].ModifiedDateTime = lists[i].ModifiedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].ModifiedDateTime.GetValueOrDefault());
                //农作物介绍
                lists[i].Introduce = string.IsNullOrEmpty(lists[i].Introduce) ? string.Empty : Convert.ToString(lists[i].Introduce).Trim();
                //土壤类型
                lists[i].SoilType = string.IsNullOrEmpty(lists[i].SoilType) ? string.Empty : Convert.ToString(lists[i].SoilType).Trim();
            }
        }

        ///<summary>
        ///赋值数据行
        ///</summary>
        ///<param name="model">GSK货品移动表C21A</param>
        public DbParameter[] SetDbParameter(SeedInfo model)
        {
            #region 赋值Sql参数
            DbParameter[] result = new DbParameter[]
            {
                 Database.MakeInParam("CropsId",model.CropsId),
                 Database.MakeInParam("CompanyId",model.CompanyId),
                 Database.MakeInParam("SeedName",model.SeedName),
                 Database.MakeInParam("SeedVariety",model.SeedVariety),
                 Database.MakeInParam("SeedClass",model.SeedClass),
                 Database.MakeInParam("PlantArea",model.PlantArea),
                 Database.MakeInParam("CreatedDateTime",model.CreatedDateTime),
                 Database.MakeInParam("ModifiedDateTime",model.ModifiedDateTime),
                 Database.MakeInParam("Introduce",model.Introduce),
                 Database.MakeInParam("SoilType",model.SoilType)
            };
            #endregion

            return result;
        }

        ///<summary>
        ///生成插入Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<returns>插入Sql语句字符串数组</returns>
        private List<string> MarkInsertSql(List<SeedInfo> lists)
        {
            List<string> result = new List<string>();
            foreach (SeedInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "insert into SeedInfo(";
                Sql += "CropsId,";
                Sql += "CompanyId,";
                Sql += "SeedName,";
                Sql += "SeedVariety,";
                Sql += "SeedClass,";
                Sql += "PlantArea,";
                Sql += "CreatedDateTime,";
                Sql += "ModifiedDateTime,";
                Sql += "Introduce,";
                Sql += "SoilType";
                Sql += ") values(";
                Sql += "'" + FilteSQLStr(model.CropsId) + "',";
                Sql += "'" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "'" + FilteSQLStr(model.SeedName) + "',";
                Sql += "'" + FilteSQLStr(model.SeedVariety) + "',";
                Sql += "'" + FilteSQLStr(model.SeedClass) + "',";
                Sql += "'" + FilteSQLStr(model.PlantArea) + "',";
                Sql += "CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "CAST('" + model.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "'" + FilteSQLStr(model.Introduce) + "',";
                Sql += "'" + FilteSQLStr(model.SoilType) + "'";
                Sql += ")";
                #endregion
                result.Add(Sql);
            }
            return result;
        }

        ///<summary>
        ///生成更新Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<param name="SqlWhere">更新条件</param>
        ///<returns>更新Sql语句字符串数组</returns>
        private List<string> MarkUpdateSql(List<SeedInfo> lists, string SqlWhere)
        {
            List<string> result = new List<string>();
            foreach (SeedInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "update SeedInfo set ";
                Sql += "CropsId='" + FilteSQLStr(model.CropsId) + "',";
                Sql += "CompanyId='" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "SeedName='" + FilteSQLStr(model.SeedName) + "',";
                Sql += "SeedVariety='" + FilteSQLStr(model.SeedVariety) + "',";
                Sql += "SeedClass='" + FilteSQLStr(model.SeedClass) + "',";
                Sql += "PlantArea='" + FilteSQLStr(model.PlantArea) + "',";
                Sql += "CreatedDateTime=CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "ModifiedDateTime=CAST('" + model.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "Introduce='" + FilteSQLStr(model.Introduce) + "',";
                Sql += "SoilType='" + FilteSQLStr(model.SoilType) + "'";
                if (!string.IsNullOrEmpty(SqlWhere))
                    Sql += " Where " + SqlWhere;
                #endregion
                result.Add(Sql);
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.ShowFields 增删改

        #region 增加数据
        /// <summary>
        /// 单条增加
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="message">消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertShowFields(cropsTraceDataAccess.Model.ShowFields model)
        {
            #region 声明和初始化

            //错误消息
           string message = string.Empty;

            //返回值
            Message result = new Message(true, string.Empty);

            //存储过程参数
            List<DbParameter> parameters = null;
            #endregion

            if (model != null)
            {
                try
                {
                    parameters = SetDbParameter(model).ToList();
                    result = await MessageHelper.GetMessageForObjectAsync<Id32>(m_dbHelper, "Create_ShowFields", parameters);
                }
                catch (Exception ex)
                {
                    result = new Message(false, ex.Message);
                }
            }
            else
            {
                result = new Message(false, "参数为空不能添加");
            }

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public async Task<Message> InsertShowFields(List<cropsTraceDataAccess.Model.ShowFields> lists)
        {
            int DbState = -1;
            Message result = new Message(true,string.Empty);
            if (lists == null)
                return new Message(false, "参数为空不能添加");
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkInsertSql(lists);
            try
            {
                DbState = ExecuteNonQuery(string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            return result;
        }
        #endregion

        #region 修改方法
        /// <summary>
        /// 单条修改
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">消息</param>
        /// <returns>修改条数</returns>
        public async Task<Message> UpdateShowFields(cropsTraceDataAccess.Model.ShowFields model, string SqlWhere)
        {
            List<DbParameter> sqlparms = this.SetDbParameter(model).ToList<DbParameter>();
            sqlparms.Add(Database.MakeInParam("SqlWhere", SqlWhere));
            Message result = new Message(true, string.Empty);
            int DbState = -1;
            try
            {
                DbState = ExecuteNonQuery(CommandType.StoredProcedure, "Update_ShowFields", sqlparms.ToArray());
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public async Task<Message> UpdateShowFields(List<cropsTraceDataAccess.Model.ShowFields> lists, string SqlWhere)
        {
            int DbState = -1;
            Message result = new Message(true, string.Empty);
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkUpdateSql(lists, SqlWhere);
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
                return result;
            }
            result = new Message(true, string.Empty);
            return result;
        }

        /// <summary>
        /// 设置字段是否显示
        /// </summary>
        /// <param name="RecordIds">记录编号字符串</param>
        /// <param name="isShow">是否显示_0不显示_1显示_默认为0</param>
        /// <returns></returns>
        public async Task<Message> SetShowFieldsIsShow(int CompanyId,List<long> RecordIds,int isShow=0) 
        {
            #region 声明变量
            int DbState = -1;
            string sql = string.Empty;
            Message result = new Message(true, string.Empty);
            string RecordIdString = string.Empty;
            #endregion

            if (RecordIds == null || RecordIds.Count == 0) 
            {
                result=new Message(false, "记录编号为空无法设置");
                return result;
            }

            #region 循环记录编号
            RecordIds.ForEach(recordId => {
                RecordIdString += $"'{recordId}',";
            });
            if(!string.IsNullOrEmpty(RecordIdString))
                RecordIdString=RecordIdString.Substring(0,RecordIdString.Length-1);
            #endregion

            #region 拼写SQL语句
            sql = $"update ShowFields set IsShow='0' where CompanyId='{CompanyId}';";//将之前选中的清空
            sql+= $"update ShowFields set IsShow='{isShow}' where RecordId in ({RecordIdString}) and CompanyId='{CompanyId}' ";
            #endregion

            #region 执行SQL语句
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            #endregion

            return result;
        }
        #endregion

        #region 删除方法
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="IDStr">编号字符串</param>
        /// <returns>返回消息</returns>
        public async Task<Message> DeleteShowFields(string IDStr)
        {
            #region 声明和初始化

            //错误消息
            string message = string.Empty;

            //返回值
            Message result = null;

            //SQL语句
            string sql = string.Empty;

            //数据库状态
            int DbState = -1;
            #endregion

            //初始化SQL语句
            sql = $" delete from ShowFields where RecordId in ({IDStr}) ";

            #region 执行SQL语句
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = new Message(false, ex.Message);
            }
            #endregion

            if (result == null)
                result = new Message(true, string.Empty);

            return result;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.ShowFields> QueryShowFields(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<cropsTraceDataAccess.Model.ShowFields> result = new List<cropsTraceDataAccess.Model.ShowFields>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere)) 
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_ShowFields", sqlparms);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.ShowFields model = new cropsTraceDataAccess.Model.ShowFields();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.ShowFields> QueryShowFields(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<cropsTraceDataAccess.Model.ShowFields> result = new List<cropsTraceDataAccess.Model.ShowFields>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_ShowFields_Page", sqlparm);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.ShowFields model = new cropsTraceDataAccess.Model.ShowFields();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #endregion

        #region cropsTraceDataAccess.Model.ShowFields 显示字段基础方法

        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref ShowFields model, DataRow dr)
        {
            //记录编号
            model.RecordId = Convert.IsDBNull(dr["RecordId"]) ? 0 : Convert.ToInt64(dr["RecordId"]);
            //公司编号
            model.CompanyId = Convert.IsDBNull(dr["CompanyId"]) ? 0 : Convert.ToInt32(dr["CompanyId"]);
            //设备
            model.Device = Convert.IsDBNull(dr["Device"]) ? string.Empty : Convert.ToString(dr["Device"]).Trim();
            //测点
            model.PointId = Convert.IsDBNull(dr["PointId"]) ? 0 : Convert.ToInt64(dr["PointId"]);
            //字段名
            model.FieldName = Convert.IsDBNull(dr["FieldName"]) ? string.Empty : Convert.ToString(dr["FieldName"]).Trim();
            //界面显示名称
            model.ShowFieldName = Convert.IsDBNull(dr["ShowFieldName"]) ? string.Empty : Convert.ToString(dr["ShowFieldName"]).Trim();
            //单位
            model.Unit = Convert.IsDBNull(dr["Unit"]) ? string.Empty : Convert.ToString(dr["Unit"]).Trim();
            //是否显示_0不显示_1显示
            model.IsShow = Convert.IsDBNull(dr["IsShow"]) ? 0 : Convert.ToInt32(dr["IsShow"]);
            //设备编号
            model.id = Convert.IsDBNull(dr["id"]) ? 0 : Convert.ToInt32(dr["id"]);
            //设备值
            model.value = Convert.IsDBNull(dr["value"]) ? string.Empty : Convert.ToString(dr["value"]).Trim();
            //更新时间
            model.updateTime = Convert.IsDBNull(dr["updateTime"]) ? string.Empty : Convert.ToString(dr["updateTime"]).Trim();
            //设备编码
            model.deviceCode = Convert.IsDBNull(dr["deviceCode"]) ? string.Empty : Convert.ToString(dr["deviceCode"]).Trim();
            //设备名称
            model.deviceName = Convert.IsDBNull(dr["deviceName"]) ? string.Empty : Convert.ToString(dr["deviceName"]).Trim();
            //名称
            model.name = Convert.IsDBNull(dr["name"]) ? string.Empty : Convert.ToString(dr["name"]).Trim();
        }

        ///<summary>
        ///检查是否空值
        ///</summary>
        private void CheckEmpty(ref List<ShowFields> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                //记录编号
                lists[i].RecordId = lists[i].RecordId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].RecordId);
                //公司编号
                lists[i].CompanyId = lists[i].CompanyId == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].CompanyId);
                //设备
                lists[i].Device = string.IsNullOrEmpty(lists[i].Device) ? string.Empty : Convert.ToString(lists[i].Device).Trim();
                //测点
                lists[i].PointId = lists[i].PointId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].PointId);
                //字段名
                lists[i].FieldName = string.IsNullOrEmpty(lists[i].FieldName) ? string.Empty : Convert.ToString(lists[i].FieldName).Trim();
                //界面显示名称
                lists[i].ShowFieldName = string.IsNullOrEmpty(lists[i].ShowFieldName) ? string.Empty : Convert.ToString(lists[i].ShowFieldName).Trim();
                //单位
                lists[i].Unit = string.IsNullOrEmpty(lists[i].Unit) ? string.Empty : Convert.ToString(lists[i].Unit).Trim();
                //是否显示_0不显示_1显示
                lists[i].IsShow = lists[i].IsShow == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].IsShow);
                //设备编号
                lists[i].id = lists[i].id == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].id);
                //设备值
                lists[i].value = string.IsNullOrEmpty(lists[i].value) ? string.Empty : Convert.ToString(lists[i].value).Trim();
                //更新时间
                lists[i].updateTime = string.IsNullOrEmpty(lists[i].updateTime) ? string.Empty : Convert.ToString(lists[i].updateTime).Trim();
                //设备编码
                lists[i].deviceCode = string.IsNullOrEmpty(lists[i].deviceCode) ? string.Empty : Convert.ToString(lists[i].deviceCode).Trim();
                //设备名称
                lists[i].deviceName = string.IsNullOrEmpty(lists[i].deviceName) ? string.Empty : Convert.ToString(lists[i].deviceName).Trim();
                //名称
                lists[i].name = string.IsNullOrEmpty(lists[i].name) ? string.Empty : Convert.ToString(lists[i].name).Trim();
            }
        }

        ///<summary>
        ///赋值数据行
        ///</summary>
        ///<param name="model">GSK货品移动表C21A</param>
        public DbParameter[] SetDbParameter(ShowFields model)
        {
            #region 赋值Sql参数
            DbParameter[] result = new DbParameter[]
            {
                 Database.MakeInParam("RecordId",model.RecordId),
                 Database.MakeInParam("CompanyId",model.CompanyId),
                 Database.MakeInParam("Device",model.Device),
                 Database.MakeInParam("PointId",model.PointId),
                 Database.MakeInParam("FieldName",model.FieldName),
                 Database.MakeInParam("ShowFieldName",model.ShowFieldName),
                 Database.MakeInParam("Unit",model.Unit),
                 Database.MakeInParam("IsShow",model.IsShow),
                 Database.MakeInParam("id",model.id),
                 Database.MakeInParam("value",model.value),
                 Database.MakeInParam("updateTime",model.updateTime),
                 Database.MakeInParam("deviceCode",model.deviceCode),
                 Database.MakeInParam("deviceName",model.deviceName),
                 Database.MakeInParam("name",model.name)
            };
            #endregion

            return result;
        }

        ///<summary>
        ///生成插入Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<returns>插入Sql语句字符串数组</returns>
        private List<string> MarkInsertSql(List<ShowFields> lists)
        {
            List<string> result = new List<string>();
            foreach (ShowFields model in lists)
            {
                #region 拼写Sql语句
                string Sql = "insert into ShowFields(";
                Sql += "RecordId,";
                Sql += "CompanyId,";
                Sql += "Device,";
                Sql += "PointId,";
                Sql += "FieldName,";
                Sql += "ShowFieldName,";
                Sql += "Unit,";
                Sql += "IsShow";
                Sql += ") values(";
                Sql += "'" + FilteSQLStr(model.RecordId) + "',";
                Sql += "'" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "'" + FilteSQLStr(model.Device) + "',";
                Sql += "'" + FilteSQLStr(model.PointId) + "',";
                Sql += "'" + FilteSQLStr(model.FieldName) + "',";
                Sql += "'" + FilteSQLStr(model.ShowFieldName) + "',";
                Sql += "'" + FilteSQLStr(model.Unit) + "',";
                Sql += "'" + FilteSQLStr(model.IsShow) + "'";
                Sql += ")";
                #endregion
                result.Add(Sql);
            }
            return result;
        }

        ///<summary>
        ///生成更新Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<param name="SqlWhere">更新条件</param>
        ///<returns>更新Sql语句字符串数组</returns>
        private List<string> MarkUpdateSql(List<ShowFields> lists, string SqlWhere)
        {
            List<string> result = new List<string>();
            foreach (ShowFields model in lists)
            {
                #region 拼写Sql语句
                string Sql = "update ShowFields set ";
                Sql += "RecordId='" + FilteSQLStr(model.RecordId) + "',";
                Sql += "CompanyId='" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "Device='" + FilteSQLStr(model.Device) + "',";
                Sql += "PointId='" + FilteSQLStr(model.PointId) + "',";
                Sql += "FieldName='" + FilteSQLStr(model.FieldName) + "',";
                Sql += "ShowFieldName='" + FilteSQLStr(model.ShowFieldName) + "',";
                Sql += "Unit='" + FilteSQLStr(model.Unit) + "',";
                Sql += "IsShow='" + FilteSQLStr(model.IsShow) + "'";
                if (!string.IsNullOrEmpty(SqlWhere))
                    Sql += " Where " + SqlWhere;
                #endregion
                result.Add(Sql);
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.StorageInfo 增删改

        #region 增加数据
        /// <summary>
        /// 单条增加
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="message">消息</param>
        /// <returns>添加条数</returns>
        public Task<Message> InsertStorageInfo(cropsTraceDataAccess.Model.StorageInfo model, out string message)
        {
            #region 声明和初始化

            //错误消息
            message = string.Empty;

            //返回值
            Task<Message> result = null;

            //存储过程参数
            List<DbParameter> parameters = null;
            #endregion

            if (model != null)
            {
                try
                {
                    parameters = SetDbParameter(model).ToList();
                    result = MessageHelper.GetMessageForObjectAsync<Id32>(m_dbHelper, "Create_StorageInfo", parameters);
                }
                catch (Exception ex)
                {
                    result = new Task<Message>(() => new Message(false, ex.Message));
                }
            }
            else
            {
                result = new Task<Message>(() => new Message(false, "参数为空不能添加"));
            }

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public Task<Message> InsertStorageInfo(List<cropsTraceDataAccess.Model.StorageInfo> lists)
        {
            int DbState = -1;
            Task<Message> result = null;
            if (lists == null)
                return new Task<Message>(() => new Message(false, "参数为空不能添加"));
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkInsertSql(lists);
            try
            {
                DbState = ExecuteNonQuery(string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Task<Message>(() => new Message(false, ex.Message));
            }
            return result;
        }
        #endregion

        #region 修改方法
        /// <summary>
        /// 单条修改
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">消息</param>
        /// <returns>修改条数</returns>
        public int UpdateStorageInfo(cropsTraceDataAccess.Model.StorageInfo model, string SqlWhere, out string message)
        {
            message = string.Empty;
            List<DbParameter> sqlparms = this.SetDbParameter(model).ToList<DbParameter>();
            sqlparms.Add(Database.MakeInParam("SqlWhere", SqlWhere));
            int result = -1;
            try
            {
                result = ExecuteNonQuery(CommandType.StoredProcedure, "Update_StorageInfo", sqlparms.ToArray());
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public int UpdateStorageInfo(List<cropsTraceDataAccess.Model.StorageInfo> lists, string SqlWhere, out string message)
        {
            int result = -1;
            message = string.Empty;
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkUpdateSql(lists, SqlWhere);
            try
            {
                result = ExecuteNonQuery(CommandType.Text, string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.StorageInfo> QueryStorageInfo(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<cropsTraceDataAccess.Model.StorageInfo> result = new List<cropsTraceDataAccess.Model.StorageInfo>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere)) 
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_StorageInfo", sqlparms);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.StorageInfo model = new cropsTraceDataAccess.Model.StorageInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<cropsTraceDataAccess.Model.StorageInfo> QueryStorageInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<cropsTraceDataAccess.Model.StorageInfo> result = new List<cropsTraceDataAccess.Model.StorageInfo>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_StorageInfo_Page", sqlparm);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                cropsTraceDataAccess.Model.StorageInfo model = new cropsTraceDataAccess.Model.StorageInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #endregion

        #region cropsTraceDataAccess.Model.StorageInfo 仓库信息基础方法

        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref StorageInfo model, DataRow dr)
        {
            //农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
            model.CropsId = Convert.IsDBNull(dr["CropsId"]) ? 0 : Convert.ToInt64(dr["CropsId"]);
            //入库批次号[雪花ID生成]
            model.BatchNumber = Convert.IsDBNull(dr["BatchNumber"]) ? 0 : Convert.ToInt64(dr["BatchNumber"]);
            //公司编号
            model.CompanyId = Convert.IsDBNull(dr["CompanyId"]) ? 0 : Convert.ToInt32(dr["CompanyId"]);
            //入库数量
            model.InQuantity = Convert.IsDBNull(dr["InQuantity"]) ? 0 : Convert.ToDecimal(dr["InQuantity"]);
            //出库数量
            model.OutQuantity = Convert.IsDBNull(dr["OutQuantity"]) ? 0 : Convert.ToDecimal(dr["OutQuantity"]);
            //入库时间
            model.InDateTime = Convert.IsDBNull(dr["InDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["InDateTime"]);
            //出库时间
            model.OutDateTime = Convert.IsDBNull(dr["OutDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["OutDateTime"]);
            //仓库编号
            model.WarehouseNumber = Convert.IsDBNull(dr["WarehouseNumber"]) ? string.Empty : Convert.ToString(dr["WarehouseNumber"]).Trim();
            //仓库温度
            model.WarehouseTemperature = Convert.IsDBNull(dr["WarehouseTemperature"]) ? 0 : Convert.ToDecimal(dr["WarehouseTemperature"]);
            //仓库湿度
            model.WarehouseHumidity = Convert.IsDBNull(dr["WarehouseHumidity"]) ? 0 : Convert.ToDecimal(dr["WarehouseHumidity"]);
            //创建时间
            model.CreatedDateTime = Convert.IsDBNull(dr["CreatedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["CreatedDateTime"]);
            //有效期
            model.validityDateTime = Convert.IsDBNull(dr["validityDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["validityDateTime"]);
            //状态[0-入库、1-出库]
            model.State = Convert.IsDBNull(dr["State"]) ? 0 : Convert.ToInt32(dr["State"]);
        }

        ///<summary>
        ///检查是否空值
        ///</summary>
        private void CheckEmpty(ref List<StorageInfo> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                //农作物编号[外键对应农作物表SeedInfo字段CropsId多对1]
                lists[i].CropsId = lists[i].CropsId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].CropsId);
                //入库批次号[雪花ID生成]
                lists[i].BatchNumber = lists[i].BatchNumber == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].BatchNumber);
                //公司编号
                lists[i].CompanyId = lists[i].CompanyId == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].CompanyId);
                //入库数量
                lists[i].InQuantity = lists[i].InQuantity == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].InQuantity);
                //出库数量
                lists[i].OutQuantity = lists[i].OutQuantity == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].OutQuantity);
                //入库时间
                lists[i].InDateTime = lists[i].InDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].InDateTime.GetValueOrDefault());
                //出库时间
                lists[i].OutDateTime = lists[i].OutDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].OutDateTime.GetValueOrDefault());
                //仓库编号
                lists[i].WarehouseNumber = string.IsNullOrEmpty(lists[i].WarehouseNumber) ? string.Empty : Convert.ToString(lists[i].WarehouseNumber).Trim();
                //仓库温度
                lists[i].WarehouseTemperature = lists[i].WarehouseTemperature == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].WarehouseTemperature);
                //仓库湿度
                lists[i].WarehouseHumidity = lists[i].WarehouseHumidity == null ? Convert.ToDecimal(0) : Convert.ToDecimal(lists[i].WarehouseHumidity);
                //创建时间
                lists[i].CreatedDateTime = lists[i].CreatedDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].CreatedDateTime.GetValueOrDefault());
                //有效期
                lists[i].validityDateTime = lists[i].validityDateTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].validityDateTime.GetValueOrDefault());
                //状态[0-入库、1-出库]
                lists[i].State = lists[i].State == null ? Convert.ToInt32(0) : Convert.ToInt32(lists[i].State);
            }
        }

        ///<summary>
        ///赋值数据行
        ///</summary>
        ///<param name="model">GSK货品移动表C21A</param>
        public DbParameter[] SetDbParameter(StorageInfo model)
        {
            #region 赋值Sql参数
            DbParameter[] result = new DbParameter[]
            {
                 Database.MakeInParam("CropsId",model.CropsId),
                 Database.MakeInParam("BatchNumber",model.BatchNumber),
                 Database.MakeInParam("CompanyId",model.CompanyId),
                 Database.MakeInParam("InQuantity",model.InQuantity),
                 Database.MakeInParam("OutQuantity",model.OutQuantity),
                 Database.MakeInParam("InDateTime",model.InDateTime),
                 Database.MakeInParam("OutDateTime",model.OutDateTime),
                 Database.MakeInParam("WarehouseNumber",model.WarehouseNumber),
                 Database.MakeInParam("WarehouseTemperature",model.WarehouseTemperature),
                 Database.MakeInParam("WarehouseHumidity",model.WarehouseHumidity),
                 Database.MakeInParam("CreatedDateTime",model.CreatedDateTime),
                 Database.MakeInParam("validityDateTime",model.validityDateTime),
                 Database.MakeInParam("State",model.State)
            };
            #endregion

            return result;
        }

        ///<summary>
        ///生成插入Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<returns>插入Sql语句字符串数组</returns>
        private List<string> MarkInsertSql(List<StorageInfo> lists)
        {
            List<string> result = new List<string>();
            foreach (StorageInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "insert into StorageInfo(";
                Sql += "CropsId,";
                Sql += "BatchNumber,";
                Sql += "CompanyId,";
                Sql += "InQuantity,";
                Sql += "OutQuantity,";
                Sql += "InDateTime,";
                Sql += "OutDateTime,";
                Sql += "WarehouseNumber,";
                Sql += "WarehouseTemperature,";
                Sql += "WarehouseHumidity,";
                Sql += "CreatedDateTime,";
                Sql += "validityDateTime,";
                Sql += "State";
                Sql += ") values(";
                Sql += "'" + FilteSQLStr(model.CropsId) + "',";
                Sql += "'" + FilteSQLStr(model.BatchNumber) + "',";
                Sql += "'" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "'" + FilteSQLStr(model.InQuantity) + "',";
                Sql += "'" + FilteSQLStr(model.OutQuantity) + "',";
                Sql += "CAST('" + model.InDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "CAST('" + model.OutDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "'" + FilteSQLStr(model.WarehouseNumber) + "',";
                Sql += "'" + FilteSQLStr(model.WarehouseTemperature) + "',";
                Sql += "'" + FilteSQLStr(model.WarehouseHumidity) + "',";
                Sql += "CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "CAST('" + model.validityDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "'" + FilteSQLStr(model.State) + "'";
                Sql += ")";
                #endregion
                result.Add(Sql);
            }
            return result;
        }

        ///<summary>
        ///生成更新Sql语句
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<param name="SqlWhere">更新条件</param>
        ///<returns>更新Sql语句字符串数组</returns>
        private List<string> MarkUpdateSql(List<StorageInfo> lists, string SqlWhere)
        {
            List<string> result = new List<string>();
            foreach (StorageInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "update StorageInfo set ";
                Sql += "CropsId='" + FilteSQLStr(model.CropsId) + "',";
                Sql += "BatchNumber='" + FilteSQLStr(model.BatchNumber) + "',";
                Sql += "CompanyId='" + FilteSQLStr(model.CompanyId) + "',";
                Sql += "InQuantity='" + FilteSQLStr(model.InQuantity) + "',";
                Sql += "OutQuantity='" + FilteSQLStr(model.OutQuantity) + "',";
                Sql += "InDateTime=CAST('" + model.InDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "OutDateTime=CAST('" + model.OutDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "WarehouseNumber='" + FilteSQLStr(model.WarehouseNumber) + "',";
                Sql += "WarehouseTemperature='" + FilteSQLStr(model.WarehouseTemperature) + "',";
                Sql += "WarehouseHumidity='" + FilteSQLStr(model.WarehouseHumidity) + "',";
                Sql += "CreatedDateTime=CAST('" + model.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "validityDateTime=CAST('" + model.validityDateTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "State='" + FilteSQLStr(model.State) + "'";
                if (!string.IsNullOrEmpty(SqlWhere))
                    Sql += " Where " + SqlWhere;
                #endregion
                result.Add(Sql);
            }
            return result;
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<vw_GrowthInfo> QueryViewGrowthInfo(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<vw_GrowthInfo> result = new List<vw_GrowthInfo>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere))
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_vw_GrowthInfo", sqlparms);
            }
            catch(Exception ex) 
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                vw_GrowthInfo model = new vw_GrowthInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<vw_GrowthInfo> QueryViewGrowthInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<vw_GrowthInfo> result = new List<vw_GrowthInfo>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_vw_GrowthInfo_Page", sqlparm);
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                vw_GrowthInfo model = new vw_GrowthInfo();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.vw_GrowthInfo 生长视图基础方法
        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref vw_GrowthInfo model, DataRow dr)
        {
            //生长信息编号
            model.RecordId = Convert.IsDBNull(dr["RecordId"]) ? 0 : Convert.ToInt64(dr["RecordId"]);
            //泵房编号
            model.PumpId = Convert.IsDBNull(dr["PumpId"]) ? 0 : Convert.ToInt64(dr["PumpId"]);
            //农作物编号
            model.CropsId = Convert.IsDBNull(dr["CropsId"]) ? 0 : Convert.ToInt64(dr["CropsId"]);
            //生长信息名称
            model.GrowthName = Convert.IsDBNull(dr["GrowthName"]) ? string.Empty : Convert.ToString(dr["GrowthName"]).Trim();
            //位置名称
            model.LandName = Convert.IsDBNull(dr["LandName"]) ? string.Empty : Convert.ToString(dr["LandName"]).Trim();
            //株高
            model.PlantHeight = Convert.IsDBNull(dr["PlantHeight"]) ? 0 : Convert.ToDecimal(dr["PlantHeight"]);
            //胸径
            model.DBH = Convert.IsDBNull(dr["DBH"]) ? 0 : Convert.ToDecimal(dr["DBH"]);
            //叶片数
            model.NumberOfBlades = Convert.IsDBNull(dr["NumberOfBlades"]) ? 0 : Convert.ToInt32(dr["NumberOfBlades"]);
            //出苗率
            model.EmergenceRate = Convert.IsDBNull(dr["EmergenceRate"]) ? 0 : Convert.ToDecimal(dr["EmergenceRate"]);
            //创建时间
            model.CreatedDateTime = Convert.IsDBNull(dr["CreatedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["CreatedDateTime"]);
            //修改时间
            model.ModifiedDateTime = Convert.IsDBNull(dr["ModifiedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["ModifiedDateTime"]);
            //泵房名称
            model.PumpHouseName = Convert.IsDBNull(dr["PumpHouseName"]) ? string.Empty : Convert.ToString(dr["PumpHouseName"]).Trim();
            //农作物名称
            model.SeedName = Convert.IsDBNull(dr["SeedName"]) ? string.Empty : Convert.ToString(dr["SeedName"]).Trim();
            //公司编号
            model.CompanyId = Convert.IsDBNull(dr["CompanyId"]) ? 0 : Convert.ToInt32(dr["CompanyId"]);
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询单表方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<vw_GrowthInfo_Plus> QueryViewGrowthInfoPlus(string SqlWhere, out string message)
        {
            message = string.Empty;
            List<vw_GrowthInfo_Plus> result = new List<vw_GrowthInfo_Plus>();
            DataTable dt = null;
            DataSet ds = null;
            DbParameter[] sqlparms = new DbParameter[] {
                 m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            if (string.IsNullOrEmpty(SqlWhere))
            {
                sqlparms = new DbParameter[] {
                    m_dbHelper.MakeInParam("SqlWhere",null)
                };
            }
            try
            {
                ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_vw_GrowthInfo_Plus", sqlparms);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            foreach (DataRow dr in dt.Rows)
            {
                vw_GrowthInfo_Plus model = new vw_GrowthInfo_Plus();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段名</param>
        /// <param name="SortMethod">排序方法[ASC|DESC]</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>查询结果</returns>
        public List<vw_GrowthInfo_Plus> QueryViewGrowthInfoPlus(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message)
        {
            message = string.Empty;
            PageCount = 0;
            DataTable dt = null;
            DataSet ds = null;
            List<vw_GrowthInfo_Plus> result = new List<vw_GrowthInfo_Plus>();
            if (string.IsNullOrEmpty(SqlWhere))
                SqlWhere = null;
            DbParameter[] sqlparm = new DbParameter[] {
                m_dbHelper.MakeInParam("StartRow",((CurPage - 1) * PageSize + 1)),
                m_dbHelper.MakeInParam("EndRow",(CurPage * PageSize)),
                m_dbHelper.MakeOutParam("TotalNumber",typeof(System.Int32)),
                m_dbHelper.MakeInParam("SortMethod",SortMethod),
                m_dbHelper.MakeInParam("SortField",SortField),
                m_dbHelper.MakeInParam("SqlWhere",SqlWhere)
            };
            ds = m_dbHelper.ExecuteDataSet(CommandType.StoredProcedure, "Query_vw_GrowthInfo_Plus_Page", sqlparm);
            #region 非空检查
            if (ds == null)
                return result;
            if (ds.Tables == null || ds.Tables.Count == 0)
                return result;
            dt = ds.Tables[0];
            if (dt == null)
                return result;
            if (dt.Rows.Count == 0)
                return result;
            #endregion
            PageCount = Convert.ToInt32(sqlparm[2].Value);
            foreach (DataRow dr in dt.Rows)
            {
                vw_GrowthInfo_Plus model = new vw_GrowthInfo_Plus();
                this.ReadDataRow(ref model, dr);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #region cropsTraceDataAccess.Model.vw_GrowthInfo_Plus 生长视图基础方法
        /// <summary>
        /// 读取数据行到model
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private void ReadDataRow(ref vw_GrowthInfo_Plus model, DataRow dr)
        {
            //生长信息编号
            model.RecordId = Convert.IsDBNull(dr["RecordId"]) ? 0 : Convert.ToInt64(dr["RecordId"]);
            //泵房编号
            model.PumpId = Convert.IsDBNull(dr["PumpId"]) ? 0 : Convert.ToInt64(dr["PumpId"]);
            //农作物编号
            model.CropsId = Convert.IsDBNull(dr["CropsId"]) ? 0 : Convert.ToInt64(dr["CropsId"]);
            //生长信息名称
            model.GrowthName = Convert.IsDBNull(dr["GrowthName"]) ? string.Empty : Convert.ToString(dr["GrowthName"]).Trim();
            //位置名称
            model.LandName = Convert.IsDBNull(dr["LandName"]) ? string.Empty : Convert.ToString(dr["LandName"]).Trim();
            //株高
            model.PlantHeight = Convert.IsDBNull(dr["PlantHeight"]) ? 0 : Convert.ToDecimal(dr["PlantHeight"]);
            //胸径
            model.DBH = Convert.IsDBNull(dr["DBH"]) ? 0 : Convert.ToDecimal(dr["DBH"]);
            //叶片数
            model.NumberOfBlades = Convert.IsDBNull(dr["NumberOfBlades"]) ? 0 : Convert.ToInt32(dr["NumberOfBlades"]);
            //出苗率
            model.EmergenceRate = Convert.IsDBNull(dr["EmergenceRate"]) ? 0 : Convert.ToDecimal(dr["EmergenceRate"]);
            //创建时间
            model.CreatedDateTime = Convert.IsDBNull(dr["CreatedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["CreatedDateTime"]);
            //修改时间
            model.ModifiedDateTime = Convert.IsDBNull(dr["ModifiedDateTime"]) ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(dr["ModifiedDateTime"]);
            //泵房名称
            model.PumpHouseName = Convert.IsDBNull(dr["PumpHouseName"]) ? string.Empty : Convert.ToString(dr["PumpHouseName"]).Trim();
            //农作物名称
            model.SeedName = Convert.IsDBNull(dr["SeedName"]) ? string.Empty : Convert.ToString(dr["SeedName"]).Trim();
            //公司编号
            model.CompanyId = Convert.IsDBNull(dr["CompanyId"]) ? 0 : Convert.ToInt32(dr["CompanyId"]);
            //种子品种
            model.SeedVariety = Convert.IsDBNull(dr["SeedVariety"]) ? string.Empty : Convert.ToString(dr["SeedVariety"]).Trim();
            //种植面积
            model.PlantArea = Convert.IsDBNull(dr["PlantArea"]) ? 0 : Convert.ToDecimal(dr["PlantArea"]);
            //农作物介绍
            model.Introduce = Convert.IsDBNull(dr["Introduce"]) ? string.Empty : Convert.ToString(dr["Introduce"]).Trim();
            //土壤类型
            model.SoilType = Convert.IsDBNull(dr["SoilType"]) ? string.Empty : Convert.ToString(dr["SoilType"]).Trim();
            //文件URL地址
            model.FileUrl = Convert.IsDBNull(dr["FileUrl"]) ? string.Empty : Convert.ToString(dr["FileUrl"]).Trim();
            //文件名
            model.FileName = Convert.IsDBNull(dr["FileName"]) ? string.Empty : Convert.ToString(dr["FileName"]).Trim();
            //文件长度
            model.FileLength = Convert.IsDBNull(dr["FileLength"]) ? 0 : Convert.ToInt64(dr["FileLength"]);
            //显示参数
            model.ShowParamJson = Convert.IsDBNull(dr["ShowParamJson"]) ? string.Empty : Convert.ToString(dr["ShowParamJson"]).Trim();
        }
        #endregion

        #region CompanyInfo增删改查
        /// <summary>
        /// 添加公司信息表 
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public Message InsertCompanyInfo(List<CompanyInfo> lists)
        {
            int DbState = -1;
            Message result = null;
            string message = string.Empty;
            if (lists == null)
                return new Message(false, "参数为空不能添加");
            CheckEmpty(ref lists);
            CheckMaxLength(ref lists, out message);
            if (!string.IsNullOrEmpty(message))
                return new Message(false, message);
            List<string> sqls = this.MarkInsertSql(lists);
            try
            {
                DbState = ExecuteNonQuery(string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                result = new Message(false, ex.Message);
            }
            if (result == null)
            {
                result = new Message(true, string.Empty);
            }
            return result;
        }

        /// <summary>
        /// 修改公司信息表 
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public Message UpdateCompanyInfo(List<CompanyInfo> lists, string SqlWhere)
        {
            Message result = null;
            int DbState = -1;
            string message = string.Empty;
            CheckEmpty(ref lists);
            List<string> sqls = this.MarkUpdateSql(lists, SqlWhere);
            try
            {
                DbState = ExecuteNonQuery(CommandType.Text, string.Join(';', sqls.ToArray()));
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = new Message(false, ex.Message);
            }
            if (result == null)
            {
                result = new Message(true, string.Empty);
            }
            return result;
        }

        /// <summary>
        /// 查询CompanyInfo数据
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>返回值</returns>
        public List<CompanyInfo> QueryCompanyInfo(string SqlWhere, out string message)
        {
            List<CompanyInfo> result = new List<CompanyInfo>();
            string sql = $"select {FieldCompanyInfo()} from CompanyInfo";
            DataSet ds = new DataSet();
            DataTable dt = null;
            message = string.Empty;
            if (!string.IsNullOrEmpty(SqlWhere))
                sql += $" where {SqlWhere} ";
            try
            {
                ds = m_dbHelper.ExecuteDataSet(sql);
                #region 非空检查
                if (ds == null)
                    return result;
                if (ds.Tables == null || ds.Tables.Count == 0)
                    return result;
                dt = ds.Tables[0];
                if (dt == null)
                    return result;
                if (dt.Rows.Count == 0)
                    return result;
                #endregion
                foreach (DataRow dtRow in dt.Rows)
                    result.Add(ReadDataRow(dtRow, new CompanyInfo()));
            }
            catch (Exception exp)
            {
                message = exp.Message;
            }
            return result;
        }

        /// <summary>
        /// 分页查询CompanyInfo数据
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段</param>
        /// <param name="SortMethod">排序方法</param>
        /// <param name="PageSize">每页分页数据</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="TotalNumber">总数据量</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns>返回数据</returns>
        public List<CompanyInfo> QueryPageCompanyInfo(
            string SqlWhere,
            string SortField,
            string SortMethod,
            int PageSize,
            int CurPage,
            out int TotalNumber,
            out int PageCount,
            out string message
            )
        {
            List<CompanyInfo> result = null;
            string FieldStr = FieldCompanyInfo();
            Func<DataRow, CompanyInfo, CompanyInfo> func = (DataRow dr, CompanyInfo model) => {
                return ReadDataRow(dr, model);
            };
            result = QueryPage<CompanyInfo>(
                SqlWhere,
                SortField,
                SortMethod,
                FieldStr,
                "CompanyInfo",
                PageSize,
                CurPage,
                func,
                out TotalNumber,
                out PageCount,
                out message
                );
            return result;
        }
        #endregion

        #region CompanyInfo基础方法
        /// <summary>
        /// 返回CompanyInfo字段列表
        /// </summary>
        /// <returns>字段列表</returns>
        private string FieldCompanyInfo()
        {
            return @"
                    [companyId],
                    [companyName],
                    [barcodeUrl],
                    [Backup01],
                    [Backup02],
                    [Backup03],
                    [Backup04],
                    [Backup05],
                    [Backup06],
                    [created],
                    [CreatedTime],
                    [Modifier],
                    [ModifiedTime]
                     "
                     .Trim()
                     .Replace("\t", "")
                     .Replace("\r", "")
                     .Replace("\n", "");
        }

        /// <summary>
        /// 读取数据行到model(CompanyInfo)
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="dr">数据行</param>
        private CompanyInfo ReadDataRow(DataRow dr, CompanyInfo model)
        {
            model = new CompanyInfo();
            //公司编号
            model.companyId = Utils.GetDataRow(dr, "companyId") == null ? 0 : Convert.ToInt64(Utils.GetDataRow(dr, "companyId"));
            //公司名称
            model.companyName = Utils.GetDataRow(dr, "companyName") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "companyName")).Trim();
            //二维码URL
            model.barcodeUrl = Utils.GetDataRow(dr, "barcodeUrl") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "barcodeUrl")).Trim();
            //备用字段01
            model.Backup01 = Utils.GetDataRow(dr, "Backup01") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "Backup01")).Trim();
            //备用字段02
            model.Backup02 = Utils.GetDataRow(dr, "Backup02") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "Backup02")).Trim();
            //备用字段03
            model.Backup03 = Utils.GetDataRow(dr, "Backup03") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "Backup03")).Trim();
            //备用字段04
            model.Backup04 = Utils.GetDataRow(dr, "Backup04") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "Backup04")).Trim();
            //备用字段05
            model.Backup05 = Utils.GetDataRow(dr, "Backup05") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "Backup05")).Trim();
            //备用字段06
            model.Backup06 = Utils.GetDataRow(dr, "Backup06") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "Backup06")).Trim();
            //创建人
            model.created = Utils.GetDataRow(dr, "created") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "created")).Trim();
            //创建时间
            model.CreatedTime = Utils.GetDataRow(dr, "CreatedTime") == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(Utils.GetDataRow(dr,"CreatedTime"));
            //修改人
            model.Modifier = Utils.GetDataRow(dr, "Modifier") == null ? string.Empty : Convert.ToString(Utils.GetDataRow(dr, "Modifier")).Trim();
            //修改时间
            model.ModifiedTime = Utils.GetDataRow(dr, "ModifiedTime") == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(Utils.GetDataRow(dr,"ModifiedTime"));

            return model;
        }

        ///<summary>
        ///检查是否空值(CompanyInfo)
        ///</summary>
        private void CheckEmpty(ref List<CompanyInfo> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                //公司编号
                lists[i].companyId = lists[i].companyId == null ? Convert.ToInt64(0) : Convert.ToInt64(lists[i].companyId);
                //公司名称
                lists[i].companyName = string.IsNullOrEmpty(lists[i].companyName) ? string.Empty : Convert.ToString(lists[i].companyName).Trim();
                //二维码URL
                lists[i].barcodeUrl = string.IsNullOrEmpty(lists[i].barcodeUrl) ? string.Empty : Convert.ToString(lists[i].barcodeUrl).Trim();
                //备用字段01
                lists[i].Backup01 = string.IsNullOrEmpty(lists[i].Backup01) ? string.Empty : Convert.ToString(lists[i].Backup01).Trim();
                //备用字段02
                lists[i].Backup02 = string.IsNullOrEmpty(lists[i].Backup02) ? string.Empty : Convert.ToString(lists[i].Backup02).Trim();
                //备用字段03
                lists[i].Backup03 = string.IsNullOrEmpty(lists[i].Backup03) ? string.Empty : Convert.ToString(lists[i].Backup03).Trim();
                //备用字段04
                lists[i].Backup04 = string.IsNullOrEmpty(lists[i].Backup04) ? string.Empty : Convert.ToString(lists[i].Backup04).Trim();
                //备用字段05
                lists[i].Backup05 = string.IsNullOrEmpty(lists[i].Backup05) ? string.Empty : Convert.ToString(lists[i].Backup05).Trim();
                //备用字段06
                lists[i].Backup06 = string.IsNullOrEmpty(lists[i].Backup06) ? string.Empty : Convert.ToString(lists[i].Backup06).Trim();
                //创建人
                lists[i].created = string.IsNullOrEmpty(lists[i].created) ? string.Empty : Convert.ToString(lists[i].created).Trim();
                //创建时间
                lists[i].CreatedTime = lists[i].CreatedTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].CreatedTime.GetValueOrDefault());
                //修改人
                lists[i].Modifier = string.IsNullOrEmpty(lists[i].Modifier) ? string.Empty : Convert.ToString(lists[i].Modifier).Trim();
                //修改时间
                lists[i].ModifiedTime = lists[i].ModifiedTime == null ? (DateTime)SqlDateTime.MinValue : Convert.ToDateTime(lists[i].ModifiedTime.GetValueOrDefault());
            }
        }

        ///<summary>
        ///检查是否超过长度(CompanyInfo)
        ///</summary>
        ///<param name="lists">数据集</param>
        ///<param name="message">错误消息</param>
        private void CheckMaxLength(ref List<CompanyInfo> lists, out string message)
        {
            #region 声明变量

            //错误消息
            message = string.Empty;

            //超过的长度
            int OutLength = 0;
            #endregion

            #region 循环验证长度
            for (int i = 0; i < lists.Count; i++)
            {
                if (!string.IsNullOrEmpty(lists[i].companyName))
                {
                    if (lists[i].companyName.Length > 300)
                    {
                        OutLength = lists[i].companyName.Length - 300;
                        message += "字段名[companyName]描述[公司名称]超长、字段最长[300]实际" + lists[i].companyName.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].barcodeUrl))
                {
                    if (lists[i].barcodeUrl.Length > 800)
                    {
                        OutLength = lists[i].barcodeUrl.Length - 800;
                        message += "字段名[barcodeUrl]描述[二维码URL]超长、字段最长[800]实际" + lists[i].barcodeUrl.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].Backup01))
                {
                    if (lists[i].Backup01.Length > 300)
                    {
                        OutLength = lists[i].Backup01.Length - 300;
                        message += "字段名[Backup01]描述[备用字段01]超长、字段最长[300]实际" + lists[i].Backup01.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].Backup02))
                {
                    if (lists[i].Backup02.Length > 300)
                    {
                        OutLength = lists[i].Backup02.Length - 300;
                        message += "字段名[Backup02]描述[备用字段02]超长、字段最长[300]实际" + lists[i].Backup02.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].Backup03))
                {
                    if (lists[i].Backup03.Length > 300)
                    {
                        OutLength = lists[i].Backup03.Length - 300;
                        message += "字段名[Backup03]描述[备用字段03]超长、字段最长[300]实际" + lists[i].Backup03.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].Backup04))
                {
                    if (lists[i].Backup04.Length > 300)
                    {
                        OutLength = lists[i].Backup04.Length - 300;
                        message += "字段名[Backup04]描述[备用字段04]超长、字段最长[300]实际" + lists[i].Backup04.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].Backup05))
                {
                    if (lists[i].Backup05.Length > 300)
                    {
                        OutLength = lists[i].Backup05.Length - 300;
                        message += "字段名[Backup05]描述[备用字段05]超长、字段最长[300]实际" + lists[i].Backup05.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].Backup06))
                {
                    if (lists[i].Backup06.Length > 300)
                    {
                        OutLength = lists[i].Backup06.Length - 300;
                        message += "字段名[Backup06]描述[备用字段06]超长、字段最长[300]实际" + lists[i].Backup06.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].created))
                {
                    if (lists[i].created.Length > 50)
                    {
                        OutLength = lists[i].created.Length - 50;
                        message += "字段名[created]描述[创建人]超长、字段最长[50]实际" + lists[i].created.Length + "超过长度" + OutLength + ",";
                    }
                }
                if (!string.IsNullOrEmpty(lists[i].Modifier))
                {
                    if (lists[i].Modifier.Length > 50)
                    {
                        OutLength = lists[i].Modifier.Length - 50;
                        message += "字段名[Modifier]描述[修改人]超长、字段最长[50]实际" + lists[i].Modifier.Length + "超过长度" + OutLength + ",";
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(message)) message = message.Substring(0, message.Length - 1);
        }

        ///<summary>
        ///生成插入Sql语句(CompanyInfo)
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<returns>插入Sql语句字符串数组</returns>
        private List<string> MarkInsertSql(List<CompanyInfo> lists)
        {
            List<string> result = new List<string>();
            foreach (CompanyInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "insert into CompanyInfo(";
                Sql += "companyId,";
                Sql += "companyName,";
                Sql += "barcodeUrl,";
                Sql += "Backup01,";
                Sql += "Backup02,";
                Sql += "Backup03,";
                Sql += "Backup04,";
                Sql += "Backup05,";
                Sql += "Backup06,";
                Sql += "created,";
                Sql += "CreatedTime,";
                Sql += "Modifier,";
                Sql += "ModifiedTime";
                Sql += ") values(";
                Sql += "'" + FilteSQLStr(model.companyId) + "',";
                Sql += "'" + FilteSQLStr(model.companyName) + "',";
                Sql += "'" + FilteSQLStr(model.barcodeUrl) + "',";
                Sql += "'" + FilteSQLStr(model.Backup01) + "',";
                Sql += "'" + FilteSQLStr(model.Backup02) + "',";
                Sql += "'" + FilteSQLStr(model.Backup03) + "',";
                Sql += "'" + FilteSQLStr(model.Backup04) + "',";
                Sql += "'" + FilteSQLStr(model.Backup05) + "',";
                Sql += "'" + FilteSQLStr(model.Backup06) + "',";
                Sql += "'" + FilteSQLStr(model.created) + "',";
                Sql += "CAST('" + model.CreatedTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "'" + FilteSQLStr(model.Modifier) + "',";
                Sql += "CAST('" + model.ModifiedTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME)";
                Sql += ")";
                #endregion
                result.Add(Sql);
            }
            return result;
        }

        ///<summary>
        ///生成更新Sql语句(CompanyInfo)
        ///</summary>
        ///<param name="lists">数据List</param>
        ///<param name="SqlWhere">更新条件</param>
        ///<returns>更新Sql语句字符串数组</returns>
        private List<string> MarkUpdateSql(List<CompanyInfo> lists, string SqlWhere)
        {
            List<string> result = new List<string>();
            foreach (CompanyInfo model in lists)
            {
                #region 拼写Sql语句
                string Sql = "update CompanyInfo set ";
                Sql += "companyId='" + FilteSQLStr(model.companyId) + "',";
                Sql += "companyName='" + FilteSQLStr(model.companyName) + "',";
                Sql += "barcodeUrl='" + FilteSQLStr(model.barcodeUrl) + "',";
                Sql += "Backup01='" + FilteSQLStr(model.Backup01) + "',";
                Sql += "Backup02='" + FilteSQLStr(model.Backup02) + "',";
                Sql += "Backup03='" + FilteSQLStr(model.Backup03) + "',";
                Sql += "Backup04='" + FilteSQLStr(model.Backup04) + "',";
                Sql += "Backup05='" + FilteSQLStr(model.Backup05) + "',";
                Sql += "Backup06='" + FilteSQLStr(model.Backup06) + "',";
                Sql += "created='" + FilteSQLStr(model.created) + "',";
                Sql += "CreatedTime=CAST('" + model.CreatedTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME),";
                Sql += "Modifier='" + FilteSQLStr(model.Modifier) + "',";
                Sql += "ModifiedTime=CAST('" + model.ModifiedTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") + "' AS DATETIME)";
                if (!string.IsNullOrEmpty(SqlWhere))
                    Sql += " Where " + SqlWhere;
                #endregion
                result.Add(Sql);
            }
            return result;
        }
        #endregion

        #region Private

        private void CreateDBHelper()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            m_connectionString = configuration["ConnectionStrings:MOIConnStr"];
            m_dbHelper = new DbHelper(m_connectionString);
        }

        /// <summary>
        /// 过滤不安全的字符串
        /// </summary>
        /// <param name="Str">要过滤的值</param>
        /// <returns>返回结果</returns>
        private static string FilteSQLStr(object str)
        {
            if (str == null)
                return string.Empty;
            if (IsNumeric(str))
                return Convert.ToString(str);
            string Str = Convert.ToString(str);
            if (!string.IsNullOrEmpty(Str))
            {
                Str = Str.Replace("'", "");
                Str = Str.Replace("\"", "");
                Str = Str.Replace("&", "&amp");
                Str = Str.Replace("<", "&lt");
                Str = Str.Replace(">", "&gt");

                Str = Str.Replace("delete", "");
                Str = Str.Replace("update", "");
                Str = Str.Replace("insert", "");
            }
            return Str;
        }

        /// <summary>
        /// 判断object是否数字
        /// </summary>
        /// <param name="AObject">要判断的Object</param>
        /// <returns>是否数字</returns>       
        public static bool IsNumeric(object AObject)
        {
            return AObject is sbyte || AObject is byte ||
                AObject is short || AObject is ushort ||
                AObject is int || AObject is uint ||
                AObject is long || AObject is ulong ||
                AObject is double || AObject is char ||
                AObject is decimal || AObject is float ||
                AObject is double;
        }
        #endregion
    }
}
