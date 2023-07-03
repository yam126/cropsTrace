using AutoMapper;
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
    /// 公司信息
    /// </summary>
    [EnableCors("Cors")]
    [Route("api/company-info")]
    [ApiController]
    public class CompanyInfoController : Controller
    {
        #region Fields

        /// <summary>
        /// 数据库操作类
        /// </summary>
        private readonly IMOIRepository? m_repository;

        /// <summary>
        /// AutoMapper参数映射类
        /// </summary>
        private readonly IMapper? m_mapper;

        /// <summary>
        /// 获取网站路径
        /// </summary>
        private readonly IWebHostEnvironment m_webHostEnvironment;
        #endregion

        #region Constructors

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="repository">数据库操作类</param>
        /// <param name="_mapper">字段映射类</param>
        /// <param name="webHostEnvironment">网站路径类</param>
        public CompanyInfoController(IMOIRepository repository, IMapper _mapper, IWebHostEnvironment webHostEnvironment)
        {
            m_repository = repository;
            m_mapper = _mapper == null ? throw new ArgumentNullException(nameof(_mapper)) : _mapper;
            m_webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Get

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页(默认从1开始)</param>
        /// <param name="pageSize">每页记录数(默认每页10条)</param>
        /// <param name="where">查询条件(SQL查询条件,可以为空,为空返回所有数据)</param>
        /// <param name="SortField">排序字段(默认创建时间)</param>
        /// <param name="SortMethod">排序方法[DESC|ASC(默认DESC)]</param>
        /// <returns>返回查询结果</returns>
        [HttpGet("page")]
        public async Task<ActionResult<PageResult<CompanyInfoParameter>>> GetPage(
            string? where = "",
            int? pageIndex = 1,
            int? pageSize = 10,
            string sortField = "CreatedTime",
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
            List<CompanyInfo> pageData = null;

            //接口返回值
            List<CompanyInfoParameter> pageResultData = null;

            //返回值
            var result = new PageResult<CompanyInfoParameter>();
            #endregion

            #region 非空验证
            if (pageIndex == null)
                checkMessage += "当前页、";
            if (pageSize == null)
                checkMessage += "每页记录数、";
            if (!string.IsNullOrEmpty(checkMessage))
            {
                checkMessage = checkMessage.Substring(0, checkMessage.Length - 1);
                result = new PageResult<CompanyInfoParameter>()
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
                result = new PageResult<CompanyInfoParameter>()
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
                List<string> sqlWhere = new List<string>();
                sqlWhere.Add($" companyName like '%{where}%' ");
                where = String.Join(" Or ", sqlWhere.ToArray());
            }
            pageData = m_repository.QueryPageCompanyInfo(
                where == null ? string.Empty : where,
                sortField,
                sortMethod,
                pageSize.GetValueOrDefault(),
                pageIndex.GetValueOrDefault(),
                out totalRecordCount,
                out pageCount,
                out message
            );
            if (pageData == null || pageData.Count <= 0)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    result = new PageResult<CompanyInfoParameter>()
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
            pageResultData = new List<CompanyInfoParameter>();
            pageData.ForEach(item =>
            {
                var resultItem = m_mapper.Map<CompanyInfo, CompanyInfoParameter>(item);
                resultItem.CreatedTime = item.CreatedTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                resultItem.ModifiedTime = item.ModifiedTime.GetValueOrDefault().ToString("yyyy-MM-dd");
                pageResultData.Add(resultItem);
            });
            result = new PageResult<CompanyInfoParameter>()
            {
                Status = 0,
                PageCount = pageCount,
                RecordCount = totalRecordCount,
                Msg = String.Empty,
                Result = pageResultData.ToArray()
            };
            #endregion

            return result;
        }
        #endregion

        #region Post

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="parameter">添加参数</param>
        /// <returns>返回结果</returns>        
        [HttpPost]
        public ActionResult<Result> AddData([FromBody] CompanyInfoParameter parameter)
        {
            return SaveData("Add", parameter, null);
        }
        #endregion

        #region Put

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="parameter">修改数据</param>
        /// <returns>返回结果</returns>  
        [HttpPut("{companyId}")]
        public ActionResult<Result> ModifyData(string companyId, [FromBody] CompanyInfoParameter parameter)
        {
            parameter.companyId = companyId;
            return SaveData("Edit", parameter, null);
        }
        #endregion

        #region Private

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="SaveMethod">[Add|Edit]</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回结果</returns>
        private Result SaveData(string SaveMethod, CompanyInfoParameter parameter, IdWorker? snowId)
        {
            #region 声明和初始化

            //数据库返回值
            Message dbResult = null;

            //返回值
            Result? result = null;

            //查询条件
            string SqlWhere = string.Empty;

            //错误消息
            string message = string.Empty;

            //验证消息
            string checkMessage = string.Empty;

            //是否存在文艺小组
            List<CompanyInfo> isHave = new List<CompanyInfo>();

            //文艺小组表
            CompanyInfo saveData = new CompanyInfo();
            #endregion

            if (snowId == null)
                snowId = new IdWorker(1, 1);

            #region 非空验证
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
            if (SaveMethod == "Edit" && string.IsNullOrEmpty(parameter.companyId))
                checkMessage += "公司编号、";
            if (string.IsNullOrEmpty(parameter.companyName))
                checkMessage += "公司名称、";
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

            #region 查询数据
            if (SaveMethod == "Edit")
            {
                SqlWhere = $" companyId='{parameter.companyId}' ";
                isHave = m_repository.QueryCompanyInfo(SqlWhere, out message);
                if (isHave == null || isHave.Count <= 0)
                {
                    if (!string.IsNullOrEmpty(message))
                        message = $"查询文艺小组信息出错,原因[{message}]";
                    else
                        message = $"查询文艺小组信息出错,原因[没有找到文艺小组数据]";
                    result =  new Result()
                    {
                        Status = -1,
                        Msg = message
                    };
                    return result;
                }
            }
            #endregion

            #region 保存数据
            if (SaveMethod == "Add")
            {
                saveData = m_mapper!.Map<CompanyInfo>(parameter);
                saveData.companyId = snowId.NextId();
                saveData.CreatedTime = DateTime.Now;
                saveData.ModifiedTime = DateTime.Now;
                dbResult = m_repository!.InsertCompanyInfo(new List<CompanyInfo>() { saveData });
            }
            else if (SaveMethod == "Edit")
            {
                saveData = m_mapper!.Map<CompanyInfo>(parameter);
                saveData.created = isHave[0].created;
                saveData.CreatedTime = isHave[0].CreatedTime;
                saveData.ModifiedTime = DateTime.Now;
                SqlWhere = $" companyId='{parameter.companyId}' ";
                dbResult = m_repository!.UpdateCompanyInfo(new List<CompanyInfo>() { saveData }, SqlWhere);
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
