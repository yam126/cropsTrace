using cropsTraceDataAccess.Model;
using ePioneer.Data.Kernel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cropsTraceDataAccess.Data
{
    public interface IMOIRepository
    {
        #region Common
        public Task<PagerSet> GetPagerSetAsync(String tableName, Int32 pageIndex, Int32 pageSize, String where, String oderBy, String[] fields);

        public Task<PagerSet> GetPagerSetAsync(String tableName, Int32 pageIndex, Int32 pageSize, String where, String orderBy);

        public Task<DataSet> ExecuteDataSetAsync(String commandText);

        public DataSet ExecuteDataSet(String commandText);

        public Task<Int32> ExecuteNonQueryAsync(CommandType commandType, String commandText, params DbParameter[] commandParameters);

        public Task<Int32> ExecuteNonQueryAsync(String commandText);

        public Int32 ExecuteNonQuery(String commandText);

        public Int32 ExecuteNonQuery(CommandType commandType, String commandText, params DbParameter[] commandParameters);
        #endregion

        #region 文件信息增删改查

        public Task<Message> InsertFileInfo(cropsTraceDataAccess.Model.FileInfo model);

        public Task<Message> InsertFileInfo(List<cropsTraceDataAccess.Model.FileInfo> lists);

        public int UpdateFileInfo(cropsTraceDataAccess.Model.FileInfo model, string SqlWhere, out string message);

        public int UpdateFileInfo(List<cropsTraceDataAccess.Model.FileInfo> lists, string SqlWhere, out string message);

        public Task<Message> DeleteFileInfo(string where);

        public List<cropsTraceDataAccess.Model.FileInfo> QueryFileInfo(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.FileInfo> QueryFileInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);
        #endregion

        #region 生长信息增删改查

        public Task<Message> InsertGrowthInfo(cropsTraceDataAccess.Model.GrowthInfo model);

        public Task<Message> InsertGrowthInfo(List<cropsTraceDataAccess.Model.GrowthInfo> lists);

        public Task<Message> UpdateGrowthInfo(cropsTraceDataAccess.Model.GrowthInfo model, string SqlWhere);

        public Task<Message> UpdateGrowthInfo(List<cropsTraceDataAccess.Model.GrowthInfo> lists, string SqlWhere);

        public Task<Message> DeleteGrowthInfo(string IDStr);

        public List<int> GetAllGrowthInfoYear(int CompanyId, out string message);

        public List<cropsTraceDataAccess.Model.GrowthInfo> QueryGrowthInfo(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.GrowthInfo> QueryGrowthInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);

        public List<cropsTraceDataAccess.Model.vw_GrowthInfo> QueryViewGrowthInfo(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.vw_GrowthInfo> QueryViewGrowthInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);

        public List<cropsTraceDataAccess.Model.vw_GrowthInfo_Plus> QueryViewGrowthInfoPlus(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.vw_GrowthInfo_Plus> QueryViewGrowthInfoPlus(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);
        #endregion

        #region 泵房信息增删改查

        public Task<Message> InsertPumpHouseInfo(cropsTraceDataAccess.Model.PumpHouseInfo model);

        public Task<Message> InsertPumpHouseInfo(List<cropsTraceDataAccess.Model.PumpHouseInfo> lists);

        /// <summary>
        /// 批量删除泵房信息
        /// </summary>
        /// <param name="IDStr">泵房编号字符串</param>
        /// <returns></returns>
        public Task<Message> BatchDeletePumpHouse(string IDStr);

        public Task<Message> UpdatePumpHouseInfo(cropsTraceDataAccess.Model.PumpHouseInfo model, string SqlWhere);

        public Task<Message> UpdatePumpHouseInfo(List<cropsTraceDataAccess.Model.PumpHouseInfo> lists, string SqlWhere);

        public List<cropsTraceDataAccess.Model.PumpHouseInfo> QueryPumpHouseInfo(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.PumpHouseInfo> QueryPumpHouseInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);
        #endregion

        #region 种子信息增删改查

        public Task<Message> InsertSeedInfo(cropsTraceDataAccess.Model.SeedInfo model);

        public Task<Message> InsertSeedInfo(List<cropsTraceDataAccess.Model.SeedInfo> lists);

        public Task<Message> UpdateSeedInfo(cropsTraceDataAccess.Model.SeedInfo model, string SqlWhere, out string message);

        public Task<Message> UpdateSeedInfo(List<cropsTraceDataAccess.Model.SeedInfo> lists, string SqlWhere);

        public Task<Message> DeleteSeedInfo(string IDStr);

        public List<cropsTraceDataAccess.Model.SeedInfo> QuerySeedInfo(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.SeedInfo> QuerySeedInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);
        #endregion

        #region 显示字段增删改查

        public Task<Message> InsertShowFields(cropsTraceDataAccess.Model.ShowFields model);

        public Task<Message> InsertShowFields(List<cropsTraceDataAccess.Model.ShowFields> lists);

        public Task<Message> UpdateShowFields(cropsTraceDataAccess.Model.ShowFields model, string SqlWhere);

        public Task<Message> UpdateShowFields(List<cropsTraceDataAccess.Model.ShowFields> lists, string SqlWhere);

        public Task<Message> DeleteShowFields(string IDStr);

        public Task<Message> SetShowFieldsIsShow(int CompanyId,List<long> RecordIds, int isShow = 0);

        public List<cropsTraceDataAccess.Model.ShowFields> QueryShowFields(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.ShowFields> QueryShowFields(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);
        #endregion

        #region 库存信息增删改查

        public Task<Message> InsertStorageInfo(cropsTraceDataAccess.Model.StorageInfo model, out string message);

        public Task<Message> InsertStorageInfo(List<cropsTraceDataAccess.Model.StorageInfo> lists);

        public int UpdateStorageInfo(cropsTraceDataAccess.Model.StorageInfo model, string SqlWhere, out string message);

        public int UpdateStorageInfo(List<cropsTraceDataAccess.Model.StorageInfo> lists, string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.StorageInfo> QueryStorageInfo(string SqlWhere, out string message);

        public List<cropsTraceDataAccess.Model.StorageInfo> QueryStorageInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int PageCount, out string message);
        #endregion

        #region CompanyInfo增删改查
        /// <summary>
        /// 添加公司信息表 
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="message">错误消息</param>
        /// <returns>添加条数</returns>
        public Message InsertCompanyInfo(List<CompanyInfo> lists);

        /// <summary>
        /// 修改公司信息表
        /// </summary>
        /// <param name="lists">批量数据</param>
        /// <param name="SqlWhere">更新条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>修改条数</returns>
        public Message UpdateCompanyInfo(List<CompanyInfo> lists, string SqlWhere);

        /// <summary>
        /// 查询公司信息表数据
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="message">错误消息</param>
        /// <returns>返回值</returns>
        public List<CompanyInfo> QueryCompanyInfo(string SqlWhere, out string message);

        /// <summary>
        /// 分页查询公司信息表数据
        /// </summary>
        /// <param name="SqlWhere">查询条件</param>
        /// <param name="SortField">排序字段</param>
        /// <param name="SortMethod">排序方法</param>
        /// <param name="PageSize">每页分页数据</param>
        /// <param name="CurPage">当前页</param>
        /// <param name="TotalNumber">总数据量</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public List<CompanyInfo> QueryPageCompanyInfo(string SqlWhere, string SortField, string SortMethod, int PageSize, int CurPage, out int TotalNumber, out int PageCount, out string message);
        #endregion
    }
}
