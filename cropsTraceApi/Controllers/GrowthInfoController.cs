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
    /// 生长信息
    /// </summary>
    [EnableCors("Cors")]
    [Route("api/growth-info")]
    [ApiController]
    public class GrowthInfoController : Controller
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
        public GrowthInfoController(IMOIRepository repository, IMapper _mapper)
        {
            m_repository = repository;
            m_mapper = _mapper == null ? throw new ArgumentNullException(nameof(_mapper)) : _mapper;
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
        public async Task<ActionResult<PageResult<GrowthInfoParameter>>> GetPage(
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
            List<vw_GrowthInfo> pageData = null;

            //接口返回值
            List<GrowthInfoParameter> pageResultData = null;

            //返回值
            var result = new PageResult<GrowthInfoParameter>();
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
                result = new PageResult<GrowthInfoParameter>()
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
                result = new PageResult<GrowthInfoParameter>()
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
                    $" GrowthName like '%{where}%'",
                    $" LandName like '%{where}%'",
                    $" SeedName like '%{where}%'",
                    $" PumpHouseName like '%{where}%'"
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
            pageData = m_repository.QueryViewGrowthInfo(
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
                    result = new PageResult<GrowthInfoParameter>()
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
            pageResultData = new List<GrowthInfoParameter>();
            pageData.ForEach(item =>
            {
                GrowthInfoParameter itemParam = m_mapper.Map<vw_GrowthInfo, GrowthInfoParameter>(item);
                itemParam.CreatedDateTime = item.CreatedDateTime.ToString("yyyy-MM-dd");
                itemParam.ModifiedDateTime = item.ModifiedDateTime.ToString("yyyy-MM-dd");
                pageResultData.Add(itemParam);
            });
            result = new PageResult<GrowthInfoParameter>()
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
        /// 返回生长周期所有年份
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>年份数据</returns>
        [HttpGet("allYear/{CompanyId}")]
        public ListResult<int> GetAllYear(int CompanyId) 
        {
            #region 声明变量

            //错误消息
            string message = string.Empty;

            //数据库返回值
            List<int> dbResult = new List<int>();

            //返回值
            ListResult<int> result = new ListResult<int>();
            #endregion

            dbResult = m_repository.GetAllGrowthInfoYear(CompanyId,out message);
            if (dbResult == null || dbResult.Count <= 0) 
            {
                if (!string.IsNullOrEmpty(message)) 
                {
                    result = new ListResult<int>() 
                    {
                        Status=-1,
                        Msg=$"获取年份数据出错，原因[{message}]"
                    };
                    return result;
                }
            }

            result = new ListResult<int>() 
            {
                Status=0,
                Msg=string.Empty,
                Result=dbResult
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
        public async Task<ActionResult<EntityResult<GrowthInfoParameter>>> AddData([FromBody] GrowthInfoParameter parameter)
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
        [HttpPut("{RecordId}")]
        public async Task<ActionResult<EntityResult<GrowthInfoParameter>>> ModifyData(string RecordId, [FromBody]GrowthInfoParameter parameter)
        {
            parameter.RecordId = RecordId;
            return await SaveData("Edit", parameter);
        }
        #endregion

        #region Delete

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="GrowthID">生长信息编号</param>
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
            string[] GrowthID = null;

            //返回值
            Result result = new Result() { Status=0,Msg=string.Empty };
            #endregion

            GrowthID=string.IsNullOrEmpty(IDStr)?null:IDStr.Split('-');

            #region 参数验证
            if (GrowthID==null||GrowthID.Length==0)
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
                for (var i = 0; i < GrowthID.Length; i++) 
                {
                    if (!Utils.isLong(GrowthID[i]))
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

            #region 删除文件
            SqlWhere = $" GrowthRecordId in ('{string.Join("','",GrowthID)}')";
            dbMessage =await m_repository.DeleteFileInfo(SqlWhere);
            if (!dbMessage.Successful) 
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = $"删除生长信息关联的文件信息失败，原因[{dbMessage.Content}]"
                };
                return result;
            }
            #endregion

            #region 删除生长信息
            SqlWhere = $"'{string.Join("','", GrowthID)}'";
            dbMessage = await m_repository.DeleteGrowthInfo(SqlWhere);
            if (!dbMessage.Successful)
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = $"删除生长信息失败，原因[{dbMessage.Content}]"
                };
                return result;
            }
            #endregion

            return result;
        }
        #endregion

        #region Private

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="SaveMethod">[Add|Edit]</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回结果</returns>
        private async Task<EntityResult<GrowthInfoParameter>> SaveData(string SaveMethod, GrowthInfoParameter parameter)
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

            //返回值
            EntityResult<GrowthInfoParameter> result = new EntityResult<GrowthInfoParameter>() { Status=0,Msg=string.Empty };

            //返回生长信息
            GrowthInfoParameter resultInfo = new GrowthInfoParameter();

            //生长物信息
            List<GrowthInfo> growthInfos = new List<GrowthInfo>();

            //泵房信息
            List<PumpHouseInfo> pumpHouseInfos = new List<PumpHouseInfo>();

            //农作物信息
            List<SeedInfo> seedInfos = new List<SeedInfo>();

            //保存数据
            GrowthInfo saveData = new GrowthInfo();
            #endregion

            #region 参数非空验证
            if (parameter == null)
            {
                result = new EntityResult<GrowthInfoParameter>()
                {
                    Status = -1,
                    Msg = "参数不能为空"
                };
                return result;
            }
            if(string.IsNullOrEmpty(SaveMethod))
                checkMessage += "保存方式、";
            if (string.IsNullOrEmpty(parameter.PumpId))
                checkMessage += "泵房编号、";
            if (string.IsNullOrEmpty(parameter.CropsId))
                checkMessage += "农作物编号、";
            if (string.IsNullOrEmpty(parameter.CompanyId))
                checkMessage += "公司编号、";
            if (string.IsNullOrEmpty(parameter.GrowthName))
                checkMessage += "周期名称、";
            if (string.IsNullOrEmpty(parameter.CreatedDateTime))
                checkMessage += "录入时间、";
            if (!string.IsNullOrEmpty(checkMessage)) 
            {
                result = new EntityResult<GrowthInfoParameter>()
                {
                    Status = -1,
                    Msg = $"非空验证出错，原因[{checkMessage}]不能为空"
                };
                return result;
            }
            #endregion

            #region 有效验证
            if (!Utils.IsDate(parameter.CreatedDateTime))
                checkMessage += "录入时间不是有效的日期格式、";
            if (!string.IsNullOrEmpty(parameter.PlantHeight) && !Utils.IsDecimal(parameter.PlantHeight))
                checkMessage += "株高不是数字、";
            if (!string.IsNullOrEmpty(parameter.DBH) && !Utils.IsDecimal(parameter.DBH))
                checkMessage += "胸径不是数字、";
            if (!string.IsNullOrEmpty(parameter.NumberOfBlades) && !Utils.IsIntNum(parameter.NumberOfBlades))
                checkMessage += "叶片数不是数字、";
            if(!string.IsNullOrEmpty(parameter.EmergenceRate)&&!Utils.IsDecimal(parameter.EmergenceRate))
                checkMessage += "出苗率不是数字、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                result = new EntityResult<GrowthInfoParameter>()
                {
                    Status = -1,
                    Msg = $"参数有效验证出错，原因[{checkMessage}]"
                };
                return result;
            }
            #endregion

            #region 验证泵房信息
            SqlWhere = $" PumpId='{parameter.PumpId}' ";
            pumpHouseInfos = m_repository.QueryPumpHouseInfo(SqlWhere, out message);
            if (pumpHouseInfos == null || pumpHouseInfos.Count <= 0) 
            {
                if (!string.IsNullOrEmpty(message)) 
                {
                    result = new EntityResult<GrowthInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"读取泵房编号为[{parameter.PumpId}]数据出错，原因[{message}]"
                    };
                    return result;
                }
                else 
                {
                    result = new EntityResult<GrowthInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"读取泵房编号为[{parameter.PumpId}]数据出错，原因[没有泵房数据]"
                    };
                    return result;
                }
            }
            #endregion

            #region 验证农作物
            SqlWhere = $" CropsId='{parameter.CropsId}' ";
            seedInfos = m_repository.QuerySeedInfo(SqlWhere, out message);
            if (seedInfos == null || seedInfos.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    result = new EntityResult<GrowthInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"读取农作物编号为[{parameter.CropsId}]名称为[{parameter.SeedName}]数据出错，原因[{message}]"
                    };
                    return result;
                }
                else
                {
                    result = new EntityResult<GrowthInfoParameter>()
                    {
                        Status = -1,
                        Msg = $"读取农作物编号为[{parameter.CropsId}]名称为[{parameter.SeedName}]数据出错，原因[没有农作物数据]"
                    };
                    return result;
                }
            }
            #endregion

            #region 保存数据
            try
            {
                if (SaveMethod == "Add")
                {
                    saveData = m_mapper.Map<GrowthInfo>(parameter);
                    saveData.RecordId = new IdWorker(1, 1).NextId();//生成雪花ID
                    saveData.CreatedDateTime = DateTime.Now;
                    saveData.ModifiedDateTime = DateTime.Now;
                    dbResult = await m_repository.InsertGrowthInfo(saveData);
                }
                else if (SaveMethod == "Edit")
                {
                    SqlWhere = $" RecordId='{parameter.RecordId}' ";
                    saveData = m_mapper.Map<GrowthInfo>(parameter);
                    saveData.CreatedDateTime = saveData.CreatedDateTime;
                    saveData.ModifiedDateTime = DateTime.Now;
                    dbResult = await m_repository.UpdateGrowthInfo(new List<GrowthInfo>() { saveData }, SqlWhere);
                }
                resultInfo = m_mapper.Map<GrowthInfoParameter>(saveData);
            }
            catch (Exception ex)
            {
                dbResult = new Message(false, ex.Message);
            }
            if (dbResult != null && !dbResult.Successful)
            {
                result = new EntityResult<GrowthInfoParameter>()
                {
                    Status = -1,
                    Msg = $"保存失败，原因[{dbResult.Content}]"
                };
                return result;
            }
            #endregion

            result.Result = resultInfo;
            return result;
        }
        #endregion
    }
}
