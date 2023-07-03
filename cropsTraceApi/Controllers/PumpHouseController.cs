using AutoMapper;
using Common;
using cropsTraceApi.Models;
using cropsTraceApi.Models.Result;
using cropsTraceDataAccess.Data;
using cropsTraceDataAccess.Model;
using ePioneer.Data.Kernel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Snowflake.Net;

namespace cropsTraceApi.Controllers
{
    /// <summary>
    /// 泵房控制器
    /// </summary>
    [EnableCors("Cors")]
    [Route("api/pump-house")]
    [ApiController]
    public class PumpHouseController : Controller
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
        public PumpHouseController(IMOIRepository repository, IMapper _mapper)
        {
            m_repository = repository;
            m_mapper = _mapper == null ? throw new ArgumentNullException(nameof(_mapper)) : _mapper;
        }
        #endregion

        #region Get

        /// <summary>
        /// 分页查询泵房数据
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="where">查询条件</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">总页数</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortMethod">排序方法</param>
        /// <returns>每页结果</returns>
        [HttpGet("page")]
        public async Task<ActionResult<PageResult<PumpHouseInfoParameter>>> GetPage(
            int? CompanyId,
            string? where = "",
            string? startTime = "",
            string? endTime = "",
            int? pageIndex = 1,
            int? pageSize = 10,
            string sortField = "ModifiedDateTime",
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
            List<PumpHouseInfo> pageData = null;

            //接口返回值
            List<PumpHouseInfoParameter> pageResultData = null;

            //返回值
            var result = new PageResult<PumpHouseInfoParameter>();
            #endregion

            #region 非空验证
            if (CompanyId == null)
                checkMessage += "公司编号、";
            if (pageIndex == null)
                checkMessage += "当前页、";
            if (pageSize == null)
                checkMessage += "每页记录数、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new PageResult<PumpHouseInfoParameter>()
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
                result = new PageResult<PumpHouseInfoParameter>()
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
            {
                List<string> searchWhere = new List<string>() {
                    $" PumpHouseName like '%{where}%'",
                    $" PumpHouseClass like '%{where}%'",
                    $" PersonIinCharge like '%{where}%'",
                    $" MonitoringAddress like '%{where}%'"
                };
                where = $" and ({string.Join(" Or ", searchWhere)}) ";
            }
            if (!string.IsNullOrEmpty(startTime) && Utils.IsDate(startTime))
                where += $" and CreatedDateTime>=cast('{startTime}' as datetime) ";
            if (!string.IsNullOrEmpty(endTime) && Utils.IsDate(endTime))
                where += $" and CreatedDateTime<=cast('{endTime}' as datetime) ";
            if (string.IsNullOrEmpty(where))
                where = $" CompanyId='{CompanyId}' ";
            else
                where = $" CompanyId='{CompanyId}' {where} ";
            pageData = m_repository.QueryPumpHouseInfo(
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
                    result = new PageResult<PumpHouseInfoParameter>()
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
            pageResultData = new List<PumpHouseInfoParameter>();
            pageData.ForEach(item =>
            {
                PumpHouseInfoParameter itemParam = m_mapper.Map<PumpHouseInfo, PumpHouseInfoParameter>(item);
                itemParam.CreatedDateTime = item.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                itemParam.ModifiedDateTime = item.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                pageResultData.Add(itemParam);
            });
            result = new PageResult<PumpHouseInfoParameter>()
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
        /// 泵房自动完成
        /// </summary>
        /// <param name="where">自动完成查询关键字</param>
        /// <param name="resultDataCount">自动完成返回的数据量</param>
        /// <returns></returns>
        [HttpGet("auto-complate/{resultDataCount}")]
        public async Task<ActionResult<ListResult<PumpHouseInfoParameter>>> QueryAutoComplate(
            int CompanyId,
            string where="",
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
            List<PumpHouseInfo> pumpHouses = null;

            //返回数据
            List<PumpHouseInfoParameter> resultList = new List<PumpHouseInfoParameter>();

            //返回值
            ListResult<PumpHouseInfoParameter> result = null;
            #endregion

            #region 参数验证
            if (resultDataCount == null)
            {
                result = new ListResult<PumpHouseInfoParameter>()
                {
                    Status = -1,
                    Msg = "返回的数据量不能为空",
                    Result = resultList
                };
                return result;
            }
            if(CompanyId==0)
            {
                result = new ListResult<PumpHouseInfoParameter>()
                {
                    Status = -1,
                    Msg = "公司编号不能为空",
                    Result = resultList
                };
                return result;
            }
            #endregion

            #region 拼写查询条件
            if (!string.IsNullOrEmpty(where) && where != "all")
            {
                SqlWhere.Add($" cast(PumpId as nvarchar(50)) like '%{where}%'");
                SqlWhere.Add($" PumpHouseName like '%{where}%'");
                SqlWhere.Add($" PumpHouseClass like '%{where}%'");
                SqlWhere.Add($" PersonIinCharge like '%{where}%'");
                SqlQueryWhere = $"({string.Join(" Or ", SqlWhere.ToArray())}) and CompanyId='{CompanyId}'";
            }
            else 
            {
                SqlQueryWhere = $" CompanyId='{CompanyId}' ";
            }
            #endregion

            #region 查询数据
            pumpHouses = m_repository.QueryPumpHouseInfo(
                SqlQueryWhere,
                "CreatedDateTime",
                "DESC",
                resultDataCount.GetValueOrDefault(),
                1,
                out totalRecordCount,
                out message);
            if (pumpHouses == null || pumpHouses.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    result = new ListResult<PumpHouseInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"查询泵房数据出错,原因[{message}]",
                        Result = resultList
                    };
                }
                else
                {
                    result = new ListResult<PumpHouseInfoParameter>()
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
            resultList = new List<PumpHouseInfoParameter>();
            pumpHouses.ForEach(item =>
            {
                PumpHouseInfoParameter itemParam = m_mapper.Map<PumpHouseInfo, PumpHouseInfoParameter>(item);
                itemParam.CreatedDateTime = item.CreatedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                itemParam.ModifiedDateTime = item.ModifiedDateTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                resultList.Add(itemParam);
            });
            result = new ListResult<PumpHouseInfoParameter>()
            {
                Status = 0,
                Msg = string.Empty,
                Result = resultList
            };
            #endregion

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
        public async Task<ActionResult<Result>> AddData([FromBody]PumpHouseInfoParameter parameter)
        {
            return await SaveData("Add", parameter);
        }
        #endregion

        #region Delete

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="GrowthID">泵房编号字符串</param>
        /// <returns>删除结果</returns>
        [EnableCors("Cors")]
        [HttpDelete("{IDStr}")]
        public async Task<ActionResult<Result>> BatchDeleteData(string IDStr) 
        {
            #region 声明变量

            //查询条件
            string SqlWhere = string.Empty;

            //错误消息
            string message = string.Empty;

            //数据库返回消息
            Message dbMessage = null;

            //生长信息编号
            string[] PumpId = null;

            //返回值
            Result result = new Result() { Status = 0, Msg = string.Empty };
            #endregion

            PumpId = string.IsNullOrEmpty(IDStr) ? null : IDStr.Split('-');

            #region 参数验证
            if (PumpId == null || PumpId.Length == 0)
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = "参数为空无法删除"
                };
                return result;
            }
            else
            {
                for (var i = 0; i < PumpId.Length; i++)
                {
                    if (!Utils.isLong(PumpId[i]))
                        message += $"第{i}条数据非数字、";
                }
                if (!string.IsNullOrEmpty(message))
                {
                    result = new Result()
                    {
                        Status = -1,
                        Msg = $"参数有非数字参数无法删除,原因[{message}]"
                    };
                    return result;
                }
            }
            #endregion

            #region 删除数据
            SqlWhere = $"'{string.Join("','", PumpId)}'";
            dbMessage = await m_repository.BatchDeletePumpHouse(SqlWhere);
            if (!dbMessage.Successful)
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = $"删除泵房信息失败，原因[{dbMessage.Content}]"
                };
                return result;
            }
            #endregion

            return result;
        }
        #endregion

        #region Put

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPut("{PumpId}")]
        public async Task<ActionResult<Result>> ModifyData(string PumpId, [FromBody] PumpHouseInfoParameter parameter)
        {
            parameter.PumpId = PumpId;
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
        private async Task<Result> SaveData(string SaveMethod, PumpHouseInfoParameter parameter) 
        {
            #region 声明变量
            //查询条件
            string SqlWhere = string.Empty;

            //错误消息
            string message = string.Empty;

            //验证消息
            string checkMessage = string.Empty;

            //数据库返回值
            Message dbResult = null;

            //要保存的数据
            PumpHouseInfo saveData = new PumpHouseInfo();

            //返回值
            Result result = new Result() { Status=0,Msg=string.Empty };
            #endregion

            #region 参数非空验证
            if (parameter == null)
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = "参数不能为空"
                };
                return result;
            }
            if (string.IsNullOrEmpty(SaveMethod))
                checkMessage += "保存方式、";
            if (SaveMethod == "Edit" && string.IsNullOrEmpty(parameter.PumpId))
                checkMessage += "泵房编号、";
            if (string.IsNullOrEmpty(parameter.PumpHouseName))
                checkMessage += "泵房名称、";
            if (string.IsNullOrEmpty(parameter.PumpHouseClass))
                checkMessage += "泵房种类、";
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

            #region 有效验证
            if (SaveMethod == "Edit"&&!Utils.IsDate(parameter.CreatedDateTime))
                checkMessage += "创建时间不是有效的时间字符串推荐[yyyy-MM-dd]、";
            if (SaveMethod == "Edit"&&!Utils.IsDate(parameter.ModifiedDateTime))
                checkMessage += "修改时间不是有效的时间字符串推荐[yyyy-MM-dd]、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new Result()
                {
                    Status = -1,
                    Msg = $"有效验证出错，原因[{checkMessage}]不能为空"
                };
                return result;
            }
            #endregion

            #region 保存数据
            try
            {
                if (SaveMethod == "Add")
                {
                    saveData = m_mapper.Map<PumpHouseInfo>(parameter);
                    saveData.PumpId = new IdWorker(1, 1).NextId();//生成雪花ID
                    saveData.CreatedDateTime = DateTime.Now;
                    saveData.ModifiedDateTime = DateTime.Now;
                    dbResult = await m_repository.InsertPumpHouseInfo(new List<PumpHouseInfo>() { saveData });
                }
                else if (SaveMethod == "Edit")
                {
                    SqlWhere = $" PumpId='{parameter.PumpId}' ";
                    saveData = m_mapper.Map<PumpHouseInfo>(parameter);
                    saveData.CreatedDateTime = saveData.CreatedDateTime;
                    saveData.ModifiedDateTime = DateTime.Now;
                    dbResult = await m_repository.UpdatePumpHouseInfo(new List<PumpHouseInfo>() { saveData }, SqlWhere);
                }
            }
            catch (Exception ex)
            {
                dbResult = new Message(false, ex.Message);
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

            return result;
        }

        #endregion
    }
}
