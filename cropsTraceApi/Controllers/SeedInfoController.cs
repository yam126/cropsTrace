using AutoMapper;
using cropsTraceApi.Models;
using cropsTraceApi.Models.Result;
using cropsTraceDataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Common;
using cropsTraceDataAccess.Model;
using ePioneer.Data.Kernel;
using Snowflake.Net;
using Microsoft.AspNetCore.Cors;

namespace cropsTraceApi.Controllers
{
    /// <summary>
    /// 农作物
    /// </summary>
    [EnableCors("Cors")]
    [Route("api/seed-info")]
    [ApiController]
    public class SeedInfoController : Controller
    {
        #region Fields

        /// <summary>
        /// 数据库操作类
        /// </summary>
        private IMOIRepository m_repository;

        /// <summary>
        /// AutoMapper参数映射类
        /// </summary>
        private IMapper m_mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="repository">数据库操作类</param>
        public SeedInfoController(IMOIRepository repository, IMapper _mapper) 
        {
            m_repository = repository;
            m_mapper = _mapper == null ? throw new ArgumentNullException(nameof(_mapper)) : _mapper;
        }
        #endregion

        #region Delete

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">数据编号</param>
        /// <returns>是否成功消息</returns>
        [EnableCors("Cors")]
        [HttpDelete("{IDStr}")]
        public async Task<Result> DeleteData(string IDStr) 
        {
            #region 声明变量

            //错误消息
            string message = string.Empty;

            //数据库返回消息
            Message dbMessage = null;

            //返回值
            Result result = null;
            #endregion

            if (string.IsNullOrEmpty(IDStr)) 
            {
                result = new Result() {
                    Msg="参数为空不能删除",
                    Status=-1
                };
            }

            try 
            {
                IDStr = "'" + IDStr.Replace("-", "','")+"'";
                dbMessage = await m_repository.DeleteSeedInfo(IDStr);
            } 
            catch (Exception ex) 
            {
                result = new Result() {
                    Msg = $"删除农作物信息报错，原因[{ex.Message}]",
                    Status=-1
                };
                return result;
            }

            result = new Result()
            {
                Msg = string.Empty,
                Status=0
            };
            return result;
        }
        #endregion

        #region Get

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页(默认从1开始)</param>
        /// <param name="pageSize">每页记录数(默认每页10条)</param>
        /// <param name="where">查询条件(SQL查询条件,可以为空,为空返回所有数据)</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="SortField">排序字段(默认创建时间)</param>
        /// <param name="SortMethod">排序方法[DESC|ASC(默认DESC)]</param>
        /// <returns>返回查询结果</returns>
        [HttpGet("page")]
        public async Task<ActionResult<PageResult<SeedInfoParameter>>> GetPage(
            string? where = "",
            string? startTime="",
            string? endTime="",
            int? pageIndex = 1,
            int? pageSize = 10,
            string sortField = "CreatedDateTime",
            string sortMethod = "DESC"
            )
        {
            #region 声明变量

            //总页数
            int pageCount = 0;

            //总记录数
            int totalRecordCount = 0;

            //方法返回错误消息
            string message = string.Empty;

            //错误消息
            string checkMessage = string.Empty;

            //页面返回值
            List<SeedInfo> pageData = null;

            //接口返回值
            List<SeedInfoParameter> pageResultData = null;

            //返回值
            var result = new PageResult<SeedInfoParameter>();
            #endregion

            #region 非空验证
            if (pageIndex == null)
                checkMessage += "当前页、";
            if (pageSize == null)
                checkMessage += "每页记录数、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new PageResult<SeedInfoParameter>()
                {
                    Status = -1,
                    PageCount = 0,
                    RecordCount = 0,
                    Msg = $"非空验证出错，原因[{checkMessage}]",
                    Result = null
                };
                return result;
            }
            #endregion

            #region 有效验证
            if (pageIndex <= 0)
                checkMessage += "当前页不能小于或等于0、";
            if (pageSize <= 0)
                checkMessage += "每页记录数不能小于或等于0、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new PageResult<SeedInfoParameter>()
                {
                    Status = -1,
                    PageCount = 0,
                    RecordCount = 0,
                    Msg = $"有效验证出错，原因[{checkMessage}]",
                    Result = null
                };
                return result;
            }
            #endregion

            #region 查询数据
            if (!string.IsNullOrEmpty(where))
                where = $" SeedName like '%{where}%' and SeedVariety like '%{where}%' and SeedClass like '%{where}%' ";
            if (!string.IsNullOrEmpty(startTime) && Utils.IsDate(startTime))
                where += $" and CreatedDateTime>=cast('{startTime}' as datetime) ";
            if (!string.IsNullOrEmpty(endTime) && Utils.IsDate(endTime))
                where += $" and CreatedDateTime<=cast('{endTime}' as datetime) ";
            pageData = m_repository.QuerySeedInfo(
                where == null ? string.Empty : where,
                sortField,
                sortMethod,
                pageSize.GetValueOrDefault(),
                pageIndex.GetValueOrDefault(),
                out totalRecordCount,
                out message
                );
            if (pageData == null || pageData.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    result = new PageResult<SeedInfoParameter>()
                    {
                        Status = -1,
                        PageCount = 0,
                        RecordCount = 0,
                        Msg = $"查询数据出错，原因[{message}]",
                        Result = null
                    };
                    return result;
                }
            }
            #endregion

            #region 计算总页数
            if (totalRecordCount % pageSize == 0)
                pageCount = Convert.ToInt32(totalRecordCount / pageSize);
            else
                pageCount = Convert.ToInt32(totalRecordCount / pageSize) + 1;
            #endregion

            #region 赋值返回数据
            pageResultData = new List<SeedInfoParameter>();
            pageData.ForEach(item =>
            {
                SeedInfoParameter itemParam = m_mapper.Map<SeedInfo, SeedInfoParameter>(item);
                itemParam.CreatedDateTime = item.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                itemParam.ModifiedDateTime = item.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                pageResultData.Add(itemParam);
            });
            result = new PageResult<SeedInfoParameter>()
            {
                Status = 0,
                PageCount = pageCount,
                RecordCount = totalRecordCount,
                Msg = string.Empty,
                Result = pageResultData.ToArray()
            };
            #endregion

            return result;
        }

        /// <summary>
        /// 农作物自动完成
        /// </summary>
        /// <param name="where">自动完成查询关键字</param>
        /// <param name="resultDataCount">自动完成返回的数据量</param>
        /// <returns></returns>
        [HttpGet("auto-complate/{resultDataCount}")]
        public async Task<ActionResult<ListResult<SeedInfoParameter>>> QueryAutoComplate(
            int CompanyId,
            string where = "",
            int? resultDataCount = 10)
        {
            #region 声明变量

            //总记录数
            int totalRecordCount = 0;

            //方法返回错误消息
            string message = string.Empty;

            //查询条件
            List<string> SqlWhere = new List<string>();

            string SqlQueryWhere = string.Empty;

            //泵房数据库信息
            List<SeedInfo> seedInfos = null;

            //返回数据
            List<SeedInfoParameter> resultList = new List<SeedInfoParameter>();

            //返回值
            ListResult<SeedInfoParameter> result = null;
            #endregion

            #region 参数验证
            if (resultDataCount == null)
            {
                result = new ListResult<SeedInfoParameter>()
                {
                    Status = -1,
                    Msg = "返回的数据量不能为空",
                    Result = resultList
                };
                return result;
            }
            if (CompanyId == 0)
            {
                result = new ListResult<SeedInfoParameter>()
                {
                    Status = -1,
                    Msg = "公司编号不能为空",
                    Result = resultList
                };
                return result;
            }
            #endregion

            #region 拼写查询条件
            if (!string.IsNullOrEmpty(where)&&where!="all")
            {
                SqlWhere.Add($" cast(CropsId as nvarchar(50)) like '%{where}%'");
                SqlWhere.Add($" SeedName like '%{where}%'");
                SqlWhere.Add($" SeedVariety like '%{where}%'");
                SqlWhere.Add($" SeedClass like '%{where}%'");
                SqlQueryWhere = $"({string.Join(" Or ", SqlWhere.ToArray())}) and CompanyId='{CompanyId}' ";
            }
            else 
            {
                SqlQueryWhere = $" CompanyId='{CompanyId}' ";
            }
            #endregion

            #region 查询数据
            seedInfos = m_repository.QuerySeedInfo(
                SqlQueryWhere,
                "CreatedDateTime",
                "DESC",
                resultDataCount.GetValueOrDefault(),
                1,
                out totalRecordCount,
                out message);
            if (seedInfos == null || seedInfos.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    result = new ListResult<SeedInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"查询农作物数据出错,原因[{message}]",
                        Result = resultList
                    };
                }
                else
                {
                    result = new ListResult<SeedInfoParameter>()
                    {
                        Status = 0,
                        Msg = string.Empty,
                        Result = resultList
                    };
                }
                return result;
            }
            #endregion

            #region 赋值返回数据
            resultList = new List<SeedInfoParameter>();
            seedInfos.ForEach(item =>
            {
                SeedInfoParameter itemParam = m_mapper.Map<SeedInfo, SeedInfoParameter>(item);
                itemParam.CreatedDateTime = item.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                itemParam.ModifiedDateTime = item.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                resultList.Add(itemParam);
            });
            result = new ListResult<SeedInfoParameter>()
            {
                Status = 0,
                Msg = string.Empty,
                Result = resultList
            };
            #endregion

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        [HttpGet("{CompanyId}/{Year}")]
        public async Task<ActionResult<ListResult<SeedInfoParameter>>> QueryAllSeedInfoByCompanyId(int? CompanyId,int? Year) 
        {
            #region 声明变量

            //查询条件
            string SqlWhere = string.Empty;

            //方法返回错误消息
            string message = string.Empty;

            //数据库返回值
            List<SeedInfo> dbResult = new List<SeedInfo>();

            //接口返回数据
            List<SeedInfoParameter> apiResult = new List<SeedInfoParameter>();

            //返回值
            ListResult<SeedInfoParameter> result = new ListResult<SeedInfoParameter>() { Status=0,Msg=string.Empty };
            #endregion

            #region 读取数据
            SqlWhere = $" CompanyId='11' and DateDiff(year,CreatedDateTime,cast('{Year}-01-01 00:00:00' as datetime))=0";
            dbResult = m_repository.QuerySeedInfo(SqlWhere, out message);
            if (dbResult == null || dbResult.Count <= 0) 
            {
                if (!string.IsNullOrEmpty(message)) 
                {
                    result = new ListResult<SeedInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"读取农作物信息失败，原因[{message}]"
                    };
                    return result;
                }
            }
            dbResult.ForEach(item => {
                SeedInfoParameter apiDataItem = m_mapper.Map<SeedInfoParameter>(item);
                apiResult.Add(apiDataItem);
            });
            #endregion

            result = new ListResult<SeedInfoParameter>()
            {
                Status=0,
                Msg=string.Empty,
                Result=apiResult
            };
            return result;
        }
        #endregion

        #region Post

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<ActionResult<Result>> AddData([FromBody]SeedInfoParameter parameter)
        {
            return await SaveData("Add", parameter);
        }
        #endregion

        #region Put

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPut("{CropsId}")]
        public async Task<ActionResult<Result>> ModifyData(string CropsId,[FromBody] SeedInfoParameter parameter)
        {
            parameter.CropsId=CropsId;
            return await SaveData("Edit", parameter);
        }
        #endregion

        #region Private

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="SaveMethod">[Add|Edit]</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回结果</returns>
        private async Task<Result> SaveData(string SaveMethod, SeedInfoParameter parameter) 
        {
            #region 声明和初始化

            //数据库返回值
            Message dbResult = null;

            //返回值
            Result result = null;

            //查询条件
            string SqlWhere = string.Empty;

            //错误消息
            string message = string.Empty;

            //验证消息
            string checkMessage = string.Empty;

            //农作物信息
            List<SeedInfo> seedInfos = null;

            //保存数据
            SeedInfo saveData = new SeedInfo();
            #endregion

            #region 参数非空验证
            if (parameter == null) 
            {
                result = new Result() 
                {
                    Status=-1,
                    Msg="参数不能为空"
                };
            }
            if (string.IsNullOrEmpty(SaveMethod))
                checkMessage += "保存方式、";
            if(SaveMethod=="Edit"&&string.IsNullOrEmpty(parameter.CropsId))
                checkMessage += "农作物编号、";
            if (string.IsNullOrEmpty(parameter.CompanyId))
                checkMessage += "公司编号、";
            if (string.IsNullOrEmpty(parameter.SeedName))
                checkMessage += "农作物名称、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new Result()
                {
                    Status = -1,
                    Msg = $"非空验证出错，原因[{checkMessage}]不能为空"
                };
                return result;
            }
            #endregion

            #region 参数有效验证
            checkMessage = string.Empty;
            if (SaveMethod == "Edit" && (Utils.isLong(parameter.CropsId) == false || Utils.StrToLong(parameter.CropsId) <= 0))
                checkMessage += "农作物编号非数字、";
            if (Utils.IsIntNum(parameter.CompanyId)==false)
                checkMessage += "公司编号非数字、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new Result()
                {
                    Status = -1,
                    Msg = $"参数有效验证出错，原因[{checkMessage}]"
                };
                return result;
            }
            #endregion

            #region 数据验证
            SqlWhere = $" CropsId='{parameter.CropsId}' and CompanyId='{parameter.CompanyId}'";
            seedInfos = m_repository.QuerySeedInfo(SqlWhere, out message);
            if(seedInfos == null || seedInfos.Count <= 0) 
            {
                if(!string.IsNullOrEmpty(message))
                {
                    result = new Result() 
                    {
                        Status=-1,
                        Msg=$"读取农作物信息出错，原因[{message}]"
                    };
                    return result;
                }
                else if (SaveMethod == "Edit") 
                {
                    result = new Result()
                    {
                        Status = -1,
                        Msg = "读取农作物信息出错，原因[没有读取到农作物数据]"
                    };
                    return result;
                }
            }
            #endregion

            #region 保存数据
            if (SaveMethod == "Add") 
            {
                saveData= m_mapper.Map<SeedInfo>(parameter);
                saveData.CropsId = new IdWorker(1, 1).NextId();//生成雪花ID
                saveData.CreatedDateTime = DateTime.Now;
                saveData.ModifiedDateTime = DateTime.Now;
                dbResult = await m_repository.InsertSeedInfo(saveData);
            }
            else if(SaveMethod=="Edit")
            {
                saveData = m_mapper.Map<SeedInfo>(parameter);
                saveData.CreatedDateTime = saveData.CreatedDateTime;
                saveData.ModifiedDateTime = DateTime.Now;
                dbResult = await m_repository.UpdateSeedInfo(new List<SeedInfo>() { saveData }, SqlWhere);
            }
            if (dbResult != null && !dbResult.Successful)
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = $"保存失败，原因[{dbResult.Content}]"
                };
                return result;
            }
            #endregion

            result = new Result()
            {
                Status = 0,
                Msg = string.Empty
            };
            return result;
        }
        #endregion
    }
}
