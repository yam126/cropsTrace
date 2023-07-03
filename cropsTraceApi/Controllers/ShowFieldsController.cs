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
    /// 显示字段
    /// </summary>
    [EnableCors("Cors")]
    [Route("api/show-field")]
    [ApiController]
    public class ShowFieldsController : Controller
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
        public ShowFieldsController(IMOIRepository repository, IMapper _mapper)
        {
            m_repository = repository;
            m_mapper = _mapper == null ? throw new ArgumentNullException(nameof(_mapper)) : _mapper;
        }
        #endregion

        #region Get

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="pageIndex">当前页(默认从1开始)</param>
        /// <param name="pageSize">每页记录数(默认每页10条)</param>
        /// <param name="where">查询条件(SQL查询条件,可以为空,为空返回所有数据)</param>
        /// <param name="condition">查询条件</param>
        /// <param name="SortField">排序字段(默认创建时间)</param>
        /// <param name="SortMethod">排序方法[DESC|ASC(默认DESC)]</param>
        /// <returns>返回查询结果</returns>
        [HttpGet("page")]
        public async Task<ActionResult<PageResult<ShowFieldsParameter>>> GetPage(
            int? CompanyId,
            string? where = "",
            string? condition = "",            
            int? pageIndex = 1,
            int? pageSize = 10,
            string sortField = "RecordId",
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
            List<ShowFields> pageData = null;

            //接口返回值
            List<ShowFieldsParameter> pageResultData = null;

            //返回值
            var result = new PageResult<ShowFieldsParameter>();
            #endregion

            #region 非空验证
            if (pageIndex == null)
                checkMessage += "当前页、";
            if (pageSize == null)
                checkMessage += "每页记录数、";
            if (CompanyId == null)
                checkMessage += "公司编号、";
            if (!string.IsNullOrEmpty(where) && string.IsNullOrEmpty(condition))
                checkMessage += "请选择查询条件、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new PageResult<ShowFieldsParameter>()
                {
                    Status = -1,
                    PageCount = 0,
                    RecordCount = 0,
                    Msg = $"非空验证出错，原因[{checkMessage}]不能为空",
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
                result = new PageResult<ShowFieldsParameter>()
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
            if (!string.IsNullOrEmpty(where) && !string.IsNullOrEmpty(condition))
            {
                switch(condition)
                {
                    case "RecordId":
                        where = $"cast(RecordId as nvarchar(20)) like '%{where}%'";
                        break;
                    case "Device":
                        where = $"Device like '%{where}%'";
                        break;
                    case "PointId":
                        where = $"PointId like '%{where}%'";
                        break;
                    case "FieldName":
                        where = $"FieldName like '%{where}%'";
                        break;
                    case "ShowFieldName":
                        where = $"ShowFieldName like '%{where}%'";
                        break;
                    case "Unit":
                        where = $"Unit like '%{where}%'";
                        break;
                    default:
                        result = new PageResult<ShowFieldsParameter>()
                        {
                            Status = -1,
                            PageCount = 0,
                            RecordCount = 0,
                            Msg = $"查询条件出错，原因[不是已知的查询条件]",
                            Result = null
                        };
                        return result;
                        break;
                }
                where += $" and CompanyId='{CompanyId}'";
            }
            pageData = m_repository.QueryShowFields(
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
                    result = new PageResult<ShowFieldsParameter>()
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
            pageResultData = new List<ShowFieldsParameter>();
            pageData.ForEach(item =>
            {
                ShowFieldsParameter itemParam = m_mapper.Map<ShowFields, ShowFieldsParameter>(item);
                pageResultData.Add(itemParam);
            });
            result = new PageResult<ShowFieldsParameter>()
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
        #endregion

        #region POST

        /// <summary>
        /// 添加显示字段的值
        /// </summary>
        /// <param name="model">要添加的数据</param>
        /// <returns>返回数据</returns>
        [HttpPost]
        public async Task<ActionResult<Result>> AddShowFields([FromBody]ShowFieldsParameter parameter) 
        {
            #region 声明和初始化

            //数据库返回值
            Message dbResult = null;

            //验证消息
            string checkMessage = string.Empty;

            //返回值
            Result result = null;

            //雪花ID
            IdWorker idWorker = new IdWorker(1, 1);

            List<ShowFields> insertData = new List<ShowFields>();

            //保存数据
            ShowFields saveData = new ShowFields();
            #endregion

            #region 参数验证
            if (parameter == null)
            {
                result = new Result() 
                {
                    Status=-1,
                    Msg="参数不能为空"
                };
                return result;
            }
            if (string.IsNullOrEmpty(parameter.CompanyId))
                checkMessage += "公司编号不能为空、";
            if (string.IsNullOrEmpty(parameter.id))
                checkMessage += "测点编号不能为空、";
            if (string.IsNullOrEmpty(parameter.name))
                checkMessage += "名称不能为空、";
            if (string.IsNullOrEmpty(parameter.value))
                checkMessage += "设备值不能为空、";
            if (string.IsNullOrEmpty(parameter.Unit))
                checkMessage += "单位不能为空、";
            if (string.IsNullOrEmpty(parameter.updateTime)||Utils.IsDate(parameter.updateTime)==false)
                checkMessage += "更新时间为空或非正确的时间格式、";
            if (string.IsNullOrEmpty(parameter.deviceCode))
                checkMessage += "设备编码不能为空、";
            if (string.IsNullOrEmpty(parameter.deviceName))
                checkMessage += "设备名称不能为空、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new Result()
                {
                    Status = -1,
                    Msg = $"参数验证出错，原因[{checkMessage}]"
                };
                return result;
            }
            #endregion

            #region 参数赋值
            saveData = m_mapper.Map<ShowFields>(parameter);
            saveData.RecordId = idWorker.NextId();
            #endregion

            #region 保存数据
            insertData.Add(saveData);
            dbResult = await m_repository.InsertShowFields(insertData);
            if (dbResult.Successful == false) 
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = $"添加数据失败，原因[{dbResult.Content}]"
                };
                return result;
            }
            #endregion

            result = new Result() 
            {
                Status=0,
                Msg=string.Empty
            };
            return result;
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
                result = new Result()
                {
                    Msg = "参数为空不能删除",
                    Status = -1
                };
            }

            try
            {
                IDStr = "'" + IDStr.Replace("-", "','") + "'";
                dbMessage = await m_repository.DeleteShowFields(IDStr);
            }
            catch (Exception ex)
            {
                result = new Result()
                {
                    Msg = $"删除显示参数信息报错，原因[{ex.Message}]",
                    Status = -1
                };
                return result;
            }

            result = new Result()
            {
                Msg = string.Empty,
                Status = 0
            };
            return result;
        }
        #endregion

        #region Put

        /// <summary>
        /// 设置是否显示
        /// </summary>
        /// <param name="parameter">是否显示参数</param>
        /// <returns></returns>
        [HttpPut("set-is-show-fields")]
        public async Task<ActionResult<Result>> SetIsShowFields(SetIsShowFieldsParameter parameter) 
        {
            #region 声明变量

            //错误消息
            string message = string.Empty;

            //非空验证
            string checkEmptyMessage = string.Empty;

            //查询条件
            string SqlWhere = string.Empty;

            //转换后的记录编号
            List<long> changeRecordIds = new List<long>();

            //最终的记录编号
            List<long> finalRecordIds = new List<long>();

            //未选中的ID字符串
            string unSelectedRecordIdStr = string.Empty;

            //数据库返回值
            ePioneer.Data.Kernel.Message dbMessage = null;

            //已经设置为显示的数据
            List<ShowFields> isShowFields = null;

            //选中的数据RecordId
            List<long> isSelectedShowFields = new List<long>();

            //返回值
            Result result = null;
            #endregion

            #region 参数验证
            if (parameter == null) 
            {
                result = new Result()
                {
                    Status = -1,
                    Msg = "参数为空"
                };
                return result;
            }
            if (string.IsNullOrEmpty(parameter.CompanyId))
                checkEmptyMessage += "公司编号不能为空、";
            if (parameter.RecordIds == null || parameter.RecordIds.Length <= 0)
                checkEmptyMessage += "选中的记录编号不能为空、";
            else if (parameter.RecordIds.Any(recordid => !Utils.isLong(recordid)))
                checkEmptyMessage += "选中的记录编号非数字、";
            else if (parameter.RecordIds.Length > 6)
                checkEmptyMessage += "最多只能选择6个参数";
            if (string.IsNullOrEmpty(parameter.isShow))
                checkEmptyMessage += "是否显示不能为空、";
            else if (!Utils.IsIntNum(parameter.isShow))
                checkEmptyMessage += "是否显示非数字";
            else if (Utils.StrToInt(parameter.isShow, 0) != 0 && Utils.StrToInt(parameter.isShow, 0) != 1)
                checkEmptyMessage += "是否显示只能是0或1";
            if (!string.IsNullOrEmpty(checkEmptyMessage)) 
            {
                checkEmptyMessage = checkEmptyMessage.Substring(0, checkEmptyMessage.Length - 1);
                result = new Result()
                {
                    Status = -1,
                    Msg = $"参数验证出错，原因[{checkEmptyMessage}]"
                };
                return result;
            }
            #endregion

            #region 转换为long值
            parameter.RecordIds.ToList().ForEach(recordId => {
                changeRecordIds.Add(Utils.StrToLong(recordId));
            });
            #endregion

            #region 读取已经选中的数据
            if (parameter.unSelectedRecordIds != null && parameter.unSelectedRecordIds.Length > 0)
            {
                unSelectedRecordIdStr = "'" + string.Join("','", parameter.unSelectedRecordIds) + "'";
                SqlWhere = $" IsShow='1' and CompanyId='{parameter.CompanyId}' and RecordId not in ({unSelectedRecordIdStr})";
            }
            else
                SqlWhere = $" IsShow='1' and CompanyId='{parameter.CompanyId}' ";
            isShowFields = m_repository.QueryShowFields(SqlWhere, out message);
            if (isShowFields == null || isShowFields.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    result = new Result() { Status = -1, Msg = $"读取已选中数据出错，原因[{message}]" };
                    return result;
                }
                else 
                {
                    finalRecordIds = changeRecordIds;
                }
            }
            else
            {
                isShowFields.ForEach(item => {
                    isSelectedShowFields.Add(item.RecordId.GetValueOrDefault());
                });
                #region LINQ合并去重复
                var mergeSelectedShowFields = changeRecordIds.Union(isSelectedShowFields).ToList().GroupBy(item=>item);
                foreach(var item in mergeSelectedShowFields) 
                    finalRecordIds.Add(item.First());
                #endregion
                if (finalRecordIds.Count > 6) 
                {
                    result = new Result()
                    {
                        Status = -1,
                        Msg = "最多只能选中6项"
                    };
                    return result;
                }
            }
            #endregion
            
            #region 执行设置代码
            dbMessage = await m_repository.SetShowFieldsIsShow(
                Utils.StrToInt(parameter.CompanyId, -1),
                finalRecordIds, 
                Utils.StrToInt(parameter.isShow, 0));
            #endregion

            #region 返回结果
            result = new Result() {
                Status=dbMessage.Successful?0:-1,
                Msg=dbMessage.Content
            };
            #endregion
            return result;
        }
        #endregion
    }
}
